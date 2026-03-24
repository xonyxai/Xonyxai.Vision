using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using VisionApp.Classes;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace VisionApp
{
    public partial class Main : Form
    {

        private MLContext _context = new MLContext();

        private IDataView? _data;
        private FileInfo _root = new FileInfo(typeof(Program).Assembly.Location);

        private string _assemblyFolderPath;
        private string _modelPath;

        private bool _enableInspection = false;
        private bool _runningInspection = false;

        private float confidenceThreshold = 0.30f;

        private Camera _camera;

        private string[] _labels;


        private EstimatorChain<ImagePixelExtractingTransformer> _pipeline;

        private PredictionEngine<StopSignInput, StopSignPrediction> _predictionEngine;

        public Main()
        {
            InitializeComponent();
        }


        private void Initialise()
        {
            _data = _context.Data.LoadFromEnumerable(new List<StopSignInput>());
            _assemblyFolderPath = _root.Directory.FullName;
            _modelPath = Path.Combine(_assemblyFolderPath, Constants.ModelPath);

            var modelName = Path.Combine(_modelPath, Constants.ModelName);

            if (!File.Exists(modelName))
                MessageBox.Show($"Model {Constants.ModelName} not found at {_modelPath}");

            var pipeline = _context.Transforms.ResizeImages(
                resizing: ImageResizingEstimator.ResizingKind.Fill, outputColumnName: "image_tensor",
                imageWidth: ImageSettings.imageWidth, imageHeight: ImageSettings.imageHeight,
                inputColumnName: nameof(StopSignInput.Image)).
                Append(_context.Transforms.ExtractPixels(outputColumnName: "image_tensor")).
                Append(_context.Transforms.ApplyOnnxModel(outputColumnNames: new string[] { "detected_boxes", "detected_scores", "detected_classes" },
                    inputColumnNames: new string[] { "image_tensor" }, modelFile: modelName));   // 

            // Now that you've defined a pipeline, you can use it to build the ML.NET model. Use the Fit method on the pipeline and pass in the empty IDataView.
            var model = pipeline.Fit(_data);

            // Next, to make predictions, use the model to create a prediction engine. This is a generic method, so it takes in the StopSignInput and StopSignPrediction classes that were created earlier.
            _predictionEngine = _context.Model.CreatePredictionEngine<StopSignInput, StopSignPrediction>(model);

            // To map the model outputs to its labels, you need to extract the labels provided by Custom Vision.
            // These labels are in the labels.txt file that was included in the zip file with the ONNX model.
            // Call the ReadAllLines method to read all the labels form the file.
            
            var lablesPath = Path.Combine(_modelPath, "labels.txt");

            _labels = File.ReadAllLines(lablesPath);

            _camera = new Camera();
            _camera.OnNewFrame += _camera_OnNewFrame;


        }


        private void Main_Load(object sender, EventArgs e)
        {
            Initialise();
        }



        private void UpdateImageSafe(Bitmap image)
        {
            if (ImageBox.InvokeRequired)
            {
                // Marshal the call to the UI thread
                ImageBox.Invoke(new Action<Bitmap>(UpdateImageSafe), image);
            }
            else
            {
                ImageBox.Image = image;
            }
        }


        private void _camera_OnNewFrame(object? sender, FrameEventAgrs e)
        {
            // ImageBox.Image = e.FrameImage;

            if(!_runningInspection)
                UpdateImageSafe(e.FrameImage);
  


            if (_enableInspection && !_runningInspection)
            {
                _runningInspection = true;
                var frame = (Bitmap)e.FrameImage.Clone(); // Clone the frame to avoid issues with the original frame being modified by the video source


                var s = HelperTools.BitmapToStream(frame, ImageFormat.Jpeg);
                var testImage = MLImage.CreateFromStream(s);

                var prediction = _predictionEngine.Predict(new StopSignInput { Image = testImage });

                var boundingBoxes = prediction.BoundingBoxes.Chunk(prediction.BoundingBoxes.Count() / prediction.PredictedLabels.Count());

                var originalWidth = testImage.Width;
                var originalHeight = testImage.Height;

                

                // Draw bounding boxes with labels
                for (int i = 0; i < boundingBoxes.Count(); i++)
                {
                    if (prediction.Scores[i] < confidenceThreshold) continue;


                    // calculate the position of the x and y coordinates as well as the width and height of the box to draw on the image.
                    var boundingBox = boundingBoxes.ElementAt(i);

                    // calculate where to draw the box.
                    var left = boundingBox[0] * originalWidth;
                    var top = boundingBox[1] * originalHeight;
                    var right = boundingBox[2] * originalWidth;
                    var bottom = boundingBox[3] * originalHeight;

                    // Calculate the width and the height of the box to draw around the detected object within the image.
                    var x = left;
                    var y = top;
                    var width = Math.Abs(right - left);
                    var height = Math.Abs(top - bottom);

                    // get the predicted label from the array of labels.
                    var label = _labels[prediction.PredictedLabels[i]];

                    var score = prediction.Scores[i].ToString("P2");

                    using (var g = Graphics.FromImage(frame))
                    {
                        g.DrawRectangle(new Pen(Color.NavajoWhite, 6), x, y, width, height);
                        g.DrawString(label, new System.Drawing.Font(FontFamily.Families[0], 18f), Brushes.NavajoWhite, x + 5, y + 5);
                        g.DrawString(score, new System.Drawing.Font(FontFamily.Families[0], 18f), Brushes.NavajoWhite, x + 5, y + 25);
                    }
                }

                UpdateImageSafe(frame);

                _runningInspection = false;

            }

        }

        private void chkCamera_CheckedChanged(object sender, EventArgs e)
        {

            if (chkCamera.Checked)
                _camera.StartCamera();
            else
                _camera.StopCamera();
        }

        private void chkEnableInspection_CheckedChanged(object sender, EventArgs e)
        {
            _enableInspection = chkEnableInspection.Checked;
        }
    }
}

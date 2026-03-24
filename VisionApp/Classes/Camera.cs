using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static Google.Protobuf.Reflection.ExtensionRangeOptions.Types;

namespace VisionApp.Classes
{
    public class Camera
    {
        public event EventHandler<FrameEventAgrs> OnNewFrame;

        private VideoCaptureDevice _videoSource;  // Declare a variable to hold the video source

        private int frameCount = 0; // Declare a variable to count the number of frames captured    


        private string CameraToUse = "Azure Kinect 4K Camera";

        public Camera()
        {
            InitializeCamera(); // Call the method to initialize the camera when the Camera class is instantiated
        }

        private void InitializeCamera()
        {
            var videoDeviceMoniker = GetVideoDeviceMoniker(); // Get the moniker string for the desired video device
           
            if (!string.IsNullOrEmpty(videoDeviceMoniker))
            {
                _videoSource = new VideoCaptureDevice(videoDeviceMoniker); // Create a new video source using the moniker string
                _videoSource.NewFrame += video_NewFrame; // Subscribe to the NewFrame event to receive frames from the video source
            }
            else
            {
                MessageBox.Show($"Camera '{CameraToUse}' not found. Please check the camera name and try again.");
            }
        }


        public static string GetVideoDeviceMoniker()
        {
            var MonikerString = string.Empty;

            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo fi in devices)
            {
                if (fi.Name == Constants.PrimaryCamera)
                    MonikerString = fi.MonikerString;
            }

            return MonikerString;
        }

        protected virtual void OnFrameCaptured(FrameEventAgrs e)
        {
            OnNewFrame?.Invoke(this, e);
        }


        protected virtual void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // This method will be called every time a new frame is captured by the video source
            // You can add code here to process the frame, such as displaying it in a PictureBox or performing image analysis

            try
            {
                var frame = (Bitmap)eventArgs.Frame.Clone(); // Clone the frame to avoid issues with the original frame being modified by the video source

                var e = new FrameEventAgrs()
                {
                    CameraName = CameraToUse,
                    ImageDate = DateTime.Now,
                    FrameImage = frame
                };

                OnFrameCaptured(e); // Raise the event to notify subscribers that a new frame has been captured


                //frameCount++; // Increment the frame count
                //frame.Save($"frame-{frameCount}.jpg"); // Save the frame as an image file (for demonstration purposes)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing frame: {ex.Message}");
            }

        }


        public void StartCamera()
        {
            _videoSource.Start(); // Start the video source to begin capturing frames
        }


        public void StopCamera()
        {
            if (_videoSource.IsRunning)
            {
                _videoSource.SignalToStop(); // Signal the video source to stop capturing frames
                _videoSource.WaitForStop(); // Wait for the video source to stop before proceeding
            }

        }
    }
}

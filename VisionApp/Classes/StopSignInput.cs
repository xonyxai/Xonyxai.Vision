using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace VisionApp.Classes
{
    public struct ImageSettings
    {
        public const int imageHeight = 320;
        public const int imageWidth = 320;
    }


    public class StopSignInput
    {
      
        [ImageType(ImageSettings.imageHeight, ImageSettings.imageWidth)]
        public MLImage Image { get; set; }
    }
}

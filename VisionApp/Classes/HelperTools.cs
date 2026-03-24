using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace VisionApp.Classes
{
    public static class HelperTools
    {
        public static Bitmap ToBitmap(MLImage mlImage)
        {

            var width = mlImage.Width;
            var height = mlImage.Height;
            var pixels = mlImage.Pixels; // Get pixel data
                                                 // Create Bitmap from raw pixel bytes
            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, width, height);
            var bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(pixels.ToArray(), 0, bmpData.Scan0, pixels.Length);
            bitmap.UnlockBits(bmpData);

            return bitmap;
        }

        public static MemoryStream BitmapToStream(Bitmap bmp, ImageFormat format = null)
        {
            var ms = new MemoryStream();
            bmp.Save(ms, format ?? ImageFormat.Png);
            ms.Position = 0;
            return ms;
        }


    }
}

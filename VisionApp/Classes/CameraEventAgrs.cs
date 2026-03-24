using System;
using System.Collections.Generic;
using System.Text;

namespace VisionApp.Classes
{
    public class FrameEventAgrs:EventArgs
    {
      

        public string CameraName { get; set; }
        public DateTime ImageDate { get; set; }
        public Bitmap FrameImage { get; set; }


        public FrameEventAgrs() 
        { 
           
        }
    }
}

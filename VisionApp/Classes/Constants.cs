using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace VisionApp.Classes
{
    public static class Constants
    {
        // Read app setting

        public static string? ModelPath = ConfigurationManager.AppSettings["ModelPath"];
        public static string? ModelName = ConfigurationManager.AppSettings["ModelName"];

        public static string? PrimaryCamera = ConfigurationManager.AppSettings["PrimaryCamera"];
    }

}

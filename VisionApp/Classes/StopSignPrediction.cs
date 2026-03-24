using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisionApp.Classes
{
    public class StopSignPrediction
    {
        [ColumnName("detected_classes")]
        public long[] PredictedLabels { get; set; }

        [ColumnName("detected_boxes")]
        public float[] BoundingBoxes { get; set; }

        [ColumnName("detected_scores")]
        public float[] Scores { get; set; }
    }
}

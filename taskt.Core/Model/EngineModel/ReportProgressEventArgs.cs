using System;
using System.Drawing;

namespace taskt.Core.Model.EngineModel
{
    public class ReportProgressEventArgs : EventArgs
    {
        public string ProgressUpdate { get; set; }
        public Color LoggerColor { get; set; }
    }
}

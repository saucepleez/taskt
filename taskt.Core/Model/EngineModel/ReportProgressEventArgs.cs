using System;

namespace taskt.Core.Model.EngineModel
{
    public class ReportProgressEventArgs : EventArgs
    {
        public string ProgressUpdate { get; set; }
    }
}

using System;

namespace taskt.Core.Automation.Engine
{
    public class ReportProgressEventArgs : EventArgs
    {
        public string ProgressUpdate { get; set; }
    }
}

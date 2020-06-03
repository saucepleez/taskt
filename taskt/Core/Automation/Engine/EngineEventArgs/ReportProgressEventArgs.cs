using System;

namespace taskt.Core.Automation.Engine.EngineEventArgs
{
    public class ReportProgressEventArgs : EventArgs
    {
        public string ProgressUpdate { get; set; }
    }
}

using System;

namespace taskt.Core.Automation.Engine
{
    /// <summary>
    /// script finished event args
    /// </summary>
    public class ScriptFinishedEventArgs : EventArgs
    {
        public DateTime LoggedOn { get; set; }
        public ScriptFinishedResult Result { get; set; }
        public string Error { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public string FileName { get; set; }
        public enum ScriptFinishedResult
        {
            Successful,
            Error,
            Cancelled
        }
    }
}

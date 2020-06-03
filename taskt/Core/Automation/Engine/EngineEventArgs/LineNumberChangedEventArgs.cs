using System;

namespace taskt.Core.Automation.Engine.EngineEventArgs
{
    public class LineNumberChangedEventArgs : EventArgs
    {
        public int CurrentLineNumber { get; set; }
    }
}

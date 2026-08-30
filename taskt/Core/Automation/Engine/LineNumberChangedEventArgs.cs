using System;

namespace taskt.Core.Automation.Engine
{
    /// <summary>
    /// execute command line number changed event args
    /// </summary>
    public class LineNumberChangedEventArgs : EventArgs
    {
        /// <summary>
        /// current line number
        /// </summary>
        public int CurrentLineNumber { get; set; }
    }
}

using System;

namespace taskt.Core.Model.EngineModel
{
    public class LineNumberChangedEventArgs : EventArgs
    {
        public int CurrentLineNumber { get; set; }
    }
}

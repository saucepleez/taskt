using System;
using System.Windows;

namespace taskt.Core.Automation.User32
{
    public class MouseCoordinateEventArgs : EventArgs
    {
        public Point MouseCoordinates { get; set; }
    }
}

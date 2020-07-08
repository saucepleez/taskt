using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using taskt.Core;
using taskt.Properties;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public class UIElement
    {
        public string AutomationID { get; set; }
        public string ControlName { get; set; }
        public string ControlType { get; set; }
    }
}

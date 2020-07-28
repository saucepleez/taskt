using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public class UITreeView : TreeView
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
        public UITreeView()
        {
            DoubleBuffered = true;
            SetWindowTheme(Handle, "explorer", null);
        }
    }
}

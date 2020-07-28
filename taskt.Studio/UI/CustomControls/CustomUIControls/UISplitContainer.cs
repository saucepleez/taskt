using System.Reflection;
using System.Windows.Forms;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public class UISplitContainer : SplitContainer
    {
        public UISplitContainer()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            MethodInfo mi = typeof(Control).GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] args = new object[] { ControlStyles.OptimizedDoubleBuffer, true };
            mi.Invoke(Panel1, args);
            mi.Invoke(Panel2, args);
        }
    }
}

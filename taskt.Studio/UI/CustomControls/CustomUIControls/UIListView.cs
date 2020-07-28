using System.Windows.Forms;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public class UIListView : ListView
    {
        public UIListView()
        {
            DoubleBuffered = true;
        }

        public event ScrollEventHandler Scroll;
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler handler = Scroll;
            if (handler != null)
                handler(this, e);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x115)
            { // Trap WM_VSCROLL
                OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), 0));
            }
        }
    }
}

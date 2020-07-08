using System.Drawing;
using System.Windows.Forms;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public class UIMenuStrip : MenuStrip
    {
        public UIMenuStrip()
        {
            //Renderer = new UIMenuStripRenderer();
            var renderer = new ToolStripProfessionalRenderer(new UIMenuStripColorTable());
            renderer.RenderMenuItemBackground += Renderer_RenderMenuItemBackground;
            Renderer = renderer;
        }

        private void Renderer_RenderMenuItemBackground(object sender, ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            Color c = e.Item.Selected ? Color.FromArgb(59, 59, 59) : Color.FromArgb(30, 30, 30);
            using (SolidBrush brush = new SolidBrush(c))
                e.Graphics.FillRectangle(brush, rc);
        }
    }
}

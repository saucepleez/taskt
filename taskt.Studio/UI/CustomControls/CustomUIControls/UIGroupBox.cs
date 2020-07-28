using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public class UIGroupBox : GroupBox
    {
        public Color TitleBackColor { get; set; }
        public HatchStyle TitleHatchStyle { get; set; }
        public Font TitleFont { get; set; }
        public Color TitleForeColor { get; set; }

        public UIGroupBox()
        {
            DoubleBuffered = true;
            TitleBackColor = Color.SteelBlue;
            TitleForeColor = Color.White;
            TitleFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GroupBoxRenderer.DrawParentBackground(e.Graphics, ClientRectangle, this);
            var rect = ClientRectangle;

            SolidBrush backColorBrush = new SolidBrush(TitleBackColor);
            e.Graphics.FillRectangle(backColorBrush, 0, 0, Width, 18);
            backColorBrush.Dispose();

            TextRenderer.DrawText(e.Graphics, Text, TitleFont, new Point(2, 2), TitleForeColor);
            //ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.SteelBlue, ButtonBorderStyle.None);
        }
    }
}

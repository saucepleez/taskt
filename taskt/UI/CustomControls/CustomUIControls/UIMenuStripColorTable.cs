using System.Drawing;
using System.Windows.Forms;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public class UIMenuStripColorTable : ProfessionalColorTable
    {
        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(59, 59, 59); }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get { return Color.FromArgb(59, 59, 59); }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(59, 59, 59); }
        }

        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(59, 59, 59); }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color MenuBorder
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color ImageMarginGradientMiddle
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color ButtonSelectedHighlight
        {
            get { return Color.FromArgb(59, 59, 59); }
        }

    }
}

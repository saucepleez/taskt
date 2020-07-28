using System.Drawing;
using System.Drawing.Drawing2D;

namespace taskt.Utilities
{
    public class Theme
    {
        public Color BgGradientStartColor { get; set; } = Color.FromArgb(20, 136, 204);
        public Color BgGradientEndColor { get; set; } = Color.FromArgb(43, 50, 178);

        public LinearGradientBrush CreateGradient(Rectangle rect)
        {
            return new LinearGradientBrush(rect, BgGradientStartColor, BgGradientEndColor, 180);
        }
    }
}

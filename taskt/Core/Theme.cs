using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public class Theme
    {

        Color _BgGradientStartColor = Color.FromArgb(20, 136, 204);
        public Color BgGradientStartColor
        {
            get { return _BgGradientStartColor; }
            set { _BgGradientStartColor = value; }
        }

        Color _BgGradientEndColor = Color.FromArgb(43, 50, 178);
        public Color BgGradientEndColor
        {
            get { return _BgGradientEndColor; }
            set { _BgGradientEndColor = value; }
        }

        public LinearGradientBrush CreateGradient(Rectangle rect)
        {
            return new LinearGradientBrush(rect, _BgGradientStartColor, _BgGradientEndColor, 180);
        }

    }
}

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
       public LinearGradientBrush CreateGradient(Rectangle rect)
        {
            var tasktBlue = Color.FromArgb(20, 136, 204);
            var tasktPurple = Color.FromArgb(43, 50, 178);
            LinearGradientBrush linearGradientBrush =
            new LinearGradientBrush(rect, tasktPurple, tasktBlue, 180);
            return linearGradientBrush;

            //e.Graphics.FillRectangle(linearGradientBrush, pnlMain.ClientRectangle);

        }

    }
}

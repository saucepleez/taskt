using System.Windows.Forms;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public class UITabControl : TabControl
    {
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    DrawControl(e.Graphics);
        //}

        //internal void DrawControl(Graphics g)
        //{
        //    if (!Visible)
        //        return;

        //    Brush br = new SolidBrush(Color.Black);
        //    Brush brTab = new SolidBrush(Color.Orange);

        //    Rectangle TabControlArea = ClientRectangle;
        //    Rectangle TabArea = DisplayRectangle;

        //    g.FillRectangle(br, TabControlArea);
        //    g.FillRectangle(brTab, TabArea);

        //    br.Dispose();
        //    brTab.Dispose();

        //    //for (int i = 0; i < TabCount; i++)
        //    //    DrawTab(g, TabPages[i], i, false);

        //    //if (_mouseTabIndex != null && _mouseTabIndex != _mouseTabIndexSave && _mouseTabIndex != SelectedIndex)
        //    //    DrawTab(g, TabPages[(int)_mouseTabIndex], (int)_mouseTabIndex, true);

        //    //_mouseTabIndexSave = _mouseTabIndex;
        //}

        //internal void DrawTab(Graphics g, TabPage tabPage, int nIndex, bool mouseOverTab)
        //{
        //    //var recBounds = GetTabRect(nIndex);

        //    //SetBounds(ref recBounds);
        //    //var pt = SetPointsForTabFill(recBounds);

        //    //DrawTabBounds(g, recBounds);

        //    //FillTabl(g, recBounds, pt, false);

        //    //DrawTabSeparators(g, recBounds, nIndex, 0 /*y-bottom*/);

        //    //if (SelectedIndex == nIndex)
        //    //{
        //    //    DrawTabGradient(g, recBounds, pt, nIndex, 0/*width*/, 1/*height*/);
        //    //    DrawTabSeparators(g, recBounds, nIndex, 1 /*y-bottom*/);
        //    //}

        //    //if (mouseOverTab)
        //    //    DrawTabGradient(g, recBounds, pt, nIndex, -2/*width*/, 0/*height*/);

        //    //DrawText(g, recBounds, tabPage.Text);
        //}

        //private void DrawText(Graphics g, Rectangle recBounds, string text)
        //{
        //    var strFormat = new StringFormat();
        //    strFormat.Alignment = strFormat.LineAlignment = StringAlignment.Center;

        //    g.TextRenderingHint =
        //        System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        //    //var fnt = new Font(MsFonts.familyPTSans, 8F, FontStyle.Regular,  GraphicsUnit.Point, (byte)204);
        //    var fnt = new Font("Arial", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));

        //    RectangleF tabTextArea = recBounds;
        //    var br = new SolidBrush(Color.Black);
        //    g.DrawString(text, fnt, br, tabTextArea);

        //    br.Dispose();
        //    strFormat.Dispose();
        //    fnt.Dispose();
        //}
    }
}

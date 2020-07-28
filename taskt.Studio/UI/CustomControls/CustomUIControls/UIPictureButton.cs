using System;
using System.Drawing;
using System.Windows.Forms;
using taskt.Properties;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public partial class UIPictureButton : PictureBox
    {
        private bool _isMouseOver;
        public bool IsMouseOver
        {
            get
            {
                return _isMouseOver;
            }
            set
            {
                _isMouseOver = value;
                Invalidate();
            }
        }

        private string _displayText;
        public string DisplayText
        {
            get
            {
                return _displayText;
            }
            set
            {
                _displayText = value;
                Invalidate();
            }
        }

        private Color _displayTextBrush;
        public Color DisplayTextBrush
        {
            get
            {
                return _displayTextBrush;
            }
            set
            {
                _displayTextBrush = value;
                Invalidate();
            }
        }

        public UIPictureButton()
        {
            Image = Resources.logo;
            DisplayTextBrush = Color.White;
            Size = new Size(48, 48);
            DisplayText = "Text";
            Font = new Font("Segoe UI", 8, FontStyle.Bold);
            MouseEnter += UIPictureButton_MouseEnter;
            MouseLeave += UIPictureButton1_MouseLeave;
        }

        public override string Text
        {
            get
            {
                return _displayText;
            }
            set
            {
                _displayText = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (IsMouseOver)
            {
                Cursor = Cursors.Hand;
                BackColor = Color.Transparent;
            }
            else
            {
                Cursor = Cursors.Arrow;
                BackColor = Color.Transparent;
            }

            if (Image != null)
                e.Graphics.DrawImage(Image, (Width / 2) - 16, 3, 32, 32);

            if (DisplayText != null)
            {
                var stringSize = e.Graphics.MeasureString(DisplayText, new Font("Segoe UI Bold", 8, FontStyle.Bold), 200);
                e.Graphics.DrawString(DisplayText, new Font("Segoe UI", 8, FontStyle.Bold),
                                      new SolidBrush(DisplayTextBrush), ((Width / 2) - (stringSize.Width / 2)), Height - 14);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }

        private void UIPictureButton_MouseEnter(object sender, EventArgs e)
        {
            IsMouseOver = true;
        }

        private void UIPictureButton1_MouseLeave(object sender, EventArgs e)
        {
            IsMouseOver = false;
        }
    }
}

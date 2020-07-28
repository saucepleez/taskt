using System;
using System.Drawing;
using System.Windows.Forms;
using taskt.Utilities;

namespace taskt.UI.Forms
{
    public partial class ThemedForm : Form
    {
        public Theme Theme { get; set; } = new Theme();

        public ThemedForm()
        {
            InitializeComponent();
        }

        private void ThemedForm_Load(object sender, EventArgs e)
        {

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // Resizes if too tall to fit
            if (ClientSize.Height > Screen.PrimaryScreen.WorkingArea.Height - CurrentAutoScaleDimensions.Height)
            {
                int width = ClientSize.Width;
                int height = Screen.PrimaryScreen.WorkingArea.Height - (int)CurrentAutoScaleDimensions.Height;

                ClientSize = new Size(width, height);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (ClientRectangle.Width != 0 && ClientRectangle.Height != 0)
                e.Graphics.FillRectangle(Theme.CreateGradient(ClientRectangle), ClientRectangle);
            base.OnPaint(e);
        }

        public static void MoveFormToBottomRight(Form sender)
        {
            int x = Screen.FromPoint(sender.Location).WorkingArea.Right - sender.Width;
            int y = Screen.FromPoint(sender.Location).WorkingArea.Bottom - sender.Height;

            sender.Location = new Point(x, y);
        }
    }
}

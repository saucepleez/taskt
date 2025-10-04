using System;
using System.Drawing;
using System.Windows.Forms;

namespace taskt.UI.Forms
{
    /// <summary>
    /// taskt theme colored form
    /// </summary>
    public partial class ThemedForm : Form
    {
        private Core.Theme _Theme = new Core.Theme();
        public Core.Theme Theme
        {
            get { return _Theme; }
            set { _Theme = value; }
        }

        public ThemedForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.ClientSize.Height > Screen.PrimaryScreen.WorkingArea.Height - this.CurrentAutoScaleDimensions.Height) // Resizes if too tall to fit
            {
                this.ClientSize = new Size(this.ClientSize.Width, Screen.PrimaryScreen.WorkingArea.Height - (int)this.CurrentAutoScaleDimensions.Height);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                e.Graphics.FillRectangle(this.Theme.CreateGradient(this.ClientRectangle), this.ClientRectangle);
                base.OnPaint(e);
            }
        }
    }
}

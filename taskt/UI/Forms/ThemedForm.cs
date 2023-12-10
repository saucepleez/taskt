using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms
{
    public partial class ThemedForm : Form
    {
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
            if (this.ClientSize.Height > Screen.PrimaryScreen.WorkingArea.Height - this.CurrentAutoScaleDimensions.Height) // Resizes if too tall to fit
            {
                this.ClientSize = new Size(this.ClientSize.Width, Screen.PrimaryScreen.WorkingArea.Height - (int)this.CurrentAutoScaleDimensions.Height);
            }
        }

        private taskt.Core.Theme _Theme = new taskt.Core.Theme();
        public taskt.Core.Theme Theme
        {
            get { return _Theme; }
            set { _Theme = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(this.Theme.CreateGradient(this.ClientRectangle), this.ClientRectangle);
            base.OnPaint(e);
        }
        public static void MoveFormToBottomRight(Form sender)
        {
            sender.Location = new Point(Screen.FromPoint(sender.Location).WorkingArea.Right - sender.Width, Screen.FromPoint(sender.Location).WorkingArea.Bottom - sender.Height);
        }
    }
}

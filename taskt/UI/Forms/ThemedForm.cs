using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new Core.Theme().CreateGradient(this.ClientRectangle), this.ClientRectangle);
            base.OnPaint(e);
        }
        public static void MoveFormToBottomRight(Form sender)
        {
            sender.Location = new Point(Screen.FromPoint(sender.Location).WorkingArea.Right - sender.Width, Screen.FromPoint(sender.Location).WorkingArea.Bottom - sender.Height);
        }
    }
}

using System;
using System.Reflection;
using System.Windows.Forms;
using UIAutomationTester.Properties;

namespace UIAutomationTester
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        #region Load
        private void frmMain_Load(object sender, EventArgs e)
        {
            var asm = Assembly.GetExecutingAssembly();
            this.Text += " v" + asm.GetName().Version;

            cmbListViewView.SelectedIndex = 1;

            dataGridView1.Rows.Add(new string[] { "A1", "B1", "C1" });
            dataGridView1.Rows.Add(new string[] { "A2", "B2", "C2" });
            dataGridView1.Rows.Add(new string[] { "A3", "B3", "C3" });

            pictureBox1.Image = Properties.Resources.taskt_logo_alt;
        }
        #endregion

        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "button1 Clicked";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "button2 Clicked";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "button3 Clicked";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "button4 Clicked";
        }
        #endregion

        #region listView1
        private void cmbListViewView_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.View = (View)cmbListViewView.SelectedIndex;
        }
        #endregion
    }
}

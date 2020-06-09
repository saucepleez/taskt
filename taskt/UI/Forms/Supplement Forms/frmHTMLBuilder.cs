using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class frmHTMLBuilder : Form
    {
        public frmHTMLBuilder()
        {
            InitializeComponent();
        }

        private void uiBtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void rtbHTML_TextChanged(object sender, EventArgs e)
        {
            webBrowserHTML.ScriptErrorsSuppressed = true;
            webBrowserHTML.DocumentText = rtbHTML.Text;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplemental
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
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void rtbHTML_TextChanged(object sender, EventArgs e)
        {
            webBrowserHTML.ScriptErrorsSuppressed = true;
            webBrowserHTML.DocumentText = rtbHTML.Text;
        }
    }
}

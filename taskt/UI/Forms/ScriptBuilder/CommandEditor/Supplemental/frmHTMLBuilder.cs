using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class frmHTMLBuilder : Form
    {
        public frmHTMLBuilder()
        {
            InitializeComponent();
            this.FormClosed += SupplementFormsEvents.SupplementFormClosed;
        }

        private void frmHTMLBuilder_Load(object sender, EventArgs e)
        {
            SupplementFormsEvents.SupplementFormLoad(this);
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

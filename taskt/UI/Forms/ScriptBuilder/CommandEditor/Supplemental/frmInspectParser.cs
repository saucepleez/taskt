using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    public partial class frmInspectParser : ThemedForm
    {
        public frmInspectParser()
        {
            InitializeComponent();
            this.FormClosed += SupplementFormsEvents.SupplementFormClosed;
        }

        private void frmInspectParser_Load(object sender, EventArgs e)
        {
            SupplementFormsEvents.SupplementFormLoad(this);
        }

        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public string inspectResult
        {
            get
            {
                return this.txtInspectResult.Text.Trim();
            }
        }

    }
}

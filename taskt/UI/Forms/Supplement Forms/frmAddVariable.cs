using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmAddVariable : ThemedForm
    {
        public frmAddVariable()
        {
            InitializeComponent();
        }

        public frmAddVariable(string VariableName, string variableValue)
        {
            InitializeComponent();
            Text = "edit variable";
            lblHeader.Text = "edit variable";
            txtVariableName.Text = VariableName;
            txtDefaultValue.Text = variableValue;
        }

        private void frmAddVariable_Load(object sender, EventArgs e)
        {

        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (txtVariableName.Text.Trim() == string.Empty)
            {
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}

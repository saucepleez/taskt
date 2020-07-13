using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmInputBox : ThemedForm
    {
        public frmInputBox(string prompt, string title)
        {
            InitializeComponent();
            Text = title;
            lblInput.Text = prompt;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtInput.Text.Trim() == string.Empty)
                return;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}

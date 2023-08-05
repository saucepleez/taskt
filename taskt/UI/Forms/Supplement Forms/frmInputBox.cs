using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmInputBox : Form
    {
        public frmInputBox()
        {
            InitializeComponent();
        }

        public frmInputBox(string title = "Input Form", string message = "Please input", string defaultValue = "") : this()
        {
            this.Text = title;
            lblMessage.Text = message;
            txtInputValue.Text = defaultValue;
        }

        private void frmInputBox_Load(object sender, EventArgs e)
        {
            txtInputValue.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public string InputValue
        {
            get
            {
                return this.txtInputValue.Text;
            }
        }
    }
}

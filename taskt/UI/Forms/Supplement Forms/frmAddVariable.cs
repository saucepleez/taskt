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
    public partial class frmAddVariable : ThemedForm
    {
        public Core.ApplicationSettings appSettings;

        public frmAddVariable()
        {
            InitializeComponent();
        }

        public frmAddVariable(string VariableName, string variableValue)
        {
            InitializeComponent();
            this.Text = "edit variable";
            lblHeader.Text = "edit variable";
            txtVariableName.Text = VariableName;
            txtDefaultValue.Text = variableValue;
        }

        private void frmAddVariable_Load(object sender, EventArgs e)
        {
            lblDefineNameDescription.Text = lblDefineNameDescription.Tag.ToString().Replace("{{{", appSettings.EngineSettings.VariableStartMarker)
                    .Replace("}}}", appSettings.EngineSettings.VariableEndMarker);
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (txtVariableName.Text.Trim() == string.Empty)
            {
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}

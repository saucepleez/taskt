using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Script;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmAddVariable : ThemedForm
    {
        public List<ScriptVariable> ScriptVariables { get; set; }
        private bool _isEditMode;
        private string _editingVariableName;
        public frmAddVariable()
        {
            InitializeComponent();
        }

        public frmAddVariable(string variableName, string variableValue)
        {
            InitializeComponent();
            Text = "edit variable";
            lblHeader.Text = "edit variable";
            txtVariableName.Text = variableName;
            txtDefaultValue.Text = variableValue;

            _isEditMode = true;
            _editingVariableName = variableName;
        }

        private void frmAddVariable_Load(object sender, EventArgs e)
        {

        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            txtVariableName.Text = txtVariableName.Text.Trim();
            if (txtVariableName.Text == string.Empty)
            {
                lblVariableNameError.Text = "Variable Name not provided";
                return;
            }

            var existingVariable = ScriptVariables.Where(var => var.VariableName == txtVariableName.Text).FirstOrDefault();
            if (existingVariable != null)
            {
                if (!_isEditMode || existingVariable.VariableName != _editingVariableName)
                {
                    lblVariableNameError.Text = "A Variable with this name already exists";
                    return;
                }
            }

            if (txtVariableName.Text.StartsWith("{") && txtVariableName.Text.EndsWith("}"))
            {
                lblVariableNameError.Text = "Variable markers '{' and '}' should not be included";
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

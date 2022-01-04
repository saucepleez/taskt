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
        private Core.ApplicationSettings appSettings;
        public frmAddVariablesEditMode editMode { get; }
        public string VariableName { get; private set; }
        public string VariableValue { get; private set; }

        public enum frmAddVariablesEditMode
        {
            Add,
            Edit
        }

        public frmAddVariable(Core.ApplicationSettings appSettings)
        {
            InitializeComponent();
            this.editMode = frmAddVariablesEditMode.Add;
            this.appSettings = appSettings;
        }

        public frmAddVariable(string VariableName, string variableValue, Core.ApplicationSettings appSettings)
        {
            InitializeComponent();
            this.Text = "edit variable";
            lblHeader.Text = "edit variable";
            txtVariableName.Text = VariableName;
            txtDefaultValue.Text = variableValue;
            this.editMode = frmAddVariablesEditMode.Edit;
            this.appSettings = appSettings;
        }

        private void frmAddVariable_Load(object sender, EventArgs e)
        {
            //lblDefineNameDescription.Text = lblDefineNameDescription.Tag.ToString().Replace("{{{", appSettings.EngineSettings.VariableStartMarker)
            //        .Replace("}}}", appSettings.EngineSettings.VariableEndMarker);
            lblDefineNameDescription.Text = appSettings.replaceApplicationKeyword(lblDefineNameDescription.Tag.ToString());
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (txtVariableName.Text.Trim() == string.Empty)
            {
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.VariableName = txtVariableName.Text;
            this.VariableValue = txtDefaultValue.Text;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmAddVariable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Script;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmAddElement : ThemedForm
    {
        public Dictionary<ScriptElementType, string> ElementValueDict { get; set; }
        public List<ScriptElement> ScriptElements { get; set; }
        private bool _isEditMode;
        private string _editingVariableName;
        public frmAddElement()
        {
            InitializeComponent();
            cbxElementType.DataSource = Enum.GetValues(typeof(ScriptElementType)).Descriptions(); 
        }

        public frmAddElement(string elementName, ScriptElementType elementType, string elementValue)
        {
            InitializeComponent();
            cbxElementType.DataSource = Enum.GetValues(typeof(ScriptElementType)).Descriptions();
            Text = "edit element";
            lblHeader.Text = "edit element";
            txtElementName.Text = elementName;
            cbxElementType.SelectedIndex = cbxElementType.Items.IndexOf(elementType.Description());
            txtDefaultValue.Text = elementValue;

            _isEditMode = true;
            _editingVariableName = elementName;
        }

        private void frmAddElement_Load(object sender, EventArgs e)
        {
            cbxElementType_SelectedIndexChanged(null, null);
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            txtElementName.Text = txtElementName.Text.Trim();
            if (txtElementName.Text == string.Empty)
            {
                lblElementNameError.Text = "Element Name not provided"; 
                return;
            }

            var existingElement = ScriptElements.Where(var => var.ElementName == txtElementName.Text).FirstOrDefault();
            if (existingElement != null)                
            {
                if (!_isEditMode || existingElement.ElementName != _editingVariableName)
                {
                    lblElementNameError.Text = "An Element with this name already exists";
                    return;
                }               
            }

            if (txtElementName.Text.StartsWith("<") && txtElementName.Text.EndsWith(">"))
            {
                lblElementNameError.Text = "Element markers '<' and '>' should not be included";
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void cbxElementType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ElementValueDict != null)
            {
                ComboBox typeBox = cbxElementType;
                ScriptElementType selectedType = (ScriptElementType)Enum.Parse(typeof(ScriptElementType), typeBox.SelectedItem.ToString().Replace(" ",""));
                string elementValue = ElementValueDict[selectedType];
                txtDefaultValue.Text = elementValue;
            }
        }
    }
}

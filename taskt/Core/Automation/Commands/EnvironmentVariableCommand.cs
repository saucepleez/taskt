using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("System Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to exclusively select a system/environment variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to exclusively retrieve a system variable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class EnvironmentVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select the required environment variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select from one of the options")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_EnvVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive output")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }


        [XmlIgnore]
        [NonSerialized]
        public ComboBox VariableNameComboBox;

        [XmlIgnore]
        [NonSerialized]
        public Label VariableValue;
        public EnvironmentVariableCommand()
        {
            this.CommandName = "EnvironmentVariableCommand";
            this.SelectionName = "Environment Variable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var environmentVariable = (string)v_EnvVariableName.ConvertToUserVariable(sender);

            var variables = Environment.GetEnvironmentVariables();
            var envValue = (string)variables[environmentVariable];

            envValue.StoreInUserVariable(sender, v_applyToVariableName);


        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ActionNameComboBoxLabel = CommandControls.CreateDefaultLabelFor("v_EnvVariableName", this);
            VariableNameComboBox = (ComboBox)CommandControls.CreateDropdownFor("v_EnvVariableName", this);


            foreach (System.Collections.DictionaryEntry env in Environment.GetEnvironmentVariables())
            {
                var envVariableKey = env.Key.ToString();
                var envVariableValue = env.Value.ToString();
                VariableNameComboBox.Items.Add(envVariableKey);
            }


            VariableNameComboBox.SelectedValueChanged += VariableNameComboBox_SelectedValueChanged;



            RenderedControls.Add(ActionNameComboBoxLabel);
            RenderedControls.Add(VariableNameComboBox);

            VariableValue = new Label();
            VariableValue.Font = new System.Drawing.Font("Segoe UI", 12);
            VariableValue.ForeColor = System.Drawing.Color.White;

            RenderedControls.Add(VariableValue);


            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_applyToVariableName", this, editor));
            

            return RenderedControls;
        }

        private void VariableNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var selectedValue = VariableNameComboBox.SelectedItem;

            if (selectedValue == null)
                return;


            var variable = Environment.GetEnvironmentVariables();
            var value = variable[selectedValue];

            VariableValue.Text = "[ex. " + value + "]";


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply '" + v_EnvVariableName + "' to Variable '" + v_applyToVariableName + "']";
        }
    }

}
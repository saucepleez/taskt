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
    public class OSVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select the required system variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select from one of the options")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OSVariableName { get; set; }

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
        public OSVariableCommand()
        {
            this.CommandName = "OSVariableCommand";
            this.SelectionName = "OS Variable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var systemVariable = (string)v_OSVariableName.ConvertToUserVariable(sender);

            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                foreach (PropertyData prop in result.Properties)
                {
                    if (prop.Name == systemVariable.ToString())
                    {
                        var sysValue = prop.Value.ToString();
                        sysValue.StoreInUserVariable(sender, v_applyToVariableName);
                        return;
                    }
                }

            }

            throw new Exception("System Property '" + systemVariable + "' not found!");


        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ActionNameComboBoxLabel = CommandControls.CreateDefaultLabelFor("v_OSVariableName", this);
            VariableNameComboBox = (ComboBox)CommandControls.CreateDropdownFor("v_OSVariableName", this);


            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                foreach (PropertyData prop in result.Properties)
                {
                    VariableNameComboBox.Items.Add(prop.Name);
                }

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


            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                foreach (PropertyData prop in result.Properties)
                {
                    if (prop.Name == selectedValue.ToString())
                    {
                        VariableValue.Text = "[ex. " + prop.Value + "]";
                        return;
                    }
                }

            }

            VariableValue.Text = "[ex. **Item not found**]";

          
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply '" + v_OSVariableName + "' to Variable '" + v_applyToVariableName + "']";
        }
    }


}
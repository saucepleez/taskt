using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command allows you to exclusively select a system/environment variable")]
    [UsesDescription("Use this command to exclusively retrieve a system variable")]
    [ImplementationDescription("")]
    public class OSVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Select the required system variable")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select from one of the options")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_OSVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive output")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox VariableNameComboBox;

        [XmlIgnore]
        [NonSerialized]
        public Label VariableValue;
        public OSVariableCommand()
        {
            CommandName = "OSVariableCommand";
            SelectionName = "OS Variable";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var systemVariable = (string)v_OSVariableName.ConvertToUserVariable(engine);

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
                        sysValue.StoreInUserVariable(engine, v_applyToVariableName);
                        return;
                    }
                }

            }

            throw new Exception("System Property '" + systemVariable + "' not found!");


        }
        public override List<Control> Render(IfrmCommandEditor editor)
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
using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("System Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get OS Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to exclusively select a system/environment variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to exclusively retrieve a system variable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetOSVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("System Variable")]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        [PropertySelectionChangeEvent(nameof(cmbVariableNameComboBox_SelectedValueChanged))]
        [PropertyComboBoxItemMethod(nameof(CreateOSVariableList))]
        [PropertySecondaryLabel(true)]
        [SampleUsage("**BootDevide** or **Name**")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_OSVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private Dictionary<string, string> osVariables = null;

        public GetOSVariableCommand()
        {
            //this.CommandName = "OSVariableCommand";
            //this.SelectionName = "OS Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var systemVariable = v_OSVariableName.ConvertToUserVariable(sender);

            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                foreach (PropertyData prop in result.Properties)
                {
                    if (prop.Name == systemVariable)
                    {
                        var sysValue = prop.Value?.ToString() ?? "";
                        sysValue.StoreInUserVariable(sender, v_applyToVariableName);
                        return;
                    }
                }

            }

            throw new Exception("System Property '" + systemVariable + "' not found!");
        }

        private void cmbVariableNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string selectedValue = ((ComboBox)sender).SelectedItem?.ToString() ?? "";

            Label lbl = (Label)ControlsList["lbl2_" + nameof(v_OSVariableName)];

            if (osVariables?.Keys.Contains(selectedValue) ?? false)
            {
                var v = osVariables[selectedValue];
                if (v.Length > 16)
                {
                    v = v.Substring(0, 16) + "...";
                }

                lbl.Text = "On your computer, the Value of " + selectedValue + " is '" + v + "'";
            }
            else
            {
                lbl.Text = "";
            }
        }

        /// <summary>
        /// create os variable list and result examples
        /// </summary>
        /// <returns></returns>
        private List<string> CreateOSVariableList()
        {
            var ret = new List<string>();

            osVariables = new Dictionary<string, string>();

            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();
            foreach (ManagementObject result in results)
            {
                foreach (PropertyData prop in result.Properties)
                {
                    ret.Add(prop.Name);
                    osVariables.Add(prop.Name, prop.Value?.ToString() ?? "");
                }
            }

            return ret;
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    // TODO: support new attribute for combobox
        //    osVariables = new Dictionary<string, string>();

        //    ComboBox cmb = (ComboBox)ControlsList[nameof(v_OSVariableName)];

        //    ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
        //    ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
        //    ManagementObjectCollection results = searcher.Get();
        //    cmb.BeginUpdate();
        //    foreach (ManagementObject result in results)
        //    {
        //        foreach (PropertyData prop in result.Properties)
        //        {
        //            cmb.Items.Add(prop.Name);
        //            osVariables.Add(prop.Name, prop.Value?.ToString() ?? "");
        //        }
        //    }
        //    cmb.EndUpdate();

        //    return RenderedControls;
        //}
    }
}
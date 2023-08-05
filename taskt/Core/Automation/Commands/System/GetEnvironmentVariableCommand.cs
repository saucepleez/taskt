using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("System Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Environment Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to exclusively select a system/environment variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to exclusively retrieve a system variable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetEnvironmentVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Environment Variable")]
        [PropertySecondaryLabel(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        [PropertySelectionChangeEvent(nameof(cmbVariableNameComboBox_SelectedValueChanged))]
        [PropertyComboBoxItemMethod(nameof(CreateEnviromnentVariablesList))]
        [SampleUsage("**HOME_PATH** or **TMP** or **PATH**")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_EnvVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private Dictionary<string, string> envVariableValues = null;

        public GetEnvironmentVariableCommand()
        {
            //this.CommandName = "EnvironmentVariableCommand";
            //this.SelectionName = "Environment Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var variables = Environment.GetEnvironmentVariables();

            var environmentVariable = v_EnvVariableName.ConvertToUserVariable(sender);
            var keys = variables.Keys.Cast<string>().ToList();
            if (keys.Contains(environmentVariable))
            {
                variables[environmentVariable].ToString().StoreInUserVariable(engine, v_applyToVariableName);
            }
            else
            {
                throw new Exception("Environment Variable '" + environmentVariable + "' does not Exists.");
            }
        }

        private void cmbVariableNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string envValueName = ((ComboBox)sender).SelectedItem?.ToString() ?? "";

            Label lbl = (Label)ControlsList["lbl2_" + nameof(v_EnvVariableName)];

            if (envVariableValues?.Keys.Contains(envValueName) ?? false)
            {
                var v = envVariableValues[envValueName];
                if (v.Length > 16)
                {
                    v = v.Substring(0, 16) + "...";
                }

                lbl.Text = "On your computer, the Value of " + envValueName + " is '" + v + "'";
            }
            else
            {
                lbl.Text = "";
            }
        }

        /// <summary>
        /// create environment variable list and result examples
        /// </summary>
        /// <returns></returns>
        private List<string> CreateEnviromnentVariablesList()
        {
            var ret = new List<string>();

            // dynamic get env value name & value
            envVariableValues = new Dictionary<string, string>();
            foreach (System.Collections.DictionaryEntry env in Environment.GetEnvironmentVariables())
            {
                var envVariableKey = env.Key.ToString();
                var envVariableValue = env.Value.ToString();
                envVariableValues.Add(envVariableKey, envVariableValue);
                ret.Add(envVariableKey);
            }

            return ret;
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    // dynamic get env value name & value
        //    envVariableValues = new Dictionary<string, string>();
        //    ComboBox cmb = (ComboBox)ControlsList[nameof(v_EnvVariableName)];
        //    cmb.BeginUpdate();
        //    foreach (System.Collections.DictionaryEntry env in Environment.GetEnvironmentVariables())
        //    {
        //        var envVariableKey = env.Key.ToString();
        //        var envVariableValue = env.Value.ToString();
        //        cmb.Items.Add(envVariableKey);

        //        envVariableValues.Add(envVariableKey, envVariableValue);
        //    }
        //    cmb.EndUpdate();

        //    return RenderedControls;
        //}
    }

}
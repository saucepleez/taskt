using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to set preferences for engine behavior.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change the engine behavior.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SetEnginePreferenceCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Parameter Type")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Enable Automatic Calculations")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Disable Automatic Calculations")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Start Variable Marker")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("End Variable Marker")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Engine Delay")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Current Window Keyword")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_PreferenceType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify Parameter Value")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ParameterValue { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox PreferenceTypeCombobox;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> ElementParameterControls;

        public SetEnginePreferenceCommand()
        {
            this.CommandName = "SetEnginePreferenceCommand";
            this.SelectionName = "Set Engine Preference";
            this.CommandEnabled = true;
            this.CustomRendering = true;

        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var preference = v_PreferenceType.ConvertToUserVariable(sender);

            var parameterValue = v_ParameterValue.ConvertToUserVariable(sender);

            switch (preference)
            {
                case "Enable Automatic Calculations":
                    engine.AutoCalculateVariables = true;
                    break;
                case "Disable Automatic Calculations":
                    engine.AutoCalculateVariables = false;
                    break;

                case "Start Variable Marker":
                    engine.engineSettings.VariableStartMarker = parameterValue;
                    break;
                case "End Variable Marker":
                    engine.engineSettings.VariableEndMarker = parameterValue;
                    break;
                case "Engine Delay":
                    engine.engineSettings.DelayBetweenCommands = int.Parse(parameterValue);
                    break;
                case "Current Window Keyword":
                    engine.engineSettings.CurrentWindowKeyword = parameterValue;
                    break;

                default:
                    throw new NotImplementedException($"The preference '{preference}' is not implemented.");
            }


        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var preference = CommandControls.CreateDefaultDropdownGroupFor("v_PreferenceType", this, editor);
            RenderedControls.AddRange(preference);

            PreferenceTypeCombobox = (ComboBox)preference.Where(t => t is ComboBox).FirstOrDefault();
            PreferenceTypeCombobox.SelectedValueChanged += (sender, e) => PreferenceCombobox_SelectedChanged(sender, e);

            ElementParameterControls = CommandControls.CreateDefaultInputGroupFor("v_ParameterValue", this, editor);
            RenderedControls.AddRange(this.ElementParameterControls);

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            switch (this.v_PreferenceType)
            {
                case "Enable Automatic Calculations":
                case "Disable Automatic Calculations":
                    return base.GetDisplayValue() + $" [{v_PreferenceType}]";
                    break;

                case "Start Variable Marker":
                case "End Variable Marker":
                case "Engine Delay":
                case "Current Window Keyword":
                    return base.GetDisplayValue() + " [" + v_PreferenceType +" set '" + (String.IsNullOrEmpty(v_ParameterValue) ? "" : v_ParameterValue) + "']";
                    break;

                default:
                    return base.GetDisplayValue() + $" [{v_PreferenceType}]";
                    break;
            }
            
        }

        private void PreferenceCombobox_SelectedChanged(object sender, System.EventArgs e)
        {
            switch (PreferenceTypeCombobox.Text)
            {
                case "Enable Automatic Calculations":
                case "Disable Automatic Calculations":
                    foreach(var ctl in ElementParameterControls)
                    {
                        ctl.Visible = false;
                    }
                    break;

                default:
                    foreach (var ctl in ElementParameterControls)
                    {
                        ctl.Visible = true;
                    }
                    break;
            }
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            this.IsValid = true;
            this.validationResult = "";

            if (String.IsNullOrEmpty(v_PreferenceType))
            {
                this.validationResult += "Parameter Type is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
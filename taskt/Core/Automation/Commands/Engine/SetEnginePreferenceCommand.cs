using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.CommandSettings("Set Engine Preference")]
    [Attributes.ClassAttributes.Description("This command allows you to set preferences for engine behavior.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change the engine behavior.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class SetEnginePreferenceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Parameter Type")]
        [PropertyUISelectionOption("Enable Automatic Calculations")]
        [PropertyUISelectionOption("Disable Automatic Calculations")]
        [PropertyUISelectionOption("Start Variable Marker")]
        [PropertyUISelectionOption("End Variable Marker")]
        [PropertyUISelectionOption("Engine Delay")]
        [PropertyUISelectionOption("Current Window Keyword")]
        [PropertyValidationRule("Parameter Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Parameter Type")]
        [PropertySelectionChangeEvent(nameof(cmbPreferenceCombobox_SelectedChanged))]
        public string v_PreferenceType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Parameter Value")]
        [InputSpecification("Parameter Value", true)]
        [PropertyDisplayText(false, "")]
        public string v_ParameterValue { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private ComboBox PreferenceTypeCombobox;

        //[XmlIgnore]
        //[NonSerialized]
        //private List<Control> ElementParameterControls;

        public SetEnginePreferenceCommand()
        {
            //this.CommandName = "SetEnginePreferenceCommand";
            //this.SelectionName = "Set Engine Preference";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var preference = v_PreferenceType.ConvertToUserVariable(engine);
            var preference = this.GetUISelectionValue(nameof(v_PreferenceType), engine);

            var parameterValue = v_ParameterValue.ConvertToUserVariable(engine);

            switch (preference)
            {
                case "enable automatic calculations":
                    engine.AutoCalculateVariables = true;
                    break;
                case "disable automatic calculations":
                    engine.AutoCalculateVariables = false;
                    break;

                case "start variable marker":
                    engine.engineSettings.VariableStartMarker = parameterValue;
                    break;
                case "end variable marker":
                    engine.engineSettings.VariableEndMarker = parameterValue;
                    break;

                case "engine delay":
                    if (int.TryParse(parameterValue, out int delay))
                    {
                        if (delay >= 1)
                        {
                            engine.engineSettings.DelayBetweenCommands = delay;
                        }
                        else
                        {
                            throw new Exception("Engine Delay is Less Than or Equals zero.");
                        }
                    }
                    else
                    {
                        throw new Exception("Engine Delay is not Number. Value: '" + parameterValue + "'");
                    }
                    break;

                case "current window keyword":
                    engine.engineSettings.CurrentWindowKeyword = parameterValue;
                    break;
            }
        }

        private void cmbPreferenceCombobox_SelectedChanged(object sender, EventArgs e)
        {
            var item = ((ComboBox)sender).SelectedItem?.ToString() ?? "";
            switch (item)
            {
                case "Enable Automatic Calculations":
                case "Disable Automatic Calculations":
                    //foreach (var ctl in ElementParameterControls)
                    //{
                    //    ctl.Visible = false;
                    //}
                    GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_ParameterValue), false);
                    break;

                default:
                    //foreach (var ctl in ElementParameterControls)
                    //{
                    //    ctl.Visible = true;
                    //}
                    GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_ParameterValue), true);
                    break;
            }
        }

        public override string GetDisplayValue()
        {
            switch (this.v_PreferenceType)
            {
                case "Enable Automatic Calculations":
                case "Disable Automatic Calculations":
                    return base.GetDisplayValue() + $" [ {v_PreferenceType} ]";

                case "Start Variable Marker":
                case "End Variable Marker":
                case "Engine Delay":
                case "Current Window Keyword":
                    return base.GetDisplayValue() + " [ " + v_PreferenceType +" set '" + (String.IsNullOrEmpty(v_ParameterValue) ? "" : v_ParameterValue) + "' ]";

                default:
                    return base.GetDisplayValue() + $" [ {v_PreferenceType} ]";
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var preference = CommandControls.CreateDefaultDropdownGroupFor("v_PreferenceType", this, editor);
        //    RenderedControls.AddRange(preference);

        //    PreferenceTypeCombobox = (ComboBox)preference.Where(t => t is ComboBox).FirstOrDefault();
        //    PreferenceTypeCombobox.SelectedValueChanged += (sender, e) => PreferenceCombobox_SelectedChanged(sender, e);

        //    ElementParameterControls = CommandControls.CreateDefaultInputGroupFor("v_ParameterValue", this, editor);
        //    RenderedControls.AddRange(this.ElementParameterControls);

        //    return RenderedControls;
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    this.IsValid = true;
        //    this.validationResult = "";

        //    if (String.IsNullOrEmpty(v_PreferenceType))
        //    {
        //        this.validationResult += "Parameter Type is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        //public override void ConvertToIntermediate(EngineSettings settings, List<ScriptVariable> variables)
        //{
        //    switch (v_PreferenceType)
        //    {
        //        case "Start Variable Marker":
        //        case "End Variable Marker":
        //            // not convert
        //            break;

        //        default:
        //            if (v_ParameterValue != null)
        //            {
        //                v_ParameterValue = settings.convertToIntermediate(v_ParameterValue);
        //            }
        //            if (v_PreferenceType != null)
        //            {
        //                v_PreferenceType = settings.convertToIntermediate(v_PreferenceType);
        //            }
        //            break;
        //    }
        //}

        //public override void ConvertToRaw(EngineSettings settings)
        //{
        //    switch (v_PreferenceType)
        //    {
        //        case "Start Variable Marker":
        //        case "End Variable Marker":
        //            // not convert
        //            break;

        //        default:
        //            if (v_ParameterValue != null)
        //            {
        //                v_ParameterValue = settings.convertToRaw(v_ParameterValue);
        //            }
        //            if (v_PreferenceType != null)
        //            {
        //                v_PreferenceType = settings.convertToRaw(v_PreferenceType);
        //            }
        //            break;
        //    }
        //}
    }
}
using System;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Modify Text")]
    [Attributes.ClassAttributes.Description("This command allows you to trim Text, convert Text, etc.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to trim Text, convert Text, etc.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ModifyTextCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Supply the Text or Variable to modify")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Select or provide a variable or text value")]
        //[SampleUsage("**Hello** or **{{{vText}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        [PropertyDescription("Text")]
        [InputSpecification("Text", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyDisplayText(true, "Text")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Modify Method")]
        [PropertyUISelectionOption("To Upper Case")]
        [PropertyUISelectionOption("To Lower Case")]
        [PropertyUISelectionOption("To Base64 String")]
        [PropertyUISelectionOption("From Base64 String")]
        [PropertyUISelectionOption("Trim")]
        [PropertyUISelectionOption("Trim Start")]
        [PropertyUISelectionOption("Trim End")]
        [PropertyUISelectionOption("Reverse")]
        [PropertyValidationRule("Modify Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Modify Method")]
        public string v_ConvertType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the changes")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public ModifyTextCommand()
        {
            //this.CommandName = "ModifyTextCommand";
            //this.SelectionName = "Modify Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var stringValue = v_userVariableName.ConvertToUserVariable(engine);

            //var caseType = v_ConvertType.ConvertToUserVariable(sender);
            var caseType = this.GetUISelectionValue(nameof(v_ConvertType), engine);
            switch (caseType)
            {
                case "to upper case":
                    stringValue = stringValue.ToUpper();
                    break;
                case "to lower case":
                    stringValue = stringValue.ToLower();
                    break;
                case "to base64 string":
                    byte[] textAsBytes = System.Text.Encoding.ASCII.GetBytes(stringValue);
                    stringValue = Convert.ToBase64String(textAsBytes);
                    break;
                case "from base64 string":
                    byte[] encodedDataAsBytes = Convert.FromBase64String(stringValue);
                    stringValue = System.Text.Encoding.ASCII.GetString(encodedDataAsBytes);
                    break;
                case "trim":
                    stringValue = stringValue.Trim();
                    break;
                case "trim start":
                    stringValue = stringValue.TrimStart();
                    break;
                case "trim end":
                    stringValue = stringValue.TrimEnd();
                    break;
                case "reverse":
                    stringValue = new string(stringValue.Reverse().ToArray());
                    break;
            }

            stringValue.StoreInUserVariable(engine, v_applyToVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));

        //    ////create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ConvertType", this, editor));


        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;

        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Convert '" + v_userVariableName + "', Method: '" + v_ConvertType + "', Store: '" + v_applyToVariableName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "Text to modify is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_ConvertType))
        //    {
        //        this.validationResult += "Case type is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
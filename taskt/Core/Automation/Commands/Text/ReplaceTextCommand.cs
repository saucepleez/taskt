using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Replace Text")]
    [Attributes.ClassAttributes.Description("This command allows you to replace text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to replace existing text within text or a variable with new text")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReplaceTextCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please select text or variable to modify")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Select or provide a variable or text value")]
        //[SampleUsage("**Hello** or **{{{vText}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Text to modify", PropertyValidationRule.ValidationRuleFlags.Empty)]
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
        [PropertyDescription("Text to be Replaced")]
        [InputSpecification("Text to be Replaced", true)]
        //[SampleUsage("**H** or **{{{vTextA}}}**")]
        [PropertyDetailSampleUsage("**H**", PropertyDetailSampleUsage.ValueType.Value, "Text to be Replaced")]
        [PropertyDetailSampleUsage("**{{{vTextA}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text to be Replaced")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Text to be Replaced", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "To be Replaced")]
        public string v_replacementText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Replacement Text")]
        [InputSpecification("Replacement Text", true)]
        //[SampleUsage("**J**, **{{{vTextB}}}**")]
        [PropertyDetailSampleUsage("**J**", PropertyDetailSampleUsage.ValueType.Value, "Replacement Text")]
        [PropertyDetailSampleUsage("**{{{vTextB}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Replacement Text")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(true, "Replacement")]
        public string v_replacementValue { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the changes")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public ReplaceTextCommand()
        {
            //this.CommandName = "ReplaceTextCommand";
            //this.SelectionName = "Replace Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //get full text
            string targetText = v_userVariableName.ConvertToUserVariable(engine);

            //get replacement text and value
            string replacementText = v_replacementText.ConvertToUserVariable(engine);
            string replacementValue = v_replacementValue.ConvertToUserVariable(engine);

            //perform replacement
            targetText = targetText.Replace(replacementText, replacementValue);

            //store in variable
            targetText.StoreInUserVariable(engine, v_applyToVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));

        //    ////create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_replacementText", this, editor));

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_replacementValue", this, editor));

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Replace '" + v_replacementText + "' with '" + v_replacementValue + "', apply to '" + v_userVariableName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "Text is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_replacementText))
        //    {
        //        this.validationResult += "Text tobe replaced is empty.\n";
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
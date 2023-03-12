using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Concatenate Text Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to you to concatenate text to Text Variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to concatenate text to Text Variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConcatenateTextVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Target Text Variable Name")]
        [InputSpecification("Text Variable Name", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**vText**", "Specify Variable Name **vText**")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", "Specify Variable Name **vText**")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        [PropertyValidationRule("Text Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Text Variable")]
        public string v_TargetVariable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text to Concatenate")]
        [InputSpecification("Text to Concatenate", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        [PropertyDetailSampleUsage("**{{{vConText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyValidationRule("Concatenate Text", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Concatenate")]
        public string v_ConcatText { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Insert Line Break before Concatenate or Not")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyIsOptional(true, "No")]
        public string v_InsertNewLine { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Concatenate Position")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyUISelectionOption("Before Variable Value")]
        [PropertyUISelectionOption("After Variable Value")]
        [PropertyIsOptional(true, "After Variable Value")]
        public string v_ConcatenatePosition { get; set; }

        public ConcatenateTextVariableCommand()
        {
            //this.CommandName = "ConcatenateTextVariableCommand";
            //this.SelectionName = "Concatenate Text Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetVariable = VariableNameControls.GetWrappedVariableName(v_TargetVariable, engine);
            var text = targetVariable.ConvertToUserVariable(engine);
            var con = v_ConcatText.ConvertToUserVariable(engine);

            var insertNewLine = this.GetUISelectionValue(nameof(v_InsertNewLine), "Insert New Line", engine);
            var concatPosition = this.GetUISelectionValue(nameof(v_ConcatenatePosition), "Concatenate Position", engine);

            switch (insertNewLine)
            {
                case "yes":
                    switch (concatPosition)
                    {
                        case "before variable value":
                            (con + "\n" + text).StoreInUserVariable(engine, targetVariable);
                            break;
                        case "after variable value":
                            (text + "\n" + con).StoreInUserVariable(engine, targetVariable);
                            break;
                    }
                    break;
                case "no":
                    switch (concatPosition)
                    {
                        case "before variable value":
                            (con + text).StoreInUserVariable(engine, targetVariable);
                            break;
                        case "after variable value":
                            (text + con).StoreInUserVariable(engine, targetVariable);
                            break;
                    }
                    break;
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [ Variable: '" + v_TargetVariable + "', Text: '" + v_ConcatText + "', NewLine: '" + v_InsertNewLine + "']";
        //}
    }
}
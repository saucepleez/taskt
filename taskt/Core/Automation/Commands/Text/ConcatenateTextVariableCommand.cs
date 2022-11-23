using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to you to concatenate text to Text Variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to concatenate text to Text Variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConcatenateTextVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify Target Text Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vText** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Text Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetVariable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Text to Concatenate")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyValidationRule("Concatenate Text", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ConcatText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select Insert Line Break before Concatenate or Not")]
        [InputSpecification("")]
        [SampleUsage("**Yes** or **No**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyIsOptional(true, "No")]
        public string v_InsertNewLine { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select Concatenate Position")]
        [InputSpecification("")]
        [SampleUsage("**Before Variable Value** or **After Variable Value**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Before Variable Value")]
        [PropertyUISelectionOption("After Variable Value")]
        [PropertyIsOptional(true, "After Variable Value")]
        public string v_ConcatenatePosition { get; set; }

        public ConcatenateTextVariableCommand()
        {
            this.CommandName = "ConcatenateTextVariableCommand";
            this.SelectionName = "Concatenate Text Variable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine
            var engine = (Engine.AutomationEngineInstance)sender;

            string targetVariable;
            if (engine.engineSettings.isWrappedVariableMarker(v_TargetVariable))
            {
                targetVariable = v_TargetVariable;
            }
            else
            {
                targetVariable = engine.engineSettings.wrapVariableMarker(v_TargetVariable);
            }
            var text = targetVariable.ConvertToUserVariable(engine);
            var con = v_ConcatText.ConvertToUserVariable(engine);

            //var insertNewLine = v_InsertNewLine.GetUISelectionValue("v_InsertNewLine", this, engine);
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

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [ Variable: '" + v_TargetVariable + "', Text: '" + v_ConcatText + "', NewLine: '" + v_InsertNewLine + "']";
        }
    }
}
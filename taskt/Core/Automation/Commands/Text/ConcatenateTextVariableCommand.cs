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

            var targetVariable = v_TargetVariable;
            var text = v_TargetVariable.ConvertToUserVariable(engine);
            var con = v_ConcatText.ConvertToUserVariable(engine);

            var insertNewLine = v_InsertNewLine.GetUISelectionValue("v_InsertNewLine", this, engine);

            switch (insertNewLine)
            {
                case "yes":
                    (text + "\n" + con).StoreInUserVariable(engine, targetVariable);
                    break;
                case "no":
                    (text + con).StoreInUserVariable(engine, targetVariable);
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
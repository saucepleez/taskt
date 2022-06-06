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
    [Attributes.ClassAttributes.Description("This command allows you to replace text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to replace existing text within text or a variable with new text")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class ReplaceTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select text or variable to modify")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or text value")]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Text to modify", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Indicate the text to be replaced")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the old value of the text that will be replaced")]
        [SampleUsage("**H** or **{{{vTextA}}}**")]
        [Remarks("H in Hello would be targeted for replacement")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Text to be Replaced", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_replacementText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Indicate the replacement value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the new value after replacement")]
        [SampleUsage("**J**, **{{{vTextB}}}**")]
        [Remarks("H would be replaced with J to create 'Jello'")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        public string v_replacementValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the changes")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_applyToVariableName { get; set; }
        public ReplaceTextCommand()
        {
            this.CommandName = "ReplaceTextCommand";
            this.SelectionName = "Replace Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            //get full text
            string replacementVariable = v_userVariableName.ConvertToUserVariable(sender);

            //get replacement text and value
            string replacementText = v_replacementText.ConvertToUserVariable(sender);
            string replacementValue = v_replacementValue.ConvertToUserVariable(sender);

            //perform replacement
            replacementVariable = replacementVariable.Replace(replacementText, replacementValue);

            //store in variable
            replacementVariable.StoreInUserVariable(sender, v_applyToVariableName);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));

            ////create standard group controls
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_replacementText", this, editor));

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_replacementValue", this, editor));

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Replace '" + v_replacementText + "' with '" + v_replacementValue + "', apply to '" + v_userVariableName + "']";
        }

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
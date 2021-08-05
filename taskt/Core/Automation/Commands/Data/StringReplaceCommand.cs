using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.SubGruop("Text")]
    [Attributes.ClassAttributes.Description("This command allows you to replace text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to replace existing text within text or a variable with new text")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class StringReplaceCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select text or variable to modify")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **{{{vText}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the text to be replaced (ex. H, {{{vTextA}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the old value of the text that will be replaced")]
        [Attributes.PropertyAttributes.SampleUsage("**H** or **{{{vTextA}}}**")]
        [Attributes.PropertyAttributes.Remarks("H in Hello would be targeted for replacement")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_replacementText { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the replacement value")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the new value after replacement")]
        [Attributes.PropertyAttributes.SampleUsage("**J**, **{{{vTextB}}}**")]
        [Attributes.PropertyAttributes.Remarks("H would be replaced with J to create 'Jello'")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_replacementValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the changes")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_applyToVariableName { get; set; }
        public StringReplaceCommand()
        {
            this.CommandName = "StringReplaceCommand";
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

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_userVariableName))
            {
                this.validationResult += "Text is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_replacementText))
            {
                this.validationResult += "Text tobe replaced is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_applyToVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
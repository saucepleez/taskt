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
    [Attributes.ClassAttributes.Description("This command allows you to trim Text, convert Text, etc.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to trim Text, convert Text, etc.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ModifyTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Supply the Text or Variable to modify")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or text value")]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Select Modify Method")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Indicate if only so many characters should be kept")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUISelectionOption("To Upper Case")]
        [PropertyUISelectionOption("To Lower Case")]
        [PropertyUISelectionOption("To Base64 String")]
        [PropertyUISelectionOption("From Base64 String")]
        [PropertyUISelectionOption("Trim")]
        [PropertyUISelectionOption("Trim Start")]
        [PropertyUISelectionOption("Trim End")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Modify Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ConvertType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the changes")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_applyToVariableName { get; set; }
        public ModifyTextCommand()
        {
            this.CommandName = "ModifyTextCommand";
            this.SelectionName = "Modify Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
;
        }
        public override void RunCommand(object sender)
        {
            var stringValue = v_userVariableName.ConvertToUserVariable(sender);

            var caseType = v_ConvertType.ConvertToUserVariable(sender);

            switch (caseType)
            {
                case "To Upper Case":
                    stringValue = stringValue.ToUpper();
                    break;
                case "To Lower Case":
                    stringValue = stringValue.ToLower();
                    break;
                case "To Base64 String":
                    byte[] textAsBytes = System.Text.Encoding.ASCII.GetBytes(stringValue);
                    stringValue = Convert.ToBase64String(textAsBytes);
                    break;
                case "From Base64 String":
                    byte[] encodedDataAsBytes = Convert.FromBase64String(stringValue);
                    stringValue = System.Text.Encoding.ASCII.GetString(encodedDataAsBytes);
                    break;
                case "Trim":
                    stringValue = stringValue.Trim();
                    break;
                case "Trim Start":
                    stringValue = stringValue.TrimStart();
                    break;
                case "Trim End":
                    stringValue = stringValue.TrimEnd();
                    break;

                default:
                    throw new NotImplementedException("Conversion Type '" + caseType + "' not implemented!");
            }

            stringValue.StoreInUserVariable(sender, v_applyToVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));

            ////create standard group controls
            //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ConvertType", this, editor));


            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Convert '" + v_userVariableName + "', Method: '" + v_ConvertType + "', Store: '" + v_applyToVariableName + "']";
        }

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
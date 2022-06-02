using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Check/Get")]
    [Attributes.ClassAttributes.Description("This command allows you to check a string")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select a subset of text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class CheckTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Supply the Text or Variable to Checked")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or text value")]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Select the Check Method")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Contains** or **Starts with** or **Ends with** or **Index of** or **Last Index of**")]
        [Remarks("")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Index of")]
        [PropertyUISelectionOption("Last Index of")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Check Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_CheckMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Text to Check or Search")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Ha** or **{{{vSearchedText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Text to Check or Search", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_CheckParameter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Select Case sensitive")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Indicate if only so many characters should be kept")]
        [SampleUsage("**Yes** or **No**")]
        [Remarks("")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Yes")]
        public string v_CaseSensitive { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the result")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertySecondaryLabel(true)]
        [PropertyAddtionalParameterInfo("Contains", "Result is TRUE or FALSE")]
        [PropertyAddtionalParameterInfo("Start with", "Result is TRUE or FALSE")]
        [PropertyAddtionalParameterInfo("End with", "Result is TRUE or FALSE")]
        [PropertyAddtionalParameterInfo("Index of", "Result is a found position. If not found, the result is -1.")]
        [PropertyAddtionalParameterInfo("Last Index of", "Result is the last position found. If not found, the result is -1.")]
        [PropertyControlIntoCommandField("", "", "variable2ndLabel")]
        public string v_applyToVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private Label variable2ndLabel;

        [XmlIgnore]
        [NonSerialized]
        private Label variableLabel;

        public CheckTextCommand()
        {
            this.CommandName = "CheckTextCommand";
            this.SelectionName = "Check Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var stringValue = v_userVariableName.ConvertToUserVariable(sender);

            var checkMethod = v_CheckMethod.ConvertToUserVariable(sender);

            var searchedValue = v_CheckParameter.ConvertToUserVariable(sender);

            var caseSensitive = v_CaseSensitive.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(caseSensitive))
            {
                caseSensitive = "Yes";
            }

            var resultValue = "";
            switch (checkMethod)
            {
                case "Contains":
                    if (caseSensitive == "Yes")
                    {
                        resultValue = stringValue.Contains(searchedValue) ? "TRUE" : "FALSE";
                    }
                    else
                    {
                        resultValue = stringValue.ToLower().Contains(searchedValue.ToLower()) ? "TRUE" : "FALSE";
                    }
                    break;

                case "Starts with":
                    if (caseSensitive == "Yes")
                    {
                        resultValue = stringValue.StartsWith(searchedValue) ? "TRUE" : "FALSE";
                    }
                    else
                    {
                        resultValue = stringValue.ToLower().StartsWith(searchedValue.ToLower()) ? "TRUE" : "FALSE";
                    }
                    break;

                case "Ends with":
                    if (caseSensitive == "Yes")
                    {
                        resultValue = stringValue.EndsWith(searchedValue) ? "TRUE" : "FALSE";
                    }
                    else
                    {
                        resultValue = stringValue.ToLower().EndsWith(searchedValue.ToLower()) ? "TRUE" : "FALSE";
                    }
                    break;

                case "Index of":
                    if (caseSensitive == "Yes")
                    {
                        resultValue = stringValue.IndexOf(searchedValue).ToString();
                    }
                    else
                    {
                        resultValue = stringValue.ToLower().IndexOf(searchedValue.ToLower()).ToString();
                    }
                    break;

                case "Last Index of":
                    if (caseSensitive == "Yes")
                    {
                        resultValue = stringValue.LastIndexOf(searchedValue).ToString();
                    }
                    else
                    {
                        resultValue = stringValue.ToLower().LastIndexOf(searchedValue.ToLower()).ToString();
                    }
                    break;

                default:
                    throw new NotImplementedException("Check Method '" + checkMethod + "' not implemented!");
            }

            resultValue.StoreInUserVariable(sender, v_applyToVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctls);

            //variable2ndLabel = (Label)ctls.Where(t => t.Name == "lbl2_v_applyToVariableName").FirstOrDefault();

            variableLabel = (Label)ctls.GetControlsByName("v_applyToVariableName", CommandControls.CommandControlType.Label)[0];

            var chkCombobox = (ComboBox)ctls.Where(t => t.Name == "v_CheckMethod").FirstOrDefault();
            chkCombobox.SelectedIndexChanged += (sender, e) => CheckMethod_SelectedIndexChanged(sender, e);

            return RenderedControls;
        }

        private void CheckMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string searchedKey = ((ComboBox)sender).Text;
            //var info = resultInfo.Where(t => t.searchKey == searchedKey).FirstOrDefault();
            Dictionary<string, string> dic = (Dictionary<string, string>)variableLabel.Tag;
            variable2ndLabel.Text = (dic.ContainsKey(searchedKey) ? dic[searchedKey] : "");
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Check '" + v_userVariableName + "', Method '" + v_CheckMethod + "', Result '" + v_applyToVariableName + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "Text to check is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_CheckMethod))
        //    {
        //        this.validationResult += "Search Method is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_CheckParameter))
        //    {
        //        this.validationResult += "Check Parameter is empty.\n";
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
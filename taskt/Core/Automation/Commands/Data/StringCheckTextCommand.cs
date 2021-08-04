using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.SubGruop("Text")]
    [Attributes.ClassAttributes.Description("This command allows you to check a string")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select a subset of text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class StringCheckTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the value or variable to check")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **{{{vText}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select the check method")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Contains** or **Start with** or **End with** or **Index of** or **Last Index of**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Contains")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Start with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("End with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Index of")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Last Index of")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_CheckMethod { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select the check method")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Ha** or **{{{vSearchedText}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_CheckParameter { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Case sensitive (Default is Yes)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Indicate if only so many characters should be kept")]
        [Attributes.PropertyAttributes.SampleUsage("**Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_CaseSensitive { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the result")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertySecondaryLabel(true)]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Contains", "Result is TRUE or FALSE")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Start with", "Result is TRUE or FALSE")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("End with", "Result is TRUE or FALSE")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Index of", "Result is a found position. If not found, the result is -1.")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Last Index of", "Result is the last position found. If not found, the result is -1.")]
        public string v_applyToVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private List<Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo> resultInfo;

        [XmlIgnore]
        [NonSerialized]
        private Label variable2ndLabel;

        public StringCheckTextCommand()
        {
            this.CommandName = "CheckStringCommand";
            this.SelectionName = "Check String";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            resultInfo = ScriptCommand.GetAdditionalParameterInfo(this.GetProperty("v_applyToVariableName"));
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

                case "Start with":
                    if (caseSensitive == "Yes")
                    {
                        resultValue = stringValue.StartsWith(searchedValue) ? "TRUE" : "FALSE";
                    }
                    else
                    {
                        resultValue = stringValue.ToLower().StartsWith(searchedValue.ToLower()) ? "TRUE" : "FALSE";
                    }
                    break;

                case "End with":
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

            variable2ndLabel = (Label)ctls.Where(t => t.Name == "lbl2_v_applyToVariableName").FirstOrDefault();

            var chkCombobox = (ComboBox)ctls.Where(t => t.Name == "v_CheckMethod").FirstOrDefault();
            chkCombobox.SelectedIndexChanged += (sender, e) => CheckMethod_SelectedIndexChanged(sender, e);

            return RenderedControls;
        }

        private void CheckMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string searchedKey = ((ComboBox)sender).Text;
            var info = resultInfo.Where(t => t.searchKey == searchedKey).FirstOrDefault();
            variable2ndLabel.Text = (info != null) ? info.description : "";
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Check '" + v_userVariableName + "', Method '" + v_CheckMethod + "', Result '" + v_applyToVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_userVariableName))
            {
                this.validationResult += "Text to check is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_CheckMethod))
            {
                this.validationResult += "Search Method is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_CheckParameter))
            {
                this.validationResult += "Check Parameter is empty.\n";
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
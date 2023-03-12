using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Check/Get")]
    [Attributes.ClassAttributes.CommandSettings("Check Text")]
    [Attributes.ClassAttributes.Description("This command allows you to check a Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check a Text")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Text to be Checked")]
        [InputSpecification("Text", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Text to be Checked")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Check Method")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Index of")]
        [PropertyUISelectionOption("Last Index of")]
        [PropertyUISelectionOption("Has Value")]
        [PropertyUISelectionOption("Is a Number")]
        [PropertyUISelectionOption("Is a Boolean")]
        [PropertyValidationRule("Check Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Method")]
        [PropertySelectionChangeEvent(nameof(cmbCheckMethod_SelectionChanged))]
        [PropertySecondaryLabel(true)]
        [PropertyAddtionalParameterInfo("Contains", "Result is **TRUE** or **FALSE**")]
        [PropertyAddtionalParameterInfo("Starts with", "Result is **TRUE** or **FALSE**")]
        [PropertyAddtionalParameterInfo("Ends with", "Result is **TRUE** or **FALSE**")]
        [PropertyAddtionalParameterInfo("Index of", "Result is a found position. If not found, the result is -1.")]
        [PropertyAddtionalParameterInfo("Last Index of", "Result is the last position found. If not found, the result is -1.")]
        [PropertyAddtionalParameterInfo("Has Value", "Result is **TRUE** or **FALSE**")]
        [PropertyAddtionalParameterInfo("Is a Number", "Result is **TRUE** or **FALSE**")]
        [PropertyAddtionalParameterInfo("Is a Boolean", "Result is **TRUE** or **FALSE**")]
        public string v_CheckMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text to Check or Search")]
        [InputSpecification("Text to Check or Search", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**Ha**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Text to Check or Search")]
        public string v_CheckParameter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Case sensitive")]
        [InputSpecification("", true)]
        [SampleUsage("**Yes** or **No**")]
        [Remarks("")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Yes")]
        public string v_CaseSensitive { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public CheckTextCommand()
        {
            //this.CommandName = "CheckTextCommand";
            //this.SelectionName = "Check Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetValue = v_userVariableName.ConvertToUserVariable(engine);
            var checkValue = v_CheckParameter.ConvertToUserVariable(engine);

            var caseSensitive = this.GetUISelectionValue(nameof(v_CaseSensitive), engine);
            if (caseSensitive == "no")
            {
                targetValue = targetValue.ToLower();
                checkValue = checkValue.ToLower();
            }

            var checkMethod = this.GetUISelectionValue(nameof(v_CheckMethod), engine);
            bool resultValue = false;
            switch (checkMethod)
            {
                case "contains":
                    resultValue = targetValue.Contains(checkValue);
                    break;
                case "starts with":
                    resultValue = targetValue.StartsWith(checkValue);
                    break;
                case "ends with":
                    resultValue = targetValue.EndsWith(checkValue);
                    break;
                case "has value":
                    resultValue = String.IsNullOrEmpty(targetValue);
                    break;
                case "is a number":
                    resultValue = decimal.TryParse(targetValue, out _);
                    break;
                case "is a boolean":
                    resultValue = bool.TryParse(targetValue, out _);
                    break;
                case "index of":
                    targetValue.IndexOf(checkValue).ToString().StoreInUserVariable(engine, v_applyToVariableName);
                    return;
                case "last index of":
                    targetValue.LastIndexOf(checkValue).ToString().StoreInUserVariable(engine, v_applyToVariableName);
                    return;
            }

            resultValue.StoreInUserVariable(engine, v_applyToVariableName);
        }

        private void cmbCheckMethod_SelectionChanged(object sender, EventArgs e)
        {
            string searchedKey = ((ComboBox)sender).SelectedItem.ToString();

            Dictionary<string, string> dic = (Dictionary<string, string>)(ControlsList["lbl_" + nameof(v_CheckMethod)].Tag);

            var lbl = (Label)ControlsList["lbl2_" + nameof(v_CheckMethod)];
            lbl.Text = (dic.ContainsKey(searchedKey) ? dic[searchedKey] : "");
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctls);

        //    //variable2ndLabel = (Label)ctls.Where(t => t.Name == "lbl2_v_applyToVariableName").FirstOrDefault();

        //    variableLabel = (Label)ctls.GetControlsByName("v_applyToVariableName", CommandControls.CommandControlType.Label)[0];

        //    var chkCombobox = (ComboBox)ctls.Where(t => t.Name == "v_CheckMethod").FirstOrDefault();
        //    chkCombobox.SelectedIndexChanged += (sender, e) => CheckMethod_SelectedIndexChanged(sender, e);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Check '" + v_userVariableName + "', Method '" + v_CheckMethod + "', Result '" + v_applyToVariableName + "']";
        //}

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
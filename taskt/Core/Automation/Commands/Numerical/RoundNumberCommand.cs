using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to Round up, down, or round off numbers.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Round up, down, or round off numbers.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RoundNumberCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify Number Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**100** or **{{{vNum}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Number", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Number")]
        public string v_Numeric { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select Round Type")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(false)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Round")]
        [PropertyUISelectionOption("Round Up")]
        [PropertyUISelectionOption("Round Down")]
        [PropertyValidationRule("Round Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_RoundType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vValue** or **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public string v_Result { get; set; }

        public RoundNumberCommand()
        {
            this.CommandName = "RoundNumberCommand";
            this.SelectionName = "Round Number";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var num = v_Numeric.ConvertToUserVariableAsDecimal("Numeric", engine);
            var num = (v_Numeric, "Number").ConvertToUserVariableAsDecimal(engine);

            var round = v_RoundType.GetUISelectionValue("v_RoundType", this, engine);

            decimal res = 0;
            switch (round)
            {
                case "round":
                    res = Math.Round(num, MidpointRounding.AwayFromZero);
                    break;
                case "round up":
                    res = Math.Ceiling(num);
                    break;
                case "round down":
                    res = Math.Truncate(num);
                    break;
            }

            res.ToString().StoreInUserVariable(engine, v_Result);
        }
    }
}
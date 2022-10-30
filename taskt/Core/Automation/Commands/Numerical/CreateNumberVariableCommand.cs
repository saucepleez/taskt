using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create Number Variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Number Variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class CreateNumberVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vValue** or **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable Name")]
        public string v_VariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Number Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **1.1** or **{{{vNumber}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Number", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyDisplayText(true, "Number")]
        public string v_NumberValue { get; set; }

        public CreateNumberVariableCommand()
        {
            this.CommandName = "CreateNumberVariableCommand";
            this.SelectionName = "Create Number Variable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string numString = v_NumberValue.ConvertToUserVariable(engine);
            if (decimal.TryParse(numString, out _))
            {
                numString.StoreInUserVariable(engine, v_VariableName);
            }
            else
            {
                throw new Exception("Number '" + v_NumberValue + "' is not number");
            }
        }
    }
}
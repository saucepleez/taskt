using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.CommandSettings("Decrease Numerical Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to Decrease Value in Numerical Variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Decrease Value in Numerical Variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class DecreaseNumericalVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_BothNumericalVariableName))]
        public string v_VariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Numerical Value to Decrease")]
        [PropertyIsOptional(true, "1")]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_Value { get; set; }

        public DecreaseNumericalVariableCommand()
        {
            //this.CommandName = "DecreaseNumericalVariableCommand";
            //this.SelectionName = "Decrease Numerical Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var variableName = VariableNameControls.GetWrappedVariableName(v_VariableName, engine);
            var variableValue = new PropertyConvertTag(variableName, "Variable Name").ConvertToUserVariableAsDecimal(engine);

            var add = this.ConvertToUserVariableAsDecimal(nameof(v_VariableName), engine);

            (variableValue - add).ToString().StoreInUserVariable(engine, variableName);
        }
    }
}
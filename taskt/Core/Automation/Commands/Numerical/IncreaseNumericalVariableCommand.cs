using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.CommandSettings("Increase Numerical Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to Increase Value in Numerical Variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Increase Value in Numerical Variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class IncreaseNumericalVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_BothNumericalVariableName))]
        public string v_VariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Numerical Value to Increase")]
        [PropertyIsOptional(true, "1")]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_Value { get; set; }

        public IncreaseNumericalVariableCommand()
        {
            //this.CommandName = "IncreaseNumericalVariableCommand";
            //this.SelectionName = "Increase Numerical Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var variableName = VariableNameControls.GetWrappedVariableName(v_VariableName, engine);
            var variableValue = new PropertyConvertTag(variableName, "Variable Name").ConvertToUserVariableAsDecimal(engine);

            //var add = new PropertyConvertTag(v_Value, "v_Value", "Value").ConvertToUserVariableAsDecimal(this, engine);
            //var add = this.ConvertToUserVariableAsDecimal(nameof(v_Value), "Value To Increase", engine);
            var add = this.ConvertToUserVariableAsDecimal(nameof(v_Value), engine);

            (variableValue + add).ToString().StoreInUserVariable(engine, variableName);
        }
    }
}
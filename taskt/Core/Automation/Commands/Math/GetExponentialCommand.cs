using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Exponential")]
    [Attributes.ClassAttributes.Description("This command allows you to get exp.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get exp.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetExponentialCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Power")]
        [InputSpecification("Power", true)]
        [PropertyDisplayText(true, "Power")]
        public string v_Power { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        public string v_Result { get; set; }

        public GetExponentialCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var v = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Power), engine);
            Math.Exp(v).StoreInUserVariable(engine, v_Result);
        }
    }
}
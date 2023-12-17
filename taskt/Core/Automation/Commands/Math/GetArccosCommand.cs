using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Arccos")]
    [Attributes.ClassAttributes.Description("This command allows you to get arccos.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get arccos.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetArccosCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_AngleType))]
        public string v_AngleType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_WhenValueIsOutOfRange))]
        public string v_WhenValueIsOutOfRange { get; set; }

        public GetArccosCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var r = MathControls.InverseTrignometicFunctionAction(this, nameof(v_Value), nameof(v_AngleType), nameof(v_WhenValueIsOutOfRange),
                Math.Acos, new Func<double, bool>(v =>
                {
                    return (v >= -1 && v <= 1);
                }), engine);
            r.StoreInUserVariable(engine, v_Result);
        }
    }
}
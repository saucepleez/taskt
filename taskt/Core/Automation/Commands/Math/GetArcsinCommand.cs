using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Arcsin")]
    [Attributes.ClassAttributes.Description("This command allows you to get arcsin.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get arcsin.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetArcsinCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_AngleType))]
        public string v_AngleType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_WhenValueIsOutOfRange))]
        public string v_WhenValueIsOutOfRange { get; set; }

        public GetArcsinCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var r = MathControls.InverseTrignometicFunctionAction(this, nameof(v_Value), nameof(v_AngleType), nameof(v_WhenValueIsOutOfRange),
                Math.Asin, new Func<double, bool>(v =>
                {
                    return (v >= -1 && v <= 1);
                }), engine);
            r.StoreInUserVariable(engine, v_Result);
        }
    }
}
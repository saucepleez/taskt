using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Cos")]
    [Attributes.ClassAttributes.Description("This command allows you to get cos.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get cos.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetCosCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_AngleType))]
        public string v_AngleType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public GetCosCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var value = MathControls.ConvertAngleValueToRadian(this, nameof(v_Value), nameof(v_AngleType), engine);

            //Math.Cos(value).StoreInUserVariable(engine, v_Result);

            MathControls.TrignometicFunctionAction(this, nameof(v_Value), nameof(v_AngleType), nameof(v_Result),
                Math.Cos, engine);
        }
    }
}
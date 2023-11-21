using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Sin")]
    [Attributes.ClassAttributes.Description("This command allows you to get sin.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get sin.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetSinCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_AngleType))]
        public string v_AngleType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        public string v_Result { get; set; }

        public GetSinCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var value = NumberControls.ConvertAngleValueToRadian(this, nameof(v_Value), nameof(v_AngleType), engine);

            //Math.Sin(value).StoreInUserVariable(engine, v_Result);
            var r = MathControls.TrignometicFunctionAction(this, nameof(v_Value), nameof(v_AngleType), 
                Math.Sin, engine);
            r.StoreInUserVariable(engine, v_Result);
        }
    }
}
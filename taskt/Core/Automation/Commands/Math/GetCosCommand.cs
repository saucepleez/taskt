using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Cos")]
    [Attributes.ClassAttributes.Description("This command allows you to get cos.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get cos.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetCosCommand : ATrignometricCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        //public string v_Value { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_AngleType))]
        //public string v_AngleType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        //public string v_Result { get; set; }

        public GetCosCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var value = MathControls.ConvertAngleValueToRadian(this, nameof(v_Value), nameof(v_AngleType), engine);

            //Math.Cos(value).StoreInUserVariable(engine, v_Result);

            //var r = MathControls.TrignometicFunctionAction(this, nameof(v_Value), nameof(v_AngleType), 
            //    Math.Cos, engine);
            //var r = MathControls.TrignometicFunctionAction(this, Math.Cos, engine);
            //r.StoreInUserVariable(engine, v_Result);

            this.TrignometicFunctionAction(Math.Cos, engine);
        }
    }
}
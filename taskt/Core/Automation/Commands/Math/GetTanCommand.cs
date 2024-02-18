using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Tan")]
    [Attributes.ClassAttributes.Description("This command allows you to get tan.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get tan.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetTanCommand : ATrignometricCommands
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

        public GetTanCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var value = NumberControls.ConvertAngleValueToRadian(this, nameof(v_Value), nameof(v_AngleType), engine);

            //Math.Tan(value).StoreInUserVariable(engine, v_Result);
            //var r = MathControls.TrignometicFunctionAction(this, nameof(v_Value), nameof(v_AngleType),
            //    Math.Tan, engine);

            //var r = MathControls.TrignometicFunctionAction(this, Math.Tan, engine);
            //r.StoreInUserVariable(engine, v_Result);

            this.TrignometicFunctionAction(Math.Tan, engine);
        }
    }
}
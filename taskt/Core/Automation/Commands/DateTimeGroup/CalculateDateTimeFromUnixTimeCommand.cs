using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime")]
    [Attributes.ClassAttributes.SubGruop("Calculate")]
    [Attributes.ClassAttributes.CommandSettings("Calculate DateTime From Unix Time")]
    [Attributes.ClassAttributes.Description("This command allows you to Calculate DateTime From Unix Time. Add Day, Minute, etc.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Calculate DateTime From Unix Time. Add Day, Minute, etc.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CalculateDateTimeFromUnixTimeCommand : ACalculateDateTimeCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_UnixTime))]
        public override string v_DateTime { get; set; }

        //[XmlAttribute]
        //public string v_CalculationMethod { get; set; }

        //[XmlAttribute]
        //public string v_Value { get; set; }

        //[XmlAttribute]
        //public override string v_Result { get; set; }

        public CalculateDateTimeFromUnixTimeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //using (var myDT = new InnerScriptVariable(engine)) 
            //{
            //    var cdu = new CreateDateTimeFromUnixTimeCommand()
            //    {
            //        v_UnixTime = this.v_DateTime,
            //        v_DateTime = myDT.VariableName,
            //    };
            //    cdu.RunCommand(engine);

            //    var calc = new CalculateDateTimeCommand()
            //    {
            //        v_DateTime = myDT.VariableName,
            //        v_CalculationMethod = this.v_CalculationMethod,
            //        v_Value = this.v_Value,
            //        v_Result = this.v_Result,
            //    };
            //    calc.RunCommand(engine);
            //}

            var cdu = new CreateDateTimeFromUnixTimeCommand()
            {
                v_UnixTime = this.v_DateTime,
            };
            this.CommandProcess(
                cdu,
                engine
            );
        }
    }
}
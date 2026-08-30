using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime")]
    [Attributes.ClassAttributes.SubGruop("Format")]
    [Attributes.ClassAttributes.CommandSettings("Format DateTime By Excel Serial")]
    [Attributes.ClassAttributes.Description("This command allows you to Format DateTime By Excel Serial.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Format DateTime By Excel Serial.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class FormatDateTimeByExcelSerialCommand : AFormatDateTimeCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_ExcelSerial))]
        public override string v_DateTime { get; set; }

        //[XmlAttribute]
        //public string v_Format { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //public string v_Result { get; set; }

        public FormatDateTimeByExcelSerialCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //using (var v = new InnerScriptVariable(engine)) 
            //{
            //    var cde = new CreateDateTimeFromExcelSerialCommand()
            //    {
            //        v_DateTime = v.VariableName,
            //        v_Serial = this.v_DateTime,
            //    };
            //    cde.RunCommand(engine);

            //    var fdt = new FormatDateTimeCommand()
            //    {
            //        v_DateTime = v.VariableName,
            //        v_Format = this.v_Format,
            //        v_Result = this.v_Result,
            //    };
            //    fdt.RunCommand(engine);
            //}
            var cde = new CreateDateTimeFromExcelSerialCommand()
            {
                v_Serial = this.v_DateTime,
            };
            this.CommandProcess(
                cde,
                engine
            );
        }
    }
}
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime")]
    [Attributes.ClassAttributes.SubGruop("Calculate")]
    [Attributes.ClassAttributes.CommandSettings("Get Formatted DateTime From Calculated Excel Serial DateTime")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Formatted DateTime Text From Calculated Excel Serial DateTime Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Formatted DateTime Text From Calculated Excel Serial DateTime Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetFormattedDateTimeFromCalculatedExcelSerialDateTimeCommand : AGetFormattedDateTimeFromCalculatedDateTimeCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_ExcelSerial))]
        public override string v_DateTime { get; set; }
        
        public GetFormattedDateTimeFromCalculatedExcelSerialDateTimeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.CommandProcess(
                new CalculateDateTimeFromExcelSerialCommand(),
                engine
            );
        }
    }
}

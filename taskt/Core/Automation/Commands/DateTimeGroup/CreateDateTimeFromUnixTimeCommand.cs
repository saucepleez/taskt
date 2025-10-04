using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime")]
    [Attributes.ClassAttributes.SubGruop("Create")]
    [Attributes.ClassAttributes.CommandSettings("Create DateTime From Unix Time")]
    [Attributes.ClassAttributes.Description("This command allows you to create DateTime from Unix Time.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create DateTime from Unix Time.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CreateDateTimeFromUnixTimeCommand : ADateTimeCreateCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_OutputDateTime))]
        //public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_UnixTime))]
        [PropertyParameterOrder(6000)]
        public string v_UnixTime { get; set; }

        public CreateDateTimeFromUnixTimeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var ut = v_UnixTime.ExpandValueOrUserVariable(engine);

            try
            {
                if (long.TryParse(ut, out var utv))
                {
                    var dto = DateTimeOffset.FromUnixTimeSeconds(utv);
                    this.StoreDateTimeInUserVariable(dto.DateTime, nameof(v_DateTime), engine);
                }
                else
                {
                    throw new Exception($"Specified Unix Time is not Number. Value: '{v_UnixTime}', Expand Value: '{ut}'");
                }
            }
            catch
            {
                throw new Exception($"Specified Unix Time is not DateTime. Value: '{v_UnixTime}', Expand Value: '{ut}'");
            }
        }
    }
}
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert DateTime To Unix Time")]
    [Attributes.ClassAttributes.Description("This command allows you to Convert DateTime To Unix Time.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want Convert DateTime To Unix Time.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDateTimeToUnixTimelCommand : ADateTimeConvertCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_InputDateTime))]
        //public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Variable Name to Store Unix Time")]
        public override string v_Result { get; set; }

        public ConvertDateTimeToUnixTimelCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var myDT = this.ExpandValueOrVariableAsDateTime(engine);

            var unixTime = new DateTimeOffset(myDT.ToUniversalTime());
            unixTime.ToUnixTimeSeconds().ToString().StoreInUserVariable(engine, v_Result);
        }
    }
}
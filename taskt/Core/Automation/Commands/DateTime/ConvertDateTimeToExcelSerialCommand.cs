using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Convert DateTime To Excel Serial")]
    [Attributes.ClassAttributes.Description("This command allows you to Convert DateTime To Excel Serial Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want Convert DateTime To Excel Serial Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDateTimeToExcelSerialCommand : ADateTimeConvertCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_InputDateTime))]
        //public string v_DateTime { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Excel Serial Value")]
        //[PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        //[PropertyDetailSampleUsage("**vSerial**", PropertyDetailSampleUsage.ValueType.VariableName)]
        //[PropertyDetailSampleUsage("**{{{vSerial}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        public override string v_Result { get; set; }

        public ConvertDateTimeToExcelSerialCommand()
        {
            //this.CommandName = "ConvertDateTimeToExcelSerialCommand";
            //this.SelectionName = "Convert DateTime To Excel Serial";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //DateTime myDT = v_DateTime.ExpandUserVariableAsDateTime(engine);
            //var myDT = this.ExpandValueOrVariableAsDateTime(nameof(v_DateTime), engine);
            var myDT = this.ExpandValueOrVariableAsDateTime(engine);

            //double serial = myDT.ToOADate();
            //serial.ToString().StoreInUserVariable(engine, v_Serial);

            myDT.ToOADate().StoreInUserVariable(engine, v_Result);
        }
    }
}
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Power")]
    [Attributes.ClassAttributes.Description("This command allows you to get power.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get power.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetPowerCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Base")]
        [InputSpecification("Base", true)]
        [PropertyDisplayText(true, "Base")]
        public string v_Base { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Power")]
        [InputSpecification("Power", true)]
        [PropertyDisplayText(true, "Power")]

        public string v_Power { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public GetPowerCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var b = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Base), engine);
            var n = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Power), engine);

            Math.Pow(b, n).StoreInUserVariable(engine, v_Result);
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.CommandSettings("Get JSON Value")]
    [Attributes.ClassAttributes.Description("This command allows you to Get JSON Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get JSON Value")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetJSONValueCommand : AJSONGetFromJContainerCommands
    {
        //[XmlAttribute]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public override string v_Result { get; set; }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (_, var jCon, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);
            jCon.ToString().StoreInUserVariable(engine, v_Result);
        }
    }
}

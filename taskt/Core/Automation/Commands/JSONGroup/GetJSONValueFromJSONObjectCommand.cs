using Newtonsoft.Json.Linq;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.CommandSettings("Get JSON Value From JSON Object")]
    [Attributes.ClassAttributes.Description("This command allows you to Get JSON Value From JSON Object")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get JSON Value From JSON Object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetJSONValueFromJSONObjectCommand : AJSONGetFromJContainerCommands
    {
        //[XmlAttribute]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_PropertyName))]
        [PropertyParameterOrder(7000)]
        public string v_PropertyName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public override string v_Result { get; set; }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (_, var jCon, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);
            if (jCon is JObject obj)
            {
                var propName = this.ExpandValueOrUserVariable(nameof(v_PropertyName), "Property Name", engine);
                if (obj.ContainsKey(propName))
                {
                    obj[propName].ToString().StoreInUserVariable(engine, v_Result);
                }
                else
                {
                    throw new Exception($"Specified property does not Exists. Property: '{v_PropertyName}', Expand: '{propName}'");
                }
            }
            else
            {
                throw new Exception($"Extraction Result is NOT Supported Type. Result: '{jCon}', JSONPath: '{v_JsonExtractor}'");
            }
        }
    }
}

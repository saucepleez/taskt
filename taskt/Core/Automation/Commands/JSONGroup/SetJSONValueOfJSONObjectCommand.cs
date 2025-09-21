using Newtonsoft.Json.Linq;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.CommandSettings("Set JSON Value Of JSON Object")]
    [Attributes.ClassAttributes.Description("This command allows you to Set Value in JSON Object")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Set Value in JSON Object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SetJSONValueOfJSONObjectCommand : AJSONSetValueOfJContainerCommands
    {
        //[XmlAttribute]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_PropertyName))]
        [PropertyParameterOrder(7000)]
        public string v_PropertyName { get; set; }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var root, var jCon, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);
            if (jCon is JObject obj)
            {
                var propName = this.ExpandValueOrUserVariable(nameof(v_PropertyName), "Property Name", engine);
                if (obj.ContainsKey(propName))
                {
                    var v = this.ExpandValueOrVariableValueAsJSONSupportedValueInJSONValue(engine);
                    obj[propName] = v;
                    this.StoreJSONInUserVariable(root, engine);
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

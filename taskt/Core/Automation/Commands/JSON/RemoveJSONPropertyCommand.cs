using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Remove JSON Property")]
    [Attributes.ClassAttributes.Description("This command allows you to remove a property in JSON")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RemoveJSONPropertyCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_BothJSONName))]
        [PropertyDescription("JSON Object Variable Name")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        public string v_JsonExtractor { get; set; }

        public RemoveJSONPropertyCommand()
        {
            //this.CommandName = "Remove JSON Property";
            //this.SelectionName = "Remove JSON Property";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Action<JToken> removeFunc = new Action<JToken>((searchResult) =>
            {
                var p = searchResult.Parent;
                if (p is JProperty prop)
                {
                    prop.Remove();
                }
                else if (p is JArray ary)
                {
                    ary.Remove(searchResult);
                }
                else
                {
                    throw new Exception("Strange Search Result. Fail Remove. Value: '" + searchResult.ToString() + "'");
                }
            });
            this.JSONModifyByJSONPath(nameof(v_InputValue), nameof(v_JsonExtractor), removeFunc, removeFunc, engine);
        }
    }
}
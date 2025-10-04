using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Add JSON Object Property")]
    [Attributes.ClassAttributes.Description("This command allows you to add property to JSON Object.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class AddJSONObjectPropertyCommand : AJSONAddInsertSetJContainerCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_BothJSONName))]
        //[PropertyDescription("JSON Object Variable Name")]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_PropertyName))]
        [PropertyParameterOrder(7000)]
        public string v_PropertyName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        //public string v_Value { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        //public string v_ValueType { get; set; }

        public AddJSONObjectPropertyCommand()
        {
            //this.CommandName = "AddJSONObjectProperty";
            //this.SelectionName = "Add JSON Object Property";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //Action<JToken> addPropertyFunc = new Action<JToken>((searchResult) =>
            //{
            //    if (!(searchResult is JObject))
            //    {
            //        throw new Exception("Extraction Result is not JSON Object and can not Add JSON Property. Value: '" + searchResult.ToString() + "'");
            //    }
            //    JObject obj = (JObject)searchResult;

            //    var propertyValue = this.GetJSONValue(nameof(v_Value), nameof(v_ValueType), "Add", engine);
            //    var propertyName = v_PropertyName.ExpandValueOrUserVariable(engine);
            //    obj.Add(new JProperty(propertyName, propertyValue));
            //});
            //this.JSONModifyByJSONPath(nameof(v_Json), nameof(v_JsonExtractor), addPropertyFunc, addPropertyFunc, engine);

            (var root, var json, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);
            if (json is JObject obj)
            {
                var v = this.ExpandValueOrVariableValueAsJSONSupportedValueInJSONValue(engine);
                var key = this.ExpandValueOrUserVariable(nameof(v_PropertyName), "Property", engine);
                obj.Add(new JProperty(key, v));
                //root.ToString().StoreInUserVariable(engine, v_Json);
                this.StoreJSONInUserVariable(root, engine);
            }
            else
            {
                throw new Exception($"Extraction Result is NOT JSON Object. Result: '{json}', JSONPath: '{v_JsonExtractor}'");
            }
        }
    }
}
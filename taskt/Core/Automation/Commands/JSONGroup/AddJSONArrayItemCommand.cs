using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Add JSON Array Item")]
    [Attributes.ClassAttributes.Description("This command allows you to add item to JSON Array.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class AddJSONArrayItemCommand : AJSONAddInsertSetJContainerCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_BothJSONName))]
        //[PropertyDescription("JSON Array Variable Name")]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        //public string v_JsonExtractor { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        //public string v_Value { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        //public string v_ValueType { get; set; }

        public AddJSONArrayItemCommand()
        {
            //this.CommandName = "AddJSONArrayItem";
            //this.SelectionName = "Add JSON Array Item";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //Action<JToken> addItemFunc = new Action<JToken>((searchResult) =>
            //{
            //    if (!(searchResult is JArray))
            //    {
            //        throw new Exception("Extraction Result is not JSON Array and can not Add Item. Value: '" + searchResult.ToString() + "'");
            //    }
            //    JArray ary = (JArray)searchResult;

            //    var addItem = this.GetJSONValue(nameof(v_Value), nameof(v_ValueType), "Add", engine);
            //    ary.Add(addItem);
            //});
            //this.JSONModifyByJSONPath(nameof(v_Json), nameof(v_JsonExtractor), addItemFunc, addItemFunc, engine);

            (var root, var json, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);
            if (json is JArray ary)
            {
                var v = this.ExpandValueOrVariableValueAsJSONSupportedValueInJSONValue(engine);
                ary.Add(v);
                //root.ToString().StoreInUserVariable(engine, v_Json);
                this.StoreJSONInUserVariable(root, engine);
            }
            else
            {
                throw new Exception($"Extraction Result is NOT JSON Array. Result: '{json}', JSONPath: '{v_JsonExtractor}'");
            }
        }
    }
}

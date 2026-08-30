using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Insert JSON Object Property")]
    [Attributes.ClassAttributes.Description("This command allows you to add property to JSON Object.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class InsertJSONObjectPropertyCommand : AJSONInsertValueToJContainerCommands
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
        [PropertyDescription("Property Name of Insert Position")]
        [PropertyIsOptional(true, "")]
        [PropertyValidationRule("Property Name of Insert Position", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8000)]
        public string v_PropertyNameInsertPosition { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Property Name to Insert")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[PropertyDetailSampleUsage("**Name**", PropertyDetailSampleUsage.ValueType.Value, "Property Name")]
        //[PropertyDetailSampleUsage("**{{{vName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Property Name")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Property Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Property Name")]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_PropertyName))]
        [PropertyParameterOrder(8100)]
        public string v_PropertyName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        //public string v_Value { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        //public string v_ValueType { get; set; }

        public InsertJSONObjectPropertyCommand()
        {
            //this.CommandName = "InsertJSONObjectProperty";
            //this.SelectionName = "Insert JSON Object Property";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //Action<JToken> addPropertyFunc = new Action<JToken>((searchResult) =>
            //{
            //    if (!(searchResult.Parent is JProperty))
            //    {
            //        throw new Exception("Extraction Result is not JSON Object and can not Add JSON Property. Value: '" + searchResult.ToString() + "'");
            //    }

            //    var propertyValue = this.GetJSONValue(nameof(v_Value), nameof(v_ValueType), "Insert", engine);
            //    var propertyName = v_PropertyName.ExpandValueOrUserVariable(engine);
            //    searchResult.Parent.AddAfterSelf(new JProperty(propertyName, propertyValue));
            //});
            //this.JSONModifyByJSONPath(nameof(v_Json), nameof(v_JsonExtractor), addPropertyFunc, addPropertyFunc, engine);
            (var root, var json, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);

            var propName = this.ExpandValueOrUserVariable(nameof(v_PropertyName), "Property Name", engine);
            var v = this.ExpandValueOrVariableValueAsJSONSupportedValueInJSONValue(engine);

            if (json is JObject obj)
            {
                // json is JObject -> Add or Insert

                if (string.IsNullOrEmpty(v_PropertyNameInsertPosition))
                {
                    obj.Add(propName, v);
                }
                else
                {
                    var insertPos = this.ExpandValueOrUserVariable(nameof(v_PropertyNameInsertPosition), "Insert Position", engine);
                    if (obj.ContainsKey(insertPos))
                    {
                        obj[insertPos].Parent.AddAfterSelf(new JProperty(propName, v));
                    }
                    else
                    {
                        throw new Exception($"Specified property does not Exists. Property: '{v_PropertyNameInsertPosition}', Expand: '{insertPos}'");
                    }
                }
                
                root.ToString().StoreInUserVariable(engine, v_Json);
            }
            //else if (json?.Parent is JProperty)
            //{
            //    // json is JProperty -> insert
            //    json.Parent.AddAfterSelf(new JProperty(propName, v));
            //    //root.ToString().StoreInUserVariable(engine, v_Json);
            //    this.StoreJSONInUserVariable(root, engine);
            //}
            else
            {
                throw new Exception($"Extraction Result is NOT JSON Object or JSON Object Property. Result: '{json}', JSONPath: '{v_JsonExtractor}'");
            }
        }
    }
}
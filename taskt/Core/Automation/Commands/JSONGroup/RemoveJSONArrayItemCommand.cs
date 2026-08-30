using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Remove JSON Array Item")]
    [Attributes.ClassAttributes.Description("This command allows you to remove item to JSON Array.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RemoveJSONArrayItemCommand : AJSONJSONPathCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_BothJSONName))]
        //[PropertyDescription("JSON Array Variable Name")]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ArrayIndex))]
        //[PropertyDescription("Index to Remove")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        ////[SampleUsage("**0** or **1** or **{{{vIndex}}}**")]
        //[PropertyDetailSampleUsage("**0**", "Specify the First Index to be Removed")]
        //[PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index to Remove")]
        //[PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index to Remove")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "Index")]
        //[PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyParameterOrder(8000)]
        public string v_Index { get; set; }

        public RemoveJSONArrayItemCommand()
        {
            //this.CommandName = "RemoveJSONArrayItem";
            //this.SelectionName = "Remove JSON Array Item";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //Action<JToken> removeItemFunc = new Action<JToken>((searchResult) =>
            //{
            //    if (!(searchResult is JArray))
            //    {
            //        throw new Exception("Extraction Result is not JSON Array and can not Add Item. Value: '" + searchResult.ToString() + "'");
            //    }
            //    JArray ary = (JArray)searchResult;

            //    var index = this.ExpandValueOrUserVariableAsInteger(nameof(v_Index), engine);

            //    if ((index < 0) && (index > ary.Count))
            //    {
            //        throw new Exception("Index is Out of Range. Value: " + index);
            //    }

            //    ary.RemoveAt(index);
            //});
            //this.JSONModifyByJSONPath(nameof(v_Json), nameof(v_JsonExtractor), removeItemFunc, removeItemFunc, engine);

            (var root, var json, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);
            if (json is JArray ary)
            {
                if (string.IsNullOrEmpty(v_Index))
                {
                    v_Index = "-1";
                }
                var index = this.ExpandValueOrUserVariableAsInteger(nameof(v_Index), "Index", engine);
                if (index < 0)
                {
                    index += ary.Count;
                }
                if (index < 0 || index > ary.Count)
                {
                    throw new Exception($"Index is Out of Range. Value: '{v_Index}', Expand Value: '{index}'");
                }
                else
                {
                    ary.RemoveAt(index);
                    this.StoreJSONInUserVariable(root, engine);
                }
            }
            else
            {
                throw new Exception($"Extraction Result is NOT JSON Array. Result: '{json}', JSONPath: '{v_JsonExtractor}'");
            }
        }
    }
}
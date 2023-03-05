using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Insert JSON Array Item")]
    [Attributes.ClassAttributes.Description("This command allows you to insert item to JSON Array.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class InsertJSONArrayItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_BothJSONName))]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        [PropertyDescription("JSON Array Variable Name")]
        public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ArrayIndex))]
        [PropertyIsOptional(true, "Last Index")]
        //[PropertyDescription("Index to Insert")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        ////[SampleUsage("**0** or **1** or **{{{vIndex}}}**")]
        //[PropertyDetailSampleUsage("**0**", "Specify the First Index to be Inserted")]
        //[PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index to Insert")]
        //[PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index to Insert")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "Index")]
        public string v_InsertIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        public string v_InsertItem { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        public string v_ValueType { get; set; }

        public InsertJSONArrayItemCommand()
        {
            //this.CommandName = "InsertJSONArrayItem";
            //this.SelectionName = "Insert JSON Array Item";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Action<JToken> addItemFunc = new Action<JToken>((searchResult) =>
            {
                if (!(searchResult is JArray))
                {
                    throw new Exception("Extraction Result is not JSON Array and can not Add Item. Value: '" + searchResult.ToString() + "'");
                }
                JArray ary = (JArray)searchResult;

                var insertItem = this.GetJSONValue(nameof(v_InsertItem), nameof(v_ValueType), "Insert", engine);

                if (String.IsNullOrEmpty(v_InsertIndex))
                {
                    v_InsertIndex = ary.Count.ToString();
                }
                var index = this.ConvertToUserVariableAsInteger(nameof(v_InsertIndex), engine);

                if ((index < 0) && (index > ary.Count))
                {
                    throw new Exception("Index is Out of Range. Value: " + index);
                }

                ary.Insert(index, JToken.FromObject(insertItem));
            });
            this.JSONModifyByJSONPath(nameof(v_InputValue), nameof(v_JsonExtractor), addItemFunc, addItemFunc, engine);
        }
    }
}
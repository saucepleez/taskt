using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert JSON To List")]
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON Array into a List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON Array into a List")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertJSONToListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_InputJSONName))]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_applyToVariableName { get; set; }

        public ConvertJSONToListCommand()
        {
            //this.CommandName = "ConvertJSONToListCommand";
            //this.SelectionName = "Convert JSON To List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Action<JObject> objFunc = new Action<JObject>((obj) =>
            {
                List<string> resultList = new List<string>();
                foreach (var result in obj)
                {
                    resultList.Add(result.Value.ToString());
                }
                resultList.StoreInUserVariable(engine, v_applyToVariableName);
            });
            Action<JArray> aryFunc = new Action<JArray>((ary) =>
            {
                List<string> resultList = new List<string>();
                foreach (var result in ary)
                {
                    resultList.Add(result.ToString());
                }
                resultList.StoreInUserVariable(engine, v_applyToVariableName);
            });
            this.JSONProcess(nameof(v_InputValue), objFunc, aryFunc, engine);
        }
    }
}
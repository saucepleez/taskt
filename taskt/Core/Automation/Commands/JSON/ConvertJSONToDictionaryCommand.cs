using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert JSON To Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON to Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON to Dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertJSONToDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_InputJSONName))]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public string v_applyToVariableName { get; set; }

        public ConvertJSONToDictionaryCommand()
        {
            //this.CommandName = "ConvertJSONToDictionaryCommand";
            //this.SelectionName = "Convert JSON To Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Action<JObject> objFunc = new Action<JObject>((obj) =>
            {
                Dictionary<string, string> resultDic = new Dictionary<string, string>();
                foreach (var result in obj)
                {
                    resultDic.Add(result.Key, result.Value.ToString());
                }
                resultDic.StoreInUserVariable(engine, v_applyToVariableName);
            });
            Action<JArray> aryFunc = new Action<JArray>((ary) =>
            {
                Dictionary<string, string> resultDic = new Dictionary<string, string>();
                for (int i = 0; i < ary.Count; i++)
                {
                    resultDic.Add("key" + i.ToString(), ary[i].ToString());
                }
                resultDic.StoreInUserVariable(engine, v_applyToVariableName);
            });
            this.JSONProcess(nameof(v_InputValue), objFunc, aryFunc, engine);
        }
    }
}
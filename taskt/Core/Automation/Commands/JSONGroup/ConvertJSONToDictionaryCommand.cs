using Newtonsoft.Json.Linq;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert JSON To Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON to Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON to Dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertJSONToDictionaryCommand : AJSONGetFromJContainerCommands, IDictionaryResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_InputJSONName))]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public override string v_Result { get; set; }

        public ConvertJSONToDictionaryCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //Action<JObject> objFunc = new Action<JObject>((obj) =>
            //{
            //    var resultDic = new Dictionary<string, string>();
            //    foreach (var result in obj)
            //    {
            //        resultDic.Add(result.Key, result.Value.ToString());
            //    }
            //    //resultDic.StoreInUserVariable(engine, v_applyToVariableName);
            //    this.StoreDictionaryInUserVariable(resultDic, nameof(v_Result), engine);
            //});
            //Action<JArray> aryFunc = new Action<JArray>((ary) =>
            //{
            //    var resultDic = new Dictionary<string, string>();
            //    for (int i = 0; i < ary.Count; i++)
            //    {
            //        resultDic.Add("key" + i.ToString(), ary[i].ToString());
            //    }
            //    //resultDic.StoreInUserVariable(engine, v_applyToVariableName);
            //    this.StoreDictionaryInUserVariable(resultDic, nameof(v_Result), engine);
            //});
            //this.JSONProcess(nameof(v_Json), objFunc, aryFunc, engine);

            //(_, var jCon, _) = this.ExpandValueOrUserVariableAsJSON(nameof(v_Json), engine);
            //(_, var jCon, _) = this.ExpandValueOrUserVariableAsJSON(engine);
            (_, var jCon, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);

            var res = this.CreateEmptyDictionary();
            if (jCon is JObject obj)
            {
                foreach(var item in obj)
                {
                    res.Add(item.Key, item.Value.ToString());
                }
            }
            else if (jCon is JArray ary)
            {
                int i = 0;
                foreach (var item in ary)
                {
                    res.Add($"key{i}", item.ToString());
                }
            }
            else if (jCon is JValue va)
            {
                res.Add("key0", va.ToString());
            }
            else
            {
                throw new Exception($"Extraction Result is NOT Supported Type. Result: '{jCon}', JSONPath: '{v_JsonExtractor}'");
            }
            this.StoreDictionaryInUserVariable(res, engine);
        }
    }
}
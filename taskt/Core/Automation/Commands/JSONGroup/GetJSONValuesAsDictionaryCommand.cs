using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.CommandSettings("Get JSON Values As Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to Get JSON Values From JSON and Result Values is Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Values From JSON and Result Values is Dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetJSONValuesAsDictionaryCommand : AJSONGetFromJContainerCommands, IDictionaryResultProperties
    {
        //[XmlAttribute]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public override string v_Result { get; set; }

        public GetJSONValuesAsDictionaryCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (_, var root, _) = this.ExpandValueOrUserVariableAsJSON(engine);
            var jsonPath = v_JsonExtractor.ExpandValueOrUserVariable(engine);

            var elems = root.SelectTokens(jsonPath);

            var dic = this.CreateEmptyDictionary();
            foreach(var e in elems)
            {
                dic.Add(e.Path, e.ToString());
            }
            this.StoreDictionaryInUserVariable(dic, engine);
        }
    }
}
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Covnert Dictionary To Visualized Text")]
    [Attributes.ClassAttributes.Description("This command allows you to Covnert Dictionary to Visualized Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Covnert Dictionary to Visualized Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDictionaryToVisualizedTextCommand : ADictionaryGetFromDictionaryCommands
    {
        //public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public override string v_Result { get; set; }

        public ConvertDictionaryToVisualizedTextCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var dic = this.ExpandUserVariableAsDictionary(engine);

            var maxValueLength = 0;
            var maxKeyLength = 0;
            foreach (var item in dic)
            {
                var vs = item.Value.Length;
                if (maxValueLength < vs)
                {
                    maxValueLength = vs;
                }
                var ks = item.Key.Length;
                if (maxKeyLength < ks)
                {
                    maxKeyLength = ks;
                }
            }
            if (maxValueLength < 5)
            {
                maxValueLength = 5;  // Value
            }
            if (maxKeyLength < 3)
            {
                maxKeyLength = 3;   // Key
            }

            int size = dic.Count;
            int indexDigits = (int)Math.Log10(size) + 1;
            if (indexDigits < 3)
            {
                indexDigits = 3; // No.
            }

            string indexFormat = $"{{0,{indexDigits}}}";
            string keyFormat = $"{{0,-{maxKeyLength}}}";
            string valueFormat = $"{{0,-{maxValueLength}}}";

            string txt = $"{string.Format(indexFormat, "No.")} | {string.Format(keyFormat, "Key")} | {string.Format(valueFormat, "Value")}\r\n";
            for (int i = 0; i < indexDigits; i++)
            {
                txt += "-";
            }
            txt += "-|-";
            for (int i = 0; i < maxKeyLength; i++)
            {
                txt += "-";
            }
            txt += "-|-";
            for (int i=0; i< maxValueLength; i++)
            {
                txt += "-";
            }
            txt += "\r\n";

            int cnt = 0;
            foreach (var item in dic)
            {
                txt += $"{string.Format(indexFormat, cnt)} | {string.Format(keyFormat, item.Key)} | {string.Format(valueFormat, item.Value)}\r\n";
                cnt++;
            }

            txt.StoreInUserVariable(engine, v_Result);
        }
    }
}
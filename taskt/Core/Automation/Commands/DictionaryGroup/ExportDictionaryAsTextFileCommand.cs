using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.CommandSettings("Export Dictionary As Text File")]
    [Attributes.ClassAttributes.Description("This command allows you to Export Dictionary as Text File.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Export Dictionary as Text File.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExportDictionaryAsTextFileCommand : ADictionaryInputDictionaryCommands
    {
        //public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "txt")]
        [PropertyParameterOrder(6000)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Export Header")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyValidationRule("Export Header", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(7000)]
        public string v_ExportHeader { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Export Index")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyValidationRule("Export Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(7001)]
        public string v_ExportIndex { get; set; }

        public ExportDictionaryAsTextFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var dic = this.ExpandUserVariableAsDictionary(engine);

            var exportHeader = this.ExpandValueOrUserVariableAsYesNo(nameof(v_ExportHeader), engine);
            var exportIndex = this.ExpandValueOrUserVariableAsYesNo(nameof(v_ExportIndex), engine);

            string txt = "";
            if (exportHeader)
            {
                if (exportIndex)
                {
                    txt = "index,key,value\r\n";
                }
                else
                {
                    txt = "key,value\r\n";
                }
            }
            Func<string, string, int, string> textAction;
            if (exportIndex)
            {
                textAction = new Func<string, string, int, string>((key, value, idx) =>
                {
                    return $"{idx},{key},{value}\r\n";
                });
            }
            else
            {
                textAction = new Func<string, string, int, string>((key, value, idx) =>
                {
                    return $"{key},{value}\r\n";
                });
            }

            int index = 0;
            foreach (var item in dic)
            {
                txt += textAction(item.Key, item.Value, index);
                index++;
            }

            var writeText = new WriteTextFileCommand()
            {
                v_FilePath = this.v_FilePath,
                v_TextToWrite = txt.Trim(),
            };
            writeText.RunCommand(engine);
        }
    }
}
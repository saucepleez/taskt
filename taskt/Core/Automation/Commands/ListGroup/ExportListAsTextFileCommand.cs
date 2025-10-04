using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.CommandSettings("Export List As Text File")]
    [Attributes.ClassAttributes.Description("This command allows you to Export List as Text File.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Export List as Text File.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExportListAsTextFileCommand : AListInputListCommands
    {
        //[XmlAttribute]
        //public string v_List { get; set; }

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

        public ExportListAsTextFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var list = this.ExpandUserVariableAsList(engine);

            string txt = "";

            var exportHeader = this.ExpandValueOrUserVariableAsYesNo(nameof(v_ExportHeader), engine);
            var exportIndex = this.ExpandValueOrUserVariableAsYesNo(nameof(v_ExportIndex), engine);

            if (exportHeader)
            {
                if (exportIndex)
                {
                    txt = "index,value\r\n";
                }
                else
                {
                    txt = "value\r\n";
                }
            }

            Func<string, int, string> textAction;
            if (exportIndex)
            {
                textAction = new Func<string, int, string>((v, i) =>
                {
                    return $"{i},{v}\r\n";
                });
            }
            else
            {
                textAction = new Func<string, int, string>((v, i) =>
                {
                    return $"{v}\r\n";
                });
            }

            for (int i = 0; i < list.Count; i++)
            {
                txt += textAction(list[i], i);
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
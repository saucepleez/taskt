using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert List To Text")]
    [Attributes.ClassAttributes.Description("This command allows you to Convert List to Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Convert List to Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertListToTextCommand : AListGetFromListCommands
    {
        //[XmlAttribute]
        //public string v_List { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

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

        public ConvertListToTextCommand()
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

            txt.Trim().StoreInUserVariable(engine, v_Result);
        }
    }
}
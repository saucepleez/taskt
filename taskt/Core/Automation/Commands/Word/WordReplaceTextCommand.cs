using System;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Word;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to replace text in a Word document.")]
    [Attributes.ClassAttributes.CommandSettings("Replace Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to replace text in a document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordReplaceTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Text to Find")]
        [InputSpecification("Text to Find", true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text to Find")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text to Find")]
        [PropertyValidationRule("Text to Find", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Text to Find")]
        public string v_FindText { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Text to Replace with")]
        [InputSpecification("Text to Replace with", true)]
        [PropertyDetailSampleUsage("**Hi!**", PropertyDetailSampleUsage.ValueType.Value, "Text to Replace with")]
        [PropertyDetailSampleUsage("**{{{vNewText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text to Replace with")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(true, "Text to Replace with")]
        public string v_ReplaceWithText { get; set; }

        public WordReplaceTextCommand()
        {
            //this.CommandName = "WordReplaceTextCommand";
            //this.SelectionName = "Replace Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Engine.AutomationEngineInstance)sender;

            (var _, var wordDocument) = v_InstanceName.GetWordInstanceAndDocument(engine);

            var vFindText = v_FindText.ConvertToUserVariable(engine);
            var vReplaceWithText = v_ReplaceWithText.ConvertToUserVariable(engine);

            Range range = wordDocument.Content;

            //replace text
            Find findObject = range.Find;
            findObject.ClearFormatting();
            findObject.Text = vFindText;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = vReplaceWithText;

            object replaceAll = WdReplace.wdReplaceAll;
            findObject.Execute(Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                               Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                               ref replaceAll, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
    }
}
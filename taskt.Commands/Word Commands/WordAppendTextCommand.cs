using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using Application = Microsoft.Office.Interop.Word.Application;
using Group = taskt.Core.Attributes.ClassAttributes.Group;

namespace taskt.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command appends text to a Word Document.")]

    public class WordAppendTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance || {vWordInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text")]
        [InputSpecification("Enter the text to append to the Document.")]
        [SampleUsage("Hello World || {vText}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TextToSet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Font Name")]
        [PropertyUISelectionOption("Arial")]
        [PropertyUISelectionOption("Calibri")]
        [PropertyUISelectionOption("Helvetica")]
        [PropertyUISelectionOption("Times New Roman")]
        [PropertyUISelectionOption("Verdana")]
        [InputSpecification("Select or provide a valid font name.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Font Size")]
        [PropertyUISelectionOption("10")]
        [PropertyUISelectionOption("11")]
        [PropertyUISelectionOption("12")]
        [PropertyUISelectionOption("14")]
        [PropertyUISelectionOption("16")]
        [PropertyUISelectionOption("18")]
        [PropertyUISelectionOption("20")]
        [InputSpecification("Select or provide a valid font size.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontSize { get; set; }

        [XmlAttribute]
        [PropertyDescription("Bold")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the text font should be bold.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontBold { get; set; }

        [XmlAttribute]
        [PropertyDescription("Italic")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the text font should be italic.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontItalic { get; set; }

        [XmlAttribute]
        [PropertyDescription("Underline")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the text font should be underlined.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontUnderline { get; set; }

        public WordAppendTextCommand()
        {
            CommandName = "WordAppendTextCommand";
            SelectionName = "Append Text";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
            v_FontName = "Calibri";
            v_FontSize = "11";
            v_FontBold = "No";
            v_FontItalic = "No";
            v_FontUnderline = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vText = v_TextToSet.ConvertToUserVariable(engine);
            var wordObject = engine.GetAppInstance(vInstance);

            Application wordInstance = (Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;

            Paragraph paragraph = wordDocument.Content.Paragraphs.Add();
            paragraph.Range.Text = vText;
            paragraph.Range.Font.Name = v_FontName;
            paragraph.Range.Font.Size = float.Parse(v_FontSize);

            if (v_FontBold == "Yes")
                paragraph.Range.Font.Bold = 1;
            else 
                paragraph.Range.Font.Bold = 0;

            if (v_FontItalic == "Yes")
                paragraph.Range.Font.Italic = 1;
            else 
                paragraph.Range.Font.Italic = 0;

            if (v_FontUnderline == "Yes")
                paragraph.Range.Font.Underline = WdUnderline.wdUnderlineSingle;
            else 
                paragraph.Range.Font.Underline = WdUnderline.wdUnderlineNone;

            paragraph.Range.InsertParagraphAfter();
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontSize", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontBold", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontItalic", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontUnderline", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Append '{v_TextToSet}' - Instance Name '{v_InstanceName}']";
        }
    }
}
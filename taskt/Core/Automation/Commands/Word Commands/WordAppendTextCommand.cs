using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command appends text to a word document.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to append text to a specific document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    public class WordAppendTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **wordInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Text Variable Name to Set")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text value that will be set.")]
        [Attributes.PropertyAttributes.SampleUsage("Hello World or [vText]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_TextToSet { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select or Enter the text font name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Arial")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Calibri")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Helvetica")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Times New Roman")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Verdana")]
        [Attributes.PropertyAttributes.InputSpecification("Specify the font name.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Arial**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FontName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select or Enter the text font size")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("10")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("11")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("12")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("14")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("16")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("18")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("20")]
        [Attributes.PropertyAttributes.InputSpecification("Specify the font name.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **14**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FontSize { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Bold")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the text font should be bold.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FontBold { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Italic")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the text font should be italic.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FontItalic { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Underline")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the text font should be underlined.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FontUnderline { get; set; }

        public WordAppendTextCommand()
        {
            this.CommandName = "WordAppendTextCommand";
            this.SelectionName = "Append Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_FontName = "Calibri";
            this.v_FontSize = "11";
            this.v_FontBold = "No";
            this.v_FontItalic = "No";
            this.v_FontUnderline = "No";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vText = v_TextToSet.ConvertToUserVariable(engine);
            var wordObject = engine.GetAppInstance(vInstance);

            Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;

            Paragraph paragraph = wordDocument.Content.Paragraphs.Add();
            paragraph.Range.Text = vText;
            paragraph.Range.Font.Name = v_FontName;
            paragraph.Range.Font.Size = float.Parse(v_FontSize);
            if (v_FontBold == "Yes")
                paragraph.Range.Font.Bold = 1;
            else paragraph.Range.Font.Bold = 0;
            if (v_FontItalic == "Yes")
                paragraph.Range.Font.Italic = 1;
            else paragraph.Range.Font.Italic = 0;
            if (v_FontUnderline == "Yes")
                paragraph.Range.Font.Underline = WdUnderline.wdUnderlineSingle;
            else paragraph.Range.Font.Underline = WdUnderline.wdUnderlineNone;

            paragraph.Range.InsertParagraphAfter();

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
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
            return base.GetDisplayValue() + " ['" + v_TextToSet + "' To Instance Name: '" + v_InstanceName + "']";
        }
    }
}
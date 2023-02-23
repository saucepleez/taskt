using System;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Word;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command appends text to a word document.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to append text to a specific document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordAppendTextCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        //[SampleUsage("**myInstance** or **wordInstance**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Word)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text to Set")]
        [InputSpecification("Text to Set", true)]
        [Remarks("")]
        //[SampleUsage("Hello World or {vText}")]
        [PropertyDetailSampleUsage("**Hello World**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyDisplayText(true, "Text")]
        public string v_TextToSet { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Text Font Name")]
        [PropertyUISelectionOption("Arial")]
        [PropertyUISelectionOption("Calibri")]
        [PropertyUISelectionOption("Helvetica")]
        [PropertyUISelectionOption("Times New Roman")]
        [PropertyUISelectionOption("Verdana")]
        //[InputSpecification("Specify the font name.")]
        //[SampleUsage("Select **Arial**")]
        //[Remarks("")]
        [PropertyFirstValue("Calibri")]
        [PropertyIsOptional(true, "Calibri")]
        public string v_FontName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Font Size")]
        [InputSpecification("Font Size", true)]
        //[SampleUsage("Select **14**")]
        [PropertyFirstValue("11")]
        [PropertyValidationRule("Font Size", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.GreaterThanZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyIsOptional(true, "11")]
        [PropertyDisplayText(false, "")]
        public string v_FontSize { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Bold")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        //[InputSpecification("Specify whether the text font should be bold.")]
        //[SampleUsage("Select **Yes** or **No**")]
        //[Remarks("")]
        [PropertyFirstValue("No")]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "")]
        public string v_FontBold { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Italic")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        //[InputSpecification("Specify whether the text font should be italic.")]
        //[SampleUsage("Select **Yes** or **No**")]
        //[Remarks("")]
        [PropertyFirstValue("No")]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "")]
        public string v_FontItalic { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Underline")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        //[InputSpecification("Specify whether the text font should be underlined.")]
        //[SampleUsage("Select **Yes** or **No**")]
        //[Remarks("")]
        [PropertyFirstValue("No")]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "")]
        public string v_FontUnderline { get; set; }

        public WordAppendTextCommand()
        {
            this.CommandName = "WordAppendTextCommand";
            this.SelectionName = "Append Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            //this.v_FontName = "Calibri";
            //this.v_FontSize = "11";
            //this.v_FontBold = "No";
            //this.v_FontItalic = "No";
            //this.v_FontUnderline = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var wordObject = engine.GetAppInstance(vInstance);
            //Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;
            //Document wordDocument = wordInstance.ActiveDocument;
            (var _, var wordDocument) = v_InstanceName.GetWordInstanceAndDocument(engine);

            var vText = v_TextToSet.ConvertToUserVariable(engine);

            Paragraph paragraph = wordDocument.Content.Paragraphs.Add();
            paragraph.Range.Text = vText;
            paragraph.Range.Font.Name = v_FontName;
            paragraph.Range.Font.Size = float.Parse(v_FontSize);

            //if (v_FontBold == "Yes")
            if (this.GetUISelectionValue(nameof(v_FontBold), engine) == "yes")
            {
                paragraph.Range.Font.Bold = 1;
            }
            else
            {
                paragraph.Range.Font.Bold = 0;
            }
            //if (v_FontItalic == "Yes")
            if (this.GetUISelectionValue(nameof(v_FontItalic), engine) == "yes")
            {
                paragraph.Range.Font.Italic = 1;
            }
            else
            {
                paragraph.Range.Font.Italic = 0;
            }
            //if (v_FontUnderline == "Yes")
            if (this.GetUISelectionValue(nameof(v_FontUnderline), engine) == "yes")
            {
                paragraph.Range.Font.Underline = WdUnderline.wdUnderlineSingle;
            }
            else
            {
                paragraph.Range.Font.Underline = WdUnderline.wdUnderlineNone;
            }

            paragraph.Range.InsertParagraphAfter();
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.Word);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontSize", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontBold", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontItalic", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FontUnderline", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultWordInstanceName;
        //    }

        //    return RenderedControls;
        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " ['" + v_TextToSet + "' To Instance Name: '" + v_InstanceName + "']";
        //}
    }
}
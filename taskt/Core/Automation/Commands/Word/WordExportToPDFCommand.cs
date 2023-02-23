using System;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Word;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to export a Word document to a PDF.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save a document to a PDF.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordExportToPDFCommand : ScriptCommand
    {
        //[XmlAttribute]
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
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Path of the New PDF")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Path of the PDF", true)]
        //[SampleUsage("C:\\temp\\myfile.pdf or {vWordPDFFilePath}")]
        [PropertyDetailSampleUsage("**C:\\temp\\myfile.pdf**", PropertyDetailSampleUsage.ValueType.Value, "Path")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path")]
        [PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Path")]
        public string v_FileName { get; set; }

        public WordExportToPDFCommand()
        {
            this.CommandName = "WordExportToPDFCommand";
            this.SelectionName = "Export To PDF";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Engine.AutomationEngineInstance)sender;

            //convert variables
            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            ////get word app object
            //var wordObject = engine.GetAppInstance(vInstance);
            ////convert object
            //Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;
            //Document wordDocument = wordInstance.ActiveDocument;

            (var _, var wordDocument) = v_InstanceName.GetWordInstanceAndDocument(engine);

            var fileName = v_FileName.ConvertToUserVariable(engine);

            object fileFormat = WdSaveFormat.wdFormatPDF;
            wordDocument.SaveAs(fileName, ref fileFormat, Type.Missing, Type.Missing,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    UI.CustomControls.CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultWordInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Save To '" + v_FileName + "', Instance Name: '" + v_InstanceName + "']";
        //}
    }
}
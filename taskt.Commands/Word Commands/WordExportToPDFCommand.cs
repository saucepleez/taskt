using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace taskt.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command exports a Word Document to a PDF file.")]

    public class WordExportToPDFCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance || {vWordInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("PDF Location")]
        [InputSpecification("Enter or Select the path of the folder to export the PDF to.")]
        [SampleUsage(@"C:\temp || {vFolderPath} || {ProjectPath}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_FolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("PDF File Name")]
        [InputSpecification("Enter or Select the name of the PDF file.")]
        [SampleUsage("myFile.pdf || {vFilename}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FileName { get; set; }

        public WordExportToPDFCommand()
        {
            CommandName = "WordExportToPDFCommand";
            SelectionName = "Export To PDF";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vFileName = v_FileName.ConvertToUserVariable(engine);
            var vFolderPath = v_FolderPath.ConvertToUserVariable(engine);

            //get word app object
            var wordObject = engine.GetAppInstance(vInstance);

            //convert object
            Application wordInstance = (Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;
            
            object fileFormat = WdSaveFormat.wdFormatPDF;
            string pdfPath = Path.Combine(vFolderPath, vFileName);
            wordDocument.SaveAs(pdfPath, ref fileFormat, Type.Missing, Type.Missing,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FolderPath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Export to '{v_FolderPath}\\{v_FileName}' - Instance Name '{v_InstanceName}']";
        }
    }
}
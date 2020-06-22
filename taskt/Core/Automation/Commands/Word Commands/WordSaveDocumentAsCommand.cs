using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Application = Microsoft.Office.Interop.Word.Application;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command saves a Word Document to a specific file.")]

    public class WordSaveDocumentAsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance || {vWordInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Document Location")]
        [InputSpecification("Enter or Select the path of the folder to save the Document in.")]
        [SampleUsage(@"C:\temp || {vFolderPath} || {ProjectPath}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_FolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Document File Name")]
        [InputSpecification("Enter or Select the name of the Document file.")]
        [SampleUsage("myFile.docx || {vFilename}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FileName { get; set; }

        public WordSaveDocumentAsCommand()
        {
            CommandName = "WordSaveDocumentAsCommand";
            SelectionName = "Save Document As";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vFileName = v_FileName.ConvertToUserVariable(engine);
            var vFolderPath = v_FolderPath.ConvertToUserVariable(sender);

            //get word app object
            var wordObject = engine.GetAppInstance(vInstance);

            //convert object
            Application wordInstance = (Application)wordObject;
            string filePath = Path.Combine(vFolderPath, vFileName);

            //overwrite and save
            wordInstance.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            wordInstance.ActiveDocument.SaveAs(filePath);
            wordInstance.DisplayAlerts = WdAlertLevel.wdAlertsAll;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FolderPath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Save to '{v_FolderPath}\\{v_FileName}' - Instance Name '{v_InstanceName}']";
        }
    }
}
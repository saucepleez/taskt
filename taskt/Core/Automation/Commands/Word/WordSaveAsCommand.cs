using System;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Word;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to save an Word document.")]
    [Attributes.ClassAttributes.CommandSettings("Save Document As")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save a document to a file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordSaveDocumentAsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "docx")]
        public string v_FileName { get; set; }

        public WordSaveDocumentAsCommand()
        {
            //this.CommandName = "WordSaveAsCommand";
            //this.SelectionName = "Save Document As";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Engine.AutomationEngineInstance)sender;

            //string fileName;
            //if (FilePathControls.ContainsFileCounter(v_FileName, engine))
            //{
            //    fileName = FilePathControls.FormatFilePath_ContainsFileCounter(v_FileName, engine, "docx");
            //}
            //else
            //{
            //    fileName = FilePathControls.FormatFilePath_NoFileCounter(v_FileName, engine, "docx");
            //}
            string fileName = this.ConvertToUserVariableAsFilePath(nameof(v_FileName), engine);

            var wordInstance = v_InstanceName.GetWordInstance(engine);

            //overwrite and save
            wordInstance.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            wordInstance.ActiveDocument.SaveAs(fileName);
            wordInstance.DisplayAlerts = WdAlertLevel.wdAlertsAll;
        }
    }
}
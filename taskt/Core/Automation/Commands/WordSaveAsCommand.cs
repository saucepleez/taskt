using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to save an Word document.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save a document to a file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    public class WordSaveAsCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **wordInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path of the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.docx or [vWordFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FileName { get; set; }

        public WordSaveAsCommand()
        {
            this.CommandName = "WordSaveAsCommand";
            this.SelectionName = "Save Document As";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //convert variables
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var fileName = v_FileName.ConvertToUserVariable(engine);

            //get word app object
            var wordObject = engine.GetAppInstance(vInstance);

            //convert object
            Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;

            //overwrite and save
            wordInstance.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            wordInstance.ActiveDocument.SaveAs(fileName);
            wordInstance.DisplayAlerts = WdAlertLevel.wdAlertsAll;

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Save To '" + v_FileName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}
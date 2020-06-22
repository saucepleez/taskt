using System;
using System.Collections.Generic;
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
    [Description("This command opens a Word Document.")]

    public class WordOpenDocumentCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance || {vWordInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Document File Path")]
        [InputSpecification("Enter or Select the path to the Document file.")]
        [SampleUsage(@"C:\temp\myfile.docx || {vFilePath} || {ProjectPath}\myfile.docx")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        public WordOpenDocumentCommand()
        {
            CommandName = "WordOpenDocumentCommand";
            SelectionName = "Open Document";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vFilePath = v_FilePath.ConvertToUserVariable(sender);
            var wordObject = engine.GetAppInstance(vInstance);

            Application wordInstance = (Application)wordObject;
            wordInstance.Documents.Open(vFilePath);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Open '{v_FilePath}' - Instance Name '{v_InstanceName}']";
        }
    }
}
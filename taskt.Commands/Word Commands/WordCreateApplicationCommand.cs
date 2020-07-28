using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [Description("This command creates a Word Instance.")]

    public class WordCreateApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter a unique name that will represent the application instance.")]
        [SampleUsage("MyWordInstance")]
        [Remarks("This unique name allows you to refer to the instance by name in future commands, " +
                 "ensuring that the commands you specify run against the correct application.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("New/Open Document")]
        [PropertyUISelectionOption("New Document")]
        [PropertyUISelectionOption("Open Document")]
        [InputSpecification("Indicate whether to create a new Document or to open an existing Document.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_NewOpenDocument { get; set; }

        [XmlAttribute]
        [PropertyDescription("Document File Path")]
        [InputSpecification("Enter or Select the path to the Document file.")]
        [SampleUsage(@"C:\temp\myfile.docx || {vFilePath} || {ProjectPath}\myfile.docx")]
        [Remarks("This input should only be used for opening existing Documents.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Visible")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the Word automation should be visible or not.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Visible { get; set; }

        [XmlAttribute]
        [PropertyDescription("Close All Existing Word Instances")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether to close any existing Word instances before executing Word Automation.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_CloseAllInstances { get; set; }

        public WordCreateApplicationCommand()
        {
            CommandName = "WordCreateApplicationCommand";
            SelectionName = "Create Word Application";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
            v_NewOpenDocument = "New Workbook";
            v_Visible = "No";
            v_CloseAllInstances = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vFilePath = v_FilePath.ConvertToUserVariable(engine);

            if (v_CloseAllInstances == "Yes")
            {
                var processes = Process.GetProcessesByName("winword");
                foreach (var prc in processes)
                {
                    prc.Kill();
                }
            }

            var newWordSession = new Application();

            if (v_Visible == "Yes")
                newWordSession.Visible = true;
            else
                newWordSession.Visible = false;

            engine.AddAppInstance(v_InstanceName, newWordSession);

            if (v_NewOpenDocument == "New Document")
            {
                if (!string.IsNullOrEmpty(vFilePath))
                    throw new InvalidDataException("File path should not be provided for a new Word Document");
                else
                    newWordSession.Documents.Add();
            }
            else if (v_NewOpenDocument == "Open Document")
            {
                if (string.IsNullOrEmpty(vFilePath))
                    throw new NullReferenceException("File path for Word Document not provided");
                else
                    newWordSession.Documents.Open(vFilePath);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_NewOpenDocument", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_Visible", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CloseAllInstances", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_NewOpenDocument} - Visible '{v_Visible}' - Close Instances '{v_CloseAllInstances}' - New Instance Name '{v_InstanceName}']";
        }
    }
}

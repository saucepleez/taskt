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

namespace taskt.Commands
{
    [Serializable]
    [Group("File Operation Commands")]
    [Description("This command moves/copies a file to a specified destination.")]
    public class MoveCopyFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("File Operation Type")]
        [PropertyUISelectionOption("Move File")]
        [PropertyUISelectionOption("Copy File")]
        [InputSpecification("Specify whether you intend to move the file or copy the file.")]
        [SampleUsage("")]
        [Remarks("Moving will remove the file from the original path while Copying will not.")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Source File Path")]
        [InputSpecification("Enter or Select the path to the file.")]
        [SampleUsage(@"C:\temp\myfile.txt || {ProjectPath}\myfile.txt || {vTextFilePath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Destination File Path")]
        [InputSpecification("Enter or Select the new (destination) path to the file.")]
        [SampleUsage(@"C:\temp\new path || {ProjectPath}\new path || {vTextFolderPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Create Folder")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the directory should be created if it does not already exist.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Overwrite File")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the file should be overwritten if it already exists.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_OverwriteFile { get; set; }

        public MoveCopyFileCommand()
        {
            CommandName = "MoveCopyFileCommand";
            SelectionName = "Move/Copy File";
            CommandEnabled = true;
            CustomRendering = true;
            v_CreateDirectory = "Yes";
            v_OverwriteFile = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertToUserVariable(engine);
            var destinationFolder = v_DestinationDirectory.ConvertToUserVariable(engine);

            if ((v_CreateDirectory == "Yes") && (!Directory.Exists(destinationFolder)))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            //get source file name and info
            FileInfo sourceFileInfo = new FileInfo(sourceFile);

            //create destination
            var destinationPath = Path.Combine(destinationFolder, sourceFileInfo.Name);

            //delete if it already exists per user
            if (v_OverwriteFile == "Yes")
            {
                File.Delete(destinationPath);
            }

            if (v_OperationType == "Move File")
            {
                //move file
                File.Move(sourceFile, destinationPath);
            }
            else
            {
                //copy file
                File.Copy(sourceFile, destinationPath);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CreateDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_OverwriteFile", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_OperationType} From '{v_SourceFilePath}' to '{v_DestinationDirectory}']";
        }
    }
}
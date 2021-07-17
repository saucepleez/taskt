using System;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{


 
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command moves a file to a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to move a file to a new destination.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class MoveFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Indicate whether to move or copy the file (default is Move File)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Move File")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Copy File")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether you intend to move the file or copy the file.  Moving will remove the file from the original path while Copying will not.")]
        [Attributes.PropertyAttributes.SampleUsage("Select either **Move File** or **Copy File**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source file (ex. C:\\temp\\myfile.txt, {{{vFilePath}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the directory to move/copy to (ex. C:\\temp\\new_folder, {{{vFolderPath}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the new path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\new path\\** or **{{{vTextFolderPath}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Create folder if destination does not exist (default is No)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the directory should be created if it does not already exist.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Delete file if it already exists (default is No)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the file should be deleted first if it is already found to exist.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DeleteExisting { get; set; }


        public MoveFileCommand()
        {
            this.CommandName = "MoveFileCommand";
            this.SelectionName = "Move/Copy File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {

            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertToUserVariable(sender);
            var destinationFolder = v_DestinationDirectory.ConvertToUserVariable(sender);

            if (!System.IO.Directory.Exists(destinationFolder))
            {
                var vCreateDirectroy = v_CreateDirectory.ConvertToUserVariable(sender);
                if (String.IsNullOrEmpty(vCreateDirectroy))
                {
                    vCreateDirectroy = "No";
                }
                if (vCreateDirectroy.ToLower() == "yes")
                {
                    System.IO.Directory.CreateDirectory(destinationFolder);
                }
                else
                {
                    throw new Exception("destination folder does not exists: " + destinationFolder);
                }
            }

            //get source file name and info
            System.IO.FileInfo sourceFileInfo = new FileInfo(sourceFile);

            //create destination
            var destinationPath = System.IO.Path.Combine(destinationFolder, sourceFileInfo.Name);

            //delete if it already exists per user
            var vDeleteExistsint = v_DeleteExisting.ConvertToUserVariable(sender);
            if (vDeleteExistsint.ToLower() == "yes")
            {
                System.IO.File.Delete(destinationPath);
            }

            var vOperationType = v_OperationType.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(vOperationType))
            {
                vOperationType = "Move File";
            }
            if (vOperationType == "Move File")
            {
                //move file
                System.IO.File.Move(sourceFile, destinationPath);
            }
            else
            {
                //copy file
                System.IO.File.Copy(sourceFile, destinationPath);
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CreateDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DeleteExisting", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_OperationType + " from '" + v_SourceFilePath + "' to '" + v_DestinationDirectory + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(v_SourceFilePath))
            {
                this.validationResult += "Source file is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(v_SourceFilePath))
            {
                this.validationResult += "Move/copy directory is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
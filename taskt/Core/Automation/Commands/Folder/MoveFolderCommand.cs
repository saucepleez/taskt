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
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.Description("This command moves a folder to a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to move a folder to a new destination.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class MoveFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Indicate whether to move or copy the folder (default is Move Folder)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Move Folder")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Copy Folder")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether you intend to move the folder or copy the folder. Moving will remove the folder from the original path while Copying will not.")]
        [Attributes.PropertyAttributes.SampleUsage("Select either **Move Folder** or **Copy Folder**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source folder (ex. C:\\temp\\myfolder, {{{vFolderPath}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the folder.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myfolder** or **{{{vTextFolderPath}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the directory to move/copy to (ex. C:\\temp\\newfolder, {{{vFolderPath}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the new path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\newPath** or **{{{vTextFolderPath}}}**")]
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
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Delete folder if it already exists (defualt is No)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the folder should be deleted first if it is already found to exist.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DeleteExisting { get; set; }

        public MoveFolderCommand()
        {
            this.CommandName = "MoveFolderCommand";
            this.SelectionName = "Move/Copy Folder";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertToUserVariable(sender);
            var destinationFolder = v_DestinationDirectory.ConvertToUserVariable(sender);

            if (!System.IO.Directory.Exists(destinationFolder))
            {
                var vCreateDirectory = v_CreateDirectory.ConvertToUserVariable(sender);
                if (String.IsNullOrEmpty(vCreateDirectory))
                {
                    vCreateDirectory = "No";
                }
                if (vCreateDirectory.ToLower() == "yes")
                {
                    Directory.CreateDirectory(destinationFolder);
                }
            }

            //get source folder name and info
            DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);

            //create final path
            var finalPath = System.IO.Path.Combine(destinationFolder, sourceFolderInfo.Name);

            //delete if it already exists per user
            if (System.IO.Directory.Exists(finalPath))
            {
                var vDeleteExisting = v_DeleteExisting.ConvertToUserVariable(sender);
                if (String.IsNullOrEmpty(vDeleteExisting))
                {
                    vDeleteExisting = "No";
                }
                if (vDeleteExisting.ToLower() == "yes")
                {
                    Directory.Delete(finalPath, true);
                }
            }

            var vOperationType = v_OperationType.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(vOperationType))
            {
                vOperationType = "Move Folder";
            }
            if (vOperationType == "Move Folder")
            {
                //move folder
                Directory.Move(sourceFolder, finalPath);
            }
            else if (vOperationType == "Copy Folder")
            {
                //copy folder
                DirectoryCopy(sourceFolder, finalPath, true);   
            }

        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            Directory.GetParent(destDirName);
            if (!Directory.GetParent(destDirName).Exists)
            {
                throw new DirectoryNotFoundException(
                    "Destination directory does not exist or could not be found: "
                    + Directory.GetParent(destDirName));
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolderPath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CreateDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DeleteExisting", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_OperationType + " from '" + v_SourceFolderPath + "' to '" + v_DestinationDirectory + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_SourceFolderPath))
            {
                this.validationResult += "Source folder is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_DestinationDirectory))
            {
                this.validationResult += "Move/copy folder is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
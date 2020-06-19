using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Folder Operation Commands")]
    [Description("This command moves/copies a folder to a specified location.")]
    public class MoveCopyFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Folder Operation Type")]
        [PropertyUISelectionOption("Move Folder")]
        [PropertyUISelectionOption("Copy Folder")]
        [InputSpecification("Specify whether you intend to move or copy the folder.")]
        [SampleUsage("")]
        [Remarks("Moving will remove the folder from the original path while Copying will not.")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Source Folder Path")]
        [InputSpecification("Enter or Select the path to the original folder.")]
        [SampleUsage(@"C:\temp\myfolder || {ProjectPath}\myfolder || {vOriginalFolderPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)] 
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Destination Folder Path")]
        [InputSpecification("Enter or Select the destination folder path.")]
        [SampleUsage(@"C:\temp\DestinationFolder || {ProjectPath}\DestinationFolder || {vDestinationFolderPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)] 
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Create Destination Folder")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the destination directory should be created if it does not already exist.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delete Existing Folder")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the folder should be deleted first if it already exists in the destination directory.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_DeleteExisting { get; set; }

        public MoveCopyFolderCommand()
        {
            CommandName = "MoveCopyFolderCommand";
            SelectionName = "Move/Copy Folder";
            CommandEnabled = true;
            CustomRendering = true;
            v_CreateDirectory = "Yes";
            v_DeleteExisting = "Yes";
        }

        public override void RunCommand(object sender)
        {
            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertToUserVariable(sender);
            var destinationFolder = v_DestinationDirectory.ConvertToUserVariable(sender);

            if ((v_CreateDirectory == "Yes") && (!Directory.Exists(destinationFolder)))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            //get source folder name and info
            DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);

            //create final path
            var finalPath = Path.Combine(destinationFolder, sourceFolderInfo.Name);

            //delete if it already exists per user
            if (v_DeleteExisting == "Yes" && Directory.Exists(finalPath))
            {
                Directory.Delete(finalPath, true);
            }

            if (v_OperationType == "Move Folder")
            {
                //move folder
                Directory.Move(sourceFolder, finalPath);
            }
            else if (v_OperationType == "Copy Folder")
            {
                //copy folder
                DirectoryCopy(sourceFolder, finalPath, true);   
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
            return base.GetDisplayValue() + $" [{v_OperationType} From '{v_SourceFolderPath}' to '{v_DestinationDirectory}']";
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
    }
}
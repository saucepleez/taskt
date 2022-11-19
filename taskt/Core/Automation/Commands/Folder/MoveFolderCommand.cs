using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.Description("This command moves a folder to a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to move a folder to a new destination.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MoveFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Indicate whether to move or copy the folder")]
        [PropertyUISelectionOption("Move Folder")]
        [PropertyUISelectionOption("Copy Folder")]
        [InputSpecification("Specify whether you intend to move the folder or copy the folder. Moving will remove the folder from the original path while Copying will not.")]
        [SampleUsage("Select either **Move Folder** or **Copy Folder**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Move Folder")]
        [PropertyDisplayText(true, "Operation")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the path to the source folder")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [InputSpecification("Enter or Select the path to the folder.")]
        [SampleUsage("**C:\\temp\\myfolder** or **{{{vTextFolderPath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Target Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Target Folder")]
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the folder to move/copy to")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [InputSpecification("Enter or Select the new path to the file.")]
        [SampleUsage("**C:\\temp\\newPath** or **{{{vTextFolderPath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Folder to Move/Copy", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Move/Copy")]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Create folder if destination does not exist")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the directory should be created if it does not already exist.")]
        [SampleUsage("Select **Yes** or **No**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "No")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delete folder if it already exists")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the folder should be deleted first if it is already found to exist.")]
        [SampleUsage("Select **Yes** or **No**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "No")]
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
            var engine = (Engine.AutomationEngineInstance)sender;

            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertToUserVariable(sender);
            var destinationFolder = v_DestinationDirectory.ConvertToUserVariable(sender);

            if (!Directory.Exists(destinationFolder))
            {
                //var vCreateDirectory = v_CreateDirectory.ConvertToUserVariable(sender);
                //if (String.IsNullOrEmpty(vCreateDirectory))
                //{
                //    vCreateDirectory = "No";
                //}
                var vCreateDirectory = this.GetUISelectionValue(nameof(v_CreateDirectory), "Create Directory", engine);
                if (vCreateDirectory == "yes")
                {
                    Directory.CreateDirectory(destinationFolder);
                }
            }

            //get source folder name and info
            DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);

            //create final path
            var finalPath = Path.Combine(destinationFolder, sourceFolderInfo.Name);

            //delete if it already exists per user
            if (Directory.Exists(finalPath))
            {
                //var vDeleteExisting = v_DeleteExisting.ConvertToUserVariable(sender);
                //if (String.IsNullOrEmpty(vDeleteExisting))
                //{
                //    vDeleteExisting = "No";
                //}
                var vDeleteExisting = this.GetUISelectionValue(nameof(v_DeleteExisting), "Delete Existing", engine);
                if (vDeleteExisting == "yes")
                {
                    Directory.Delete(finalPath, true);
                }
            }

            //var vOperationType = v_OperationType.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(vOperationType))
            //{
            //    vOperationType = "Move Folder";
            //}
            var vOperationType = this.GetUISelectionValue(nameof(v_OperationType), "Operation Type", engine);
            if (vOperationType == "move folder")
            {
                //move folder
                Directory.Move(sourceFolder, finalPath);
            }
            //else if (vOperationType == "Copy Folder")
            else
            {
                //copy folder
                DirectoryCopy(sourceFolder, finalPath, true);   
            }

        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
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
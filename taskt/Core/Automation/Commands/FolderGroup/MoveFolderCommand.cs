using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Move Folder")]
    [Attributes.ClassAttributes.Description("This command moves a folder to a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to move a folder to a new destination.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class MoveFolderCommand : AFolderCopyMoveFolderCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("Folder Action")]
        //[PropertyUISelectionOption("Move Folder")]
        //[PropertyUISelectionOption("Copy Folder")]
        //[PropertyIsOptional(true, "Move Folder")]
        //[PropertyDisplayText(true, "Folder Action")]
        //public string v_OperationType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //[PropertyDescription("Target Folder")]
        //[PropertyValidationRule("Target Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Target Folder")]
        //public string v_TargetFolderPath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //[PropertyDescription("Destination Folder for Move")]
        //[PropertyValidationRule("Destination Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Destination Folder")]
        //public string v_DestinationFolderPath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        //[PropertyDescription("Create Folder when the Destination Folder does not Exists")]
        //[PropertyIsOptional(true, "No")]
        //public string v_CreateDirectory { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        //[PropertyDescription("Delete Folder when it already Exists")]
        //[PropertyIsOptional(true, "No")]
        //public string v_WhenDestinationExists { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        //[PropertyDescription("Wait Time for the Target Folder to Exist (sec)")]
        //[PropertyDisplayText(false, "")]
        //public string v_WaitTimeForFolder { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        //[PropertyDescription("Variable Name to Store Folder Path Before Move")]
        //public string v_BeforeFolderPathResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //[PropertyDescription("Variable Name to Store Folder Path After Move")]
        //public string v_AfterFolderPathResult { get; set; }

        public MoveFolderCommand()
        {
            //this.CommandName = "MoveFolderCommand";
            //this.SelectionName = "Move/Copy Folder";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////apply variable logic
            //var sourceFolder = FolderPathControls.WaitForFolder(this, nameof(v_SourceFolderPath), nameof(v_WaitForTargetFolder), engine);

            //var destinationFolder = v_DestinationDirectory.ConvertToUserVariableAsFolderPath(engine);

            //if (!Directory.Exists(destinationFolder))
            //{
            //    if (this.GetYesNoSelectionValue(nameof(v_CreateDirectory), engine))
            //    {
            //        Directory.CreateDirectory(destinationFolder);
            //    }
            //}

            ////get source folder name and info
            //DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);

            ////create final path
            //var finalPath = Path.Combine(destinationFolder, sourceFolderInfo.Name);

            ////delete if it already exists per user
            //if (Directory.Exists(finalPath))
            //{
            //    if (this.GetYesNoSelectionValue(nameof(v_DeleteExisting), engine))
            //    {
            //        Directory.Delete(finalPath, true);
            //    }
            //}

            //Directory.Move(sourceFolder, finalPath);

            //if (!string.IsNullOrEmpty(v_AfterFilePathResult))
            //{
            //    finalPath.StoreInUserVariable(engine, v_AfterFilePathResult);
            //}

            //FolderPathControls.FolderAction(this, engine,
            //    new Action<string>(path =>
            //    {
            //        var destinationFolder = v_DestinationFolderPath.ExpandValueOrUserVariableAsFolderPath(engine);

            //        if (!Directory.Exists(destinationFolder))
            //        {
            //            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_CreateDirectory), engine))
            //            {
            //                Directory.CreateDirectory(destinationFolder);
            //            }
            //        }

            //        //get source folder name and info
            //        DirectoryInfo sourceFolderInfo = new DirectoryInfo(path);

            //        //create final path
            //        var finalPath = Path.Combine(destinationFolder, sourceFolderInfo.Name);

            //        //delete if it already exists per user
            //        if (Directory.Exists(finalPath))
            //        {
            //            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_DeleteExisting), engine))
            //            {
            //                Directory.Delete(finalPath, true);
            //            }
            //        }

            //        Directory.Move(path, finalPath);

            //        if (!string.IsNullOrEmpty(v_AfterFolderPathResult))
            //        {
            //            finalPath.StoreInUserVariable(engine, v_AfterFolderPathResult);
            //        }
            //    })
            //);

            this.FolderAction(engine,
                this.CreateActionFunc(Directory.Move, engine)
            );

            //switch (this.GetUISelectionValue(nameof(v_OperationType), engine))
            //{
            //    case "move folder":
            //        Directory.Move(sourceFolder, finalPath);
            //        break;
            //    case "copy folder":
            //        DirectoryCopy(sourceFolder, finalPath, true);
            //        break;
            //}
        }

        //private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        //{
        //    // If the destination directory doesn't exist, create it.

        //    //Directory.GetParent(destDirName);
        //    if (!Directory.GetParent(destDirName).Exists)
        //    {
        //        throw new DirectoryNotFoundException(
        //            "Destination directory does not exist or could not be found: "
        //            + Directory.GetParent(destDirName));
        //    }

        //    if (!Directory.Exists(destDirName))
        //    {
        //        Directory.CreateDirectory(destDirName);
        //    }

        //    // Get the subdirectories for the specified directory.
        //    DirectoryInfo sDirInfo = new DirectoryInfo(sourceDirName);
        //    // Get the files in the directory and copy them to the new location.
        //    FileInfo[] files = sDirInfo.GetFiles();
        //    foreach (FileInfo file in files)
        //    {
        //        string temppath = Path.Combine(destDirName, file.Name);
        //        file.CopyTo(temppath, false);
        //    }

        //    // If copying subdirectories, copy them and their contents to new location.
        //    if (copySubDirs)
        //    {
        //        DirectoryInfo[] subDirs = sDirInfo.GetDirectories();
        //        foreach (DirectoryInfo subdir in subDirs)
        //        {
        //            string temppath = Path.Combine(destDirName, subdir.Name);
        //            DirectoryCopy(subdir.FullName, temppath, copySubDirs);
        //        }
        //    }
        //}
    }
}

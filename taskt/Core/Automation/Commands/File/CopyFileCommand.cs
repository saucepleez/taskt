using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Copy File")]
    [Attributes.ClassAttributes.Description("This command copies a file to a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to copy a file to a new destination.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CopyFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyDescription("Destination Folder Path to Copy")]
        [PropertyDisplayText(true, "Folder")]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Create Folder When Destination Folder does not Exist")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [Remarks("Specify whether the directory should be created if it does not already exist.")]
        [PropertyIsOptional(true, "No")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Delete File if it already Exists")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [Remarks("Specify whether the file should be deleted first if it is already found to exist.")]
        [PropertyIsOptional(true, "No")]
        public string v_DeleteExisting { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        [PropertyDescription("Variable Name to Store File Path Before Copy")]
        public string v_BeforeFilePathResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        [PropertyDescription("Variable Name to Store File Path After Copy")]
        public string v_AfterFilePathResult { get; set; }

        public CopyFileCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var sourceFile = FilePathControls.WaitForFile(this, nameof(v_SourceFilePath), nameof(v_WaitTime), engine);

            //var destinationFolder = v_DestinationDirectory.ConvertToUserVariableAsFolderPath(engine);

            //if (!Directory.Exists(destinationFolder))
            //{
            //    if (this.GetYesNoSelectionValue(nameof(v_CreateDirectory), engine))
            //    {
            //        Directory.CreateDirectory(destinationFolder);
            //    }
            //    else
            //    {
            //        throw new Exception("destination folder does not exists: " + destinationFolder);
            //    }
            //}

            ////get source file name and info
            //FileInfo sourceFileInfo = new FileInfo(sourceFile);

            ////create destination
            //var destinationPath = Path.Combine(destinationFolder, sourceFileInfo.Name);

            //// todo: check folder is same

            ////delete if it already exists per user
            //if (this.GetYesNoSelectionValue(nameof(v_DeleteExisting), engine))
            //{
            //    File.Delete(destinationPath);
            //}

            //File.Copy(sourceFile, destinationPath);

            FilePathControls.FileAction(this, engine,
                new Action<string>(path =>
                {
                    var destinationFolder = v_DestinationDirectory.ConvertToUserVariableAsFolderPath(engine);

                    if (!Directory.Exists(destinationFolder))
                    {
                        if (this.GetYesNoSelectionValue(nameof(v_CreateDirectory), engine))
                        {
                            Directory.CreateDirectory(destinationFolder);
                        }
                        else
                        {
                            throw new Exception("destination folder does not exists: " + destinationFolder);
                        }
                    }

                    //get source file name and info
                    FileInfo sourceFileInfo = new FileInfo(path);

                    //create destination
                    var destinationPath = Path.Combine(destinationFolder, sourceFileInfo.Name);

                    // todo: check folder is same

                    //delete if it already exists per user
                    if (this.GetYesNoSelectionValue(nameof(v_DeleteExisting), engine))
                    {
                        File.Delete(destinationPath);
                    }

                    File.Copy(path, destinationPath);

                    if (!string.IsNullOrEmpty(v_AfterFilePathResult))
                    {
                        destinationPath.StoreInUserVariable(engine, v_AfterFilePathResult);
                    }
                })
            );
        }
    }
}
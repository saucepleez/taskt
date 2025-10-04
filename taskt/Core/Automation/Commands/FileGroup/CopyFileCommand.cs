using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation")]
    [Attributes.ClassAttributes.CommandSettings("Copy File")]
    [Attributes.ClassAttributes.Description("This command copies a file to a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to copy a file to a new destination.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CopyFileCommand : AFileCopyMoveFileCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        //[PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        //public string v_TargetFilePath { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyDescription("Destination Folder Path to Copy")]
        public override string v_DestinationFolderPath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("Create Folder When Destination Folder does not Exist")]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]
        //[Remarks("Specify whether the directory should be created if it does not already exist.")]
        //[PropertyIsOptional(true, "No")]
        //[PropertyParameterOrder(7000)]
        //public string v_CreateDirectory { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("Delete File if it already Exists")]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]
        //[Remarks("Specify whether the file should be deleted first if it is already found to exist.")]
        //[PropertyIsOptional(true, "No")]
        //[PropertyParameterOrder(8000)]
        //public string v_WhenDestinationExists { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //[PropertyDescription("Variable Name to Store File Path Before Copy")]
        //public string v_BeforeFilePathResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //[PropertyDescription("Variable Name to Store File Path After Copy")]
        //public string v_AfterFilePathResult { get; set; }

        public CopyFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.FileAction(engine,
                this.CreateActionFunc(File.Copy, engine)
            );
        }
    }
}
using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Move/Copy File")]
    [Attributes.ClassAttributes.Description("This command moves a file to a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to move a file to a new destination.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    // TODO: change to file action command
    public class MoveFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("File Operation")]
        [PropertyUISelectionOption("Move File")]
        [PropertyUISelectionOption("Copy File")]
        [Remarks("Specify whether you intend to move the file or copy the file.  Moving will remove the file from the original path while Copying will not.")]
        [PropertyIsOptional(true, "Move File")]
        [PropertyDisplayText(true, "Operation")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Folder to Move/Copy to")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [InputSpecification("Enter or Select the new path to the file.")]
        [SampleUsage("**C:\\temp\\new path\\** or **{{{vTextFolderPath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "To")]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Destination Folder does not Exist")]
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

        public MoveFileCommand()
        {
            //this.CommandName = "MoveFileCommand";
            //this.SelectionName = "Move/Copy File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var sourceFile = FilePathControls.WaitForFile(this, nameof(v_SourceFilePath), nameof(v_WaitTime), engine);

            var destinationFolder = v_DestinationDirectory.ConvertToUserVariable(engine);

            if (!Directory.Exists(destinationFolder))
            {
                var vCreateDirectory = this.GetUISelectionValue(nameof(v_CreateDirectory), engine);
                if (vCreateDirectory == "yes")
                {
                    Directory.CreateDirectory(destinationFolder);
                }
                else
                {
                    throw new Exception("destination folder does not exists: " + destinationFolder);
                }
            }

            //get source file name and info
            FileInfo sourceFileInfo = new FileInfo(sourceFile);

            //create destination
            var destinationPath = Path.Combine(destinationFolder, sourceFileInfo.Name);

            // todo: check folder is same

            //delete if it already exists per user
            var vDeleteExisting = this.GetUISelectionValue(nameof(v_DeleteExisting), engine);
            if (vDeleteExisting == "yes")
            {
                File.Delete(destinationPath);
            }

            var vOperationType = this.GetUISelectionValue(nameof(v_OperationType), engine);
            switch (vOperationType) 
            {
                case "move file":
                    File.Move(sourceFile, destinationPath);
                    break;

                case "copy file":
                    File.Copy(sourceFile, destinationPath);
                    break;
            }
        }
    }
}
using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command moves a file to a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to move a file to a new destination.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MoveFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Indicate whether to move or copy the file")]
        [PropertyUISelectionOption("Move File")]
        [PropertyUISelectionOption("Copy File")]
        [InputSpecification("Specify whether you intend to move the file or copy the file.  Moving will remove the file from the original path while Copying will not.")]
        [SampleUsage("Select either **Move File** or **Copy File**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Move File")]
        [PropertyDisplayText(true, "Operation")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the path to the source file")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the file.")]
        [SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Target File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the directory to move/copy to")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [InputSpecification("Enter or Select the new path to the file.")]
        [SampleUsage("**C:\\temp\\new path\\** or **{{{vTextFolderPath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Directory", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "To")]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Create folder if destination does not exist")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the directory should be created if it does not already exist.")]
        [SampleUsage("Select **Yes** or **No**")]
        [Remarks("")]
        [PropertyIsOptional(true, "No")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delete file if it already exists")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the file should be deleted first if it is already found to exist.")]
        [SampleUsage("Select **Yes** or **No**")]
        [Remarks("")]
        [PropertyIsOptional(true, "No")]
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
            var engine = (Engine.AutomationEngineInstance)sender;

            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertToUserVariable(sender);
            var destinationFolder = v_DestinationDirectory.ConvertToUserVariable(sender);

            if (!Directory.Exists(destinationFolder))
            {
                //var vCreateDirectroy = v_CreateDirectory.ConvertToUserVariable(sender);
                //if (String.IsNullOrEmpty(vCreateDirectroy))
                //{
                //    vCreateDirectroy = "No";
                //}
                var vCreateDirectory = this.GetUISelectionValue(nameof(v_CreateDirectory), "Create Directory", engine);
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

            //delete if it already exists per user
            //var vDeleteExistsint = v_DeleteExisting.ConvertToUserVariable(sender);
            var vDeleteExisting = this.GetUISelectionValue(nameof(v_DeleteExisting), "Delete Existing", engine);
            if (vDeleteExisting == "yes")
            {
                File.Delete(destinationPath);
            }

            //var vOperationType = v_OperationType.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(vOperationType))
            //{
            //    vOperationType = "Move File";
            //}
            var vOperationType = this.GetUISelectionValue(nameof(v_OperationType), "Operation Type", engine);
            if (vOperationType == "move file")
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
    }
}
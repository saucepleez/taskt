using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command renames a file at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to rename an existing file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RenameFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the path to the source file")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the file.")]
        [SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the new file name (with extension)")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify the new file name including the extension.")]
        [SampleUsage("**newfile.txt** or **{{{vNewFileName}}}**")]
        [Remarks("Changing the file extension will not automatically convert files.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("New FileName", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New FileName")]
        public string v_NewName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select If File Name Same After the Change")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error**")]
        [Remarks("")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_IfFileNameSame { get; set; }

        public RenameFileCommand()
        {
            this.CommandName = "RenameFileCommand";
            this.SelectionName = "Rename File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertToUserVariable(sender);
            var currentFileName = Path.GetFileName(sourceFile);
            var newFileName = v_NewName.ConvertToUserVariable(sender);

            //get source file name and info
            FileInfo sourceFileInfo = new FileInfo(sourceFile);

            //create destination
            var destinationPath = Path.Combine(sourceFileInfo.DirectoryName, newFileName);

            var ifSame = this.GetUISelectionValue(nameof(v_IfFileNameSame), "File Name Same", engine);
            if (currentFileName == newFileName)
            {
                switch (ifSame)
                {
                    case "ignore":
                        return;

                    case "error":
                        throw new Exception("File Name before and after the changes is same. Name '" + newFileName + "'");
                }
            }

            //rename file
            File.Move(sourceFile, destinationPath);
        }
    }
}
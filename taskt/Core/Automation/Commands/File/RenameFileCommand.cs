using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Rename File")]
    [Attributes.ClassAttributes.Description("This command renames a file at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to rename an existing file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RenameFileCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate the path to the source file")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the file.")]
        //[SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "File")]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("New File Name (with Extension)")]
        [InputSpecification("Specify the New File Name including the Extension.")]
        [PropertyDetailSampleUsage("**newfile.txt**", PropertyDetailSampleUsage.ValueType.Value, "File Name")]
        [PropertyDetailSampleUsage("**{{{vNewFileName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name")]
        [Remarks("Changing the file extension will not automatically convert files.")]
        [PropertyValidationRule("New FileName", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New FileName")]
        public string v_NewName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When File Name Same After the Change")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_IfFileNameSame { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public RenameFileCommand()
        {
            //this.CommandName = "RenameFileCommand";
            //this.SelectionName = "Rename File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //apply variable logic
            var sourceFile = FilePathControls.WaitForFile(this, nameof(v_SourceFilePath), nameof(v_WaitTime), engine);
            
            var currentFileName = Path.GetFileName(sourceFile);
            var newFileName = v_NewName.ConvertToUserVariable(engine);

            //get source file name and info
            FileInfo sourceFileInfo = new FileInfo(sourceFile);

            //create destination
            var destinationPath = Path.Combine(sourceFileInfo.DirectoryName, newFileName);

            var whenSame = this.GetUISelectionValue(nameof(v_IfFileNameSame), engine);
            if (currentFileName == newFileName)
            {
                switch (whenSame)
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
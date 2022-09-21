using System;
using System.Xml.Serialization;
using System.IO;
using taskt.UI.CustomControls;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
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

        public RenameFileCommand()
        {
            this.CommandName = "RenameFileCommand";
            this.SelectionName = "Rename File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertToUserVariable(sender);
            var newFileName = v_NewName.ConvertToUserVariable(sender);

            //get source file name and info
            FileInfo sourceFileInfo = new FileInfo(sourceFile);

            //create destination
            var destinationPath = Path.Combine(sourceFileInfo.DirectoryName, newFileName);

            //rename file
            File.Move(sourceFile, destinationPath);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_NewName", this, editor));

        //    return RenderedControls;
        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [rename " + v_SourceFilePath + " to '" + v_NewName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_SourceFilePath))
        //    {
        //        this.validationResult += "Source file is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_NewName))
        //    {
        //        this.validationResult += "New file name is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
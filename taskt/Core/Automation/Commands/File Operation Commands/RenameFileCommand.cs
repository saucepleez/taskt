using System;
using System.Xml.Serialization;
using System.IO;
using taskt.UI.CustomControls;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command renames a file at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to rename an existing file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class RenameFileCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.txt or [vTextFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new file name (with extension)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify the new file name including the extension.")]
        [Attributes.PropertyAttributes.SampleUsage("newfile.txt or newfile.png")]
        [Attributes.PropertyAttributes.Remarks("Changing the file extension will not automatically convert files.")]
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
            System.IO.FileInfo sourceFileInfo = new FileInfo(sourceFile);

            //create destination
            var destinationPath = System.IO.Path.Combine(sourceFileInfo.DirectoryName, newFileName);

            //rename file
            System.IO.File.Move(sourceFile, destinationPath);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_NewName", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [rename " + v_SourceFilePath + " to '" + v_NewName + "']";
        }
    }
}
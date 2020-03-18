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
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.Description("This command renames a folder at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to rename an existing folder.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class RenameFolderCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source folder")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the folder.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myFolder or [vTextFolderPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new folder name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify the new folder name.")]
        [Attributes.PropertyAttributes.SampleUsage("newFolderName")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_NewName { get; set; }


        public RenameFolderCommand()
        {
            this.CommandName = "RenameFolderCommand";
            this.SelectionName = "Rename Folder";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {

            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertToUserVariable(sender);
            var newFolderName = v_NewName.ConvertToUserVariable(sender);

            //get source folder name and info
            System.IO.DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);

            //create destination
            var destinationPath = System.IO.Path.Combine(sourceFolderInfo.Parent.FullName, newFolderName);

            //rename folder
            System.IO.Directory.Move(sourceFolder, destinationPath);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolderPath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_NewName", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [rename " + v_SourceFolderPath + " to '" + v_NewName + "']";
        }
    }
}
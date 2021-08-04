using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.Description("This command creates a folder in a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to create a folder in a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class CreateFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the name of the new folder (ex. myFolder, {{{vFolderName}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the name of the new folder.")]
        [Attributes.PropertyAttributes.SampleUsage("**myFolderName** or **{{{vFolderName}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_NewFolderName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the directory for the new folder (ex. C:\\temp\\myfolder, {{{vFolderPath}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the directory.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myfolder** or **{{{TextFolderPath}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Delete folder if it already exists (default is No)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the folder should be deleted first if it is already found to exist.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DeleteExisting { get; set; }

        public CreateFolderCommand()
        {
            this.CommandName = "CreateFolderCommand";
            this.SelectionName = "Create Folder";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {

            //apply variable logic
            var destinationDirectory = v_DestinationDirectory.ConvertToUserVariable(sender);
            var newFolder = v_NewFolderName.ConvertToUserVariable(sender);


            var finalPath = System.IO.Path.Combine(destinationDirectory, newFolder);
            if (System.IO.Directory.Exists(destinationDirectory + "\\" + newFolder))
            {
                var deleteFolder = v_DeleteExisting.ConvertToUserVariable(sender);
                if (String.IsNullOrEmpty(deleteFolder))
                {
                    deleteFolder = "No";
                }
                if (deleteFolder.ToLower() == "yes")
                {
                    System.IO.Directory.Delete(finalPath, true);
                }
            }

            //create folder if it doesn't exist
            if (!System.IO.Directory.Exists(finalPath))
            {
                System.IO.Directory.CreateDirectory(finalPath);
            }
     
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_NewFolderName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DeleteExisting", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + "[create " + v_DestinationDirectory + "\\" + v_NewFolderName +"']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_DestinationDirectory))
            {
                this.validationResult += "Directory of new folder is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_NewFolderName))
            {
                this.validationResult += "Name of new folder is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
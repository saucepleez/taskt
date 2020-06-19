using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Group("Folder Operation Commands")]
    [Description("This command creates a folder in a specified location.")]
    public class CreateFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("New Folder Name")]
        [InputSpecification("Enter the name of the new folder.")]
        [SampleUsage("myFolderName || {vFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_NewFolderName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Directory Path")]
        [InputSpecification("Enter or Select the path to the directory to create the folder in.")]
        [SampleUsage(@"C:\temp\myfolder || {ProjectPath}\myfolder || {vTextFolderPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)] 
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delete Existing Folder")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the folder should be deleted first if it already exists.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_DeleteExisting { get; set; }

        public CreateFolderCommand()
        {
            CommandName = "CreateFolderCommand";
            SelectionName = "Create Folder";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //apply variable logic
            var destinationDirectory = v_DestinationDirectory.ConvertToUserVariable(sender);
            var newFolder = v_NewFolderName.ConvertToUserVariable(sender);

            var finalPath = Path.Combine(destinationDirectory, newFolder);
            //delete folder if it exists AND the delete option is selected 
            if (v_DeleteExisting == "Yes" && Directory.Exists(finalPath))
            {
                Directory.Delete(finalPath, true);
            }

            //create folder if it doesn't exist
            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_NewFolderName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DeleteExisting", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $"[Folder Path '{Path.Combine(v_DestinationDirectory,v_NewFolderName)}']";
        }
    }
}
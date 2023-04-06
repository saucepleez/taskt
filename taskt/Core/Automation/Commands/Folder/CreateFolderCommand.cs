using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Create Folder")]
    [Attributes.ClassAttributes.Description("This command creates a folder in a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to create a folder in a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Name of the New Folder")]
        [InputSpecification("Name of the New Folder", true)]
        //[SampleUsage("**myFolderName** or **{{{vFolderName}}}**")]
        [PropertyDetailSampleUsage("**myFolder**", PropertyDetailSampleUsage.ValueType.Value, "Folder Name")]
        [PropertyDetailSampleUsage("**{{{vFolderName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Folder Name")]
        [PropertyValidationRule("New Folder Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New Folder Name")]
        public string v_NewFolderName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyDescription("Location where you want to Create the Folder")]
        //[PropertyDescription("Please indicate the directory for the new folder")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the directory.")]
        //[SampleUsage("**C:\\temp\\myfolder** or **{{{TextFolderPath}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Directory Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Dictory")]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Delete folder if it already exists")]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]
        //[InputSpecification("Specify whether the folder should be deleted first if it is already found to exist.")]
        //[SampleUsage("Select **Yes** or **No**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "No")]
        [PropertyVirtualProperty(nameof(SelectionControls), nameof(SelectionControls.v_YesNoComboBox))]
        [PropertyDescription("Delete Folder When it already Exists")]
        [PropertyIsOptional(true, "No")]
        public string v_DeleteExisting { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        public string v_WaitForFolder { get; set; }

        public CreateFolderCommand()
        {
            //this.CommandName = "CreateFolderCommand";
            //this.SelectionName = "Create Folder";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //apply variable logic
            //var destinationDirectory = v_DestinationDirectory.ConvertToUserVariable(sender);
            var destinationDirectory = FolderPathControls.WaitForFolder(this, nameof(v_DestinationDirectory), nameof(v_WaitForFolder), engine);
            
            var newFolder = v_NewFolderName.ConvertToUserVariable(engine);

            var finalPath = System.IO.Path.Combine(destinationDirectory, newFolder);
            //if (System.IO.Directory.Exists(destinationDirectory + "\\" + newFolder))
            if (System.IO.Directory.Exists(finalPath)) { }
            {
                //var deleteFolder = this.GetUISelectionValue(nameof(v_DeleteExisting), "Delete Existing", engine);
                //if (deleteFolder == "yes")
                //{
                //    System.IO.Directory.Delete(finalPath, true);
                //}
                if (this.GetYesNoSelectionValue(nameof(v_DeleteExisting), engine))
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
    }
}
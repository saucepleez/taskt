using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.Description("This command deletes a folder from a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to delete a folder from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class DeleteFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the path to the source folder")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [InputSpecification("Enter or Select the path to the folder.")]
        [SampleUsage("**C:\\temp\\myfolder** or **{{{vTextFolderPath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Folder Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Folder")]
        public string v_SourceFolderPath { get; set; }

        public DeleteFolderCommand()
        {
            this.CommandName = "DeleteFolderCommand";
            this.SelectionName = "Delete Folder";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertToUserVariable(sender);

            //delete folder
            System.IO.Directory.Delete(sourceFolder, true);
        }
    }
}
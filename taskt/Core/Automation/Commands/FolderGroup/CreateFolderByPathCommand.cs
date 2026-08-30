using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Create Folder By Path")]
    [Attributes.ClassAttributes.Description("This command creates a folder specified by Path")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to create a folder in a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CreateFolderByPathCommand : AFolderFolderPathCommands
    {
        //public string v_TargetFolderPath {get;set}

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Folder Exists")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Delete")]
        [PropertyUISelectionOption("Delete To Recycle Bin")]
        [PropertyUISelectionOption("Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Delete**", "Delete Folder and Create Folder")]
        [PropertyDetailSampleUsage("**Delete To Recycle Bin**", "Delete Folder to Recycle Bin and Create Folder")]
        [PropertyDetailSampleUsage("**Error**", "Rise an Error")]
        [PropertyIsOptional(true, "Error")]
        [PropertyValidationRule("When Folder Exists", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(6000)]
        public string v_WhenFolderExists { get; set; }

        public CreateFolderByPathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var folderPath = this.ExpandValueOrUserVariableAsFolderPath(engine);

            if (Directory.Exists(folderPath)) 
            {
                void DeleteFolderProcess(string path, bool isRecycleBin)
                {
                    var delFolder = new DeleteFolderCommand()
                    {
                        v_TargetFolderPath = folderPath,
                        v_MoveToRecycleBin = (isRecycleBin) ? "Yes" : "No",
                    };
                    delFolder.RunCommand(engine);
                }

                switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFolderExists), engine))
                {
                    case "error":
                        throw new Exception($"Specified Folder is Already Exists. Path: '{v_TargetFolderPath}', Expand Path: '{folderPath}'");

                    case "ignore":
                        return;

                    case "delete":
                        //Directory.Delete(folderPath, true);
                        DeleteFolderProcess(folderPath, false);
                        break;

                    case "delete to recycle bin":
                        DeleteFolderProcess(folderPath, true);
                        break;
                }
            }

            Directory.CreateDirectory(folderPath);
        }
    }
}

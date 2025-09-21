using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation")]
    [Attributes.ClassAttributes.CommandSettings("Delete File")]
    [Attributes.ClassAttributes.Description("This command deletes a file from a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to detete a file from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class DeleteFileCommand : AFileExistsFilePathPathResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        //[PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        //public string v_TargetFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WhenFileDoesNotExists))]
        [PropertyParameterOrder(6000)]
        public string v_WhenFileDoesNotExists { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("File Move to the Recycle Bin")]
        [PropertyIsOptional(true, "No")]
        [PropertyParameterOrder(7000)]
        public string v_MoveToRecycleBin { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //public string v_ResultPath { get; set; }

        public DeleteFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.FileAction(engine,
                new Func<string, string>(path =>
                {
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_MoveToRecycleBin), engine))
                    {
                        Shell32.MoveToRecycleBin(path);
                    }
                    else
                    {
                        System.IO.File.Delete(path);
                    }
                    return path;
                }),
                new Action<Exception>(ex =>
                {
                    if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFileDoesNotExists), engine) == "error")
                    {
                        throw new Exception($"File does Not Exists. File Path: '{v_TargetFilePath}'");
                    }
                })
            );
        }
    }
}
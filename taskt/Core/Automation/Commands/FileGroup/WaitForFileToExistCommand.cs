using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation")]
    [Attributes.ClassAttributes.CommandSettings("Wait For File To Exists")]
    [Attributes.ClassAttributes.Description("This command waits for a file to exist at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to wait for a file to exist before proceeding.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class WaitForFileToExistCommand : AFileExistsFilePathPathResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        //[PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        //public string v_TargetFilePath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //[PropertyFirstValue("60")]
        //public string v_WaitTimeForFile { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //public string v_ResultPath { get; set; }

        public WaitForFileToExistCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.FileAction(engine,
                new Func<string, string>(path =>
                {
                    // nothing to do
                    return path;
                })
            );
        }
    }
}
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Wait For File To Exists")]
    [Attributes.ClassAttributes.Description("This command waits for a file to exist at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to wait for a file to exist before proceeding.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WaitForFileToExistCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        public string v_FileName { get; set; }


        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        [PropertyFirstValue("60")]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        public string v_ResultPath { get; set; }

        public WaitForFileToExistCommand()
        {
            //this.CommandName = "WaitForFileToExistCommand";
            //this.SelectionName = "Wait For File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            ////convert items to variables
            //var fileName = v_FileName.ConvertToUserVariable(sender);

            //var fileCheckFunc = new Func<(bool, object)>(() =>
            //{
            //    return (System.IO.File.Exists(fileName), null);
            //});
            //this.WaitProcess(nameof(v_WaitTime), "File", fileCheckFunc, engine);

            FilePathControls.FileAction(this, engine,
                new Action<string>(path =>
                {
                    // nothing to do
                })
            );
        }
    }
}
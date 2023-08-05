using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Wait For Folder To Exists")]
    [Attributes.ClassAttributes.Description("This command waits for a folder to exist at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to wait for a folder to exist before proceeding.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WaitForFolderToExistCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        public string v_FolderName { get; set; }


        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        [PropertyIsOptional(true, "60")]
        [PropertyFirstValue("60")]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        public string v_ResultPath { get; set; }

        public WaitForFolderToExistCommand()
        {
            //this.CommandName = "WaitForFolderToExistCommand";
            //this.SelectionName = "Wait For Folder To Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //try
            //{
            //    FolderPathControls.WaitForFolder(this, nameof(v_FolderName), nameof(v_WaitTime), engine);
            //}
            //catch
            //{
            //    throw new Exception("Folder was Not Found in time!");
            //}
            FolderPathControls.FolderAction(this, engine,
                new Action<string>(path =>
                {
                    // nothing to do
                })
            );
        }
    }
}
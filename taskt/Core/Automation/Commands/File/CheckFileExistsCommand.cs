using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Check File Exists")]
    [Attributes.ClassAttributes.Description("This command returns a existence of file paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a existence of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckFileExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        public string v_TargetFileName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the File Exists, Result is **TRUE**")]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        [PropertyFirstValue("0")]
        [PropertyIsOptional(true, "0")]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        public string v_ResultPath { get; set; }

        public CheckFileExistsCommand()
        {
            //this.CommandName = "CheckFileExistsCommand";
            //this.SelectionName = "Check File Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //try
            //{
            //    FilePathControls.WaitForFile(this, nameof(v_TargetFileName), nameof(v_WaitTime), engine);
            //    true.StoreInUserVariable(engine, v_UserVariableName);
            //}
            //catch
            //{
            //    false.StoreInUserVariable(engine, v_UserVariableName);
            //}
            FilePathControls.FileAction(this, engine,
                new Action<string>(path =>
                {
                    true.StoreInUserVariable(engine, v_UserVariableName);
                }),
                new Action<Exception>(ex =>
                {
                    false.StoreInUserVariable(engine, v_UserVariableName);
                })
            );
        }
    }
}
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation")]
    [Attributes.ClassAttributes.CommandSettings("Check File Exists")]
    [Attributes.ClassAttributes.Description("This command returns a existence of file paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a existence of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CheckFileExistsCommand : AFileExistsFilePathPathResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        //[PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        //public string v_TargetFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the File Exists, Result is **TRUE**")]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyFirstValue("0")]
        [PropertyIsOptional(true, "0")]
        public override string v_WaitTimeForFile { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //public string v_ResultPath { get; set; }

        public CheckFileExistsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.FileAction(engine,
                new Func<string, string>(path =>
                {
                    true.StoreInUserVariable(engine, v_Result);
                    return path;
                }),
                new Action<Exception>(ex =>
                {
                    false.StoreInUserVariable(engine, v_Result);
                })
            );
        }
    }
}
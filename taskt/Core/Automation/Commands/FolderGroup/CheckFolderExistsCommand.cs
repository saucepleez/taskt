using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Check Folder Exists")]
    [Attributes.ClassAttributes.Description("This command returns existence of folder paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a existence of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CheckFolderExistsCommand : AFolderExistsFolderPathPathResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //public string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("When the Folder Exists, Result is **TRUE**")]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        public override string v_WaitTimeForFolder { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        //public string v_ResultPath { get; set; }

        public CheckFolderExistsCommand()
        {
            //this.CommandName = "CheckFolderExistsCommand";
            //this.SelectionName = "Check Folder Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //try
            //{
            //    FolderPathControls.WaitForFolder(this, nameof(v_TargetFolderName), nameof(v_WaitForFolder), engine);

            //    true.StoreInUserVariable(engine, v_UserVariableName);
            //}
            //catch
            //{
            //    false.StoreInUserVariable(engine, v_UserVariableName);
            //}

            //FolderPathControls.FolderAction(this, engine,
            //    new Action<string>(path =>
            //    {
            //        true.StoreInUserVariable(engine, v_Result);
            //    }),
            //    new Action<Exception>(ex =>
            //    {
            //        false.StoreInUserVariable(engine, v_Result);
            //    })
            //);

            this.FolderAction(engine,
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
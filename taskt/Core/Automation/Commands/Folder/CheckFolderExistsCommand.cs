using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Check Folder Exists")]
    [Attributes.ClassAttributes.Description("This command returns existence of folder paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a existence of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckFolderExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        public string v_TargetFolderName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("When the Folder Exists, Result is **TRUE**")]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        public string v_WaitForFolder { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        public string v_ResultPath { get; set; }

        public CheckFolderExistsCommand()
        {
            //this.CommandName = "CheckFolderExistsCommand";
            //this.SelectionName = "Check Folder Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            //try
            //{
            //    FolderPathControls.WaitForFolder(this, nameof(v_TargetFolderName), nameof(v_WaitForFolder), engine);

            //    true.StoreInUserVariable(engine, v_UserVariableName);
            //}
            //catch
            //{
            //    false.StoreInUserVariable(engine, v_UserVariableName);
            //}

            FolderPathControls.FolderAction(this, engine,
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
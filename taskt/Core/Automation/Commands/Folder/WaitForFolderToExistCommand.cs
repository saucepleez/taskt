using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.Description("This command waits for a folder to exist at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to wait for a folder to exist before proceeding.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WaitForFolderToExistCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the path of the folder")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [InputSpecification("Enter or Select the path to the folder.")]
        [SampleUsage("**C:\\temp\\myfolder** or **{{{vFolderPath}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Folder Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Folder")]
        public string v_FolderName { get; set; }


        [XmlAttribute]
        [PropertyDescription("Indicate how many seconds to wait for the file to exist")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify how long to wait before an error will occur because the folder is not found.")]
        [SampleUsage("**10** or **20** or **{{{vWaitTime}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Wait Time", "s")]
        public string v_WaitTime { get; set; }

        public WaitForFolderToExistCommand()
        {
            this.CommandName = "WaitForFolderToExistCommand";
            this.SelectionName = "Wait For Folder To Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var folder = v_FolderName.ConvertToUserVariable(sender);

            //int pauseTime = v_WaitTime.ConvertToUserVariableAsInteger("Wait Time", engine);

            ////determine when to stop waiting based on user config
            //var stopWaiting = DateTime.Now.AddSeconds(pauseTime);

            ////initialize flag for file found
            //var folderFound = false;

            ////while file has not been found
            //while (!folderFound)
            //{
            //    //if file exists at the folder path
            //    if (System.IO.Directory.Exists(folder))
            //    {
            //        folderFound = true;
            //    }

            //    //test if we should exit and throw exception
            //    if (DateTime.Now > stopWaiting)
            //    {
            //        throw new Exception("Folder was not found in time!");
            //    }

            //    //put thread to sleep before iterating
            //    engine.ReportProgress("Folder Not Yet Found... " + (int)((stopWaiting - DateTime.Now).TotalSeconds) + "s remain");
            //    System.Threading.Thread.Sleep(1000);
            //}

            var checkFolderFunc = new Func<(bool, object)>(() =>
            {
                return (System.IO.Directory.Exists(folder), null);
            });

            this.WaitProcess(nameof(v_WaitTime), "Folder", checkFolderFunc, engine);
        }
    }
}
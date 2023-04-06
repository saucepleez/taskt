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
        //[PropertyDescription("Specify the path of the folder you want to check for existence")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the file.")]
        //[SampleUsage("**C:\\temp\\myfolder** or **{{{vFolderPath}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Folder")]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        public string v_TargetFolderName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Specify the variable to assign the result")]
        //[InputSpecification("")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("Result is **TRUE** or **FALSE**")]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        //[PropertyValidationRule("Resut", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("When the Folder Exists, Result is **TRUE**")]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        public string v_WaitForFolder { get; set; }

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

            //var targetFile = v_TargetFolderName.ConvertToUserVariable(sender);
            //var targetFolder = v_TargetFolderName.ConvertToUserVariableAsFolderPath(engine);

            try
            {
                FolderPathControls.WaitForFolder(this, nameof(v_TargetFolderName), nameof(v_WaitForFolder), engine);

                //System.IO.Directory.Exists(targetFolder).StoreInUserVariable(engine, v_UserVariableName);
                true.StoreInUserVariable(engine, v_UserVariableName);
            }
            catch
            {
                false.StoreInUserVariable(engine, v_UserVariableName);
            }
        }
    }
}
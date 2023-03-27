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
        //[PropertyDescription("Specify the path of the file you want to check for existence")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the file.")]
        //[SampleUsage("**C:\\temp\\myfile.txt** or **{{{vFilePath}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "File")]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        public string v_TargetFileName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Specify the variable to assign the result")]
        //[InputSpecification("")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("Result is **TRUE** or **FALSE**")]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the File Exists, Result is **TRUE**")]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        [PropertyFirstValue("0")]
        [PropertyIsOptional(true, "0")]
        public string v_WaitTime { get; set; }

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

            // var targetFile = v_TargetFileName.ConvertToUserVariable(sender);
            //var targetFile = this.ConvertToUserVariableAsFilePath(nameof(v_TargetFileName), engine);

            //System.IO.File.Exists(targetFile).StoreInUserVariable(engine, v_UserVariableName);

            try
            {
                var ret = FilePathControls.WaitForFile(this, nameof(v_TargetFileName), nameof(v_WaitTime), engine);
                (ret is string).StoreInUserVariable(engine, v_UserVariableName);
            }
            catch
            {
                false.StoreInUserVariable(engine, v_UserVariableName);
            }
        }
    }
}
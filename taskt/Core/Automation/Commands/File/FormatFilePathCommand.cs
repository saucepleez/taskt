using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to format file path.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to format file path.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FormatFilePathCommnad : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the File Path to Delete.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the file.")]
        [SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify File Path Format.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyCustomUIHelper("Format Checker", nameof(lnkFormatChecker_Click))]
        [InputSpecification("")]
        [SampleUsage("**FileName** or **FileNameWithoutExtension** or **Folder** or **Extension** or **DriveName** etc")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("FileName")]
        [PropertyUISelectionOption("FileNameWithoutExtension")]
        [PropertyUISelectionOption("Folder")]
        [PropertyUISelectionOption("Extension")]
        [PropertyUISelectionOption("DriveName")]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Format")]
        public string v_Format { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to store Result.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_Result { get; set; }

        public FormatFilePathCommnad()
        {
            this.CommandName = "Format File PathCommand";
            this.SelectionName = "Format File Path";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (taskt.Core.Automation.Engine.AutomationEngineInstance)sender;

            string filePath = v_SourceFilePath.ConvertToUserVariable(engine);
            string format = v_Format.ConvertToUserVariable(engine);

            string result = FilePathControls.formatFileFolderPath(filePath, format);
            result.StoreInUserVariable(engine, v_Result);
        }

        private void lnkFormatChecker_Click(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)((CommandItemControl)sender).Tag;
            UI.Forms.Supplement_Forms.frmFormatChecker.ShowFormatCheckerFormLinkClicked(cmb, "File Folder");
        }
    }
}
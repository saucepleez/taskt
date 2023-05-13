using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Extraction Folder Path")]
    [Attributes.ClassAttributes.Description("This command allows you to extract from folder path.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract from folder path.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExtractionFolderPathCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Folder Path Format")]
        [PropertyUISelectionOption("Folder")]
        [PropertyUISelectionOption("DriveName")]
        [PropertyCustomUIHelper("Format Checker", nameof(lnkFormatChecker_Click))]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Format")]
        public string v_Format { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public ExtractionFolderPathCommand()
        {
            //this.CommandName = "Format Folder PathCommand";
            //this.SelectionName = "Format Folder Path";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (taskt.Core.Automation.Engine.AutomationEngineInstance)sender;

            string folderPath = v_SourceFolderPath.ConvertToUserVariableAsFolderPath(engine);

            string format = v_Format.ConvertToUserVariable(engine);

            string result = FilePathControls.FormatFileFolderPath(folderPath, format);
            result.StoreInUserVariable(engine, v_Result);
        }

        private void lnkFormatChecker_Click(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)((CommandItemControl)sender).Tag;
            UI.Forms.Supplement_Forms.frmFormatChecker.ShowFormatCheckerFormLinkClicked(cmb, "File Folder");
        }
    }
}
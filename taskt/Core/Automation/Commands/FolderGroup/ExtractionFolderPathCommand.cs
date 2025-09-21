using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Extraction Folder Path")]
    [Attributes.ClassAttributes.Description("This command allows you to extract from folder path.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract from folder path.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExtractionFolderPathCommand : AFolderFolderPathCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //public string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Folder Path Format")]
        [PropertyUISelectionOption("Folder")]
        [PropertyUISelectionOption("DriveName")]
        [PropertyCustomUIHelper("Format Checker", nameof(lnkFormatChecker_Click))]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Format")]
        [PropertyParameterOrder(6000)]
        public string v_Format { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        public ExtractionFolderPathCommand()
        {
            //this.CommandName = "Format Folder PathCommand";
            //this.SelectionName = "Format Folder Path";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //string folderPath = v_TargetFolderPath.ExpandValueOrUserVariableAsFolderPath(engine);
            var folderPath = this.ExpandValueOrUserVariableAsFolderPath(engine);

            var format = v_Format.ExpandValueOrUserVariable(engine);

            var result = FilePathControls.FormatFileFolderPath(folderPath, format);
            result.StoreInUserVariable(engine, v_Result);
        }

        private void lnkFormatChecker_Click(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)((CommandItemControl)sender).Tag;
            UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmFormatChecker.ShowFormatCheckerFormLinkClicked(cmb, "File Folder");
        }
    }
}
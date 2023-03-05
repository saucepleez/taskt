using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.CommandSettings("Rename Worksheet")]
    [Attributes.ClassAttributes.Description("This command rename a Excel Worksheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a new worksheet to an Excel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelRenameWorksheetCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        [PropertyDescription("Target Worksheet Name to Rename")]
        [PropertyValidationRule("Target Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Target Sheet")]
        public string v_sourceSheet { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        [PropertyDescription("New Worksheet Name")]
        [PropertyValidationRule("New Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New Sheet")]
        [PropertyIntermediateConvert("", "")]
        public string v_newName { get; set; }

        public ExcelRenameWorksheetCommand()
        {
            //this.CommandName = "ExcelRenameWorksheetCommand";
            //this.SelectionName = "Rename Worksheet";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (_, var targetSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            var newName = v_newName.ConvertToUserVariable(sender);

            targetSheet.Name = newName;
        }
    }
}
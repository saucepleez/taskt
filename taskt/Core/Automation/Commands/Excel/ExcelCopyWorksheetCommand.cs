using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.CommandSettings("Copy Worksheet")]
    [Attributes.ClassAttributes.Description("This command copy a Excel Worksheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy a new worksheet to an Excel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCopyWorksheetCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        [PropertyDescription("Sheet Name to Copy")]
        [PropertyValidationRule("Sheet Name to Copy", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Sheet Name to Copy")]
        public string v_sourceSheet { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        [PropertyDescription("New Sheet Name")]
        [PropertyDisplayText(true, "New Sheet Name")]
        [PropertyIsOptional(true)]
        [PropertyIntermediateConvert("", "")]
        public string v_newSheetName { get; set; }

        public ExcelCopyWorksheetCommand()
        {
            //this.CommandName = "ExcelCopyWorksheetCommand";
            //this.SelectionName = "Copy Worksheet";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var targetSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            targetSheet.Copy(Before: excelInstance.Worksheets[1]);

            var newName = v_newSheetName.ConvertToUserVariable(sender);
            if (!String.IsNullOrEmpty(newName))
            {
                ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet).Name = newName;
            }
        }
    }
}
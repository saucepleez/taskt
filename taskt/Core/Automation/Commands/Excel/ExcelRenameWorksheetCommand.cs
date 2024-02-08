using Microsoft.Office.Interop.Excel;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Worksheet")]
    [Attributes.ClassAttributes.CommandSettings("Rename Worksheet")]
    [Attributes.ClassAttributes.Description("This command rename a Excel Worksheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a new worksheet to an Excel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelRenameWorksheetCommand : AExcelInstanceCommand, IExcelWorksheetCopyRenameProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        [PropertyDescription("Target Worksheet Name to Rename")]
        [PropertyValidationRule("Target Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Target Sheet")]
        [PropertyAvailableSystemVariable(Engine.SystemVariables.LimitedSystemVariableNames.Excel_Worksheet)]
        [PropertyParameterOrder(6000)]
        public string v_TargetSheetName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        [PropertyDescription("New Worksheet Name")]
        [PropertyValidationRule("New Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New Sheet")]
        [PropertyIntermediateConvert("", "")]
        [PropertyParameterOrder(6001)]
        public string v_NewSheetName { get; set; }

        public ExcelRenameWorksheetCommand()
        {
            //this.CommandName = "ExcelRenameWorksheetCommand";
            //this.SelectionName = "Rename Worksheet";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(_, var targetSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);
            (var excelInstance, var targetSheet) = this.ExpandValueOrVariableAsExcelInstanceAndTargetWorksheet(engine);

            //var newName = v_NewSheetName.ExpandValueOrUserVariable(engine);
            var newName = v_NewSheetName.ExpandValueOrVariableAsExcelWorksheetName(engine);

            if (targetSheet.Name != newName)
            {
                bool isExists = false;
                foreach(Worksheet sht in excelInstance.Worksheets)
                {
                    if (sht.Name == newName)
                    {
                        isExists = true;
                        break;
                    }
                }
                if (!isExists)
                {
                    targetSheet.Name = newName;
                }
                else
                {
                    throw new Exception($"Worksheet Name '{newName}' is already exists.");
                }
            }
        }
    }
}
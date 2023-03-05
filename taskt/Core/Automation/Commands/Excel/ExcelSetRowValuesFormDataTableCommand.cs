using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.CommandSettings("Set Row Values From DataTable")]
    [Attributes.ClassAttributes.Description("This command set Row values from DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a Row values from DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetRowValuesFromDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        public string v_ExcelRowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnStart))]
        public string v_ColumnStart { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnEnd))]
        public string v_ColumnEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTableVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("DataTable Row Index")]
        [InputSpecification("DataTable Row Index", true)]
        //[SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [PropertyDetailSampleUsage("**0**", "Specify the First Row Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Row Index")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Row Index")]
        [PropertyValidationRule("DataTable Row Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "DataTable Row")]
        public string v_DataTableRowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When DataTable Items Not Enough")]
        public string v_IfDataTableNotEnough { get; set; }

        public ExcelSetRowValuesFromDataTableCommand()
        {
            //this.CommandName = "ExcelSetRowValuesFromDataTableCommand";
            //this.SelectionName = "Set Row Values From DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (_, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);


            DataTable myDT = v_DataTableVariable.GetDataTableVariable(engine);

            (int excelRowIndex, int columnStartIndex, int columnEndIndex, string valueType) =
                ExcelControls.GetRangeIndeiesRowDirection(
                    nameof(v_ExcelRowIndex), nameof(v_ColumnType),
                    nameof(v_ColumnStart), nameof(v_ColumnEnd),
                    nameof(v_ValueType), engine, excelSheet, this,
                    myDT
                );

            int dtRowIndex = this.ConvertToUserVariableAsInteger(nameof(v_DataTableRowIndex), "DataTable Row Index", engine);
            if  (dtRowIndex >= myDT.Rows.Count)
            {
                throw new Exception("DataTable Row " + v_DataTableRowIndex + " is not exists");
            }

            string ifDataTableNotEnough = this.GetUISelectionValue(nameof(v_IfDataTableNotEnough), "If DataTable Not Enough", engine);
            int range = columnEndIndex - columnStartIndex + 1;
            if (ifDataTableNotEnough == "error")
            {
                if (range > myDT.Columns.Count)
                {
                    throw new Exception("DataTable Items not enough");
                }
            }

            int max = range;
            if (range > myDT.Columns.Count)
            {
                max = myDT.Columns.Count;
            }

            Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.GetUISelectionValue("v_ValueType", this, engine));

            for (int i = 0; i < max; i++)
            {
                string setValue = myDT.Rows[dtRowIndex][i]?.ToString() ?? "";
                setFunc(setValue, excelSheet, columnStartIndex + i, excelRowIndex);
            }
        }
    }
}
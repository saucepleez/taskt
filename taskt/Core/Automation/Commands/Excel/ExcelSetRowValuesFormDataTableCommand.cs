using System;
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
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetRowValuesFromDataTableCommand : AExcelRowRangeSetCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        //[PropertyParameterOrder(6000)]
        //public string v_RowIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        //[PropertyParameterOrder(6001)]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnStart))]
        //[PropertyParameterOrder(6002)]
        //public string v_ColumnStart { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnEnd))]
        //[PropertyParameterOrder(6003)]
        //public string v_ColumnEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyParameterOrder(10000)]
        public string v_DataTableVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("DataTable Row Index")]
        [InputSpecification("DataTable Row Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specify the First Row Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Row Index")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Row Index")]
        [PropertyValidationRule("DataTable Row Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "DataTable Row")]
        [PropertyParameterOrder(10001)]
        public string v_DataTableRowIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6006)]
        //public string v_ValueType { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When DataTable Items Not Enough")]
        //[PropertyParameterOrder(6007)]
        public override string v_WhenItemNotEnough { get; set; }

        public ExcelSetRowValuesFromDataTableCommand()
        {
            //this.CommandName = "ExcelSetRowValuesFromDataTableCommand";
            //this.SelectionName = "Set Row Values From DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(_, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);


            //var myDT = v_DataTableVariable.ExpandUserVariableAsDataTable(engine);

            //(int excelRowIndex, int columnStartIndex, int columnEndIndex, string valueType) =
            //    ExcelControls.GetRangeIndeiesRowDirection(
            //        nameof(v_RowIndex), nameof(v_ColumnType),
            //        nameof(v_ColumnStart), nameof(v_ColumnEnd),
            //        nameof(v_ValueType), engine, excelSheet, this,
            //        myDT
            //    );

            //int dtRowIndex = this.ExpandValueOrUserVariableAsInteger(nameof(v_DataTableRowIndex), "DataTable Row Index", engine);
            //if  (dtRowIndex >= myDT.Rows.Count)
            //{
            //    throw new Exception("DataTable Row " + v_DataTableRowIndex + " is not exists");
            //}

            //string ifDataTableNotEnough = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenItemNotEnough), "If DataTable Not Enough", engine);
            //int range = columnEndIndex - columnStartIndex + 1;
            //if (ifDataTableNotEnough == "error")
            //{
            //    if (range > myDT.Columns.Count)
            //    {
            //        throw new Exception("DataTable Items not enough");
            //    }
            //}

            //int max = range;
            //if (range > myDT.Columns.Count)
            //{
            //    max = myDT.Columns.Count;
            //}

            ////Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.ExpandValueOrUserVariableAsSelectionItem("v_ValueType", this, engine));
            //var setFunc = ExcelControls.SetCellValueFunction(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), engine));

            //for (int i = 0; i < max; i++)
            //{
            //    string setValue = myDT.Rows[dtRowIndex][i]?.ToString() ?? "";
            //    setFunc(setValue, excelSheet, columnStartIndex + i, excelRowIndex);
            //}

            var myDT = v_DataTableVariable.ExpandUserVariableAsDataTable(engine);
            var dtRowIndex = v_DataTableRowIndex.ExpandValueOrUserVariableAsInteger("DataTable Row Index", engine);

            (_, var sheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            (var row, var columnStart, var columnEnd) = this.ExpandValueOrVariableAsExcelRangeIndecies(engine, new Func<int>(() => myDT.Columns.Count));
            int max = (columnEnd - columnStart) + 1;
            var setFunc = this.ExpandValueOrVaribleAsSetValueAction(engine);
            for (int i = 0; i < max; i++)
            {
                string setValue = myDT.Rows[dtRowIndex][i]?.ToString() ?? "";
                setFunc(setValue, sheet, columnStart + i, row);
            }
        }
    }
}
using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Range")]
    [Attributes.ClassAttributes.CommandSettings("Get Range Values As DataTable")]
    [Attributes.ClassAttributes.Description("This command get Range values as DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Range values as DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetRangeValuesAsDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

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
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowStart))]
        [PropertyIsOptional(false)]
        public string v_RowStart { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowEnd))]
        public string v_RowEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        public string v_ValueType { get; set; }

        public ExcelGetRangeValuesAsDataTableCommand()
        {
            //this.CommandName = "ExcelGetRangeValuesAsDataTableCommand";
            //this.SelectionName = "Get Range Values As DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            string valueType = this.GetUISelectionValue(nameof(v_ValueType), "Value Type", engine);

            int rowStartIndex = this.ConvertToUserVariableAsInteger(nameof(v_RowStart), "Start Row", engine);

            int columnStartIndex = 0;
            int columnEndIndex = 0;
            switch(this.GetUISelectionValue(nameof(v_ColumnType), "Column Type", engine))
            {
                case "range":
                    columnStartIndex = ExcelControls.GetColumnIndex(excelSheet, v_ColumnStart.ConvertToUserVariable(engine));
                    if (String.IsNullOrEmpty(v_ColumnEnd))
                    {
                        columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowStartIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        columnEndIndex = ExcelControls.GetColumnIndex(excelSheet, v_ColumnEnd.ConvertToUserVariable(engine));
                    }
                    break;

                case "rc":
                    columnStartIndex = this.ConvertToUserVariableAsInteger(nameof(v_ColumnStart), "Column Start", engine);
                    if (String.IsNullOrEmpty(v_ColumnEnd))
                    {
                        columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowStartIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        columnEndIndex = this.ConvertToUserVariableAsInteger(nameof(v_ColumnEnd), "Column End", engine);
                    }
                    break;
            }

            if (columnStartIndex > columnEndIndex)
            {
                int t = columnStartIndex;
                columnStartIndex = columnEndIndex;
                columnEndIndex = t;
            }

            int rowEndIndex;
            if (String.IsNullOrEmpty(v_RowEnd))
            {
                rowEndIndex = ExcelControls.GetLastRowIndex(excelSheet, columnStartIndex, rowStartIndex, valueType);
            }
            else
            {
                rowEndIndex = this.ConvertToUserVariableAsInteger(nameof(v_RowEnd), "Row End", engine);
            }

            if (rowStartIndex > rowEndIndex)
            {
                int t = rowStartIndex;
                rowStartIndex = rowEndIndex;
                rowEndIndex = t;
            }

            ExcelControls.CheckCorrectRCRange(rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex, excelInstance);

            Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = ExcelControls.GetCellValueFunction(valueType);

            int rowRange = rowEndIndex - rowStartIndex + 1;
            int colRange = columnEndIndex - columnStartIndex + 1;

            DataTable newDT = new DataTable();
            // set columns
            for (int i = 0; i < colRange; i++) 
            {
                newDT.Columns.Add(ExcelControls.GetColumnName(excelSheet, columnStartIndex + i));
            }

            for (int i = 0; i < rowRange; i++)
            {
                newDT.Rows.Add();
                for (int j = 0; j < colRange; j++)
                {
                    newDT.Rows[i][j] = getFunc(excelSheet, columnStartIndex + j, rowStartIndex + i);
                }
            }

            newDT.StoreInUserVariable(engine, v_userVariableName);
        }
    }
}
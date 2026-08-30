using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Range")]
    [Attributes.ClassAttributes.CommandSettings("Get Range Values As DataTable")]
    [Attributes.ClassAttributes.Description("This command get Range values as DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Range values as DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGetRangeValuesAsDataTableCommand : AExcelColumnRowRangeGetCommands, IDataTableResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        //[PropertyParameterOrder(6000)]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnStart))]
        //[PropertyParameterOrder(6001)]
        //public string v_ColumnStart { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnEnd))]
        //[PropertyParameterOrder(6002)]
        //public string v_ColumnEnd { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowStart))]
        //[PropertyIsOptional(false)]
        //[PropertyParameterOrder(6003)]
        //public string v_RowStart { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowEnd))]
        //[PropertyParameterOrder(6004)]
        //public string v_RowEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        //[PropertyParameterOrder(6005)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6006)]
        //public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use the First Row as the Column Names (Value Type is Cell only)")]
        [PropertyIsOptional(true, "No")]
        [PropertyParameterOrder(13000)]
        public string v_FirstRowAsColumnName { get; set; }

        public ExcelGetRangeValuesAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (_, var excelSheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            (var rowStartIndex, var columnStartIndex, var rowEndIndex, var columnEndIndex) = this.ExpandValueOrVariableAsExcelRangeIndicies(engine);

            var getFunc = this.ExpandValueOrVariableAsGetValueFunction(engine);

            int rowRange = rowEndIndex - rowStartIndex + 1;
            int colRange = columnEndIndex - columnStartIndex + 1;

            var newDT = new DataTable();
            // set columns
            for (int i = 0; i < colRange; i++) 
            {
                newDT.Columns.Add(excelSheet.ToColumnName(columnStartIndex + i));
            }

            for (int i = 0; i < rowRange; i++)
            {
                newDT.Rows.Add();
                for (int j = 0; j < colRange; j++)
                {
                    newDT.Rows[i][j] = getFunc(excelSheet, columnStartIndex + j, rowStartIndex + i);
                }
            }

            var valueType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), "Value Type", engine);
            if ((valueType == "cell") && (this.ExpandValueOrUserVariableAsYesNo(nameof(v_FirstRowAsColumnName), engine)))
            {
                if (newDT.Rows.Count > 0)
                {
                    for (int i = newDT.Columns.Count - 1; i >= 0; i--)
                    {
                        newDT.Columns[i].ColumnName = newDT.Rows[0]?.ToString() ?? "";
                    }
                    newDT.Rows[0].Delete();
                }
            }

            this.StoreDataTableInUserVariable(newDT, engine);
        }
    }
}
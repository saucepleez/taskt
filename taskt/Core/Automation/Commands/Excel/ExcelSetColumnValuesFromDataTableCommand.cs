using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.CommandSettings("Set Column Values From DataTable")]
    [Attributes.ClassAttributes.Description("This command set Column values from DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set Column values from DataTable.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetColumnValuesFromDataTableCommand : AExcelColumnRangeSetCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        //[PropertyParameterOrder(6000)]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnNameOrIndex))]
        //[PropertyParameterOrder(6001)]
        //public string v_ColumnIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowStart))]
        //[PropertyParameterOrder(6002)]
        //public string v_RowStart { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowEnd))]
        //[PropertyParameterOrder(6003)]
        //public string v_RowEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyParameterOrder(10000)]
        public string v_DataTableVariable { get; set; }

        [XmlAttribute]
        [PropertyDescription("DataTable Column Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("DataTable Column Index", true)]
        //[SampleUsage("**0** or **1** or **{{{vColumn}}}**")]
        [PropertyDetailSampleUsage("**0**", "Specify the First Column")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Column Index")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column Index")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Column")]
        [PropertyParameterOrder(10001)]
        public string v_DataTableColumnIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6006)]
        //public string v_ValueType { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When DataTable Items Not Enough")]
        //[PropertyParameterOrder(6007)]
        public override string v_WhenItemNotEnough { get; set; }

        public ExcelSetColumnValuesFromDataTableCommand()
        {
            //this.CommandName = "ExcelSetColumnValuesFromDataTableCommand";
            //this.SelectionName = "Set Column Values From DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(_, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);
            (_, var excelSheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            DataTable myDT = v_DataTableVariable.ExpandUserVariableAsDataTable(engine);
            //int dtColumnIndex = this.ExpandValueOrUserVariableAsInteger(nameof(v_DataTableColumnIndex), "DataTable Column Index", engine);
            var dtColumnIndex = v_DataTableColumnIndex.ExpandValueOrUserVariableAsInteger("DataTable Column Index", engine);
            if ((dtColumnIndex < 0) || (dtColumnIndex >= myDT.Columns.Count))
            {
                throw new Exception($"Column index '{v_DataTableColumnIndex}' is not exists");
            }

            //(int excelColumnIndex, int rowStart, int rowEnd, string valueType) =
            //    ExcelControls.GetRangeIndeiesColumnDirection(
            //        nameof(v_ColumnIndex), nameof(v_ColumnType),
            //        nameof(v_RowStart), nameof(v_RowEnd),
            //        nameof(v_ValueType), engine, excelSheet, this,
            //        myDT
            //    );

            //int range = rowEnd - rowStart + 1;

            //string ifDataTableNotEnough = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenItemNotEnough), "Id DataTable Not Enough", engine);
            //if (ifDataTableNotEnough == "error")
            //{
            //    if (range > myDT.Rows.Count)
            //    {
            //        throw new Exception("DataTable items not enough");
            //    }
            //}

            //int max = range;
            //if (range > myDT.Rows.Count)
            //{
            //    max = myDT.Rows.Count;
            //}

            (var excelColumnIndex, var rowStartIndex, var rowEndIndex) = this.ExpandValueOrVariableAsExcelRangeIndicies(engine, new Func<int>(() => myDT.Rows.Count));

            //Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.ExpandValueOrUserVariableAsSelectionItem("v_ValueType", this, engine));
            //var setFunc = ExcelControls.SetCellValueFunction(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), engine));
            var setFunc = this.ExpandValueOrVaribleAsSetValueAction(engine);

            int max = rowEndIndex - rowStartIndex + 1;
            for (int i = 0; i < max; i++)
            {
                string value = myDT.Rows[i][dtColumnIndex]?.ToString() ?? "";
                setFunc(value, excelSheet, excelColumnIndex, rowStartIndex + i);
            }
        }
    }
}
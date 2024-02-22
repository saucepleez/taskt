using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.CommandSettings("Get Row Values As Dictionary")]
    [Attributes.ClassAttributes.Description("This command get Row values as Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Row values as Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetRowValuesAsDictionaryCommand : AExcelRowRangeGetCommands
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
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        //[PropertyParameterOrder(6004)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6005)]
        //public string v_ValueType { get; set; }

        public ExcelGetRowValuesAsDictionaryCommand()
        {
            //this.CommandName = "ExcelGetRowValuesAsDictionaryCommand";
            //this.SelectionName = "Get Row Values As Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var excelInstance, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            //(int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) =
            //    ExcelControls.GetRangeIndeiesRowDirection(
            //        nameof(v_RowIndex),
            //        nameof(v_ColumnType), nameof(v_ColumnStart), nameof(v_ColumnEnd),
            //        nameof(v_ValueType), engine, excelSheet, this
            //    );

            //Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = ExcelControls.GetCellValueFunction(valueType);

            (_, var excelSheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            (var rowIndex, var columnStartIndex, var columnEndIndex) = this.ExpandValueOrVariableAsExcelRangeIndecies(engine);
            var getFunc = this.ExpandValueOrVariableAsGetValueFunction(engine);

            var newDic = new Dictionary<string, string>();

            for (int i = columnStartIndex; i <= columnEndIndex; i++)
            {
                //string keyName = ExcelControls.GetAddress(excelSheet, rowIndex, i);
                var keyName = excelSheet.ToColumnName(i);
                newDic.Add(keyName, getFunc(excelSheet, i, rowIndex));
            }

            newDic.StoreInUserVariable(engine, v_Result);
        }
    }
}
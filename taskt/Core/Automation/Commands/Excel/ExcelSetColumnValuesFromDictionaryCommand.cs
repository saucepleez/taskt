using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.CommandSettings("Set Column Values From Dictionary")]
    [Attributes.ClassAttributes.Description("This command set Column values from Ditionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set Column values from Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetColumnValuesFromDictionaryCommand : AExcelColumnRangeSetCommands
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
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyParameterOrder(10000)]
        public string v_DictionaryVariable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6005)]
        //public string v_ValueType { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When Dictionary Items Not Enough")]
        //[PropertyParameterOrder(6006)]
        public override string v_WhenItemNotEnough { get; set; }

        public ExcelSetColumnValuesFromDictionaryCommand()
        {
            //this.CommandName = "ExcelSetColumnValuesFromDictionaryCommand";
            //this.SelectionName = "Set Column Values From Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(_, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            //(int columnIndex, int rowStart, int rowEnd, string valueType) =
            //    ExcelControls.GetRangeIndeiesColumnDirection(
            //        nameof(v_ColumnIndex), nameof(v_ColumnType),
            //        nameof(v_RowStart), nameof(v_RowEnd),
            //        nameof(v_ValueType), engine, excelSheet, this,
            //        myDic
            //    );

            //int range = rowEnd - rowStart + 1;

            //string ifDictionaryNotEnough = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenItemNotEnough), "If Dictionary Not Enough", engine);
            //if (ifDictionaryNotEnough == "error")
            //{
            //    if (range > myDic.Count)
            //    {
            //        throw new Exception("Dictionary items not enough");
            //    }
            //}

            //int max = range;
            //if (range > myDic.Count)
            //{
            //    max = myDic.Count;
            //}

            //Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.ExpandValueOrUserVariableAsSelectionItem("v_ValueType", this, engine));
            //var setFunc = ExcelControls.SetCellValueFunction(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), engine));

            //(_, var excelSheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            //var myDic = v_DictionaryVariable.ExpandUserVariableAsDictinary(engine);

            //(var columnIndex, var rowStartIndex, var rowEndIndex) = this.ExpandValueOrVariableAsExcelRangeIndicies(engine, new Func<int>(() => myDic.Count));
            //var setFunc = this.ExpandValueOrVaribleAsSetValueAction(engine);

            //// copy key list
            //string[] keys = new string[myDic.Keys.Count];
            //myDic.Keys.CopyTo(keys, 0);

            //int max = rowEndIndex - rowStartIndex + 1;
            //for (int i = 0; i < max; i++)
            //{
            //    setFunc(myDic[keys[i]], excelSheet, columnIndex, rowStartIndex + i);
            //}

            var myDic = v_DictionaryVariable.ExpandUserVariableAsDictinary(engine);
            // copy key list
            string[] keys = new string[myDic.Keys.Count];
            myDic.Keys.CopyTo(keys, 0);

            this.ColumnRangeAction(
                new Func<int>(() => myDic.Count),
                new Func<int, string>((index) =>
                {
                    return myDic[keys[index]];
                }), engine
            );
        }
    }
}
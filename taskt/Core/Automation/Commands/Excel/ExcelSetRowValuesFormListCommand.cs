using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.CommandSettings("Set Row Values From List")]
    [Attributes.ClassAttributes.Description("This command set Row values from List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a Row values from List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetRowValuesFromListCommand : AExcelRowRangeSetCommands
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
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyParameterOrder(10000)]
        public string v_ListVariable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6005)]
        //public string v_ValueType { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When List Items Not Enough")]
        //[PropertyParameterOrder(6006)]
        public override string v_WhenItemNotEnough { get; set; }

        public ExcelSetRowValuesFromListCommand()
        {
            //this.CommandName = "ExcelSetRowValuesFromListCommand";
            //this.SelectionName = "Set Row Values From List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) =
            //    ExcelControls.GetRangeIndeiesRowDirection(
            //        nameof(v_RowIndex), nameof(v_ColumnType),
            //        nameof(v_ColumnStart), nameof(v_ColumnEnd),
            //        nameof(v_ValueType), engine, excelSheet, this,
            //        myList
            //    );

            //string ifListNotEnough = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenItemNotEnough), "If List Not Enough", engine);
            //int range = columnEndIndex - columnStartIndex + 1;
            //if (ifListNotEnough == "error")
            //{
            //    if (range > myList.Count)
            //    {
            //        throw new Exception("List Items not enough");
            //    }
            //}

            //int max = range;
            //if (range > myList.Count)
            //{
            //    max = myList.Count;
            //}

            //Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.ExpandValueOrUserVariableAsSelectionItem("v_ValueType", this, engine));
            //var setFunc = ExcelControls.SetCellValueFunction(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), engine));

            (_, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            var myList = v_ListVariable.ExpandUserVariableAsList(engine);

            (var rowIndex, var columnStartIndex, var columnEndIndex) = this.ExpandValueOrVariableAsExcelRangeIndecies(engine, new Func<int>(() => myList.Count));

            var setFunc = this.ExpandValueOrVaribleAsSetValueAction(engine);
            var max = columnEndIndex - columnStartIndex + 1;
            for (int i = 0; i < max; i++)
            {
                setFunc(myList[i], excelSheet, columnStartIndex + i, rowIndex);
            }
        }
    }
}
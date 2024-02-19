using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.CommandSettings("Set Column Values From List")]
    [Attributes.ClassAttributes.Description("This command set Column values from List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set Column values from List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetColumnValuesFromListCommand : AExcelInstanceCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        [PropertyParameterOrder(6000)]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnNameOrIndex))]
        [PropertyParameterOrder(6001)]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowStart))]
        [PropertyParameterOrder(6002)]
        public string v_RowStart { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowEnd))]
        [PropertyParameterOrder(6003)]
        public string v_RowEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyParameterOrder(6004)]
        public string v_ListVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        [PropertyParameterOrder(6005)]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When List Items Not Enough")]
        [PropertyParameterOrder(6006)]
        public string v_WhenItemNotEnough { get; set; }

        public ExcelSetColumnValuesFromListCommand()
        {
            //this.CommandName = "ExcelSetColumnValuesFromListCommand";
            //this.SelectionName = "Set Column Values From List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (_, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            
            List<string> myList = v_ListVariable.ExpandUserVariableAsList(engine);

            (int columnIndex, int rowStart, int rowEnd, string valueType) =
               ExcelControls.GetRangeIndeiesColumnDirection(
                   nameof(v_ColumnIndex), nameof(v_ColumnType),
                   nameof(v_RowStart), nameof(v_RowEnd),
                   nameof(v_ValueType), engine, excelSheet, this,
                   myList
               );

            int range = rowEnd - rowStart + 1;

            string ifListNotEnough = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenItemNotEnough), "If List Not Enough", engine);
            if (ifListNotEnough == "error")
            {
                if (range > myList.Count)
                {
                    throw new Exception("List items not enough");
                }
            }

            int max = range;
            if (range > myList.Count)
            {
                max = myList.Count;
            }

            //Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.ExpandValueOrUserVariableAsSelectionItem("v_ValueType", this, engine));
            var setFunc = ExcelControls.SetCellValueFunction(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), engine));

            for (int i = 0; i < max; i++)
            {
                setFunc(myList[i], excelSheet, columnIndex, rowStart + i);
            }
        }
    }
}
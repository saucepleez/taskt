using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Get Cell RC")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetCellRCCommand : AExcelRCLocationActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        //[PropertyParameterOrder(6000)]
        //public string v_CellRow { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnLocation))]
        //[PropertyParameterOrder(6001)]
        //public string v_CellColumn { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(8000)]
        public string v_Result { get; set; }

        ////[XmlAttribute]
        ////[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        ////[PropertyParameterOrder(6003)]
        ////public string v_ValueType { get; set; }

        public ExcelGetCellRCCommand()
        {
            //this.CommandName = "ExcelGetCellRCCommand";
            //this.SelectionName = "Get Cell RC";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var excelInstance, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            //var rg = this.GetExcelRange(nameof(v_CellRow), nameof(v_CellColumn), engine, excelInstance, excelSheet);

            //var valueType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), engine);

            //var func = ExcelControls.GetCellValueFunctionFromRange(valueType);

            //func(rg).StoreInUserVariable(engine, v_Result);

            //var rg = this.ExpandValueOrVariableAsExcelCellLocation(engine);

            (_, var sheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            (int row, int column) = this.ExpandValueOrVariableAsCellRowAndColumnIndex(engine);
            var getFunc = this.ExpandValueOrVariableAsGetValueFunction(engine);
            getFunc(sheet, column, row).StoreInUserVariable(engine, v_Result);
        }
    }
}
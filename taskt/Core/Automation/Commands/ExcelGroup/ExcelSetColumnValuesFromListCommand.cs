using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.CommandSettings("Set Column Values From List")]
    [Attributes.ClassAttributes.Description("This command set Column values from List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set Column values from List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelSetColumnValuesFromListCommand : AExcelColumnRangeSetCommands, ICanHandleList
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

        public ExcelSetColumnValuesFromListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var myList = this.ExpandUserVariableAsList(nameof(v_ListVariable), engine);

            this.ColumnRangeAction(
                new Func<int>(() => myList.Count),
                new Func<int, string>((index) =>
                {
                    return myList[index];
                }), engine
            );
        }
    }
}
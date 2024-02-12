using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Check Cell Value Exists")]
    [Attributes.ClassAttributes.Description("This command checks existance value from a specified Excel Cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCheckCellValueExistsCommand : AExcelInstanceCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CellRangeLocation))]
        [PropertyParameterOrder(6000)]
        public string v_CellLocation { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Value Exists, Result is **True**")]
        [PropertyParameterOrder(6001)]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CheckableValueType))]
        [PropertySelectionChangeEvent(nameof(cmbValueType_SelectedIndexChanged))]
        [PropertyParameterOrder(6002)]
        public string v_ValueType { get; set; }

        public ExcelCheckCellValueExistsCommand()
        {
            //this.CommandName = "ExcelCheckCellValueExistsCommand";
            //this.SelectionName = "Check Cell Value Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var excelInstance, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            var rg = v_CellLocation.GetExcelRange(engine, excelInstance, excelSheet, this);

            //var valueType = this.GetUISelectionValue(nameof(v_ValueType), engine);

            //var chkFunc = ExcelControls.CheckCellValueFunctionFromRange(valueType);
            var chkFunc = ExcelControls.CheckCellValueFunctionFromRange(nameof(v_ValueType), this, engine);

            chkFunc(rg).StoreInUserVariable(engine, v_Result);
        }

        private void cmbValueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //(var body, var lblValueType, var lbl2ndValueType) = this.ControlsList.GetAllPropertyControl(nameof(v_ValueType));
            //ComboBox cmbValueType = (ComboBox)body;
            (var cmbValueType, var lblValueType, var lbl2ndValueType) = ControlsList.GetAllPropertyControl<ComboBox>(nameof(v_ValueType));

            string searchedKey = cmbValueType.SelectedItem.ToString();
            Dictionary<string, string> dic = (Dictionary<string, string>)lblValueType.Tag;

            lbl2ndValueType.Text = dic.ContainsKey(searchedKey) ? dic[searchedKey] : "";
        }
    }
}
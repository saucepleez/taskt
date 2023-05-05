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
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCheckCellValueExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CellRangeLocation))]
        public string v_ExcelCellAddress { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Value Exists, Result is **True**")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CheckableValueType))]
        [PropertySelectionChangeEvent(nameof(cmbValueType_SelectedIndexChanged))]
        public string v_ValueType { get; set; }

        public ExcelCheckCellValueExistsCommand()
        {
            //this.CommandName = "ExcelCheckCellValueExistsCommand";
            //this.SelectionName = "Check Cell Value Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            var rg = v_ExcelCellAddress.GetExcelRange(engine, excelInstance, excelSheet, this);

            var valueType = this.GetUISelectionValue(nameof(v_ValueType), engine);

            var chkFunc = ExcelControls.CheckCellValueFunctionFromRange(valueType);

            chkFunc(rg).StoreInUserVariable(engine, v_userVariableName);
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
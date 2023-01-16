using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.Description("This command checks existance value from a specified Excel Cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCheckCellValueExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Instance")]
        //[PropertyFirstValue("%kwd_default_excel_instance%")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the Cell Location")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter the actual location of the cell.")]
        //[SampleUsage("**A1** or **B10** or **{{{vAddress}}}**")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Cell", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Cell")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CellRangeLocation))]
        public string v_ExcelCellAddress { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the result")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        //[PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Value Exists, Result is **True**")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Value Type")]
        //[InputSpecification("")]
        //[Remarks("")]
        //[PropertyUISelectionOption("Cell")]
        //[PropertyUISelectionOption("Formula")]
        //[PropertyUISelectionOption("Back Color")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsOptional(true, "Cell")]
        //[PropertySecondaryLabel(true)]
        //[PropertyAddtionalParameterInfo("Cell", "Check cell has value or not")]
        //[PropertyAddtionalParameterInfo("Formula", "Check cell has formula or not")]
        //[PropertyAddtionalParameterInfo("Back Color", "Check back color is not white")]
        //[PropertySelectionChangeEvent("cmbValueType_SelectedIndexChanged")]
        //[PropertyDisplayText(true, "Type")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CheckableValueType))]
        [PropertySelectionChangeEvent(nameof(cmbValueType_SelectedIndexChanged))]
        public string v_ValueType { get; set; }

        public ExcelCheckCellValueExistsCommand()
        {
            this.CommandName = "ExcelCheckCellValueExistsCommand";
            this.SelectionName = "Check Cell Value Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
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
            (var body, var lblValueType, var lbl2ndValueType) = this.ControlsList.GetAllPropertyControl(nameof(v_ValueType));
            ComboBox cmbValueType = (ComboBox)body;

            string searchedKey = cmbValueType.SelectedItem.ToString();
            Dictionary<string, string> dic = (Dictionary<string, string>)lblValueType.Tag;

            lbl2ndValueType.Text = dic.ContainsKey(searchedKey) ? dic[searchedKey] : "";
        }
    }
}
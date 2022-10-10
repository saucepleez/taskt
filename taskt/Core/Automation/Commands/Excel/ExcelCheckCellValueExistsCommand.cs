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
    [Attributes.ClassAttributes.Description("This command checks existance value from a specified Excel Cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCheckCellValueExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Cell Location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the actual location of the cell.")]
        [SampleUsage("**A1** or **B10** or **{{{vAddress}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Cell", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Cell")]
        public string v_ExcelCellAddress { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the result")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value type")]
        [InputSpecification("")]
        [SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        [Remarks("")]
        [PropertyUISelectionOption("Cell")]
        [PropertyUISelectionOption("Formula")]
        [PropertyUISelectionOption("Back Color")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Cell")]
        [PropertySecondaryLabel(true)]
        [PropertyAddtionalParameterInfo("Cell", "Check cell has value or not")]
        [PropertyAddtionalParameterInfo("Formula", "Check cell has formula or not")]
        [PropertyAddtionalParameterInfo("Back Color", "Check back color is not white")]
        [PropertyControlIntoCommandField("cmbValueType", "lblValueType", "lbl2ndValueType")]
        [PropertySelectionChangeEvent("cmbValueType_SelectedIndexChanged")]
        [PropertyDisplayText(true, "Type")]
        public string v_ValueType { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox cmbValueType;

        [XmlIgnore]
        [NonSerialized]
        private Label lbl2ndValueType;

        [XmlIgnore]
        [NonSerialized]
        private Label lblValueType;

        public ExcelCheckCellValueExistsCommand()
        {
            this.CommandName = "ExcelCheckCellValueExistsCommand";
            this.SelectionName = "Check Cell Value Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InstanceName = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var excelObject = engine.GetAppInstance(vInstance);
            //Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;

            //var excelInstance = v_InstanceName.GetExcelInstance(engine);
            //Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndSheet(engine);

            //var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);
            var targetAddress = v_ExcelCellAddress.ConvertToUserVariableAsExcelRangeLocation(engine, excelInstance);

            //var valueType = v_ValueType.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(valueType))
            //{
            //    valueType = "Cell";
            //}
            var valueType = v_ValueType.GetUISelectionValue("v_ValueType", this, engine);

            bool valueState = false;
            switch (valueType)
            {
                case "cell":
                    valueState= !String.IsNullOrEmpty((string)excelSheet.Range[targetAddress].Text);
                    break;
                case "formula":
                    valueState = ((string)excelInstance.Range[targetAddress].Formula).StartsWith("=");
                    break;
                case "back color":
                    valueState = ((long)excelInstance.Range[targetAddress].Interior.Color) != 16777215;
                    break;
                //default:
                //    throw new Exception("Value type " + valueType + " is not support.");
                //    break;
            }
             
            valueState.StoreInUserVariable(engine, v_userVariableName);            
        }
        private void cmbValueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string searchedKey = cmbValueType.SelectedItem.ToString();
            Dictionary<string, string> dic = (Dictionary<string, string>)lblValueType.Tag;

            lbl2ndValueType.Text = dic.ContainsKey(searchedKey) ? dic[searchedKey] : "";
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctls);

        //    cmbValueType = (ComboBox)ctls.Where(t => t.Name == "v_ValueType").FirstOrDefault();
        //    cmbValueType.SelectedIndexChanged += (sender, e) => cmbValueType_SelectedIndexChanged(sender, e);

        //    lbl2ndValueType = (Label)ctls.Where(t => t.Name == "lbl2_v_ValueType").FirstOrDefault();
        //    lblValueType = (Label)ctls.GetControlsByName("v_ValueType", CommandControls.CommandControlType.Label)[0];

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Check " + v_ValueType + " Value Exists From '" + v_ExcelCellAddress + "' and apply to variable '" + v_userVariableName + "', Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_ExcelCellAddress))
        //    {
        //        this.validationResult += "Address is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.Description("This command allows you to find the last row in a used range in an Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to determine how many rows have been used in the Excel Workbook.  You can use this value in a **Number Of Times** Loop to get data.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetLastRowCommand : ScriptCommand
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
        [PropertyDisplayText(true, "Intance")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter Letter of the Column to check")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid column letter")]
        [SampleUsage("**A** or **B** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "A")]
        [PropertyDisplayText(true, "Column")]
        public string v_ColumnLetter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the row number")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_applyToVariableName { get; set; }

        public ExcelGetLastRowCommand()
        {
            this.CommandName = "ExcelGetLastRowCommand";
            this.SelectionName = "Get Last Row Index";
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
            //var excelSheet = excelInstance.ActiveSheet;
            (_, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            if (String.IsNullOrEmpty(v_ColumnLetter))
            {
                v_ColumnLetter = "A";
            }
            var columnLetter = v_ColumnLetter.ConvertToUserVariable(engine);

            //var lastRow = (int)excelSheet.Cells(excelSheet.Rows.Count, columnLetter).End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row;
            var lastRow = (int)excelSheet.Cells[excelSheet.Rows.Count, columnLetter].End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row;

            lastRow.ToString().StoreInUserVariable(sender, v_applyToVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ColumnLetter", this, editor));

        //    ////create control for variable name
        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
        //    }

        //    return RenderedControls;
        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Column '" + v_ColumnLetter + "', Apply to '" + v_applyToVariableName + "', Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
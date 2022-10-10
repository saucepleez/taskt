using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.Description("This command sets the value of a cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetCellCommand : ScriptCommand
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
        [PropertyDescription("Please Enter text to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the text value that will be set.")]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public string v_TextToSet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Cell Location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the actual location of the cell.")]
        [SampleUsage("**A1** or **B10** or **{{{vAddress}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Cell Location", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Cell")]
        public string v_ExcelCellAddress { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value type")]
        [InputSpecification("")]
        [SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        [Remarks("")]
        [PropertyUISelectionOption("Cell")]
        [PropertyUISelectionOption("Formula")]
        [PropertyUISelectionOption("Format")]
        [PropertyUISelectionOption("Font Color")]
        [PropertyUISelectionOption("Back Color")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Cell")]
        [PropertyDisplayText(true, "Value Type")]
        public string v_ValueType { get; set; }

        public ExcelSetCellCommand()
        {
            this.CommandName = "ExcelSetCellCommand";
            this.SelectionName = "Set Cell";
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
            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            //var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);
            //var targetAddress = v_ExcelCellAddress.ConvertToUserVariableAsExcelRangeLocation(engine, excelInstance);
            var rg = v_ExcelCellAddress.GetExcelRange(engine, excelInstance, excelSheet, this);

            var targetText = v_TextToSet.ConvertToUserVariable(sender);

            long colorToSet = 0;
            switch (targetText)
            {
                case "fore color":
                case "back color":
                    if (long.TryParse(targetText, out colorToSet))
                    {
                        throw new Exception("Value to set '" + targetText + "' is not color.");
                    }
                    break;
            }

            //var valueType = v_ValueType.ConvertToUserVariable(sender);
            //if (String.IsNullOrWhiteSpace(valueType))
            //{
            //    valueType = "Cell";
            //}
            var valueType = v_ValueType.GetUISelectionValue("v_ValueType", this, engine);

            switch (valueType)
            {
                case "cell":
                    //excelSheet.Range[targetAddress].Value = targetText;
                    rg.Value = targetText;
                    break;
                case "formula":
                    //excelSheet.Range[targetAddress].Formula = targetText;
                    rg.Formula = targetText;
                    break;
                case "format":
                    //excelSheet.Range[targetAddress].NumberFormatLocal = targetText;
                    rg.NumberFormatLocal = targetText;
                    break;
                case "font color":
                    //excelSheet.Range[targetAddress].Font.Color = long.Parse(targetText);
                    //excelSheet.Range[targetAddress].Font.Color = colorToSet;
                    rg.Font.Color = colorToSet;
                    break;
                case "back color":
                    //excelSheet.Range[targetAddress].Interior.Color = long.Parse(targetText);
                    //excelSheet.Range[targetAddress].Interior.Color = colorToSet;
                    rg.Interior.Color = colorToSet;
                    break;
                //default:
                //    throw new Exception(valueType + " is not support.");
                //    break;
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellAddress", this, editor));
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
        //    return base.GetDisplayValue() + " [Set Cell " + v_ValueType + " '" + v_ExcelCellAddress + "' to '" + v_TextToSet + "', Instance Name: '" + v_InstanceName + "']";
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
        //        this.validationResult += "Cell location is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.Description("This command allows you to get a sheet info.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetWorksheetInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        [SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the sheet name")]
        [InputSpecification("")]
        [SampleUsage("**mySheet** or **%kwd_current_worksheet%** or **{{{vSheet}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Sheet")]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the information type")]
        [InputSpecification("")]
        [SampleUsage("**Name** or **Visible** or **Is first sheet** or **Is last sheet** or **Next sheet** or **Previous sheet** or **Sheet index**")]
        [Remarks("")]
        [PropertyUISelectionOption("Name")]
        [PropertyUISelectionOption("Visible")]
        [PropertyUISelectionOption("Is first sheet")]
        [PropertyUISelectionOption("Is last sheet")]
        [PropertyUISelectionOption("Next sheet")]
        [PropertyUISelectionOption("Previous sheet")]
        [PropertyUISelectionOption("Sheet index")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Information Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive a sheet info")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_applyToVariable { get; set; }

        public ExcelGetWorksheetInfoCommand()
        {
            this.CommandName = "ExcelWorksheetInfoCommand";
            this.SelectionName = "Get Worksheet Info";
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
            //Microsoft.Office.Interop.Excel.Application excelInstance = ExcelControls.getExcelInstance(engine, vInstance);
            //var excelInstance = v_InstanceName.GetExcelInstance(engine);

            //var sheetName = v_SheetName.ConvertToUserVariable(sender);
            //Microsoft.Office.Interop.Excel.Worksheet targetSheet;
            //if (sheetName == engine.engineSettings.CurrentWorksheetKeyword)
            //{
            //    targetSheet = excelInstance.ActiveSheet;
            //}
            //else
            //{
            //    targetSheet = excelInstance.Worksheets[sheetName];
            //}

            //Microsoft.Office.Interop.Excel.Worksheet targetSheet = ExcelControls.getWorksheet(engine, excelInstance, sheetName);
            //var targetSheet = v_SheetName.GetExcelWorksheet(engine, excelInstance);
            //if (targetSheet == null)
            //{
            //    throw new Exception("Worksheet " + v_SheetName + " does not exists.");
            //}

            (var excelInstance, var targetSheet) = (v_InstanceName, v_SheetName).GetExcelInstanceAndWorksheet(engine);

            string ret = "";
            int idx = 1;
            //var infoType = v_InfoType.ConvertToUserVariable(sender);
            var infoType = v_InfoType.GetUISelectionValue("v_InfoType", this, engine);
            switch (infoType)
            {
                case "name":
                    ret = targetSheet.Name;
                    break;
                case "visible":
                    ret = (targetSheet.Visible == Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetVisible) ? "TRUE" : "FALSE";
                    break;
                case "is first sheet":
                    ret = (((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[1]).Name == targetSheet.Name) ? "TRUE" : "FALSE";
                    break;
                case "is last sheet":
                    ret = (((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[excelInstance.Worksheets.Count]).Name == targetSheet.Name) ? "TRUE" : "FALSE";
                    break;
                case "next sheet":
                    var nextSht = ExcelControls.GetNextWorksheet(excelInstance, targetSheet);
                    ret = (nextSht == null) ? "" : nextSht.Name;
                    break;
                case "previous sheet":
                    var prevSht = ExcelControls.GetPreviousWorksheet(excelInstance, targetSheet);
                    ret = (prevSht == null) ? "" : prevSht.Name;
                    break;
                case "sheet index":
                    foreach (Microsoft.Office.Interop.Excel.Worksheet sht in excelInstance.Worksheets)
                    {
                        if (sht.Name == targetSheet.Name)
                        {
                            break;
                        }
                        idx++;
                    }
                    ret = idx.ToString();
                    break;
                //default:
                //    throw new Exception("Information type " + infoType + " is not support.");
                //    break;
            }

            ret.StoreInUserVariable(sender, v_applyToVariable);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

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
        //    return base.GetDisplayValue() + " [Get " + v_InfoType + " Sheet '" + v_SheetName + "', Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SheetName))
        //    {
        //        this.validationResult += "Sheet is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_InfoType))
        //    {
        //        this.validationResult += "Information type is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariable))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        public override void convertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_SheetName", "convertToIntermediateExcelSheet");
            convertToIntermediate(settings, cnv, variables);
        }

        public override void convertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_SheetName", "convertToRawExcelSheet");
            convertToRaw(settings, cnv);
        }
    }
}
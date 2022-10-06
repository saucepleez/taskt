using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.Description("This command allows you to get a specific worksheet names")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to switch to a specific worksheet")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetWorksheetsCommand : ScriptCommand
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
        [PropertyDescription("Indicate the name of the sheet to search")]
        [InputSpecification("Specify the name of the actual sheet")]
        [SampleUsage("**mySheet** or **%kwd_current_worksheet%** or **{{{vSheet}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "empty, and get all sheets")]
        [PropertyDisplayText(true, "Search")]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Specify search method")]
        [InputSpecification("")]
        [SampleUsage("**Contains** or **Start with** or **End with**")]
        [Remarks("")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Start with")]
        [PropertyUISelectionOption("End with")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Contains")]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive sheet names")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_applyToVariable { get; set; }

        public ExcelGetWorksheetsCommand()
        {
            this.CommandName = "ExcelGetWorksheetsCommand";
            this.SelectionName = "Get Worksheets";
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
            var excelInstance = v_InstanceName.getExcelInstance(engine);

            List<string> sheetNames = new List<string>();

            var targetSheetName = v_SheetName.ConvertToUserVariable(sender);
            //if (targetSheetName == engine.engineSettings.CurrentWorksheetKeyword)
            //{
            //    targetSheetName = ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet).Name;
            //}
            //Microsoft.Office.Interop.Excel.Worksheet searchedSheet = ExcelControls.getWorksheet(engine, excelInstance, targetSheetName);
            //if (searchedSheet != null)
            //{
            //    targetSheetName = searchedSheet.Name;
            //}

            if (String.IsNullOrEmpty(targetSheetName))
            {
                foreach (Microsoft.Office.Interop.Excel.Worksheet sh in excelInstance.Worksheets)
                {
                    sheetNames.Add(sh.Name);
                }
            }
            else
            {
                Func<string, string, bool> func;

                //var searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
                //if (String.IsNullOrEmpty(searchMethod))
                //{
                //    searchMethod = "Contains";
                //}
                var searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

                switch (searchMethod)
                {
                    case "contains":
                        func = (sht, search) => { return sht.Contains(search); };
                        break;
                    case "start with":
                        func = (sht, search) => { return sht.StartsWith(search); };
                        break;
                    case "end with":
                        func = (sht, search) => { return sht.EndsWith(search); };
                        break;
                    default:
                        throw new Exception("Search method " + searchMethod + " is not support.");
                        break;
                }
                foreach (Microsoft.Office.Interop.Excel.Worksheet sh in excelInstance.Worksheets)
                {
                    if (func(sh.Name, targetSheetName))
                    {
                        sheetNames.Add(sh.Name);
                    }
                }
            }

            ////get variable
            //var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariable).FirstOrDefault();

            ////create if var does not exist
            //if (requiredComplexVariable == null)
            //{
            //    engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_applyToVariable, CurrentPosition = 0 });
            //    requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariable).FirstOrDefault();
            //}

            ////assign value to variable
            //requiredComplexVariable.VariableValue = sheetNames;

            sheetNames.StoreInUserVariable(engine, v_applyToVariable);
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
        //    return base.GetDisplayValue() + " [Get Sheet Name: " + v_SearchMethod + " '" + v_SheetName + "', Instance Name: '" + v_InstanceName + "', Result '" + v_applyToVariable + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (string.IsNullOrEmpty(this.v_applyToVariable))
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
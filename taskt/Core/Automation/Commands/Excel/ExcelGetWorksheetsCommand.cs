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
        [PropertyFirstValue("%kwd_default_excel_instance%")]
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
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        public string v_applyToVariable { get; set; }

        public ExcelGetWorksheetsCommand()
        {
            this.CommandName = "ExcelGetWorksheetsCommand";
            this.SelectionName = "Get Worksheets";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            List<string> sheetNames = new List<string>();

            var targetSheetName = v_SheetName.ConvertToUserVariable(sender);
            
            if (String.IsNullOrEmpty(targetSheetName))
            {
                foreach (Microsoft.Office.Interop.Excel.Worksheet sh in excelInstance.Worksheets)
                {
                    sheetNames.Add(sh.Name);
                }
            }
            else
            {
                Func<string, string, bool> func = null;

                //var searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);
                var searchMethod = this.GetUISelectionValue(nameof(v_SearchMethod), "Search Method", engine);

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
                }

                foreach (Microsoft.Office.Interop.Excel.Worksheet sh in excelInstance.Worksheets)
                {
                    if (func(sh.Name, targetSheetName))
                    {
                        sheetNames.Add(sh.Name);
                    }
                }
            }

            sheetNames.StoreInUserVariable(engine, v_applyToVariable);
        }

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
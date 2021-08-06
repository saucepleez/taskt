using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.Description("This command allows you to get a specific worksheet names")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to switch to a specific worksheet")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelGetWorksheetsCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the name of the sheet to search (Default is empty, and get all sheets)")]
        [Attributes.PropertyAttributes.InputSpecification("Specify the name of the actual sheet")]
        [Attributes.PropertyAttributes.SampleUsage("**mySheet** or **%kwd_current_worksheet%** or **{{{vSheet}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify search method (Default is Contains)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Contains** or **Start with** or **End with**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Contains")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Start with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("End with")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive sheet names")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
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
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;

            List<string> sheetNames = new List<string>();

            var targetSheetName = v_SheetName.ConvertToUserVariable(sender);
            if (targetSheetName == engine.engineSettings.CurrentWorksheetKeyword)
            {
                targetSheetName = ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet).Name;
            }
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
                var searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
                if (String.IsNullOrEmpty(searchMethod))
                {
                    searchMethod = "Contains";
                }
                switch (searchMethod)
                {
                    case "Contains":
                        func = (sht, search) => { return sht.Contains(search); };
                        break;
                    case "Start with":
                        func = (sht, search) => { return sht.StartsWith(search); };
                        break;
                    case "End with":
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

            //get variable
            var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariable).FirstOrDefault();

            //create if var does not exist
            if (requiredComplexVariable == null)
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_applyToVariable, CurrentPosition = 0 });
                requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariable).FirstOrDefault();
            }

            //assign value to variable
            requiredComplexVariable.VariableValue = sheetNames;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
            }

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Sheet Name: " + v_SearchMethod + " '" + v_SheetName + "', Instance Name: '" + v_InstanceName + "', Result '" + v_applyToVariable + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance is empty.\n";
                this.IsValid = false;
            }
            if (string.IsNullOrEmpty(this.v_applyToVariable))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
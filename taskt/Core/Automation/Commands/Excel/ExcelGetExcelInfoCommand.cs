using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("This command allows you to get current sheet name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetExcelInfoCommand : ScriptCommand
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
        [PropertyDescription("Please select the information type to receive")]
        [InputSpecification("")]
        [SampleUsage("**File name** or **Full path file name** or **Current sheet** or **Number of sheets** or **First sheet** or **Last sheet**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("File name")]
        [PropertyUISelectionOption("Full path file name")]
        [PropertyUISelectionOption("Current sheet")]
        [PropertyUISelectionOption("Number of sheets")]
        [PropertyUISelectionOption("First sheet")]
        [PropertyUISelectionOption("Last sheet")]
        [PropertyValidationRule("Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_InfoType { get; set; }
        
        [PropertyDescription("Please select the variable to receive a sheet name")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_applyToVariable { get; set; }

        public ExcelGetExcelInfoCommand()
        {
            this.CommandName = "ExcelGetExcelInfoCommand";
            this.SelectionName = "Get Excel Info";
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
            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            //var infoType = v_InfoType.ConvertToUserVariable(sender);
            var infoType = v_InfoType.GetUISelectionValue("v_InfoType", this, engine);
            string ret = "";
            switch (infoType)
            {
                case "file name":
                    ret = excelInstance.ActiveWorkbook.Name;
                    break;
                case "full path file name":
                    ret = excelInstance.ActiveWorkbook.FullName;
                    break;
                case "current sheet":
                    var sheet = engine.engineSettings.CurrentWorksheetKeyword.GetExcelWorksheet(engine, excelInstance, true);
                    ret = (sheet == null) ? "" : sheet.Name;
                    break;
                case "number of sheets":
                    ret = excelInstance.Worksheets.Count.ToString();
                    break;
                case "first sheet":
                    //ret = ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[1]).Name;
                    if (excelInstance.Worksheets.Count > 0)
                    {
                        ret = ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[1]).Name;
                    }
                    else
                    {
                        ret = "";
                    }
                    break;
                case "last sheet":
                    //ret = ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[excelInstance.Worksheets.Count]).Name;
                    if (excelInstance.Worksheets.Count > 0)
                    {
                        ret = ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[excelInstance.Worksheets.Count]).Name;
                    }
                    else
                    {
                        ret = "";
                    }
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
        //    return base.GetDisplayValue() + " [Get Info: '" + v_InfoType + "', Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
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
    }
}
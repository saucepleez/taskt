using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.Description("This command allows you to get a sheet info.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelGetWorksheetInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the sheet name")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**mySheet** or **%kwd_current_worksheet%** or **{{{vSheet}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the information type")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Visible")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive a sheet info")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
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
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;

            var sheetName = v_SheetName.ConvertToUserVariable(sender);
            Microsoft.Office.Interop.Excel.Worksheet targetSheet;
            if (sheetName == engine.engineSettings.CurrentWorksheetKeyword)
            {
                targetSheet = excelInstance.ActiveSheet;
            }
            else
            {
                targetSheet = excelInstance.Worksheets[sheetName];
            }

            string ret;
            var infoType = v_InfoType.ConvertToUserVariable(sender);
            switch (infoType)
            {
                case "Name":
                    ret = targetSheet.Name;
                    break;
                case "Visible":
                    ret = (targetSheet.Visible == Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetVisible) ? "TRUE" : "FALSE";
                    break;
                default:
                    throw new Exception("Information type " + infoType + " is not support.");
                    break;
            }

            ret.StoreInUserVariable(sender, v_applyToVariable);
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
            return base.GetDisplayValue() + " [Get " + v_InfoType + " Sheet '" + v_SheetName + "', Instance Name: '" + v_InstanceName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SheetName))
            {
                this.validationResult += "Sheet is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_InfoType))
            {
                this.validationResult += "Information type is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_applyToVariable))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
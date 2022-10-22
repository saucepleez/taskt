using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.Description("This command allows you to close Excel.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an open instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCloseApplicationCommand : ScriptCommand
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
        [PropertyDescription("Please Indicate if the Workbook should be saved")]
        [PropertyUISelectionOption("True")]
        [PropertyUISelectionOption("False")]
        [InputSpecification("Enter a True or False value")]
        [SampleUsage("**True** or **False**")]
        [Remarks("")]
        [PropertyIsOptional(true, "False")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_ExcelSaveOnExit { get; set; }

        public ExcelCloseApplicationCommand()
        {
            this.CommandName = "ExcelCloseApplicationCommand";
            this.SelectionName = "Close Excel Application";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            //check if workbook exists and save
            if (excelInstance.ActiveWorkbook != null)
            {
                string vSaved = new PropertyConvertTag(v_ExcelSaveOnExit, nameof(v_ExcelSaveOnExit), "Save Setting").GetUISelectionValue(this, engine);

                excelInstance.ActiveWorkbook.Close((vSaved == "true"));
            }

            //close excel
            excelInstance.Quit();

            //remove instance
            engine.RemoveAppInstance(vInstance);
        }
    }
}
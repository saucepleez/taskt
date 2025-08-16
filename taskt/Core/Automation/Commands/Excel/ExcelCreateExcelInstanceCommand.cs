using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Create Excel Instance")]
    [Attributes.ClassAttributes.Description("This command opens the Excel Instance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelCreateExcelInstanceCommand : AExcelInstanceCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        public override string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        [PropertyParameterOrder(6000)]
        public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Reference Style")]
        [PropertyUISelectionOption("A1")]
        [PropertyUISelectionOption("R1C1")]
        [PropertyIsOptional(true, "A1")]
        [PropertyValidationRule("Reference Style", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Reference Style")]
        [Remarks("Strongly recommend specifying **A1**")]
        [PropertyParameterOrder(7000)]
        public string v_ReferenceStyle { get; set; }

        public ExcelCreateExcelInstanceCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var vInstance = v_InstanceName.ExpandValueOrUserVariable(engine);

            var newExcelSession = new Microsoft.Office.Interop.Excel.Application
            {
                Visible = true
            };

            switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ReferenceStyle), engine))
            {
                case "a1":
                    newExcelSession.ReferenceStyle = Microsoft.Office.Interop.Excel.XlReferenceStyle.xlA1;
                    break;
                case "r1c1":
                    newExcelSession.ReferenceStyle = Microsoft.Office.Interop.Excel.XlReferenceStyle.xlR1C1;
                    break;
            }

            engine.AddAppInstance(vInstance, newExcelSession);

            if (!string.IsNullOrEmpty(v_WindowHandle))
            {
                newExcelSession.Hwnd.StoreInUserVariable(engine, v_WindowHandle);
            }
        }
    }
}
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.Description("This command opens the Excel Application.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCreateApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name (ex. myInstance, {{{vInstance}}})")]
        [InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        [SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public string v_InstanceName { get; set; }

        public ExcelCreateApplicationCommand()
        {
            this.CommandName = "ExcelOpenApplicationCommand";
            this.SelectionName = "Create Excel Application";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var newExcelSession = new Microsoft.Office.Interop.Excel.Application
            {
                Visible = true
            };

            engine.AddAppInstance(vInstance, newExcelSession);
        }

    //    public override List<Control> Render(frmCommandEditor editor)
    //    {
    //        base.Render(editor);

    //        //create standard group controls
    //        var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
    //        RenderedControls.AddRange(ctrls);
    //        //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

    //        if (editor.creationMode == frmCommandEditor.CreationMode.Add)
    //        {
    //            this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
    //        }

    //        return RenderedControls;
    //    }

    //    public override string GetDisplayValue()
    //    {
    //        return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
    //    }

    //    public override bool IsValidate(frmCommandEditor editor)
    //    {
    //        base.IsValidate(editor);

    //        if (String.IsNullOrEmpty(this.v_InstanceName))
    //        {
    //            this.validationResult += "Instance is empty.\n";
    //            this.IsValid = false;
    //        }

    //        return this.IsValid;
    //    }
    }
}
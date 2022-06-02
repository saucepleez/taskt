using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to Save EMail.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Save EMail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class MailKitSaveEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify EMail Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vEMail}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("EMail", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMail, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Path to the File")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("")]
        [SampleUsage("**vPath** or **{{{vPath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_SavePath { get; set; }

        public MailKitSaveEmailCommand()
        {
            this.CommandName = "MailKitSaveEmailCommand";
            this.SelectionName = "Save Email";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var mail = v_MailName.GetMailKitMailVariable(engine);

            var path = v_SavePath.ConvertToUserVariable(engine);

            mail.WriteTo(path);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [EMail: '" + v_MailName + "', Path: '" + v_SavePath + "']";
        }
    }
}

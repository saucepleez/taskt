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
    [Attributes.ClassAttributes.Description("This command allows you to get Attachment File Name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Attachment File Name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetEmailAttachmentsNameCommand : ScriptCommand
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
        [PropertyDisplayText(true, "EMail")]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("")]
        [SampleUsage("**vNames** or **{{{vNames}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_AttachmentsList { get; set; }

        public MailKitGetEmailAttachmentsNameCommand()
        {
            this.CommandName = "MailKitGetEmailAttachmentsNameCommand";
            this.SelectionName = "Get Email Attachments Name";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var mail = v_MailName.GetMailKitMailVariable(engine);

            List<string> attachments = new List<string>();

            foreach(var at in mail.Attachments)
            {
                if (at is MimeKit.MimePart)
                {
                    attachments.Add(((MimeKit.MimePart)at).FileName);
                }
            }

            attachments.StoreInUserVariable(engine, v_AttachmentsList);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [EMail: '" + v_MailName + "', Store: '" + v_AttachmentsList + "']";
        //}
    }
}

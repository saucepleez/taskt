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
    [Attributes.ClassAttributes.Group("Mail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to get Text from Mail.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Text from Mail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class MailKitGetMailTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify Mail Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vMail}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Mail", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Address Type")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Message Body")]
        [PropertyUISelectionOption("Text Message Body")]
        [PropertyUISelectionOption("HTML Message Body")]
        [PropertyUISelectionOption("Message-ID")]
        [PropertyUISelectionOption("Date")]
        [PropertyUISelectionOption("Resent-Message-ID")]
        [PropertyUISelectionOption("Resent-Date")]
        public string v_TextType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store Text")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vText** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Text Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ResultVariable { get; set; }

        public MailKitGetMailTextCommand()
        {
            this.CommandName = "MailKitGetMailTextCommand";
            this.SelectionName = "Get Mail Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var mail = v_MailName.GetMailKitMailVariable(engine);

            var textType = v_TextType.GetUISelectionValue("v_TextType", this, engine);

            string res = "";
            switch (textType)
            {
                case "message body":
                    if (mail.TextBody != null)
                    {
                        res = mail.TextBody;
                    }
                    else if (mail.HtmlBody != null)
                    {
                        res = mail.HtmlBody;
                    }
                    break;
                case "text message body":
                    if (mail.TextBody != null)
                    {
                        res = mail.TextBody;
                    }
                    break;
                case "html message body":
                    if (mail.HtmlBody != null)
                    {
                        res = mail.HtmlBody;
                    }
                    break;
                case "message-id":
                    res = mail.MessageId;
                    break;
                case "resent-message-id":
                    res = mail.ResentMessageId;
                    break;

                // Date
                case "date":
                    //var d = mail.Date.DateTime;
                    (mail.Date.DateTime).StoreInUserVariable(engine, v_ResultVariable);
                    return;
                    break;
                case "resent-date":
                    (mail.ResentDate.DateTime).StoreInUserVariable(engine, v_ResultVariable);
                    return;
                    break;
            }

            res.StoreInUserVariable(engine, v_ResultVariable);
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
            return base.GetDisplayValue() + "";
        }
    }
}

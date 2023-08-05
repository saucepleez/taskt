using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Get EMail Text")]
    [Attributes.ClassAttributes.Description("This command allows you to get Text from EMail.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Text from EMail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetEMailTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_InputEMailName))]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Text Type")]
        [InputSpecification("", true)]
        [PropertyUISelectionOption("Message Body")]
        [PropertyUISelectionOption("Text Message Body")]
        [PropertyUISelectionOption("HTML Message Body")]
        [PropertyUISelectionOption("Subject")]
        [PropertyUISelectionOption("Message-ID")]
        [PropertyUISelectionOption("Date")]
        [PropertyUISelectionOption("Resent-Message-ID")]
        [PropertyUISelectionOption("Resent-Date")]
        [PropertyValidationRule("Text Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_TextType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_ResultVariable { get; set; }

        public MailKitGetEMailTextCommand()
        {
            this.CommandName = "MailKitGetEMailTextCommand";
            this.SelectionName = "Get EMail Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var mail = v_MailName.GetMailKitEMailVariable(engine);

            var textType = this.GetUISelectionValue(nameof(v_TextType), engine);

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

                case "subject":
                    res = mail.Subject;
                    break;

                // Date
                case "date":
                    (mail.Date.DateTime).StoreInUserVariable(engine, v_ResultVariable);
                    return;

                case "resent-date":
                    (mail.ResentDate.DateTime).StoreInUserVariable(engine, v_ResultVariable);
                    return;
            }

            res.StoreInUserVariable(engine, v_ResultVariable);
        }

        public override void AddInstance(InstanceCounter counter)
        {
            var mail = (string.IsNullOrEmpty(v_MailName)) ? "" : v_MailName;

            counter.addInstance(mail, new PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMail, true), true);

            var tp = (string.IsNullOrEmpty(v_TextType)) ? "" : v_TextType.ToLower();
            switch (tp)
            {
                case "date":
                case "resent-date":
                    var ins = new PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true);
                    var name = (string.IsNullOrEmpty(v_ResultVariable)) ? "" : v_ResultVariable;

                    counter.addInstance(name, ins, false);
                    counter.addInstance(name, ins, true);

                    break;
            }
        }

        public override void RemoveInstance(InstanceCounter counter)
        {
            var mail = (string.IsNullOrEmpty(v_MailName)) ? "" : v_MailName;

            counter.removeInstance(mail, new PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMail, true), true);

            var tp = (string.IsNullOrEmpty(v_TextType)) ? "" : v_TextType.ToLower();
            switch (tp)
            {
                case "date":
                case "resent-date":
                    var ins = new PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true);
                    var name = (string.IsNullOrEmpty(v_ResultVariable)) ? "" : v_ResultVariable;

                    counter.removeInstance(name, ins, false);
                    counter.removeInstance(name, ins, true);

                    break;
            }
        }
    }
}

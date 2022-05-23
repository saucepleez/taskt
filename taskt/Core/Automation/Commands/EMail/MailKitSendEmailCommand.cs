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
    [Attributes.ClassAttributes.Description("This command allows you to send EMail using SMTP protocol.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send an EMail and have access to SMTP server credentials to generate an EMail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class MailKitSendEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify SMTP Host Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the host/service name that the script should use")]
        [SampleUsage("**smtp.mymail.com** or **{{{vHost}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("SMTP Host", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPHost { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify SMTP Port")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the port number that should be used when contacting the SMTP service")]
        [SampleUsage("**25** or **587** or **{{{vPort}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("SMTP Port", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPPort { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify SMTP Username")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the username to use when contacting the SMTP service")]
        [SampleUsage("**username** or **{{{vUserName}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "From Email")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPUserName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify SMTP Password")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the password to use when contacting the SMTP service")]
        [SampleUsage("**password** or **{{{vPassword}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Password", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPPassword { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify From Email Address")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify how the 'From' field should appear.")]
        [SampleUsage("**my-robot@company.com** or **{{{vMail}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("From Email", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPFromEmail { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify To Email Address")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify the destination email that should be addressed.")]
        [SampleUsage("**john@company.com** or **{{{vMail}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("To Email", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPToEmail { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify CC Email Address")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify the destination email that should be addressed.")]
        [SampleUsage("**tom@company.com** or **{{{vMail}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPCCEmail { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify BCC Email Address")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify the destination email that should be addressed.")]
        [SampleUsage("**bob@company.com** or **{{{vMail}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPBCCEmail { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Email Subject")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the text subject (or variable) that the email should have.")]
        [SampleUsage("**Alert!** or **{{{vTitle}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "")]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPSubject { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Email Message")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify the message that should be sent.")]
        [SampleUsage("**Everything ran ok at {{{DateTime.Now}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        public string v_SMTPBody { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Email Attachment File Path")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Indicates the file path to attachment.")]
        [SampleUsage("**c:\\temp\\file.txt** or **{{{vPath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "")]
        [PropertyTextBoxSetting(1, false)]
        public string v_SMTPAttachment { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Secure Option")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyFirstValue("Auto")]
        [PropertyIsOptional(true, "Auto")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Auto")]
        [PropertyUISelectionOption("No SSL or TLS")]
        [PropertyUISelectionOption("Use SSL or TLS")]
        [PropertyUISelectionOption("STARTTLS")]
        [PropertyUISelectionOption("STARTTLS When Available")]
        public string v_SMTPSecureOption { get; set; }

        public MailKitSendEmailCommand()
        {
            this.CommandName = "MailKitSendEmailCommand";
            this.SelectionName = "Send Email";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            // from, to, cc, bcc, subject, body
            string from = v_SMTPFromEmail.ConvertToUserVariable(engine);
            string to = v_SMTPToEmail.ConvertToUserVariable(engine);
            string cc = v_SMTPCCEmail.ConvertToUserVariable(engine);
            string bcc = v_SMTPBCCEmail.ConvertToUserVariable(engine);
            string subject = v_SMTPSubject.ConvertToUserVariable(engine);
            string body = v_SMTPBody.ConvertToUserVariable(engine);

            // smtp host
            string smtp = v_SMTPHost.ConvertToUserVariable(engine);
            int port = v_SMTPPort.ConvertToUserVariableAsInteger("SMTP Port", engine);
            // auth
            string user = v_SMTPUserName.ConvertToUserVariable(engine);
            if (String.IsNullOrEmpty(user))
            {
                user = from;
            }
            string pass = v_SMTPPassword.ConvertToUserVariable(engine);
            string secureOption = v_SMTPSecureOption.GetUISelectionValue("v_SMTPSecureOption", this, engine);

            // attachment
            string attachmentFilePath = v_SMTPAttachment.ConvertToUserVariable(engine);

            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress(from, from));

            var toArray = to.Split(';');
            foreach(string address in toArray)
            {
                message.To.Add(new MimeKit.MailboxAddress(address, address));
            }
            
            if (!String.IsNullOrEmpty(cc))
            {
                var ccArray = cc.Split(';');
                foreach(string address in ccArray)
                {
                    message.Cc.Add(new MimeKit.MailboxAddress(address, address));
                }
            }

            if (!String.IsNullOrEmpty(bcc))
            {
                var bccArray = bcc.Split(';');
                foreach (string address in bccArray)
                {
                    message.Bcc.Add(new MimeKit.MailboxAddress(address, address));
                }
            }

            message.Subject = subject;
            
            // has attachment file?
            if (!string.IsNullOrEmpty(attachmentFilePath))
            {
                if (!System.IO.File.Exists(pass))
                {
                    throw new Exception("Attachment File '" + v_SMTPAttachment + "' does not Exists");
                }

                var mimeType = MimeKit.MimeTypes.GetMimeType(attachmentFilePath);
                var attachment = new MimeKit.MimePart(mimeType)
                {
                    Content = new MimeKit.MimeContent(System.IO.File.OpenRead(attachmentFilePath)),
                    ContentDisposition = new MimeKit.ContentDisposition(),
                    ContentTransferEncoding = MimeKit.ContentEncoding.Base64,
                    FileName = System.IO.Path.GetFileName(attachmentFilePath)
                };
                var multipart = new MimeKit.Multipart("mixed");

                var textPart = new MimeKit.TextPart(MimeKit.Text.TextFormat.Plain);
                textPart.Text = body;
                multipart.Add(textPart);
                multipart.Add(attachment);
                message.Body = multipart;
            }
            else
            {
                message.Body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = body
                };
            }

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                var option = MailKit.Security.SecureSocketOptions.Auto;
                switch (secureOption)
                {
                    case "no ssl or tls":
                        option = MailKit.Security.SecureSocketOptions.None;
                        break;
                    case "use ssl or tls":
                        option = MailKit.Security.SecureSocketOptions.SslOnConnect;
                        break;
                    case "starttls":
                        option = MailKit.Security.SecureSocketOptions.StartTls;
                        break;
                    case "starttls when available":
                        option = MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable;
                        break;
                }
                try
                {
                    client.Connect(smtp, port, option);
                    client.Authenticate(user, pass);
                    client.Send(message);
                    client.Disconnect(true);
                }
                catch(Exception ex)
                {
                    throw new Exception("Fail Send EMail " + ex.ToString());
                }
            }
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
            return base.GetDisplayValue() + " [From: '" + v_SMTPFromEmail + "', To Address: '" + v_SMTPToEmail + "', Subject: '" + v_SMTPSubject + "']";
        }
    }
}

﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Send EMail")]
    [Attributes.ClassAttributes.Description("This command allows you to send EMail using SMTP protocol.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send an EMail and have access to SMTP server credentials to generate an EMail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitSendEMailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_Host))]
        [PropertyDescription("SMTP Host Name")]
        [PropertyDetailSampleUsage("**smtp.example.com**", PropertyDetailSampleUsage.ValueType.Value, "Host")]
        public string v_SMTPHost { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_Port))]
        [PropertyDescription("SMTP Port")]
        [PropertyDetailSampleUsage("**25**", PropertyDetailSampleUsage.ValueType.Value, "Port")]
        [PropertyDetailSampleUsage("**587**", PropertyDetailSampleUsage.ValueType.Value, "Port")]
        [PropertyDetailSampleUsage("**{{{vPort}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Port")]
        public string v_SMTPPort { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_UserName))]
        [PropertyDescription("SMTP User Name")]
        public string v_SMTPUserName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_Password))]
        [PropertyDescription("SMTP Password")]
        public string v_SMTPPassword { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_EmailAddress))]
        [PropertyDescription("From EMail Address")]
        [PropertyValidationRule("From Email", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "From")]
        public string v_SMTPFromEmail { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_EmailAddress))]
        [PropertyDescription("To EMail Address")]
        [PropertyValidationRule("To Email", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "To")]
        public string v_SMTPToEmail { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_EmailAddress))]
        [PropertyDescription("CC EMail Address")]
        [PropertyValidationRule("CC", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyIsOptional(true, "")]
        [PropertyDisplayText(true, "CC")]
        public string v_SMTPCCEmail { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_EmailAddress))]
        [PropertyDescription("BCC EMail Address")]
        [PropertyValidationRule("BCC", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyIsOptional(true, "")]
        [PropertyDisplayText(true, "BCC")]
        public string v_SMTPBCCEmail { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Email Subject")]
        [InputSpecification("EMail Subject", true)]
        [PropertyDetailSampleUsage("**Alert!**", PropertyDetailSampleUsage.ValueType.Value, "Subject")]
        [PropertyDetailSampleUsage("**{{{vSubject}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Subject")]
        [Remarks("")]
        [PropertyIsOptional(true, "")]
        [PropertyDisplayText(true, "Subject")]
        public string v_SMTPSubject { get; set; }

        [XmlAttribute]
        [PropertyDescription("Email Message")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Message", true)]
        [PropertyDetailSampleUsage("**Everything ran ok at {{{DateTime.Now}}}**", "Send result message and current Date and Time")]
        [PropertyDetailSampleUsage("**{{{vMessage}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Message")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        public string v_SMTPBody { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Email Attachment File Path")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("File Path", true)]
        //[SampleUsage("**c:\\temp\\file.txt** or **{{{vPath}}}**")]
        [PropertyDetailSampleUsage("**C:\temp\\myfile.txt**", PropertyDetailSampleUsage.ValueType.Value, "File Path")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Path")]
        [PropertyIsOptional(true, "")]
        public string v_SMTPAttachment { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_SecureOption))]
        public string v_SMTPSecureOption { get; set; }

        public MailKitSendEMailCommand()
        {
            //this.CommandName = "MailKitSendEmailCommand";
            //this.SelectionName = "Send Email";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            // from, to, cc, bcc, subject, body
            string from = v_SMTPFromEmail.ExpandValueOrUserVariable(engine);
            string to = v_SMTPToEmail.ExpandValueOrUserVariable(engine);
            string cc = v_SMTPCCEmail.ExpandValueOrUserVariable(engine);
            string bcc = v_SMTPBCCEmail.ExpandValueOrUserVariable(engine);
            string subject = v_SMTPSubject.ExpandValueOrUserVariable(engine);
            string body = v_SMTPBody.ExpandValueOrUserVariable(engine);

            // smtp host
            string smtp = v_SMTPHost.ExpandValueOrUserVariable(engine);
            var port = this.ExpandValueOrUserVariableAsInteger(nameof(v_SMTPPort), engine);

            // auth
            string user = v_SMTPUserName.ExpandValueOrUserVariable(engine);
            if (String.IsNullOrEmpty(user))
            {
                user = from;
            }
            string pass = v_SMTPPassword.ExpandValueOrUserVariable(engine);

            // attachment
            string attachmentFilePath = v_SMTPAttachment.ExpandValueOrUserVariable(engine);

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
                var option = this.GetMailKitSecureOption(nameof(v_SMTPSecureOption), engine);
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
    }
}

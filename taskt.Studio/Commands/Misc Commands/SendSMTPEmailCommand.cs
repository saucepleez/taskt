using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command allows you to send email using SMTP protocol.")]
    [UsesDescription("Use this command when you want to send an email and have access to SMTP server credentials to generate an email.")]
    [ImplementationDescription("This command implements the System.Net Namespace to achieve automation")]
    public class SendSMTPEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Host Name")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the host/service name that the script should use")]
        [SampleUsage("**smtp.gmail.com**")]
        [Remarks("")]
        public string v_SMTPHost { get; set; }
        [XmlAttribute]
        [PropertyDescription("Port")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the port number that should be used when contacting the SMTP service")]
        [SampleUsage("**587**")]
        [Remarks("")]
        public int v_SMTPPort { get; set; }
        [XmlAttribute]
        [PropertyDescription("Username")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the username to use when contacting the SMTP service")]
        [SampleUsage("**username**")]
        [Remarks("")]
        public string v_SMTPUserName { get; set; }
        [XmlAttribute]
        [PropertyDescription("Password")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the password to use when contacting the SMTP service")]
        [SampleUsage("**password**")]
        [Remarks("")]
        public string v_SMTPPassword { get; set; }
        [XmlAttribute]
        [PropertyDescription("From Email")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify how the 'From' field should appear.")]
        [SampleUsage("myRobot@company.com")]
        [Remarks("")]
        public string v_SMTPFromEmail { get; set; }
        [XmlAttribute]
        [PropertyDescription("To Email")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify the destination email that should be addressed.")]
        [SampleUsage("jason@company.com")]
        [Remarks("")]
        public string v_SMTPToEmail { get; set; }
        [XmlAttribute]
        [PropertyDescription("Subject")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the text subject (or variable) that the email should have.")]
        [SampleUsage("**Alert!** or **[vStatus]**")]
        [Remarks("")]
        public string v_SMTPSubject { get; set; }
        [XmlAttribute]
        [PropertyDescription("Body")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify the message that should be sent.")]
        [SampleUsage("**Everything ran ok at [DateTime.Now]**")]
        [Remarks("")]
        public string v_SMTPBody { get; set; }

        [XmlAttribute]
        [PropertyDescription("Attachment Path (Optional)")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Indicates the file path to attachment.")]
        [SampleUsage("**c:\\temp\\file.txt**")]
        [Remarks("")]
        public string v_SMTPAttachment { get; set; }

        [XmlElement]
        [PropertyDescription("SSL Validation")]
        [PropertyUISelectionOption("Validate SSL")]
        [PropertyUISelectionOption("Bypass SSL Validation")]
     
        [InputSpecification("Select the appropriate option")]
        [SampleUsage("Select from **Validate SSL**, **Bypass SSL Validation**")]
        [Remarks("This field manages whether taskt will attempt to validate the SSL connection")]
        public string v_SSLValidation { get; set; }
        public SendSMTPEmailCommand()
        {
            CommandName = "SendSMTPEmailCommand";
            SelectionName = "Send SMTP Email";
            CommandEnabled = true;
            CustomRendering = true;
            v_SSLValidation = "Validate SSL";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //bypass ssl validation if requested
            if (v_SSLValidation.ConvertToUserVariable(engine) == "Bypass SSL Validation")
            {
                ServicePointManager.ServerCertificateValidationCallback =
                                    (sndr, certificate, chain, sslPolicyErrors) => true;
            }

            try
            {
                string varSMTPHost = v_SMTPHost.ConvertToUserVariable(engine);
                string varSMTPPort = v_SMTPPort.ToString().ConvertToUserVariable(engine);
                string varSMTPUserName = v_SMTPUserName.ConvertToUserVariable(engine);
                string varSMTPPassword = v_SMTPPassword.ConvertToUserVariable(engine);

                string varSMTPFromEmail = v_SMTPFromEmail.ConvertToUserVariable(engine);
                string varSMTPToEmail = v_SMTPToEmail.ConvertToUserVariable(engine);
                string varSMTPSubject = v_SMTPSubject.ConvertToUserVariable(engine);
                string varSMTPBody = v_SMTPBody.ConvertToUserVariable(engine);
                string varSMTPFilePath = v_SMTPAttachment.ConvertToUserVariable(engine);

                var client = new SmtpClient(varSMTPHost, int.Parse(varSMTPPort))
                {
                    Credentials = new System.Net.NetworkCredential(varSMTPUserName, varSMTPPassword),
                    EnableSsl = true
                };


                var message = new MailMessage(varSMTPFromEmail, varSMTPToEmail);
                message.Subject = varSMTPSubject;
                message.Body = varSMTPBody;

                if (!string.IsNullOrEmpty(varSMTPFilePath))
                {
                    message.Attachments.Add(new Attachment(varSMTPFilePath));
                }

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //restore default validation
                if (v_SSLValidation.ConvertToUserVariable(engine) == "Bypass SSL Validation")
                {
                    ServicePointManager.ServerCertificateValidationCallback = null;
                }
            }
            
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SMTPHost", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SMTPPort", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SMTPUserName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SMTPPassword", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SMTPFromEmail", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SMTPToEmail", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SMTPSubject", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SMTPBody", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SMTPAttachment", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SSLValidation", this, editor));
            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [To Address: '" + v_SMTPToEmail + "']";
        }
    }
}

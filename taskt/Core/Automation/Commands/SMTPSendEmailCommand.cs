using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to send email using SMTP protocol.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send an email and have access to SMTP server credentials to generate an email.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the System.Net Namespace to achieve automation")]
    public class SMTPSendEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Host Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the host/service name that the script should use")]
        [Attributes.PropertyAttributes.SampleUsage("**smtp.gmail.com**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPHost { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Port")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the port number that should be used when contacting the SMTP service")]
        [Attributes.PropertyAttributes.SampleUsage("**587**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public int v_SMTPPort { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Username")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the username to use when contacting the SMTP service")]
        [Attributes.PropertyAttributes.SampleUsage("**username**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPUserName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Password")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the password to use when contacting the SMTP service")]
        [Attributes.PropertyAttributes.SampleUsage("**password**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPPassword { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("From Email")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify how the 'From' field should appear.")]
        [Attributes.PropertyAttributes.SampleUsage("myRobot@company.com")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPFromEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("To Email")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify the destination email that should be addressed.")]
        [Attributes.PropertyAttributes.SampleUsage("jason@company.com")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPToEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Subject")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the text subject (or variable) that the email should have.")]
        [Attributes.PropertyAttributes.SampleUsage("**Alert!** or **[vStatus]**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPSubject { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Body")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify the message that should be sent.")]
        [Attributes.PropertyAttributes.SampleUsage("**Everything ran ok at [DateTime.Now]**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPBody { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Attachment Path (Optional)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Indicates the file path to attachment.")]
        [Attributes.PropertyAttributes.SampleUsage("**c:\\temp\\file.txt**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPAttachment { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("SSL Validation")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Validate SSL")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Bypass SSL Validation")]
     
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate option")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Validate SSL**, **Bypass SSL Validation**")]
        [Attributes.PropertyAttributes.Remarks("This field manages whether taskt will attempt to validate the SSL connection")]
        public string v_SSLValidation { get; set; }
        public SMTPSendEmailCommand()
        {
            this.CommandName = "SMTPCommand";
            this.SelectionName = "Send SMTP Email";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_SSLValidation = "Validate SSL";
        }

        public override void RunCommand(object sender)
        {
            //bypass ssl validation if requested
            if (v_SSLValidation.ConvertToUserVariable(sender) == "Bypass SSL Validation")
            {
                ServicePointManager.ServerCertificateValidationCallback =
                                    (sndr, certificate, chain, sslPolicyErrors) => true;
            }

            try
            {
                string varSMTPHost = v_SMTPHost.ConvertToUserVariable(sender);
                string varSMTPPort = v_SMTPPort.ToString().ConvertToUserVariable(sender);
                string varSMTPUserName = v_SMTPUserName.ConvertToUserVariable(sender);
                string varSMTPPassword = v_SMTPPassword.ConvertToUserVariable(sender);

                string varSMTPFromEmail = v_SMTPFromEmail.ConvertToUserVariable(sender);
                string varSMTPToEmail = v_SMTPToEmail.ConvertToUserVariable(sender);
                string varSMTPSubject = v_SMTPSubject.ConvertToUserVariable(sender);
                string varSMTPBody = v_SMTPBody.ConvertToUserVariable(sender);
                string varSMTPFilePath = v_SMTPAttachment.ConvertToUserVariable(sender);

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
                if (v_SSLValidation.ConvertToUserVariable(sender) == "Bypass SSL Validation")
                {
                    ServicePointManager.ServerCertificateValidationCallback = null;
                }
            }
            
        }
        public override List<Control> Render(frmCommandEditor editor)
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

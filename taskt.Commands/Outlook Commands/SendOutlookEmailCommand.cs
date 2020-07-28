using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
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
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace taskt.Commands
{
    [Serializable]
    [Group("Outlook Commands")]
    [Description("This command sends emails with attachments in Outlook.")]

    public class SendOutlookEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Recipient(s)")]
        [InputSpecification("Enter the email address(es) of the recipient(s).")]
        [SampleUsage("test@test.com || test@test.com;test2@test.com || {vEmail} || {vEmail1};{vEmail2} || {vEmails}")]
        [Remarks("Multiple recipient email addresses should be delimited by a semicolon (;).")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Recipients { get; set; }

        [XmlAttribute]
        [PropertyDescription("Email Subject")]
        [InputSpecification("Enter the subject of the email.")]
        [SampleUsage("Hello || {vSubject}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Subject { get; set; }

        [XmlAttribute]
        [PropertyDescription("Email Body")]
        [InputSpecification("Enter text to be used as the email body.")]
        [SampleUsage("Dear John, ... || {vBody}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Body { get; set; }

        [XmlAttribute]
        [PropertyDescription("Email Body Type")]
        [PropertyUISelectionOption("Plain")]
        [PropertyUISelectionOption("HTML")]
        [InputSpecification("Select the email body format.")]
        [Remarks("")]
        public string v_BodyType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Attachment File Path(s)")]
        [InputSpecification("Enter the file path(s) of the file(s) to attach.")]
        [SampleUsage(@"C:\temp\myFile.xlsx || {vFile} || C:\temp\myFile1.xlsx;C:\temp\myFile2.xlsx || {vFile1};{vFile2} || {vFiles}")]
        [Remarks("This input is optional. Multiple attachments should be delimited by a semicolon (;).")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_Attachments { get; set; }

        public SendOutlookEmailCommand()
        {
            CommandName = "SendOutlookEmailCommand";
            SelectionName = "Send Outlook Email";
            CommandEnabled = true;
            CustomRendering = true;
            v_BodyType = "Plain";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vRecipients = v_Recipients.ConvertToUserVariable(engine);
            var vAttachment = v_Attachments.ConvertToUserVariable(engine);
            var vSubject = v_Subject.ConvertToUserVariable(engine);
            var vBody = v_Body.ConvertToUserVariable(engine);
            var splitRecipients = vRecipients.Split(';');

            Application outlookApp = new Application();
            MailItem mail = (MailItem)outlookApp.CreateItem(OlItemType.olMailItem);
            AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
            if (currentUser.Type == "EX")
            {
                ExchangeUser manager = currentUser.GetExchangeUser().GetExchangeUserManager();
                // Add recipient using display name, alias, or smtp address
                foreach (var t in splitRecipients)
                    mail.Recipients.Add(t.ToString());

                mail.Recipients.ResolveAll();

                mail.Subject = vSubject;

                if (v_BodyType == "HTML")
                    mail.HTMLBody = vBody;
                else
                    mail.Body = vBody;
 
                if (!string.IsNullOrEmpty(vAttachment))
                {
                    var splitAttachments = vAttachment.Split(';');
                    foreach (var attachment in splitAttachments)
                        mail.Attachments.Add(attachment);
                }
                mail.Send();
            }
        }
      
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Recipients", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Subject", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Body", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_BodyType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Attachments", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [To '{v_Recipients}' - Subject '{v_Subject}']";
        }
    }
}
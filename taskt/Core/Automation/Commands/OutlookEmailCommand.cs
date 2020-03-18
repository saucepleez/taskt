using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Outlook Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to send emails with outlook")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send emails with your currenty logged in outlook account")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class OutlookEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Indicate Recipients (; delimited)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the Email Addresses of the recipients in semicolon seperated values")]
        [Attributes.PropertyAttributes.SampleUsage("test@test.com;test2@test.com")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Recipients { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Attachment File Path (Optional)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the Filepath of the file you want attached.")]
        [Attributes.PropertyAttributes.SampleUsage("c:sales reports\fy06q4.xlsx")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_Attachment { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Provide Email Subject")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the subject you want sent in your Email")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Subject { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Provide Email Body")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the body you want on your email to be sent.")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Body { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Email Body Type")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Plain")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("HTML")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_BodyType { get; set; }

        public OutlookEmailCommand()
        {
            this.CommandName = "OutlookEmailCommand";
            this.SelectionName = "Send Outlook Email";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_BodyType = "Plain";
        }

        public override void RunCommand(object sender)
        {

            var vRecipients = v_Recipients.ConvertToUserVariable(sender);
            var vAttachment = v_Attachment.ConvertToUserVariable(sender);
            var vSubject = v_Subject.ConvertToUserVariable(sender);
            var vBody = v_Body.ConvertToUserVariable(sender);
            var vBodyType = v_BodyType.ConvertToUserVariable(sender);

            var splittext = vRecipients.Split(';');

            Microsoft.Office.Interop.Outlook.Application outlookApp = new Microsoft.Office.Interop.Outlook.Application();

            Microsoft.Office.Interop.Outlook.MailItem mail = (Microsoft.Office.Interop.Outlook.MailItem)outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            Microsoft.Office.Interop.Outlook.AddressEntry currentUser =
                outlookApp.Session.CurrentUser.AddressEntry;
            if (currentUser.Type == "EX")
            {
                Microsoft.Office.Interop.Outlook.ExchangeUser manager =
                    currentUser.GetExchangeUser().GetExchangeUserManager();
                // Add recipient using display name, alias, or smtp address
                foreach(var t in splittext)
                    mail.Recipients.Add(t.ToString());

                mail.Recipients.ResolveAll();

                mail.Subject = vSubject;

                if (vBodyType == "HTML")
                {
                    mail.HTMLBody = vBody;
                }
                else
                {
                    mail.Body = vBody;
                }
 
                if (!string.IsNullOrEmpty(vAttachment))
                   mail.Attachments.Add(vAttachment);

                mail.Send();
                
            }
        }
      
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Recipients", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Subject", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Body", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_BodyType", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Attachment", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [To: {v_Recipients}]";
        }
    }
}
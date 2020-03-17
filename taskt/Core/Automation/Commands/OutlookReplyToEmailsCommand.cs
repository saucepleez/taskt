using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using Microsoft.Office.Interop.Outlook;
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Outlook Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to reply to emails with outlook")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reply to emails with your currenty logged in outlook account")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class OutlookReplyToEmailsCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate whether to Reply or Reply All")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Reply")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Reply All")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether you intend to reply or reply all. Replying will reply to only the original sender. Reply all will reply to everyone.")]
        [Attributes.PropertyAttributes.SampleUsage("Select either **Reply** or **Reply All**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Provide the source mail folder name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the mail folder you want your emails to come from")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SourceFolder { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Provide a filter (Optional)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter an outlook filter string")]
        [Attributes.PropertyAttributes.SampleUsage("[Subject] = 'Hello' and [SenderName] = 'Jane Doe'")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Filter { get; set; }

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

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Attachment File Path (Optional)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the Filepath of the file you want attached.")]
        [Attributes.PropertyAttributes.SampleUsage("c:sales reports\fy06q4.xlsx")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_Attachment { get; set; }
        public OutlookReplyToEmailsCommand()
        {
            this.CommandName = "OutlookReplyToEmailsCommand";
            this.SelectionName = "Reply To Outlook Emails";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_BodyType = "Plain";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var vSourceFolder = v_SourceFolder.ConvertToUserVariable(sender);
            var vFilter = v_Filter.ConvertToUserVariable(sender);
            var vBody = v_Body.ConvertToUserVariable(sender);
            var vAttachment = v_Attachment.ConvertToUserVariable(sender);

            Application outlookApp = new Application();
            AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
            NameSpace test = outlookApp.GetNamespace("MAPI");

            if (currentUser.Type == "EX")
            {
                MAPIFolder inboxFolder = test.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
                MAPIFolder sourceFolder = inboxFolder.Folders[vSourceFolder];
                Items filteredItems = null;

                if (vFilter != "")
                {
                    filteredItems = sourceFolder.Items.Restrict(vFilter);
                }
                else
                {
                    filteredItems = sourceFolder.Items;
                }

                foreach (object _obj in filteredItems)
                {
                    if (_obj is MailItem)
                    {
                        MailItem tempMail = (MailItem)_obj;
                        if (v_OperationType == "Reply")
                        {
                            MailItem newMail = tempMail.Reply();
                            Reply(newMail, vBody, vAttachment);
                        }
                        else if(v_OperationType == "Reply All")
                        {
                            MailItem newMail = tempMail.ReplyAll();
                            Reply(newMail, vBody, vAttachment);
                        }
                    }
                }
            }
        }

        private void Reply(MailItem mail, string body, string attPath)
        {
            if(v_BodyType == "HTML")
                mail.HTMLBody = body;
            else mail.Body = body;
            
            if (!string.IsNullOrEmpty(attPath))
                mail.Attachments.Add(attPath);

            mail.Send();
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Body", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_BodyType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Attachment", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From: {v_SourceFolder}]";
        }
    }
}

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
    [Attributes.ClassAttributes.Description("This command allows you to manipulate emails with outlook")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to manipulate emails with your currenty logged in outlook account")]
    [Attributes.ClassAttributes.ImplementationDescription("")]

    public class OutlookGetEmailsCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Provide the source mail folder name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the mail folder you want your emails to come from")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Folder { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Provide a filter (Optional)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter an outlook filter string")]
        [Attributes.PropertyAttributes.SampleUsage("[Subject] = 'Hello' and [SenderName] = 'Jane Doe'")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Filter { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Get unread emails only")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether to retrieve unread email messages only")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_GetUnreadOnly { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the output directory for the messages")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the directory.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfolder or [vTextFolderPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_MessageDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Save attachments")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether to save the email attachments to a local directory")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SaveAttachments { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the output directory for the attachments")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the directory.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfolder or [vTextFolderPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_AttachmentDirectory { get; set; }

       

        public OutlookGetEmailsCommand()
        {
            this.CommandName = "OutlookGetEmailsCommand";
            this.SelectionName = "Get Outlook Emails";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var vFolder = v_Folder.ConvertToUserVariable(sender);
            var vFilter = v_Filter.ConvertToUserVariable(sender);
            var vAttachmentDirectory = v_AttachmentDirectory.ConvertToUserVariable(sender);
            var vMessageDirectory = v_MessageDirectory.ConvertToUserVariable(sender);

            if (vFolder == "") vFolder = "Inbox";

            Application outlookApp = new Application();
            AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
            NameSpace test = outlookApp.GetNamespace("MAPI");

            if (currentUser.Type == "EX")
            {
                MAPIFolder inboxFolder = test.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
                MAPIFolder userFolder = inboxFolder.Folders[vFolder];
                Items filteredItems = null;

                if (vFilter != "")
                {
                    filteredItems = userFolder.Items.Restrict(vFilter);
                }
                else{
                    filteredItems = userFolder.Items;
                }

                foreach (object _obj in filteredItems)
                {
                    if (_obj is MailItem)
                    { 
                        MailItem tempMail = (MailItem)_obj;
                        if (v_GetUnreadOnly == "Yes")
                        {
                            if (tempMail.UnRead == true)
                            {
                                ProcessEmail(tempMail, vMessageDirectory, vAttachmentDirectory);
                            }
                        }
                        else {
                            ProcessEmail(tempMail, vMessageDirectory, vAttachmentDirectory);
                        }   
                    }
                }
            }
        }
        private void ProcessEmail(MailItem mail, string msgDirectory, string attDirectory)
        {
            mail.SaveAs(System.IO.Path.Combine(msgDirectory, mail.Subject + ".msg"));
            if (v_SaveAttachments == "Yes")
            {
                foreach (Attachment attachment in mail.Attachments)
                {
                    attachment.SaveAsFile(System.IO.Path.Combine(attDirectory, attachment.FileName));
                }
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Folder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_GetUnreadOnly", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MessageDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SaveAttachments", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_AttachmentDirectory", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From: {v_Folder}, To: {v_MessageDirectory}, Save Attachments To: {v_AttachmentDirectory}]";
        }
    }
}
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
    [Attributes.ClassAttributes.Description("This command allows you to get emails and attachments with outlook")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get emails and attachments with your currenty logged in outlook account")]
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
        [Attributes.PropertyAttributes.PropertyDescription("Mark emails as read")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether to retrieve unread email messages only")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_MarkAsRead { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the output directory for the messages")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the directory.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfolder or [vTextFolderPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_MessageDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign MailItem List to variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Save messages and attachments")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether to save the email attachments to a local directory")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SaveMessagesAndAttachments { get; set; }

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

                List<MailItem> outMail = new List<MailItem>();

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
                                outMail.Add(tempMail);
                            }
                        }
                        else {
                            ProcessEmail(tempMail, vMessageDirectory, vAttachmentDirectory);
                            outMail.Add(tempMail);
                        }   
                    }
                }
                //add list of datatables to output variable
                Script.ScriptVariable mailItemList = new Script.ScriptVariable
                {
                    VariableName = v_userVariableName,
                    VariableValue = outMail
                };
                engine.VariableList.Add(mailItemList);
            }
        }
        private void ProcessEmail(MailItem mail, string msgDirectory, string attDirectory)
        {
            if (v_MarkAsRead == "Yes")
            {
                mail.UnRead = false;
            }
            if (v_SaveMessagesAndAttachments == "Yes")
            {
                if (System.IO.Directory.Exists(msgDirectory))
                    mail.SaveAs(System.IO.Path.Combine(msgDirectory, mail.Subject + ".msg"));
                if (System.IO.Directory.Exists(attDirectory))
                {
                    foreach (Attachment attachment in mail.Attachments)
                    {
                        attachment.SaveAsFile(System.IO.Path.Combine(attDirectory, attachment.FileName));
                    }
                } 
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Folder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_GetUnreadOnly", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MarkAsRead", this, editor));
            //create control for variable name
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SaveMessagesAndAttachments", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MessageDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_AttachmentDirectory", this, editor));
           
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From: {v_Folder}, To: {v_MessageDirectory}, Save Attachments To: {v_AttachmentDirectory}]";
        }
    }
}
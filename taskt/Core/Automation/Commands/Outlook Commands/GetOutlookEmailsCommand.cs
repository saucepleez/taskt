using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Outlook Commands")]
    [Description("This command gets selected emails and their attachments from Outlook.")]

    public class GetOutlookEmailsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Source Mail Folder Name")]
        [InputSpecification("Enter the name of the Outlook mail folder the emails are located in.")]
        [SampleUsage("Inbox || {vFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SourceFolder { get; set; }

        [XmlAttribute]
        [PropertyDescription("Filter")]
        [InputSpecification("Enter a valid Outlook filter string.")]
        [SampleUsage("[Subject] = 'Hello' and [SenderName] = 'Jane Doe' || {vFilter}")]
        [Remarks("This input is optional.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Filter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Unread Only")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to retrieve unread email messages only.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_GetUnreadOnly { get; set; }

        [XmlAttribute]
        [PropertyDescription("Mark As Read")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to mark retrieved emails as read.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_MarkAsRead { get; set; }

        [XmlAttribute]
        [PropertyDescription("Save MailItems and Attachments")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to save the email attachments to a local directory.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_SaveMessagesAndAttachments { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output MailItem Directory")]   
        [InputSpecification("Enter or Select the path of the directory to store the messages in.")]
        [SampleUsage(@"C:\temp\myfolder || {vFolderPath} || {ProjectPath}\myFolder")]
        [Remarks("This input is optional and will only be used if *Save MailItems and Attachments* is set to **Yes**.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_MessageDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Attachment Directory")]      
        [InputSpecification("Enter or Select the path to the directory to store the attachments in.")]
        [SampleUsage(@"C:\temp\myfolder\attachments || {vFolderPath} || {ProjectPath}\myFolder\attachments")]
        [Remarks("This input is optional and will only be used if *Save MailItems and Attachments* is set to **Yes**.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_AttachmentDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output MailItem List Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public GetOutlookEmailsCommand()
        {
            CommandName = "GetOutlookEmailsCommand";
            SelectionName = "Get Outlook Emails";
            CommandEnabled = true;
            CustomRendering = true;
            v_GetUnreadOnly = "No";
            v_MarkAsRead = "Yes";
            v_SaveMessagesAndAttachments = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vFolder = v_SourceFolder.ConvertToUserVariable(sender);
            var vFilter = v_Filter.ConvertToUserVariable(sender);
            var vAttachmentDirectory = v_AttachmentDirectory.ConvertToUserVariable(sender);
            var vMessageDirectory = v_MessageDirectory.ConvertToUserVariable(sender);

            if (vFolder == "") 
                vFolder = "Inbox";

            Application outlookApp = new Application();
            AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
            NameSpace test = outlookApp.GetNamespace("MAPI");

            if (currentUser.Type == "EX")
            {
                MAPIFolder inboxFolder = test.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
                MAPIFolder userFolder = inboxFolder.Folders[vFolder];
                Items filteredItems = null;

                if (vFilter != "")
                    filteredItems = userFolder.Items.Restrict(vFilter);
                else
                    filteredItems = userFolder.Items;

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
                engine.AddVariable(v_OutputUserVariableName, outMail);
            }
        }      

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_GetUnreadOnly", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MarkAsRead", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SaveMessagesAndAttachments", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MessageDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_AttachmentDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_SourceFolder}' - Filter by '{v_Filter}' - Store MailItem List in '{v_OutputUserVariableName}']";
        }

        private void ProcessEmail(MailItem mail, string msgDirectory, string attDirectory)
        {
            if (v_MarkAsRead == "Yes")
            {
                mail.UnRead = false;
            }
            if (v_SaveMessagesAndAttachments == "Yes")
            {
                if (Directory.Exists(msgDirectory))
                    mail.SaveAs(Path.Combine(msgDirectory, mail.Subject + ".msg"));
                if (Directory.Exists(attDirectory))
                {
                    foreach (Attachment attachment in mail.Attachments)
                    {
                        attachment.SaveAsFile(Path.Combine(attDirectory, attachment.FileName));
                    }
                }
            }
        }
    }
}
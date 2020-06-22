using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
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
    [Description("This command forwards selected emails in Outlook.")]

    public class ForwardOutlookEmailsCommand : ScriptCommand
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
        [PropertyDescription("Recipient(s)")]
        [InputSpecification("Enter the email address(es) of the recipient(s).")]
        [SampleUsage("test@test.com || {vEmail} || test@test.com;test2@test.com || {vEmail1};{vEmail2} || {vEmails}")]
        [Remarks("Multiple recipient email addresses should be delimited by a semicolon (;).")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Recipients { get; set; }

        public ForwardOutlookEmailsCommand()
        {
            CommandName = "ForwardOutlookEmailsCommand";
            SelectionName = "Forward Outlook Emails";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vSourceFolder = v_SourceFolder.ConvertToUserVariable(sender);
            var vFilter = v_Filter.ConvertToUserVariable(sender);
            var vRecipients = v_Recipients.ConvertToUserVariable(sender);
            var splitRecipients = vRecipients.Split(';');

            Application outlookApp = new Application();
            AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
            NameSpace test = outlookApp.GetNamespace("MAPI");

            if (currentUser.Type == "EX")
            {
                MAPIFolder inboxFolder = test.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
                MAPIFolder sourceFolder = inboxFolder.Folders[vSourceFolder];
                Items filteredItems = null;

                if (vFilter != "")
                    filteredItems = sourceFolder.Items.Restrict(vFilter);
                else
                    filteredItems = sourceFolder.Items;

                foreach (object _obj in filteredItems)
                {
                    if (_obj is MailItem)
                    {
                        MailItem tempMail = (MailItem)_obj;
                        MailItem newMail = tempMail.Forward();

                        foreach (var recipient in splitRecipients)
                            newMail.Recipients.Add(recipient.ToString().Trim());

                        newMail.Recipients.ResolveAll();
                        newMail.Send();
                    }
                }
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Recipients", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_SourceFolder}' - Filter by '{v_Filter}' - Forward to '{v_Recipients}']";
        }
    }
}

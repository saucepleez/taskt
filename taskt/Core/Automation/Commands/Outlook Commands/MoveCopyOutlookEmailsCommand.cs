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
    [Description("This command moves or copies selected emails in Outlook.")]

    public class MoveCopyOutlookEmailsCommand : ScriptCommand
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
        [PropertyDescription("Destination Mail Folder Name")]
        [InputSpecification("Enter the name of the Outlook mail folder the emails are being moved/copied to.")]
        [SampleUsage("New Folder || {vFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DestinationFolder { get; set; }

        [XmlAttribute]
        [PropertyDescription("Mail Operation")]
        [PropertyUISelectionOption("Move Emails")]
        [PropertyUISelectionOption("Copy Emails")]
        [InputSpecification("Specify whether to move or copy the selected emails.")]
        [SampleUsage("")]
        [Remarks("Moving will remove the emails from the original folder while copying will not.")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Unread Only")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to move/copy unread email messages only.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_MoveCopyUnreadOnly { get; set; }

        public MoveCopyOutlookEmailsCommand()
        {
            CommandName = "MoveCopyOutlookEmailsCommand";
            SelectionName = "Move/Copy Outlook Emails";
            CommandEnabled = true;
            CustomRendering = true;
            v_OperationType = "Move Emails";
            v_MoveCopyUnreadOnly = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vSourceFolder = v_SourceFolder.ConvertToUserVariable(sender);
            var vFilter = v_Filter.ConvertToUserVariable(sender);
            var vDestinationFolder = v_DestinationFolder.ConvertToUserVariable(sender);
            
            Application outlookApp = new Application();
            AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
            NameSpace test = outlookApp.GetNamespace("MAPI");

            if (currentUser.Type == "EX")
            {
                MAPIFolder inboxFolder = test.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
                MAPIFolder sourceFolder = inboxFolder.Folders[vSourceFolder];
                MAPIFolder destinationFolder = inboxFolder.Folders[vDestinationFolder];
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
                        if(v_OperationType == "Move Emails")
                        {
                            if (v_MoveCopyUnreadOnly == "Yes")
                            {
                                if (tempMail.UnRead == true)
                                    tempMail.Move(destinationFolder);
                            }
                            else
                            {
                                tempMail.Move(destinationFolder);
                            }
                        }
                        else if (v_OperationType == "Copy Emails")
                        {
                            MailItem copyMail = null;
                            if (v_MoveCopyUnreadOnly == "Yes")
                            {
                                if (tempMail.UnRead == true)
                                {
                                    copyMail = tempMail.Copy();
                                    copyMail.Move(destinationFolder);
                                }
                            }
                            else
                            {
                                copyMail = tempMail.Copy();
                                copyMail.Move(destinationFolder);
                            }
                        }
                    }
                }
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationFolder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MoveCopyUnreadOnly", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_OperationType} to '{v_DestinationFolder}' From '{v_SourceFolder}' - Filter by '{v_Filter}']";
        }
    }
}
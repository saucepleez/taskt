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
    [Description("This command deletes selected emails in Outlook.")]

    public class DeleteOutlookEmailsCommand : ScriptCommand
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
        [Remarks("A filter is required to delete emails. Failure to include one will result in a thrown exception.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Filter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delete Read Emails Only")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to delete read email messages only.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_DeleteReadOnly { get; set; }

        public DeleteOutlookEmailsCommand()
        {
            CommandName = "DeleteOutlookEmailsCommand";
            SelectionName = "Delete Outlook Emails";
            CommandEnabled = true;
            CustomRendering = true;
            v_DeleteReadOnly = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vSourceFolder = v_SourceFolder.ConvertToUserVariable(sender);
            var vFilter = v_Filter.ConvertToUserVariable(sender);

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
                    throw new System.Exception("No filter found. Filter required for deleting emails.");

                List<MailItem> tempItems = new List<MailItem>();
                foreach (object _obj in filteredItems)
                {
                    if (_obj is MailItem)
                    {
                        MailItem tempMail = (MailItem)_obj;
                        
                        if (v_DeleteReadOnly == "Yes")
                        {
                            if (tempMail.UnRead == false)
                                tempItems.Add(tempMail);
                        }
                        else
                        {
                            tempItems.Add(tempMail);
                        }
                    }
                }
                int count = tempItems.Count;
                for (int i = 0; i < count; i++)
                {
                    tempItems[i].Delete();
                }
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DeleteReadOnly", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_SourceFolder}' - Filter by '{v_Filter}']";
        }
    }
}
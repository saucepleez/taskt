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
    [Description("This command moves or copies selected emails in Outlook.")]

    public class MoveCopyOutlookEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("MailItem")]
        [InputSpecification("Enter the MailItem to move or copy.")]
        [SampleUsage("{vMailItem}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MailItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Destination Mail Folder Name")]
        [InputSpecification("Enter the name of the Outlook mail folder the emails are being moved/copied to.")]
        [SampleUsage("New Folder || {vFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DestinationFolder { get; set; }

        [XmlAttribute]
        [PropertyDescription("Mail Operation")]
        [PropertyUISelectionOption("Move MailItem")]
        [PropertyUISelectionOption("Copy MailItem")]
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

        public MoveCopyOutlookEmailCommand()
        {
            CommandName = "MoveCopyOutlookEmailCommand";
            SelectionName = "Move/Copy Outlook Email";
            CommandEnabled = true;
            CustomRendering = true;
            v_OperationType = "Move MailItem";
            v_MoveCopyUnreadOnly = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            MailItem vMailItem = (MailItem)VariableMethods.LookupVariable(engine, v_MailItem).VariableValue;
            var vDestinationFolder = v_DestinationFolder.ConvertToUserVariable(engine);
            
            Application outlookApp = new Application();
            AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
            NameSpace test = outlookApp.GetNamespace("MAPI");

            if (currentUser.Type == "EX")
            {
                MAPIFolder inboxFolder = test.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
                MAPIFolder destinationFolder = inboxFolder.Folders[vDestinationFolder];

                if(v_OperationType == "Move MailItem")
                {
                    if (v_MoveCopyUnreadOnly == "Yes")
                    {
                        if (vMailItem.UnRead == true)
                            vMailItem.Move(destinationFolder);
                    }
                    else
                    {
                        vMailItem.Move(destinationFolder);
                    }
                }
                else if (v_OperationType == "Copy MailItem")
                {
                    MailItem copyMail = null;
                    if (v_MoveCopyUnreadOnly == "Yes")
                    {
                        if (vMailItem.UnRead == true)
                        {
                            copyMail = vMailItem.Copy();
                            copyMail.Move(destinationFolder);
                        }
                    }
                    else
                    {
                        copyMail = vMailItem.Copy();
                        copyMail.Move(destinationFolder);
                    }                       
                }               
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationFolder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MoveCopyUnreadOnly", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_OperationType} '{v_MailItem}' to '{v_DestinationFolder}']";
        }
    }
}
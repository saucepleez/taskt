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
    [Attributes.ClassAttributes.Description("This command allows you to move/copy emails with outlook")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move/copy emails with your currenty logged in outlook account")]
    [Attributes.ClassAttributes.ImplementationDescription("")]

    public class OutlookMoveEmailsCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate whether to Move or Copy the emails")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Move Emails")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Copy Emails")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether you intend to move or copy the Emails. Moving will remove the emails from the original folder while Copying will not.")]
        [Attributes.PropertyAttributes.SampleUsage("Select either **Move Emails** or **Copy Emails**")]
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
        [Attributes.PropertyAttributes.InputSpecification("[Subject] = 'Hello' and [SenderName] = 'Jane Doe'")]
        [Attributes.PropertyAttributes.SampleUsage("[Subject] = 'Hello'")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Filter { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Move/Copy unread emails only")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether to move/copy unread email messages only")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_MoveUnreadOnly { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Provide the destination folder name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the directory.")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DestinationFolder { get; set; }

        public OutlookMoveEmailsCommand()
        {
            this.CommandName = "OutlookMoveEmailsCommand";
            this.SelectionName = "Move/Copy Outlook Emails";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
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
                        if(v_OperationType == "Move Emails")
                        {
                            if (v_MoveUnreadOnly == "Yes")
                            {
                                if (tempMail.UnRead == true)
                                {
                                    tempMail.Move(destinationFolder);
                                }
                            }
                            else
                            {
                                tempMail.Move(destinationFolder);
                            }
                        }
                        else if (v_OperationType == "Copy Emails")
                        {
                            MailItem copyMail = null;
                            if (v_MoveUnreadOnly == "Yes")
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
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MoveUnreadOnly", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationFolder", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From: {v_SourceFolder}, To: {v_DestinationFolder}]";
        }
    }
}
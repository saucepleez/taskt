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
    [Attributes.ClassAttributes.Description("This command allows you to forward emails with outlook")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to forward emails with your currenty logged in outlook account")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class OutlookForwardEmailsCommand : ScriptCommand
    {
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
        [Attributes.PropertyAttributes.PropertyDescription("Indicate Recipients (; delimited)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the Email Addresses of the recipients in semicolon seperated values")]
        [Attributes.PropertyAttributes.SampleUsage("test@test.com;test2@test.com")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Recipients { get; set; }

        public OutlookForwardEmailsCommand()
        {
            this.CommandName = "OutlookForwardEmailsCommand";
            this.SelectionName = "Forward Outlook Emails";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var vSourceFolder = v_SourceFolder.ConvertToUserVariable(sender);
            var vFilter = v_Filter.ConvertToUserVariable(sender);
            var vRecipients = v_Recipients.ConvertToUserVariable(sender);

            var splittext = vRecipients.Split(';');

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
                        MailItem newMail = tempMail.Forward();
                        foreach (var t in splittext)
                            newMail.Recipients.Add(t.ToString().Trim());
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
            return base.GetDisplayValue() + $" [From: {v_SourceFolder}, To: {v_Recipients}]";
        }
    }
}

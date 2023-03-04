using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Recieve EMailList Using IMAP")]
    [Attributes.ClassAttributes.Description("This command allows you to get EMailList(Emails) using IMAP protocol.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get EMailList(Emails) using IMAP protocol. Result Variable Type is EMailList.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitRecieveEmailListUsingIMAPCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_Host))]
        [PropertyDetailSampleUsage("**imap.example.com**", PropertyDetailSampleUsage.ValueType.Value, "Host")]
        public string v_IMAPHost { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_Port))]
        [PropertyDetailSampleUsage("**143**", PropertyDetailSampleUsage.ValueType.Value, "Port")]
        [PropertyDetailSampleUsage("**993**", PropertyDetailSampleUsage.ValueType.Value, "Port")]
        [PropertyDetailSampleUsage("**{{{vPort}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Port")]
        public string v_IMAPPort { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_UserName))]
        public string v_IMAPUserName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_Password))]
        public string v_IMAPPassword { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_SecureOption))]
        public string v_IMAPSecureOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_OutputMailListName))]
        public string v_MailListName { get; set; }

        public MailKitRecieveEmailListUsingIMAPCommand()
        {
            //this.CommandName = "MailKitRecieveEMailListUsingIMAPCommand";
            //this.SelectionName = "Recieve EMailList Using IMAP";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            // imap host
            string pop = v_IMAPHost.ConvertToUserVariable(engine);
            var port = this.ConvertToUserVariableAsInteger(nameof(v_IMAPPort), engine);

            // auth
            string user = v_IMAPUserName.ConvertToUserVariable(engine);
            string pass = v_IMAPPassword.ConvertToUserVariable(engine);

            using (var client = new MailKit.Net.Imap.ImapClient())
            {
                var option = this.GetMailKitSecureOption(nameof(v_IMAPSecureOption), engine);
                try
                {
                    lock (client.SyncRoot)
                    {
                        client.Connect(pop, port, option);
                        client.Authenticate(user, pass);

                        List<MimeKit.MimeMessage> messages = new List<MimeKit.MimeMessage>();

                        client.Inbox.Open(MailKit.FolderAccess.ReadOnly);
                        var uids = client.Inbox.Search(MailKit.Search.SearchQuery.All);

                        foreach (var uid in uids)
                        {
                            var message = client.Inbox.GetMessage(uid);
                            messages.Add(message);
                        }

                        client.Disconnect(true);

                        messages.StoreInUserVariable(engine, v_MailListName);
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception("Fail Recieve EMail " + ex.ToString());
                }
            }
        }
    }
}

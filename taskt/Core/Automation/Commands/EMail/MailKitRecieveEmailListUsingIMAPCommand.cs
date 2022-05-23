using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to get MailList(Emails) using IMAP protocol.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get MailList(Emails) using IMAP protocol. Result Variable Type is MailList.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class MailKitRecieveEmailListUsingIMAPCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify IMAP Host Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the host/service name that the script should use")]
        [SampleUsage("**imap.mymail.com** or **{{{vHost}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("IMAP Host", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public string v_IMAPHost { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify IMAP Port")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the port number that should be used when contacting the IMAP service")]
        [SampleUsage("**143** or **993** or **{{{vPort}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("POP Port", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyTextBoxSetting(1, false)]
        public string v_IMAPPort { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify IMAP Username")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the username to use when contacting the IMAP service")]
        [SampleUsage("**username** or **{{{vUserName}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        public string v_IMAPUserName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify IMAP Password")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the password to use when contacting the IMAP service")]
        [SampleUsage("**password** or **{{{vPassword}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Password", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public string v_IMAPPassword { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Secure Option")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyFirstValue("Auto")]
        [PropertyIsOptional(true, "Auto")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Auto")]
        [PropertyUISelectionOption("No SSL or TLS")]
        [PropertyUISelectionOption("Use SSL or TLS")]
        [PropertyUISelectionOption("STARTTLS")]
        [PropertyUISelectionOption("STARTTLS When Available")]
        public string v_IMAPSecureOption { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store MailList")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vMailList** or **{{{vMailList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        public string v_MailListName { get; set; }

        public MailKitRecieveEmailListUsingIMAPCommand()
        {
            this.CommandName = "MailKitRecieveEmailsUsingIMAPCommand";
            this.SelectionName = "Recieve Emails Using IMAP";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            // pop host
            string pop = v_IMAPHost.ConvertToUserVariable(engine);
            int port = v_IMAPPort.ConvertToUserVariableAsInteger("IMAP Port", engine);
            // auth
            string user = v_IMAPUserName.ConvertToUserVariable(engine);
            string pass = v_IMAPPassword.ConvertToUserVariable(engine);
            string secureOption = v_IMAPSecureOption.GetUISelectionValue("v_IMAPSecureOption", this, engine);

            using (var client = new MailKit.Net.Imap.ImapClient())
            {
                var option = MailKit.Security.SecureSocketOptions.Auto;
                switch (secureOption)
                {
                    case "no ssl or tls":
                        option = MailKit.Security.SecureSocketOptions.None;
                        break;
                    case "use ssl or tls":
                        option = MailKit.Security.SecureSocketOptions.SslOnConnect;
                        break;
                    case "starttls":
                        option = MailKit.Security.SecureSocketOptions.StartTls;
                        break;
                    case "starttls when available":
                        option = MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable;
                        break;
                }
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
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + "[Host: '" + v_IMAPHost + "', User: '" + v_IMAPUserName + "', Store: '" + v_MailListName + "']";
        }
    }
}

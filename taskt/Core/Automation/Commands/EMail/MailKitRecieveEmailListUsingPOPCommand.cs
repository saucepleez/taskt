using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Recieve EMailList Using POP")]
    [Attributes.ClassAttributes.Description("This command allows you to get EMailList(EMails) using POP protocol.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get EMailList(EMails) using POP protocol. Result Variable Type is EMailList.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitRecieveEMailListUsingPOPCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_Host))]
        [PropertyDetailSampleUsage("**pop.example.com**", PropertyDetailSampleUsage.ValueType.Value, "Host")]
        public string v_POPHost { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_Port))]
        [PropertyDetailSampleUsage("**110**", PropertyDetailSampleUsage.ValueType.Value, "Port")]
        [PropertyDetailSampleUsage("**995**", PropertyDetailSampleUsage.ValueType.Value, "Port")]
        [PropertyDetailSampleUsage("**{{{vPort}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Port")]
        public string v_POPPort { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_UserName))]
        public string v_POPUserName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_Password))]
        public string v_POPPassword { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_SecureOption))]
        public string v_POPSecureOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_OutputMailListName))]
        public string v_MailListName { get; set; }

        public MailKitRecieveEMailListUsingPOPCommand()
        {
            //this.CommandName = "MailKitRecieveEMailListUsingPOPCommand";
            //this.SelectionName = "Recieve EMailList Using POP";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            // pop host
            string pop = v_POPHost.ExpandValueOrUserVariable(engine);
            var port = this.ExpandValueOrUserVariableAsInteger(nameof(v_POPPort), engine);

            // auth
            string user = v_POPUserName.ExpandValueOrUserVariable(engine);
            string pass = v_POPPassword.ExpandValueOrUserVariable(engine);

            using (var client = new MailKit.Net.Pop3.Pop3Client())
            {
                var option = this.GetMailKitSecureOption(nameof(v_POPSecureOption), engine);
                try
                {
                    lock (client.SyncRoot)
                    {
                        client.Connect(pop, port, option);
                        client.Authenticate(user, pass);

                        List<MimeKit.MimeMessage> messages = new List<MimeKit.MimeMessage>();

                        for (int i = 0; i < client.Count; i++)
                        {
                            var mes = client.GetMessage(i);
                            messages.Add(mes);
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

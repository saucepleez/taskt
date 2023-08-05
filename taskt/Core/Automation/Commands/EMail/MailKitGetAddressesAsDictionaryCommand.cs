using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Get Addresses As Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to get Addresses from EMail.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Addresses from EMail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetAddressesAsDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_InputEMailName))]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_AddressType))]
        public string v_AddressesType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public string v_AddressesDictionary { get; set; }

        public MailKitGetAddressesAsDictionaryCommand()
        {
            //this.CommandName = "MailKitGetAddressesAsDictionaryCommand";
            //this.SelectionName = "Get Addresses As Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var lst = this.GetMailKitEMailAddresses(nameof(v_MailName), nameof(v_AddressesType), engine);

            Dictionary<string, string> addresses = new Dictionary<string, string>();
            foreach(MimeKit.MailboxAddress item in lst.Cast<MailboxAddress>())
            {
                addresses.Add(item.Name, item.Address);
            }
            addresses.StoreInUserVariable(engine, v_AddressesDictionary);
        }
    }
}

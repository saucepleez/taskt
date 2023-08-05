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
    [Attributes.ClassAttributes.CommandSettings("Get Addresses As List")]
    [Attributes.ClassAttributes.Description("This command allows you to get Addresses from EMail.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Addresses from EMail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetAddressesAsListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_InputEMailName))]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_AddressType))]
        public string v_AddressesType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_AddressesList { get; set; }

        public MailKitGetAddressesAsListCommand()
        {
            //this.CommandName = "MailKitGetAddressesAsListCommand";
            //this.SelectionName = "Get Addresses As List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var lst = this.GetMailKitEMailAddresses(nameof(v_MailName), nameof(v_AddressesType), engine);

            List<string> addresses = new List<string>();
            foreach(MimeKit.MailboxAddress item in lst.Cast<MailboxAddress>())
            {
                addresses.Add(item.Address);
            }
            addresses.StoreInUserVariable(engine, v_AddressesList);
        }
    }
}

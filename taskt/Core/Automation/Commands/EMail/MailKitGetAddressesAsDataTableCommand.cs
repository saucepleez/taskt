using MimeKit;
using System;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Get Addresses As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to get Addresses from EMail.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Addresses from EMail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetAddressesAsDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_InputEMailName))]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_AddressType))]
        public string v_AddressesType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public string v_AddressesDataTable { get; set; }

        public MailKitGetAddressesAsDataTableCommand()
        {
            //this.CommandName = "MailKitGetAddressesAsDataTableCommand";
            //this.SelectionName = "Get Addresses As DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var lst = this.GetMailKitEMailAddresses(nameof(v_MailName), nameof(v_AddressesType), engine);

            DataTable addresses = new DataTable();
            addresses.Columns.Add("Name");
            addresses.Columns.Add("Address");
            foreach (MimeKit.MailboxAddress item in lst.Cast<MailboxAddress>())
            {
                addresses.Rows.Add(new object[] { item.Name, item.Address });
            }
            addresses.StoreInUserVariable(engine, v_AddressesDataTable);
        }
    }
}

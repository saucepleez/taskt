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
    [Attributes.ClassAttributes.Description("This command allows you to get Addresses from EMail.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Addresses from EMail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetAddressesAsDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please specify EMail Variable Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**{{{vEMail}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("EMail", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMail, true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        //[PropertyDisplayText(true, "EMail")]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_InputEMailName))]
        public string v_MailName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify Address Type")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("From")]
        //[PropertyUISelectionOption("To")]
        //[PropertyUISelectionOption("CC")]
        //[PropertyUISelectionOption("BCC")]
        //[PropertyUISelectionOption("Reply-To")]
        //[PropertyUISelectionOption("Resent-From")]
        //[PropertyUISelectionOption("Resent-To")]
        //[PropertyUISelectionOption("Resent-CC")]
        //[PropertyUISelectionOption("Resent-BCC")]
        //[PropertyUISelectionOption("Resent-Reply-To")]
        //[PropertyValidationRule("Address Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Type")]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_AddressType))]
        public string v_AddressesType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify Variable Name to Store Addresses")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**vAddress** or **{{{vAddresses}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Addresses Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable, true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public string v_AddressesDataTable { get; set; }

        public MailKitGetAddressesAsDataTableCommand()
        {
            this.CommandName = "MailKitGetAddressesAsDataTableCommand";
            this.SelectionName = "Get Addresses As DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var mail = v_MailName.GetMailKitEMailVariable(engine);

            //var addressType = this.GetUISelectionValue(nameof(v_AddressesType), "Type", engine);

            //MimeKit.InternetAddressList lst = null;
            //switch (addressType)
            //{
            //    case "from":
            //        lst = mail.From;
            //        break;
            //    case "to":
            //        lst = mail.To;
            //        break;
            //    case "cc":
            //        lst = mail.Cc;
            //        break;
            //    case "bcc":
            //        lst = mail.Bcc;
            //        break;
            //    case "reply-to":
            //        lst = mail.ReplyTo;
            //        break;
            //    case "resent-from":
            //        lst = mail.ResentFrom;
            //        break;
            //    case "resent-to":
            //        lst = mail.ResentTo;
            //        break;
            //    case "resent-cc":
            //        lst = mail.ResentCc;
            //        break;
            //    case "resent-bcc":
            //        lst = mail.ResentBcc;
            //        break;
            //    case "resent-reply-to":
            //        lst = mail.ResentReplyTo;
            //        break;
            //}
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

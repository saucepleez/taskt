using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Get Email Attachments Name")]
    [Attributes.ClassAttributes.Description("This command allows you to get Attachment File Name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Attachment File Name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetEmailAttachmentsNameCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_InputEMailName))]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_AttachmentsList { get; set; }

        public MailKitGetEmailAttachmentsNameCommand()
        {
            //this.CommandName = "MailKitGetEmailAttachmentsNameCommand";
            //this.SelectionName = "Get Email Attachments Name";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var mail = v_MailName.GetMailKitEMailVariable(engine);

            var attachments = mail.Attachments.Cast<MimeKit.MimePart>().Select(a => a.FileName).ToList();

            attachments.StoreInUserVariable(engine, v_AttachmentsList);
        }
    }
}

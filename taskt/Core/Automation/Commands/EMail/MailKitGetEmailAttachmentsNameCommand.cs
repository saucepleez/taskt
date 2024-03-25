﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Get EMail Attachments Name")]
    [Attributes.ClassAttributes.Description("This command allows you to get EMail Attachment File Name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get EMail Attachment File Name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetEMailAttachmentsNameCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_InputEMailName))]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_AttachmentsList { get; set; }

        public MailKitGetEMailAttachmentsNameCommand()
        {
            //this.CommandName = "MailKitGetEmailAttachmentsNameCommand";
            //this.SelectionName = "Get Email Attachments Name";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var mail = v_MailName.ExpandUserVariableAsEmail(engine);

            var attachments = mail.Attachments.Cast<MimeKit.MimePart>().Select(a => a.FileName).ToList();

            attachments.StoreInUserVariable(engine, v_AttachmentsList);
        }
    }
}

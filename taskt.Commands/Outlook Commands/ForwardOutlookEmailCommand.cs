using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Outlook Commands")]
    [Description("This command forwards selected emails in Outlook.")]

    public class ForwardOutlookEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("MailItem")]
        [InputSpecification("Enter the MailItem to forward.")]
        [SampleUsage("{vMailItem}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MailItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Recipient(s)")]
        [InputSpecification("Enter the email address(es) of the recipient(s).")]
        [SampleUsage("test@test.com || {vEmail} || test@test.com;test2@test.com || {vEmail1};{vEmail2} || {vEmails}")]
        [Remarks("Multiple recipient email addresses should be delimited by a semicolon (;).")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Recipients { get; set; }

        public ForwardOutlookEmailCommand()
        {
            CommandName = "ForwardOutlookEmailCommand";
            SelectionName = "Forward Outlook Email";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            MailItem vMailItem = (MailItem)VariableMethods.LookupVariable(engine, v_MailItem).VariableValue;
  
            var vRecipients = v_Recipients.ConvertToUserVariable(engine);
            var splitRecipients = vRecipients.Split(';');

            MailItem newMail = vMailItem.Forward();

            foreach (var recipient in splitRecipients)
                newMail.Recipients.Add(recipient.ToString().Trim());

            newMail.Recipients.ResolveAll();
            newMail.Send();         
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Recipients", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [MailItem '{v_MailItem}' - Forward to '{v_Recipients}']";
        }
    }
}

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
    [Attributes.ClassAttributes.Group("Mail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to send email using SMTP protocol.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send an email and have access to SMTP server credentials to generate an email.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class MailKitRecieveEmailsUsingPOPCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify POP Host Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the host/service name that the script should use")]
        [SampleUsage("**pop.gmail.com** or **{{{vHost}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("POP Host", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public string v_POPHost { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify POP Port")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the port number that should be used when contacting the POP service")]
        [SampleUsage("**110** or **995** or **{{{vPort}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("POP Port", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyTextBoxSetting(1, false)]
        public string v_POPPort { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify POP Username")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the username to use when contacting the POP service")]
        [SampleUsage("**username** or **{{{vUserName}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        public string v_POPUserName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify POP Password")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the password to use when contacting the POP service")]
        [SampleUsage("**password** or **{{{vPassword}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Password", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public string v_POPPassword { get; set; }

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
        public string v_POPSecureOption { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store Mail List")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vMails** or **{{{vMails}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        public string v_MailListName { get; set; }

        public MailKitRecieveEmailsUsingPOPCommand()
        {
            this.CommandName = "MailKitRecieveEmailsUsingPOPCommand";
            this.SelectionName = "Recieve Emails Using POP";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override async void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            // pop host
            string pop = v_POPHost.ConvertToUserVariable(engine);
            int port = v_POPPort.ConvertToUserVariableAsInteger("POP Port", engine);
            // auth
            string user = v_POPUserName.ConvertToUserVariable(engine);
            string pass = v_POPPassword.ConvertToUserVariable(engine);
            string secureOption = v_POPSecureOption.GetUISelectionValue("v_POPSecureOption", this, engine);

            using (var client = new MailKit.Net.Pop3.Pop3Client())
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
                    await client.ConnectAsync(pop, port, option);
                    await client.AuthenticateAsync(user, pass);

                    List<MimeKit.MimeMessage> messages = new List<MimeKit.MimeMessage>();

                    for (int i = 0; i < client.Count; i++)
                    {
                        var mes = client.GetMessageAsync(i);
                        messages.Add(await mes);

                        // DBG
                        //Console.WriteLine((await mes).Subject);
                    }

                    await client.DisconnectAsync(true);

                    messages.StoreInUserVariable(engine, v_MailListName);
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
            return base.GetDisplayValue() + "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for EMail, EMailList methods
    /// </summary>
    internal static class EMailControls
    {
        /// <summary>
        /// input EMail Variable property
        /// </summary>
        [PropertyDescription("EMail Variable Name")]
        [InputSpecification("EMail Variable Name", true)]
        [PropertyDetailSampleUsage("**vEMailName**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vEMailName}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("EMail", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMail, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyDisplayText(true, "EMail")]
        public static string v_InputEMailName { get; }

        /// <summary>
        /// output EMail Variable Name property
        /// </summary>
        [PropertyDescription("EMail Variable Name")]
        [InputSpecification("EMail Variable Name", true)]
        [PropertyDetailSampleUsage("**vEMailName**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vEMailName}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("EMail", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMail, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyDisplayText(true, "EMail")]
        public static string v_OutputEMailName { get; }

        /// <summary>
        /// output EMailList Variable Property
        /// </summary>
        [PropertyDescription("EMailList Variable Name")]
        [InputSpecification("EMailList Variable Name", true)]
        [PropertyDetailSampleUsage("**vEMailList**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vEMailList}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("EMailList", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMailList, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyDisplayText(true, "EMailList")]
        public static string v_OutputMailListName { get; }

        /// <summary>
        /// Address Type property
        /// </summary>
        [PropertyDescription("Address Type")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("From")]
        [PropertyUISelectionOption("To")]
        [PropertyUISelectionOption("CC")]
        [PropertyUISelectionOption("BCC")]
        [PropertyUISelectionOption("Reply-To")]
        [PropertyUISelectionOption("Resent-From")]
        [PropertyUISelectionOption("Resent-To")]
        [PropertyUISelectionOption("Resent-CC")]
        [PropertyUISelectionOption("Resent-BCC")]
        [PropertyUISelectionOption("Resent-Reply-To")]
        [PropertyValidationRule("Address Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public static string v_AddressType { get; }

        /// <summary>
        /// pop & imap & smtp mail host proeprty
        /// </summary>
        [PropertyDescription("Host Name")]
        [InputSpecification("Host Name", true)]
        [PropertyDetailSampleUsage("**mail.example.com**", PropertyDetailSampleUsage.ValueType.Value, "Host Name")]
        [PropertyDetailSampleUsage("**{{{vHost}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Host Name")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Host", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyDisplayText(true, "Host")]
        public static string v_Host { get; set; }

        /// <summary>
        /// pop & imap & smtp port property
        /// </summary>
        [PropertyDescription("Port")]
        [InputSpecification("Port Number", true)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Port", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(0, 65535)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyDisplayText(true, "Port")]
        public static string v_Port { get; }

        /// <summary>
        /// pop & imap & smtp user name property
        /// </summary>
        [PropertyDescription("Username")]
        [InputSpecification("Username", true)]
        [PropertyDetailSampleUsage("**john**", PropertyDetailSampleUsage.ValueType.Value, "Username")]
        [PropertyDetailSampleUsage("**john@example.com**", PropertyDetailSampleUsage.ValueType.Value, "Username")]
        [PropertyDetailSampleUsage("**{{{vUser}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Username")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyDisplayText(true, "User")]
        public static string v_UserName { get; }

        /// <summary>
        /// pop & imap & smtp password property
        /// </summary>
        [PropertyDescription("Password")]
        [InputSpecification("Password")]
        [PropertyDetailSampleUsage("**password**", PropertyDetailSampleUsage.ValueType.Value, "Password")]
        [PropertyDetailSampleUsage("**{{{vPass}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Password")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Password", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public static string v_Password { get; set; }

        /// <summary>
        /// pop & imap & smtp secure option property
        /// </summary>
        [PropertyDescription("Secure Option")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyFirstValue("Auto")]
        [PropertyIsOptional(true, "Auto")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Auto")]
        [PropertyUISelectionOption("No SSL or TLS")]
        [PropertyUISelectionOption("Use SSL or TLS")]
        [PropertyUISelectionOption("STARTTLS")]
        [PropertyUISelectionOption("STARTTLS When Available")]
        public static string v_SecureOption { get; }

        /// <summary>
        /// mail address property
        /// </summary>
        [PropertyDescription("EMail Address")]
        [InputSpecification("EMail Address", true)]
        [PropertyDetailSampleUsage("**my-robot@example.com**", PropertyDetailSampleUsage.ValueType.Value, "EMail Address")]
        [PropertyDetailSampleUsage("**{{{vAddress}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "EMail Address")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("EMail Address", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyDisplayText(true, "EMail Address")]
        public static string v_EmailAddress { get; }

        /// <summary>
        /// EMail path property
        /// </summary>
        [PropertyDescription("Path of the EMail")]
        [InputSpecification("Path", true)]
        [PropertyDetailSampleUsage("**C:\\Temp\\mymail.eml**", PropertyDetailSampleUsage.ValueType.Value, "Path")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Path")]
        public static string v_EMailPath { get; }

        /// <summary>
        /// get EMailList Variable from variable name specified argument
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<MimeKit.MimeMessage> GetMailKitEMailListVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is List<MimeKit.MimeMessage> ml)
            {
                return ml;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not MailKit MailList");
            }
        }

        /// <summary>
        /// get EMail Variable from variable name specified argument
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static MimeKit.MimeMessage GetMailKitEMailVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is MimeKit.MimeMessage mail)
            {
                return mail;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not MailKit Mail");
            }
        }

        private static MimeKit.MimeMessage GetMailKitEMailVariable(this ScriptCommand command, string mailParameterName, Engine.AutomationEngineInstance engine)
        {
            var prop = command.GetProperty(mailParameterName);
            string mailName = prop.GetValue(command)?.ToString() ?? "";
            return mailName.GetMailKitEMailVariable(engine);
        }

        public static void StoreInUserVariable(this List<MimeKit.MimeMessage> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }

        public static void StoreInUserVariable(this MimeKit.MimeMessage value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }

        public static MimeKit.InternetAddressList GetMailKitEMailAddresses(this ScriptCommand command, string mailParameterName, string typeParameterName, Engine.AutomationEngineInstance engine)
        {
            var mail = command.GetMailKitEMailVariable(mailParameterName, engine);

            var addressType = command.GetUISelectionValue(typeParameterName, engine);
            switch (addressType)
            {
                case "from":
                    return mail.From;
                case "to":
                    return mail.To;
                case "cc":
                    return mail.Cc;
                case "bcc":
                    return mail.Bcc;
                case "reply-to":
                    return mail.ReplyTo;
                case "resent-from":
                    return mail.ResentFrom;
                case "resent-to":
                    return mail.ResentTo;
                case "resent-cc":
                    return mail.ResentCc;
                case "resent-bcc":
                    return mail.ResentBcc;
                case "resent-reply-to":
                    return mail.ResentReplyTo;
                default:
                    throw new Exception("Strange Address Type '" + addressType + "'");
            }
        }

        public static MailKit.Security.SecureSocketOptions GetMailKitSecureOption(this ScriptCommand command, string propertyName, Engine.AutomationEngineInstance engine)
        {
            var secureOption = command.GetUISelectionValue(propertyName, engine);

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
            return option;
        }
    }
}

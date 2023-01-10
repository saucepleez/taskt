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

        public static void StoreInUserVariable(this List<MimeKit.MimeMessage> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }

        public static void StoreInUserVariable(this MimeKit.MimeMessage value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }
    }
}

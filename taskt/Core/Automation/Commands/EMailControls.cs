using System;
using System.Collections.Generic;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for EMail, EMailList methods
    /// </summary>
    internal static class EMailControls
    {
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

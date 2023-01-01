using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// methods for Boolean
    /// </summary>
    internal static class BooleanControls
    {
        [XmlAttribute]
        [PropertyDescription("Variable Name to Store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**vResult**", "Specify Variable Name **vResult**")]
        [PropertyDetailSampleUsage("**{{{vResult}}}**", "Specify Variable Name **vResult**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        public static string v_Result { get; }

        /// <summary>
        /// Convert variable to Boolean Value
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool ConvertToUserVariableAsBool(this string str, string parameterName, object sender)
        {
            string convertedText = str.ConvertToUserVariable(sender);
            if (bool.TryParse(convertedText, out bool v))
            {
                return v;
            }
            else
            {
                throw new Exception(parameterName + " '" + str + "' is not a boolean.");
            }
        }

        public static void StoreInUserVariable(this bool value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value ? "TRUE" : "FALSE", sender, false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for word methods
    /// </summary>
    internal static class WordControls
    {
        /// <summary>
        /// word instance name property
        /// </summary>
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Word Instance Name", true)]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        [PropertyDetailSampleUsage("**RPAWord**", PropertyDetailSampleUsage.ValueType.Value, "Word Instance")]
        [PropertyDetailSampleUsage("**{{{vInstance}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Word Instance")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Word)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        [PropertyFirstValue("%kwd_default_word_instance%")]
        public static string v_InstanceName { get; }


        /// <summary>
        /// get word instace from value (specified argument)
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Application GetWordInstance(this string instanceName, Engine.AutomationEngineInstance engine)
        {
            var instance = instanceName.ConvertToUserVariable(engine);
            var instanceObject = engine.GetAppInstance(instance);
            if (instanceObject is Application wd)
            {
                return wd;
            }
            else
            {
                throw new Exception("Instance '" + instanceName + "' is not Word Instance");
            }
        }

        /// <summary>
        /// get word instance and ActiveDocument from value (specified argument)
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (Application, Document) GetWordInstanceAndDocument(this string instanceName, Engine.AutomationEngineInstance engine)
        {
            var ins = instanceName.GetWordInstance(engine);

            if (ins.Documents.Count > 0)
            {
                return (ins, ins.ActiveDocument);
            }
            else
            {
                throw new Exception("Word Instance '" + instanceName + "' has no Document");
            }
        }

    }
}

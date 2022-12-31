using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Dictionary methods
    /// </summary>
    internal static class DictionaryControls
    {
        /// <summary>
        /// output Dictionary property
        /// </summary>
        [XmlAttribute]
        [PropertyDescription("Dictionary Name")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**vDictionary**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDictionary}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
        public static string v_OutputDictionaryName { get; }

        /// <summary>
        /// input Dictionary property
        /// </summary>
        [XmlAttribute]
        [PropertyDescription("Dictionary Name")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**{{{vDictionary}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
        public static string v_InputDictionaryName { get; }

        /// <summary>
        /// Dictionary key name
        /// </summary>
        [PropertyDescription("Name of the Dictionary Key")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**key1**", PropertyDetailSampleUsage.ValueType.Value, "Dictionary Key")]
        [PropertyDetailSampleUsage("**{{{vKey}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Dictionary Key")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Key")]
        public static string v_Key { get; }

        /// <summary>
        /// get Dictionary&lt;string, string&gt; Variable from variable name
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, string> GetDictionaryVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is Dictionary<string, string> dictionary)
            {
                return dictionary;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not Dictionary");
            }
        }

        /// <summary>
        /// get Dictionary&lt;string, string&gt; and key name from property names. It supports current position to key.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dictionaryName"></param>
        /// <param name="keyName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (Dictionary<string, string>, string) GetDictionaryVariableAndKey(this ScriptCommand command, string dictionaryName, string keyName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            string dicVariable = command.ConvertToUserVariable(dictionaryName, "Dictionary", engine);
            var v = dicVariable.GetRawVariable(engine);
            if (v.VariableValue is Dictionary<string, string> dictionary)
            {
                string keyVariable = command.ConvertToUserVariable(keyName, "Key", engine);
                string key;
                if (String.IsNullOrEmpty(keyVariable))
                {
                    int pos = v.CurrentPosition;
                    string[] keys = dictionary.Keys.ToArray();
                    if ((pos >= 0) && (pos < keys.Length))
                    {
                        key = keys[pos];
                    }
                    else
                    {
                        throw new Exception("Strange Current Position in Dictionary " + pos);
                    }
                }
                else
                {
                    key = keyVariable.ConvertToUserVariable(engine);
                }
                return (dictionary, key);
            }
            else
            {
                throw new Exception("Variable " + dicVariable + " is not Dictionary");
            }
        }

        public static void StoreInUserVariable(this Dictionary<string, string> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }
    }
}

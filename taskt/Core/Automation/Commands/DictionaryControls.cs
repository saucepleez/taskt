using System;
using System.Data;
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
        [PropertyDetailSampleUsage("**vDictionary**", PropertyDetailSampleUsage.ValueType.VariableName)]
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
        /// Dictionary Key and Value property
        /// </summary>
        [PropertyDescription("Define Keys and Values")]
        [InputSpecification("Enter the Keys and Values required for your dictionary")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true)]
        [PropertyDataGridViewColumnSettings("Keys", "Keys", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Values", "Values", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDisplayText(true, "Items")]
        [PropertyDetailSampleUsage(@"
| Keys | Values |
|---|---|
| **Age** | **15** |", "Add an item whose key is **Age** and value is **15**")]
        [PropertyDetailSampleUsage(@"
| Keys | Values |
|---|---|
| **Name** | **Alice** |", "Add an item whose key is **Name** and value is **Alice**")]
        [PropertyDetailSampleUsage(@"
| Keys | Values |
|---|---|
| **{{{vKey}}** | **{{{vValue}}}** |", "Add an item whose key is Value of Variable **vKey** and value is Value of Variable **vValue**")]
        public static string v_KeyAndValue { get; }

        /// <summary>
        /// when specifed key does not exists
        /// </summary>
        [PropertyDescription("When Key does not Exists")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Error")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public static string v_WhenKeyDoesNotExists { get; }

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

        /// <summary>
        /// add new item to Dictionary from DataTable. check key name is empty
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="table"></param>
        /// <param name="engine"></param>
        /// <exception cref="Exception"></exception>
        public static void AddDataAndValueFromDataTable(this Dictionary<string, string> dic, DataTable table, Automation.Engine.AutomationEngineInstance engine)
        {
            //var keys = new List<string>();

            // Check Items
            foreach (DataRow row in table.Rows)
            {
                string k = (row.Field<string>("Keys") ?? "").ConvertToUserVariable(engine);
                if (k == "")
                {
                    throw new Exception("Key value is empty.");
                }
                //if (keys.Contains(k))
                //{
                //    throw new Exception("Duplicate Key. Name: '" + k + "'");
                //}
                //else
                //{
                //    keys.Add(k);
                //}
            }

            // Add Items
            foreach (DataRow row in table.Rows)
            {
                var key = row.Field<string>("Keys").ConvertToUserVariable(engine);
                var value = (row.Field<string>("Values") ?? "").ConvertToUserVariable(engine);
                dic.Add(key, value);
            }
        }
    }
}

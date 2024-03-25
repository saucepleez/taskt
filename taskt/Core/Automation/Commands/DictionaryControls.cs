﻿using System;
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
        [PropertyDescription("Dictionary Variable Name")]
        [InputSpecification("Dictionary Variable Name", true)]
        [PropertyDetailSampleUsage("**vDictionary**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDictionary}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
        [PropertyParameterOrder(5000)]
        public static string v_OutputDictionaryName { get; }

        /// <summary>
        /// New output Dictionary property
        /// </summary>
        [XmlAttribute]
        [PropertyDescription("New Dictionary Variable Name")]
        [InputSpecification("New Dictionary Variable Name", true)]
        [PropertyDetailSampleUsage("**vNewDictionary**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vNewDictionary}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("New Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New Dictionary")]
        [PropertyParameterOrder(5000)]
        public static string v_NewOutputDictionaryName { get; }

        /// <summary>
        /// input Dictionary property
        /// </summary>
        [XmlAttribute]
        [PropertyDescription("Dictionary Variable Name")]
        [InputSpecification("Dictionary Variable Name", true)]
        [PropertyDetailSampleUsage("**vDictionary**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDictionary}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
        [PropertyParameterOrder(5000)]
        public static string v_InputDictionaryName { get; }

        /// <summary>
        /// input & output Dictionary property
        /// </summary>
        [XmlAttribute]
        [PropertyDescription("Dictionary Variable Name")]
        [InputSpecification("Dictionary Variable Name", true)]
        [PropertyDetailSampleUsage("**vDictionary**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDictionary}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
        [PropertyParameterOrder(5000)]
        public static string v_BothDictionaryName { get; }

        /// <summary>
        /// Dictionary key name
        /// </summary>
        [PropertyDescription("Name of the Dictionary Key")]
        [InputSpecification("Key Name", true)]
        [PropertyDetailSampleUsage("**key1**", PropertyDetailSampleUsage.ValueType.Value, "Dictionary Key")]
        [PropertyDetailSampleUsage("**{{{vKey}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Dictionary Key")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Key")]
        [PropertyParameterOrder(5000)]
        public static string v_Key { get; }

        /// <summary>
        /// Dictionary Key and Value property
        /// </summary>
        [PropertyDescription("Define Keys and Values")]
        [InputSpecification("Keys and Values required for the Dictionary", true)]
        [SampleUsage("Keys and Values")]
        [Remarks("")]
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
| **{{{vKey}}}** | **{{{vValue}}}** |", "Add an item whose key is Value of Variable **vKey** and value is Value of Variable **vValue**")]
        [PropertyParameterOrder(5000)]
        public static string v_KeyAndValue { get; }

        /// <summary>
        /// when specifed key does not exists
        /// </summary>
        [PropertyDescription("When Key does not Exists")]
        [InputSpecification("", true)]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [Remarks("")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Error")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterOrder(5000)]
        public static string v_WhenKeyDoesNotExists { get; }

        /// <summary>
        /// Dictionary Value property
        /// </summary>
        [PropertyDescription("Dictionary Value")]
        [InputSpecification("Dictionary Value", true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Dictionary Value")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Dictionary Value")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Dictionary Value")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyDisplayText(true, "Value")]
        [PropertyParameterOrder(5000)]
        public static string v_Value { get; }

        /// <summary>
        /// Expand user variable as Dictionary&lt;string, string&gt;
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Value is not Dictionary</exception>
        public static Dictionary<string, string> ExpandUserVariableAsDictinary(this string variableName, Engine.AutomationEngineInstance engine)
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
        /// expand user variables as Dictionary&lt;string, string&gt; and key name from property names. It supports current position to key.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dictionaryName"></param>
        /// <param name="keyName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (Dictionary<string, string>, string) ExpandUserVariablesAsDictionaryAndKey(this ScriptCommand command, string dictionaryName, string keyName, Engine.AutomationEngineInstance engine)
        {
            string dicVariable = command.ExpandValueOrUserVariable(dictionaryName, "Dictionary", engine);
            //var v = dicVariable.GetRawVariable(engine);
            //if (v.VariableValue is Dictionary<string, string> dictionary)
            //{
            //    string keyVariable = command.ConvertToUserVariable(keyName, "Key", engine);
            //    string key;
            //    if (String.IsNullOrEmpty(keyVariable))
            //    {
            //        int pos = v.CurrentPosition;
            //        string[] keys = dictionary.Keys.ToArray();
            //        if ((pos >= 0) && (pos < keys.Length))
            //        {
            //            key = keys[pos];
            //        }
            //        else
            //        {
            //            throw new Exception("Strange Current Position in Dictionary " + pos);
            //        }
            //    }
            //    else
            //    {
            //        key = keyVariable.ConvertToUserVariable(engine);
            //    }
            //    return (dictionary, key);
            //}
            //else
            //{
            //    throw new Exception("Variable " + dicVariable + " is not Dictionary");
            //}
            var dictionary = dicVariable.ExpandUserVariableAsDictinary(engine);
            var v = dicVariable.GetRawVariable(engine);
            string keyVariable = command.ExpandValueOrUserVariable(keyName, "Key", engine);
            string key;
            if (string.IsNullOrEmpty(keyVariable))
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
                key = keyVariable.ExpandValueOrUserVariable(engine);
            }
            return (dictionary, key);
        }

        public static void StoreInUserVariable(this Dictionary<string, string> value, Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, engine, false);
        }

        /// <summary>
        /// add new item to Dictionary from DataTable. check key name is empty
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="table"></param>
        /// <param name="engine"></param>
        /// <exception cref="Exception"></exception>
        public static void AddDataAndValueFromDataTable(this Dictionary<string, string> dic, DataTable table, Engine.AutomationEngineInstance engine)
        {
            // Check Items
            foreach (DataRow row in table.Rows)
            {
                string k = (row.Field<string>("Keys") ?? "").ExpandValueOrUserVariable(engine);
                if (k == "")
                {
                    throw new Exception("Key value is empty.");
                }
            }

            // Add Items
            foreach (DataRow row in table.Rows)
            {
                var key = row.Field<string>("Keys").ExpandValueOrUserVariable(engine);
                var value = (row.Field<string>("Values") ?? "").ExpandValueOrUserVariable(engine);
                dic.Add(key, value);
            }
        }
    }
}

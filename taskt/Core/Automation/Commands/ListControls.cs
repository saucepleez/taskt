﻿using System;
using System.Collections.Generic;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// methods for List
    /// </summary>
    internal static class ListControls
    {
        #region Virtual Property
        /// <summary>
        /// input List variable property
        /// </summary>
        [PropertyDescription("List Variable Name")]
        [InputSpecification("List Variable Name", true)]
        [PropertyDetailSampleUsage("**vList**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vList}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        [PropertyParameterOrder(5000)]
        public static string v_InputListName { get; }

        /// <summary>
        /// output List variable Property
        /// </summary>
        [PropertyDescription("List Variable Name")]
        [InputSpecification("List Variable Name", true)]
        [PropertyDetailSampleUsage("**vList**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vList}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        [PropertyParameterOrder(5000)]
        public static string v_OutputListName { get; }

        /// <summary>
        /// New output List variable Property
        /// </summary>
        [PropertyDescription("New List Variable Name")]
        [InputSpecification("List Variable Name", true)]
        [PropertyDetailSampleUsage("**vNewList**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vNewList}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("New List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New List")]
        [PropertyParameterOrder(5000)]
        public static string v_NewOutputListName { get; }

        /// <summary>
        /// input & output List variable Property
        /// </summary>
        [PropertyDescription("List Variable Name")]
        [InputSpecification("List Variable Name", true)]
        [PropertyDetailSampleUsage("**vList**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vList}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        [PropertyParameterOrder(5000)]
        public static string v_BothListName { get; }

        /// <summary>
        /// column type
        /// </summary>
        [PropertyDescription("??? Type")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "List")]
        [PropertyUISelectionOption("List")]
        [PropertyUISelectionOption("Comma Separated")]
        [PropertyUISelectionOption("Space Separated")]
        [PropertyUISelectionOption("Tab Separated")]
        [PropertyUISelectionOption("NewLine Separated")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyDetailSampleUsage("**List**", "Specify the List Variable Name")]
        [PropertyDetailSampleUsage("**Comman Separated**", "Enter like **A,B,C**")]
        [PropertyDetailSampleUsage("**Space Separated**", "Enter like **A B C**")]
        [PropertyDetailSampleUsage("**Tab Separated**", "Enter like **A\tB\tC**")]
        [PropertyParameterOrder(5000)]
        public static string v_AType { get; }


        /// <summary>
        /// for convert parameter
        /// </summary>
        [PropertyDescription("When the number of items in the List is greater than the number of ???")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(5000)]
        public static string v_ANotEnough { get; }

        /// <summary>
        /// for convert parameter
        /// </summary>
        [PropertyDescription("When the number of ??? is greater than the number of items in the List")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Insert Empty Value")]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(5000)]
        public static string v_ListItemNotEnough { get; }

        /// <summary>
        /// when convert number
        /// </summary>
        [PropertyDescription("Action When List Value is Not Numeric")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("Ignore", "Ignore not numeric value")]
        [PropertyDetailSampleUsage("Error", "Rise the Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyParameterOrder(5000)]
        public static string v_WhenValueIsNotNumeric { get; }

        /// <summary>
        /// List index property
        /// </summary>
        [PropertyDescription("Index of the List")]
        [InputSpecification("Index of the List", true)]
        [PropertyDetailSampleUsage("**0**", "Get First List Item")]
        [PropertyDetailSampleUsage("**-1**", "Get Last List Item")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [Remarks("**-1** means index of the last row. If it is empty, it will be the value of Current Position, which can be used for Loop List command.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Position")]
        [PropertyDisplayText(true, "Index")]
        [PropertyParameterOrder(5000)]
        public static string v_ListIndex { get; }

        /// <summary>
        /// search value property
        /// </summary>
        [PropertyDescription("Value to Search")]
        [InputSpecification("Value to Search", true)]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Value to Search")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Value to Search")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Value to Search")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Empty")]
        [PropertyDisplayText(true, "Value to Search")]
        [PropertyParameterOrder(5000)]
        public static string v_SearchValue { get; }
        #endregion

        /// <summary>
        /// expand user variable as List&lt;string&gt;
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">value is not List</exception>
        public static List<string> ExpandUserVariableAsList(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is List<string> list)
            {
                return list;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not supported List");
            }
        }

        /// <summary>
        /// expand (value or) user variables as List Variabe and Index
        /// </summary>
        /// <param name="command"></param>
        /// <param name="variableName"></param>
        /// <param name="indexName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">value is not List, or index is out of range</exception>
        public static (List<string>, int) ExpandUserVariablesAsListAndIndex(this ScriptCommand command, string variableName, string indexName, Engine.AutomationEngineInstance engine)
        {
            var listVariableName = command.ExpandValueOrUserVariable(variableName, "List Variable Name", engine);

            var list = listVariableName.ExpandUserVariableAsList(engine);

            var indexValue = command.ExpandValueOrUserVariable(indexName, "Index", engine);
            int index;
            if (String.IsNullOrEmpty(indexValue))
            {
                var raw = listVariableName.GetRawVariable(engine);
                index = raw.CurrentPosition;
            }
            else if (!int.TryParse(indexValue, out index))
            {
                throw new Exception("Index is not Integer Number. Value: '" + indexValue + "'");
            }

            if (index < 0)
            {
                index += list.Count;
            }

            if (list.Count <= index)
            {
                throw new Exception("Index is out of List index. Index: " + index);
            }

            return (list, index);
        }

        public static void StoreInUserVariable<Type>(this List<Type> value, Core.Automation.Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, engine, false);
        }

        /// <summary>
        /// expand user variable as List&lt;string&gt; to List&lt;decimal&gt;
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="ignoreNotNumeric"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">value is not Decimal List</exception>
        public static List<decimal> ExpandUserVariableAsDecimalList(this string listName, bool ignoreNotNumeric, Engine.AutomationEngineInstance engine)
        {
            var list = listName.ExpandUserVariableAsList(engine);

            List<decimal> numList = new List<decimal>();
            foreach(var value in list)
            {
                if (decimal.TryParse(value, out decimal v))
                {
                    numList.Add(v);
                }
                else if (!ignoreNotNumeric)
                {
                    throw new Exception(listName + " has not numeric value.");
                }
            }

            return numList;
        }

        /// <summary>
        /// math calc process to List
        /// </summary>
        /// <param name="command"></param>
        /// <param name="notNumericName"></param>
        /// <param name="listName"></param>
        /// <param name="engine"></param>
        /// <param name="mathFunc"></param>
        /// <returns></returns>
        /// <exception cref=""></exception>
        public static string MathProcess(ScriptCommand command, string notNumericName, string listName, Engine.AutomationEngineInstance engine, Func<List<decimal>, decimal> mathFunc)
        {
            var notNumeric = command.ExpandValueOrUserVariableAsSelectionItem(notNumericName, "Not Numeric", engine);

            var list = ExpandUserVariableAsDecimalList(listName, (notNumeric == "ignore"), engine);

            return mathFunc(list).ToString();
        }
    }
}

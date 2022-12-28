using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// methods for List
    /// </summary>
    internal static class ListControls
    {
        /// <summary>
        /// input List variable property
        /// </summary>
        [PropertyDescription("List Variable")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**vList**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vList}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List Variable")]
        public static string v_InputListName { get; }

        /// <summary>
        /// output List variable Property
        /// </summary>
        [PropertyDescription("List Variable")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**vList**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vList}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List Variable")]
        public static string v_OutputListName { get; }

        [PropertyDescription("Columns Type")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "List")]
        [PropertyUISelectionOption("List")]
        [PropertyUISelectionOption("Comma Separated")]
        [PropertyUISelectionOption("Space Separated")]
        [PropertyUISelectionOption("Tab Separated")]
        [PropertyUISelectionOption("NewLine Separated")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public static string v_ColumnType { get; set; }


        /// <summary>
        /// for convert parameter
        /// </summary>
        [PropertyDescription("When the number of items in the List is greater than the number of ???")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDisplayText(false, "")]
        public static string v_ANotEnough { get; }

        /// <summary>
        /// for convert parameter
        /// </summary>
        [PropertyDescription("When the number of ??? is greater than the number of items in the List")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Insert Empty Value")]
        [PropertyDisplayText(false, "")]
        public static string v_ListItemNotEnough { get; }

        /// <summary>
        /// when convert number
        /// </summary>
        [PropertyDescription("Action When List Value is Not Numeric")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyDetailSampleUsage("Ignore", "Ignore not numeric value")]
        [PropertyDetailSampleUsage("Error", "Rise the Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Ignore")]
        public static string v_WhenValueIsNotNumeric { get; }

        /// <summary>
        /// List index property
        /// </summary>
        [PropertyDescription("Index of the List")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**0**", "Get First List Item")]
        [PropertyDetailSampleUsage("**-1**", "Get Last List Item")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [Remarks("**-1** means index of the last row. If it is empty, it will be the value of Current Position, which can be used for Loop List command.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Position")]
        [PropertyDisplayText(true, "Index")]
        public static string v_ListIndex { get; }

        /// <summary>
        /// get List&lt;string&gt; variable from variable name
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<string> GetListVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
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

        public static void StoreInUserVariable<Type>(this List<Type> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }

        /// <summary>
        /// get List&lt;string&gt; to List&lt;decimal&gt; from variable name
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="ignoreNotNumeric"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<decimal> GetDecimalListVariable(this string listName, bool ignoreNotNumeric, Engine.AutomationEngineInstance engine)
        {
            var list = listName.GetListVariable(engine);

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
    }
}

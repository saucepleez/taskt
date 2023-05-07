using System;
using System.Collections.Generic;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for variable name methods
    /// </summary>
    internal static class VariableNameControls
    {
        #region
        private const string InnerVariablePrefix = "__INNER_";
        #endregion

        #region virtual properties
        /// <summary>
        /// variable name property
        /// </summary>
        [PropertyDescription("Variable Name")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vSomeVariable**", PropertyDetailSampleUsage.ValueType.Value, "Variable Name")]
        [PropertyDetailSampleUsage("**{{{vSomeVariable}}}**", "Specify **vSomeVariable** for the Variable Name")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public static string v_VariableName { get; }

        /// <summary>
        /// variable value property
        /// </summary>
        [PropertyDescription("Variable Value")]
        [InputSpecification("Variable Value", true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Variable Value")]
        [PropertyDetailSampleUsage("**{{{vNum}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Variable Value")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public static string v_VariableValue { get; }
        #endregion

        #region general variable name methods
        /// <summary>
        /// check variable name is exists
        /// </summary>
        /// <param name="name">name is converted before checking</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool IsVariableExists(string name, Engine.AutomationEngineInstance engine)
        {
            return engine.VariableList.Any(v => (v.VariableName == name.ConvertToUserVariable(engine)));
        }

        /// <summary>
        /// get variable name. when name wraped variable marker, return value is respond to EngineSettings.IgnoreFirstVariableMarkerInOutputParameter
        /// </summary>
        /// <param name="name">name is not converted</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string GetVariableName(string name, Engine.AutomationEngineInstance engine)
        {
            if (IsWrapVariableMarker(name, engine) && engine.engineSettings.IgnoreFirstVariableMarkerInOutputParameter)
            {
                var len = name.Length;
                var s = engine.engineSettings.VariableStartMarker.Length;
                return name.Substring(s, len - s - engine.engineSettings.VariableEndMarker.Length);
            }
            else
            {
                return name;
            }
        }

        /// <summary>
        /// check string starts variable start maker and ends variable ends marker
        /// </summary>
        /// <param name="name">name is not converted</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool IsWrapVariableMarker(string name, Engine.AutomationEngineInstance engine)
        {
            return (name.StartsWith(engine.engineSettings.VariableStartMarker) && name.EndsWith(engine.engineSettings.VariableEndMarker));
        }

        /// <summary>
        /// get wrapped variable name.
        /// </summary>
        /// <param name="name">name is not converted</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string GetWrappedVariableName(string name, Engine.AutomationEngineInstance engine)
        {
            if (IsWrapVariableMarker(name, engine))
            {
                return name;
            }
            else
            {
                return engine.engineSettings.VariableStartMarker + name + engine.engineSettings.VariableEndMarker;
            }
        }
        #endregion

        #region inner variable methods

        /// <summary>
        /// get inner variable value
        /// </summary>
        /// <param name="index"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Script.ScriptVariable GetInnerVariable(int index, Engine.AutomationEngineInstance engine)
        {
            return GetInnerVariableName(index, engine).GetRawVariable(engine);
        }

        /// <summary>
        /// set inner variable value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <param name="engine"></param>
        public static void SetInnerVariable(object value, int index, Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = GetInnerVariableName(index, engine).GetRawVariable(engine);
            v.VariableValue = value;
        }

        /// <summary>
        /// get inner variable name
        /// </summary>
        /// <param name="index"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string GetInnerVariableName(int index, Engine.AutomationEngineInstance engine, bool wrapped = true)
        {
            var varName = InnerVariablePrefix + index.ToString();
            if (wrapped)
            {
                varName = GetWrappedVariableName(varName, engine);
            }
            return varName;
        }
        #endregion
    }
}

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
        [PropertyParameterOrder(5000)]
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
        [PropertyParameterOrder(5000)]
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
            return engine.VariableList.Any(v => (v.VariableName == name.ExpandValueOrUserVariable(engine)));
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
            return IsWrapVariableMarker(name, engine.engineSettings);
        }

        /// <summary>
        /// check string starts variable start maker and ends variable ends marker
        /// </summary>
        /// <param name="name">name is not converted</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static bool IsWrapVariableMarker(string name, ApplicationSettings settings)
        {
            return IsWrapVariableMarker(name, settings.EngineSettings);
        }

        /// <summary>
        /// check string starts variable start maker and ends variable ends marker
        /// </summary>
        /// <param name="name">name is not converted</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static bool IsWrapVariableMarker(string name, EngineSettings settings)
        {
            return (name.StartsWith(settings.VariableStartMarker) && name.EndsWith(settings.VariableEndMarker));
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
                return GetWrappedVariableName(name, engine.engineSettings);
            }
        }

        /// <summary>
        /// get wrapped variable name.
        /// </summary>
        /// <param name="name">name is not converted</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string GetWrappedVariableName(string name, ApplicationSettings settings)
        {
            if (IsWrapVariableMarker(name, settings))
            {
                return name;
            }
            else
            {
                return GetWrappedVariableName(name, settings.EngineSettings);
            }
        }

        /// <summary>
        /// get wrapped variable name.
        /// </summary>
        /// <param name="name">name is not converted</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static string GetWrappedVariableName(string name, EngineSettings settings)
        {
            return settings.VariableStartMarker + name + settings.VariableEndMarker;
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

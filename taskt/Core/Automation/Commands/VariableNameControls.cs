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
        #region const, properties
        /// <summary>
        /// inner variable name prefix
        /// </summary>
        private const string INNER_VARIABLE_PREFIX = "__INNER_";

        /// <summary>
        /// disallow variable name character list
        /// </summary>
        public static readonly List<string> DisallowVariableCharList = new List<string>()
        {
            "+", "-", "*", "%",
            "[", "]", "{", "}",
            ".",
            " ", "\t", "\r", "\n",
            IntermediateControls.INTERMEDIATE_VALIABLE_START_MARKER, IntermediateControls.INTERMEDIATE_VALIABLE_END_MARKER,
            IntermediateControls.INTERMEDIATE_KEYWORD_START_MARKER, IntermediateControls.INTERMEDIATE_VALIABLE_END_MARKER,
        };

        /// <summary>
        /// reserved key name, not use variable name
        /// </summary>
        public static readonly List<string> ReservedKeyNameList = new List<string>()
        {
            "BACKSPACE", "BS", "BKSP",
            "BREAK",
            "CAPSLOCK",
            "DELETE", "DEL",
            "UP", "DOWN", "LEFT", "RIGHT",
            "END",
            "ENTER",
            "INSERT", "INS",
            "NUMLOCK",
            "PGDN",
            "PGUP",
            "SCROLLROCK",
            "TAB",
            "F1", "F2", "F3", "F4", "F5", "F6",
            "F7", "F8", "F9", "F10", "F11", "F12",
            "ADD", "SUBTRACT", "MULTIPLY", "DIVIDE",
            "WIN_KEY",
        };

        /// <summary>
        /// disallow head variable name charactor list
        /// </summary>
        public static readonly List<string> DisallowHeadVariableCharList = new List<string>()
        {
            "0", "1", "2", "3",
            "4", "5", "6", "7",
            "8", "9",
            INNER_VARIABLE_PREFIX,
        };
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
            if (IsWrappedVariableMarker(name, engine) && engine.engineSettings.IgnoreFirstVariableMarkerInOutputParameter)
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
        public static bool IsWrappedVariableMarker(string name, Engine.AutomationEngineInstance engine)
        {
            return IsWrappedVariableMarker(name, engine.engineSettings);
        }

        /// <summary>
        /// check string starts variable start maker and ends variable ends marker
        /// </summary>
        /// <param name="name">name is not converted</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static bool IsWrappedVariableMarker(string name, ApplicationSettings settings)
        {
            return IsWrappedVariableMarker(name, settings.EngineSettings);
        }

        /// <summary>
        /// check string starts variable start maker and ends variable ends marker
        /// </summary>
        /// <param name="name">name is not converted</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static bool IsWrappedVariableMarker(string name, EngineSettings settings)
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
            if (IsWrappedVariableMarker(name, engine))
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
            if (IsWrappedVariableMarker(name, settings))
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

        /// <summary>
        /// get variable names
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static List<string> GetVariableNames(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            return editor?.scriptVariables.Select(t => t.VariableName).ToList() ?? new List<string>();
        }

        /// <summary>
        /// check valid variable name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidVariableName(string name)
        {
            //foreach (string s in ReservedKeyNameList)
            //{
            //    if (name == s)
            //    {
            //        return false;
            //    }
            //}
            if (ReservedKeyNameList.Contains(name))
            {
                return false;
            }

            foreach (string s in DisallowVariableCharList)
            {
                if (name.Contains(s))
                {
                    return false;
                }
            }

            foreach(string s in DisallowHeadVariableCharList)
            {
                if (name.StartsWith(s))
                {
                    return false;
                }
            }

            //if (name.StartsWith("__INNER_"))
            //{
            //    return false;
            //}
            return true;
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
            var varName = INNER_VARIABLE_PREFIX + index.ToString();
            if (wrapped)
            {
                varName = GetWrappedVariableName(varName, engine);
            }
            return varName;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for variable name methods
    /// </summary>
    internal static class VariableNameControls
    {

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
    }
}

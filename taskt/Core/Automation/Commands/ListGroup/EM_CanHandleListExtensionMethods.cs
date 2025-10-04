using System;
using System.Collections.Generic;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleListExtensionMethods
    {
        /// <summary>
        /// check object is List
        /// </summary>
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsList(object value, out List<string> list)
        {
            // TODO: it's ok?
            list = default;
            if (value is List<string> lst)
            {
                list = lst;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create New Empty List
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static List<string> CreateEmptyList(this ICanHandleList command)
        {
            // todo: is it ok?
            return new List<string>();
        }

        /// <summary>
        /// Expand User Variable As List
        /// </summary>
        /// <param name="command"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<string> ExpandUserVariableAsList(ScriptVariable variable)
        {
            // TODO; it's OK?
            if (IsList(variable.VariableValue, out List<string> list))
            {
                return list;
            }
            else
            {
                throw new Exception($"Variable '{variable.VariableName}' is not List");
            }
        }

        /// <summary>
        /// expand user varaible as List
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<string> ExpandUserVariableAsList(this ICanHandleList command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, "List Variable");
            //var v = variableName.GetRawVariable(engine);
            //if (v.VariableValue is List<string> list)
            //{
            //    return list;
            //}
            //else
            //{
            //    throw new Exception($"Variable '{variableName}' is not List");
            //}
            try
            {
                return ExpandUserVariableAsList(variableName.GetRawVariable(engine));
            }
            catch
            {
                throw new Exception($"Variable '{variableName}' is not List");
            }
        }

        /// <summary>
        /// Expand User Variable as Decimal(Numeric) List
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="ignoreNotNumeric"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<decimal> ExpandUserVariableAsDecimalList(this ICanHandleList command, string parameterName, bool ignoreNotNumeric, Engine.AutomationEngineInstance engine)
        {
            var list = command.ExpandUserVariableAsList(parameterName, engine);

            var numList = new List<decimal>();
            foreach (var value in list)
            {
                if (decimal.TryParse(value, out decimal v))
                {
                    numList.Add(v);
                }
                else if (!ignoreNotNumeric)
                {
                    throw new Exception($"List has Not numeric value. Value: '{value}'");
                }
            }

            return numList;
        }

        /// <summary>
        /// store list in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="list"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreListInUserVariable(this ICanHandleList command, List<string> list, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, "List Variable");
            ExtensionMethods.StoreInUserVariable(variableName, list, engine);
        }
    }
}

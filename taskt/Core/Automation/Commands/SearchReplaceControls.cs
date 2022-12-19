using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using static taskt.Core.Automation.Commands.PropertyControls;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// search or replace ScriptCommand parameters value
    /// </summary>
    public static class SearchReplaceControls
    {
        public enum ReplaceTarget
        {
            Parameters,
            Instance, 
            Comment
        }

        /// <summary>
        /// check parameters matched
        /// </summary>
        /// <param name="command"></param>
        /// <param name="keyword"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="checkParameters"></param>
        /// <param name="checkCommandName"></param>
        /// <param name="checkComment"></param>
        /// <param name="checkDisplayText"></param>
        /// <param name="checkInstanceType"></param>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public static bool CheckMatched(this ScriptCommand command, string keyword, bool caseSensitive, bool checkParameters, bool checkCommandName, bool checkComment, bool checkDisplayText, bool checkInstanceType, string instanceType)
        {
            // case sensitive function
            var caseFunc = GetCaseFunc(caseSensitive);
            keyword = caseFunc(keyword);

            // command name
            if (checkCommandName)
            {
                string name = command.SelectionName;
                if (caseFunc(name).Contains(keyword))
                {
                    return true;
                }
            }

            // display text
            if (checkDisplayText)
            {
                string disp = command.GetDisplayValue();
                if (caseFunc(disp).Contains(keyword))
                {
                    return true;
                }
            }

            // comment
            if (checkComment)
            {
                var cmt = command.v_Comment ?? "";
                if (caseFunc(cmt).Contains(keyword))
                {
                    return true;
                }
            }

            // parameters
            if (checkParameters)
            {
                if (CheckParametersMatched(command, keyword, caseSensitive))
                {
                    return true;
                }
            }

            // instance
            if (checkInstanceType)
            {
                if (CheckInstanceMatched(command, keyword, instanceType, caseSensitive))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// check all parameters matched
        /// </summary>
        /// <param name="command"></param>
        /// <param name="keyword"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        private static bool CheckParametersMatched(this ScriptCommand command, string keyword, bool caseSensitive)
        {
            var caseFunc = GetCaseFunc(caseSensitive);
            keyword = caseFunc(keyword);

            var props = command.GetParameterProperties();
            foreach (var prop in props)
            {
                var propValue = prop.GetValue(command);
                if (propValue == null)
                {
                    continue;   // next
                }

                if (propValue is string str)
                {
                    if (caseFunc(str).Contains(keyword))
                    {
                        return true;
                    }
                }
                else if (propValue is System.Data.DataTable table)
                {
                    table.AcceptChanges();
                    var trgDT = table;
                    var rows = trgDT.Rows.Count;
                    var cols = trgDT.Columns.Count;
                    for (var i = 0; i < cols; i++)
                    {
                        if (trgDT.Columns[i].ReadOnly)
                        {
                            continue;
                        }
                        for (var j = 0; j < rows; j++)
                        {
                            if (caseFunc(trgDT.Rows[j][i]?.ToString() ?? "").Contains(keyword))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// check instance name matched. this method use PropertyInstanceType attribute.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="keyword"></param>
        /// <param name="instanceType"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        private static bool CheckInstanceMatched(this ScriptCommand command, string keyword, string instanceType, bool caseSensitive)
        {
            PropertyInstanceType.InstanceType comparedType = InstanceCounter.GetInstanceType(instanceType);

            var caseFunc = GetCaseFunc(caseSensitive);
            keyword = caseFunc(keyword);

            var props = command.GetParameterProperties();
            foreach (var prop in props)
            {
                var virtualPropInfo = prop.GetVirtualProperty();
                var attrIns = GetCustomAttributeWithVirtual<PropertyInstanceType>(prop, virtualPropInfo);
                if ((attrIns?.instanceType ?? PropertyInstanceType.InstanceType.none) == comparedType)
                {
                    var targetValue = prop.GetValue(command)?.ToString() ?? "";
                    if (caseFunc(targetValue).Contains(keyword))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// replace command values
        /// </summary>
        /// <param name="command"></param>
        /// <param name="trg"></param>
        /// <param name="keyword"></param>
        /// <param name="replacedText"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public static bool Replace(this ScriptCommand command, ReplaceTarget trg, string keyword, string replacedText, bool caseSensitive, string instanceType = "")
        {
            switch (trg)
            {
                case ReplaceTarget.Parameters:
                    return ReplaceAllParameters(command, keyword, replacedText, caseSensitive);
                    
                case ReplaceTarget.Instance:
                    return ReplaceInstance(command, keyword, replacedText, instanceType, caseSensitive);
                    
                case ReplaceTarget.Comment:
                    return ReplaceComment(command, keyword, replacedText, caseSensitive);

                default:
                    return false;
            }
        }

        /// <summary>
        /// replace all parameters value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="keyword"></param>
        /// <param name="replacedText"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        private static bool ReplaceAllParameters(this ScriptCommand command, string keyword, string replacedText, bool caseSensitive)
        {
            var caseFunc = GetCaseFunc(caseSensitive);
            keyword = caseFunc(keyword);

            bool isReplaced = false;
            var props = command.GetParameterProperties();
            foreach (var prop in props)
            {
                var targetValue = prop.GetValue(command) ?? "";

                if (targetValue is string str)
                {
                    var currentValue = caseFunc(str);
                    var newValue = currentValue.Replace(keyword, replacedText);
                    if (currentValue != newValue)
                    {
                        prop.SetValue(command, newValue);
                        isReplaced = true;
                    }
                }
                else if (targetValue is System.Data.DataTable table)
                {
                    var rows = table.Rows.Count;
                    var cols = table.Columns.Count;
                    for (int i = 0; i < cols; i++)
                    {
                        if (table.Columns[i].ReadOnly)
                        {
                            continue;
                        }
                        for (int j = 0; j < rows; j++)
                        {
                            var currentValue = caseFunc(table.Rows[j][i]?.ToString() ?? "");
                            var newValue = currentValue.Replace(keyword, replacedText);
                            if (currentValue != newValue)
                            {
                                table.Rows[j][i] = newValue;
                                isReplaced = true;
                            }
                        }
                    }
                }
            }
            return isReplaced;
        }

        /// <summary>
        /// replace instance parameters value. this method use PropertyInstanceType attribute.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="keyword"></param>
        /// <param name="replacedText"></param>
        /// <param name="instanceType"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        private static bool ReplaceInstance(this ScriptCommand command, string keyword, string replacedText, string instanceType, bool caseSensitive)
        {
            PropertyInstanceType.InstanceType comparedType = InstanceCounter.GetInstanceType(instanceType);

            var caseFunc = GetCaseFunc(caseSensitive);
            keyword = caseFunc(keyword);

            bool isReplaced = false;

            var props = command.GetParameterProperties();
            foreach (var prop in props)
            {
                var virtualPropInfo = prop.GetVirtualProperty();
                var attrIns = GetCustomAttributeWithVirtual<PropertyInstanceType>(prop, virtualPropInfo);

                if (attrIns == null)
                {
                    continue;
                }
                else if (attrIns.instanceType == comparedType)
                {
                    var currentValue = caseFunc(prop.GetValue(command)?.ToString() ?? "");
                    var newValue = currentValue.Replace(keyword, replacedText);
                    if (currentValue != newValue)
                    {
                        prop.SetValue(command, newValue);
                        isReplaced = true;
                    }
                }
            }

            return isReplaced;
        }

        /// <summary>
        /// replace comment value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="keyword"></param>
        /// <param name="replacedText"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        private static bool ReplaceComment(this ScriptCommand command, string keyword, string replacedText, bool caseSensitive)
        {
            var caseFunc = GetCaseFunc(caseSensitive);
            keyword = caseFunc(keyword);

            var commentProp = command.GetProperty("v_Comment");
            var currentComment = caseFunc(commentProp.GetValue(command)?.ToString() ?? "");
            var newComment = currentComment.Replace(keyword, replacedText);
            if (currentComment != newComment)
            {
                commentProp.SetValue(command, newComment);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// get case sensitive lambda
        /// </summary>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        private static Func<string, string> GetCaseFunc(bool caseSensitive)
        {
            if (caseSensitive)
            {
                return new Func<string, string>((str) =>
                {
                    return str;
                });
            }
            else
            {
                return new Func<string, string>((str) =>
                {
                    return str.ToLower();
                });
            }
        }
    }
}

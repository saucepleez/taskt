using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;
using taskt.Core.Settings;

namespace taskt.Core.Utilities.CommonUtilities
{
    public static class VariableMethods
    {
        /// <summary>
        /// Replaces variable placeholders ([variable]) with variable text.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        public static string ConvertToUserVariable(this String str, object sender)
        {
            if (str == null)
                return string.Empty;

            if (sender == null)
                return str;

            if (str.Length < 2)
            {
                return str;
            }

            var engine = (AutomationEngineInstance)sender;

            var variableList = engine.VariableList;
            var systemVariables = Common.GenerateSystemVariables();

            var variableSearchList = new List<ScriptVariable>();
            variableSearchList.AddRange(variableList);
            variableSearchList.AddRange(systemVariables);

            var elementSearchList = engine.ElementList;

            //check if it's an element first
            if (str.StartsWith("<") && str.EndsWith(">"))
            {
                string potentialElement = str.TrimStart('<').TrimEnd('>');
                var matchingElement = elementSearchList.Where(elem => elem.ElementName == potentialElement).FirstOrDefault();
                if (matchingElement != null)
                {
                    //if element found, store it in str and continue checking for variables
                    str = matchingElement.ElementValue;
                }
            }
            
            //custom variable markers
            var startVariableMarker = engine.EngineSettings.VariableStartMarker;
            var endVariableMarker = engine.EngineSettings.VariableEndMarker;

            //split by custom markers
            string[] potentialVariables = str.Split(new string[] { startVariableMarker, endVariableMarker }, StringSplitOptions.None);

            foreach (var potentialVariable in potentialVariables)
            {
                //complex variable handling
                if (potentialVariable.Contains("=>"))
                {
                    var complexJsonVariable = potentialVariable;

                    //detect potential variables and replace
                    string[] potentialSubVariables = complexJsonVariable.Split(new string[] { "^" }, StringSplitOptions.None);

                    foreach (var potentialSubVariable in potentialSubVariables)
                    {
                        var matchingVar = (from vars in variableSearchList
                                           where vars.VariableName == potentialSubVariable
                                           select vars).FirstOrDefault();

                        if (matchingVar != null)
                        {
                            //get the value from the list

                            complexJsonVariable = complexJsonVariable.Replace("^" + potentialSubVariable + "^", matchingVar.GetDisplayValue());
                            continue;
                        }

                    }

                    //split by json select token pointer
                    var element = complexJsonVariable.Split(new string[] { "=>" }, StringSplitOptions.None);

                    //verify length
                    if (element.Length >= 2)
                    {
                        //get variable name
                        var variableName = element[0].Trim();

                        //get json pattern
                        var jsonPattern = element[1].Trim();

                        //check json pattern starts with
                        if (jsonPattern.StartsWith("$."))
                        {
                            //find variable
                            var matchingVar = (from vars in variableSearchList
                                               where vars.VariableName == variableName
                                               select vars).FirstOrDefault();
                            //if variable is found
                            if (matchingVar != null)
                            {
                                //get the value from the list
                                var complexJson = matchingVar.GetDisplayValue();

                                JToken match;
                                if (complexJson.StartsWith("[") && complexJson.EndsWith("]"))
                                {
                                    //attempt to match array based on user defined pattern
                                    JArray parsedObject = JArray.Parse(complexJson);
                                    match = parsedObject.SelectToken(jsonPattern);
                                }
                                else
                                {
                                    //attempt to match object based on user defined pattern
                                    JObject parsedObject = JObject.Parse(complexJson);
                                    match = parsedObject.SelectToken(jsonPattern);
                                }

                                //check match
                                if (match != null)
                                {
                                    //replace with value
                                    str = str.Replace(startVariableMarker + potentialVariable + endVariableMarker, match.ToString());
                                    continue;
                                }

                            }
                        }
                    }
                }

                string varcheckname = potentialVariable;
                bool isSystemVar = systemVariables.Any(vars => vars.VariableName == varcheckname);
                string[] aPotentialVariable = potentialVariable.Split(new string[] { "[", "]" }, StringSplitOptions.None);
                int directElementIndex = 0;
                bool useDirectElementIndex = false;

                if (aPotentialVariable.Length == 3 && int.TryParse(aPotentialVariable[1], out directElementIndex))
                {
                    varcheckname = aPotentialVariable[0];
                    useDirectElementIndex = true;
                }
                else if (potentialVariable.Split('.').Length == 2 && !isSystemVar)
                {
                    varcheckname = potentialVariable.Split('.')[0];
                }

                var varCheck = (from vars in variableSearchList
                                where vars.VariableName == varcheckname
                                select vars).FirstOrDefault();

                if (potentialVariable.Length == 0)
                    continue;

                if (potentialVariable == "taskt.EngineContext")
                {
                    varCheck.VariableValue = engine.GetEngineContext();
                }

                if (varCheck != null)
                {
                    var searchVariable = startVariableMarker + potentialVariable + endVariableMarker;

                    if (str.Contains(searchVariable))
                    {
                        if (useDirectElementIndex)
                        {
                            int savePosition = varCheck.CurrentPosition;
                            varCheck.CurrentPosition = directElementIndex;
                            str = str.Replace(searchVariable, (string)varCheck.GetDisplayValue());
                            varCheck.CurrentPosition = savePosition;
                        }
                        else if (varCheck.VariableValue is DataTable && potentialVariable.Split('.').Length == 2)
                        {
                            //user is trying to get data from column name or index
                            string columnName = potentialVariable.Split('.')[1];
                            var dt = varCheck.VariableValue as DataTable;

                            string cellItem;
                            if (int.TryParse(columnName, out var columnIndex))
                            {
                                cellItem = dt.Rows[varCheck.CurrentPosition].Field<object>(columnIndex).ToString();
                            }
                            else
                            {
                                cellItem = dt.Rows[varCheck.CurrentPosition].Field<object>(columnName).ToString();
                            }

                            str = str.Replace(searchVariable, cellItem);
                        }
                        else if (varCheck.VariableValue is DataRow && potentialVariable.Split('.').Length == 2)
                        {
                            //user is trying to get data from column name or index
                            string columnName = potentialVariable.Split('.')[1];
                            var dr = varCheck.VariableValue as DataRow;

                            string cellItem;
                            if (int.TryParse(columnName, out var columnIndex))
                            {
                                cellItem = dr[columnIndex].ToString();
                            }
                            else
                            {
                                cellItem = dr[columnName].ToString();
                            }

                            str = str.Replace(searchVariable, cellItem);
                        }
                        else if (potentialVariable.Split('.').Length == 2) // This handles vVariable.count 
                        {
                            string propertyName = potentialVariable.Split('.')[1];
                            str = str.Replace(searchVariable, (string)varCheck.GetDisplayValue(propertyName));
                        }
                        else
                        {
                            str = str.Replace(searchVariable, (string)varCheck.GetDisplayValue());
                        }
                    }
                    else if (str.Contains(potentialVariable))
                    {
                        try
                        {
                            str = str.Replace(potentialVariable, (string)varCheck.GetDisplayValue());
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                else if ((potentialVariable.Contains("ds") && (potentialVariable.Contains("."))))
                {
                    //peform dataset check
                    var splitVariable = potentialVariable.Split('.');

                    if (splitVariable.Length == 3)
                    {
                        string dsleading = splitVariable[0];
                        string datasetName = splitVariable[1];
                        string columnRequired = splitVariable[2];

                        var datasetVariable = variableList.Where(f => f.VariableName == datasetName).FirstOrDefault();

                        if (datasetVariable == null)
                            continue;

                        DataTable dataTable = (DataTable)datasetVariable.VariableValue;

                        if (datasetVariable == null)
                            continue;

                        if ((dsleading == "ds") && (int.TryParse(columnRequired, out int columnNumber)))
                        {
                            //get by column index
                            str = (string)dataTable.Rows[datasetVariable.CurrentPosition][columnNumber];
                        }
                        else if (dsleading == "ds")
                        {
                            //get by column index
                            str = (string)dataTable.Rows[datasetVariable.CurrentPosition][columnRequired];
                        }
                    }
                }
            }

            if (!engine.AutoCalculateVariables)
            {
                return str;
            }
            else
            {
                //track math chars
                var mathChars = new List<Char>();
                mathChars.Add('*');
                mathChars.Add('+');
                mathChars.Add('-');
                mathChars.Add('=');
                mathChars.Add('/');

                //if the string matches the char then return
                //as the user does not want to do math
                if (mathChars.Any(f => f.ToString() == str) || (mathChars.Any(f => str.StartsWith(f.ToString()))))
                {
                    return str;
                }

                //bypass math for types that are dates
                DateTime dateTest;
                if ((DateTime.TryParse(str, out dateTest) && (str.Length > 6)))
                {
                    return str;
                }

                //test if math is required
                if (mathChars.Any(f => str.Contains(f)))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        var v = dt.Compute(str, "");
                        return v.ToString();
                    }
                    catch (Exception)
                    {
                        return str;
                    }
                }
                else
                    return str;               
            }
        }

        /// <summary>
        /// Stores value of the string to a user-defined variable.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        /// <param name="targetVariable">the name of the user-defined variable to override with new value</param>
        public static void StoreInUserVariable(this String value, object sender, string targetVariable)
        {
            var engine = (AutomationEngineInstance)sender;
            
            var requiredVariable = LookupVariable(engine, targetVariable);

            //if still not found and user has elected option, create variable at runtime
            if ((requiredVariable == null) && (engine.EngineSettings.CreateMissingVariablesDuringExecution))
            {
                engine.VariableList.Add(new ScriptVariable() { VariableName = targetVariable });
                requiredVariable = LookupVariable(engine, targetVariable);
            }

            if (requiredVariable != null)
            {
                var sourceVariable = LookupVariable(engine, value);
                if(sourceVariable != null)
                {
                    requiredVariable.VariableValue = sourceVariable.VariableValue;
                }
                else
                {
                    var variableValue = value.ConvertToUserVariable(engine);

                    if (variableValue.StartsWith("{{") && variableValue.EndsWith("}}"))
                    {
                        var itemList = variableValue.Replace("{{", "").Replace("}}", "").Split('|').Select(s => s.Trim()).ToList();
                        requiredVariable.VariableValue = itemList;
                    }
                    else
                    {
                        requiredVariable.VariableValue = variableValue;
                    }
                }
            }
            else
            {
                throw new Exception("Attempted to store data in a variable, but it was not found. Enclose variables within braces, ex. {vVariable}");
            }
        }

        public static ScriptVariable LookupVariable(AutomationEngineInstance engine, string variableName)
        {
            //search for the variable
            var requiredVariable = engine.VariableList.Where(var => var.VariableName == variableName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (variableName.StartsWith(engine.EngineSettings.VariableStartMarker)) && (variableName.EndsWith(engine.EngineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = variableName.Replace(engine.EngineSettings.VariableStartMarker, "").Replace(engine.EngineSettings.VariableEndMarker, "");
                requiredVariable = engine.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        
        /// <summary>
        /// Formats item as a variable (enclosing brackets)
        /// </summary>
        /// <param name="str">The string to be wrapped as a variable</param>
        public static string ApplyVariableFormatting(this String str)
        {
            var settings = new ApplicationSettings().GetOrCreateApplicationSettings();

            return str.Insert(0, settings.EngineSettings.VariableStartMarker).Insert(str.Length + 1, settings.EngineSettings.VariableEndMarker);
        }

        /// <summary>
        /// Converts a string to SecureString
        /// </summary>
        /// <param name="value">The string to be converted to SecureString</param>
        public static SecureString GetSecureString(this string value)
        {
            var secureString = new System.Net.NetworkCredential(string.Empty, value).SecurePassword;
            return secureString;
        }

        public static string ConvertSecureStringToString(this SecureString secureString)
        {
            var strValue = new System.Net.NetworkCredential(string.Empty, secureString).Password;
            return strValue;
        }
    }
}

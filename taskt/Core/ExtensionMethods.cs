using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.Automation;
namespace taskt.Core
{
    public static class ExtensionMethods
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


            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var variableList = engine.VariableList;
            var systemVariables = Core.Common.GenerateSystemVariables();

            var searchList = new List<Core.Script.ScriptVariable>();
            searchList.AddRange(variableList);
            searchList.AddRange(systemVariables);


            //custom variable markers
            var startVariableMarker = engine.engineSettings.VariableStartMarker;
            var endVariableMarker = engine.engineSettings.VariableEndMarker;

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
                        var matchingVar = (from vars in searchList
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
                            var matchingVar = (from vars in searchList
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

                var varCheck = (from vars in searchList
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
                        catch (Exception)
                        {

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

           
        }
        /// <summary>
        /// Stores value of the string to a user-defined variable.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        /// <param name="targetVariable">the name of the user-defined variable to override with new value</param>
        public static void StoreInUserVariable(this String str, object sender, string targetVariable)
        {
            Core.Automation.Commands.VariableCommand newVariableCommand = new Core.Automation.Commands.VariableCommand
            {
                v_userVariableName = targetVariable,
                v_Input = str
            };
            newVariableCommand.RunCommand(sender);
        }
        /// <summary>
        /// Formats item as a variable (enclosing brackets)s
        /// </summary>
        /// <param name="str">The string to be wrapped as a variable</param>
        public static string ApplyVariableFormatting(this String str)
        {
            var settings = new ApplicationSettings().GetOrCreateApplicationSettings();

            return str.Insert(0, settings.EngineSettings.VariableStartMarker).Insert(str.Length + 1, settings.EngineSettings.VariableEndMarker);
        }


        public static string ToBase64(this string text)
        {
            return ToBase64(text, Encoding.UTF8);
        }

        public static string ToBase64(this string text, Encoding encoding)
        {
            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        public static bool TryParseBase64(this string text, out string decodedText)
        {
            return TryParseBase64(text, Encoding.UTF8, out decodedText);
        }

        public static bool TryParseBase64(this string text, Encoding encoding, out string decodedText)
        {
            if (string.IsNullOrEmpty(text))
            {
                decodedText = text;
                return false;
            }

            try
            {
                byte[] textAsBytes = Convert.FromBase64String(text);
                decodedText = encoding.GetString(textAsBytes);
                return true;
            }
            catch (Exception)
            {
                decodedText = null;
                return false;
            }
        }

       
    }


}


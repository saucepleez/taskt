using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var engine = (Core.AutomationEngineInstance)sender;
            
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
                    //split by json select token pointer
                    var element = potentialVariable.Split(new string[] { "=>" }, StringSplitOptions.None);

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

                                //deserialize into json object
                                JObject parsedObject = JObject.Parse(complexJson);

                                //attempt to match based on user defined pattern
                                var match = parsedObject.SelectToken(jsonPattern);

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


                var varCheck = (from vars in searchList
                                where vars.VariableName == potentialVariable
                                select vars).FirstOrDefault();


                if (potentialVariable.Length == 0)
                                    continue;


                if (potentialVariable == "taskt.EngineContext")
                {
                    //set json settings
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Error = (serializer, err) => {
                        err.ErrorContext.Handled = true;
                    };
                    settings.Formatting = Formatting.Indented;

                    varCheck.VariableValue = Newtonsoft.Json.JsonConvert.SerializeObject(engine, settings);
                }


                if (varCheck != null)
                {
                    var searchVariable = startVariableMarker + potentialVariable + endVariableMarker;

                    if (str.Contains(searchVariable))
                    {
                        str = str.Replace(searchVariable, (string)varCheck.GetDisplayValue());
                    }
                    else if (str.Contains(potentialVariable))
                    {                
                         str = str.Replace(potentialVariable, (string)varCheck.GetDisplayValue());                                 
                    }
                }

                else if ((potentialVariable.Contains("ds") && (potentialVariable.Contains("."))))
                {
                    //peform dataset check
                    var splitVariable = potentialVariable.Split('.');
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




            //track math chars
            var mathChars = new List<Char>();
            mathChars.Add('*');
            mathChars.Add('+');
            mathChars.Add('-');
            mathChars.Add('=');
            mathChars.Add('/');

            //if the string matches the char then return
            //as the user does not want to do math
            if (mathChars.Any( f => f.ToString() == str) || (mathChars.Any(f => str.StartsWith(f.ToString()))))
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
        /// <summary>
        /// Stores value of the string to a user-defined variable.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        /// <param name="targetVariable">the name of the user-defined variable to override with new value</param>
        public static void StoreInUserVariable(this String str, object sender, string targetVariable)
        {
            AutomationCommands.VariableCommand newVariableCommand = new AutomationCommands.VariableCommand
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
    }
}

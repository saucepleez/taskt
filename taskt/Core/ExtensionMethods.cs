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

            string[] potentialVariables = str.Split('[', ']');

            foreach (var potentialVariable in potentialVariables)
            {
                var varCheck = (from vars in searchList
                                where vars.VariableName == potentialVariable
                                select vars).FirstOrDefault();

                // break here; //todo -- this needs to resolve a variable with the name Row.Item(0);

                if (varCheck != null)
                {
                    var searchVariable = "[" + potentialVariable + "]";

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
            return str.Insert(0, "[").Insert(str.Length + 1, "]");
        }
    }
}

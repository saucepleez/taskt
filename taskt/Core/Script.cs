//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Data;

namespace taskt.Core.Script
{
    #region Script and Variables

    public class Script
    {
        /// <summary>
        /// Contains user-defined variables
        /// </summary>
        public List<ScriptVariable> Variables { get; set; }
        /// <summary>
        /// Contains user-selected commands
        /// </summary>
        public List<ScriptAction> Commands;

        public Script()
        {
            //initialize
            Variables = new List<ScriptVariable>();
            Commands = new List<ScriptAction>();
        }
        /// <summary>
        /// Returns a new 'Top-Level' command.  
        /// </summary>
        public ScriptAction AddNewParentCommand(Core.Automation.Commands.ScriptCommand scriptCommand)
        {
            ScriptAction newExecutionCommand = new ScriptAction() { ScriptCommand = scriptCommand };
            Commands.Add(newExecutionCommand);
            return newExecutionCommand;
        }

        /// <summary>
        /// Converts and serializes the user-defined commands into an XML file  
        /// </summary>
        public static Script SerializeScript(ListView.ListViewItemCollection scriptCommands, List<ScriptVariable> scriptVariables, string scriptFilePath = "")
        {
            var script = new Core.Script.Script();

            //save variables to file

            script.Variables = scriptVariables;

            //save listview tags to command list

            int lineNumber = 1;

            List<Core.Script.ScriptAction> subCommands = new List<Core.Script.ScriptAction>();

            foreach (ListViewItem commandItem in scriptCommands)
            {
                var command = (Core.Automation.Commands.ScriptCommand)commandItem.Tag;
                command.LineNumber = lineNumber;

                if ((command is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (command is Core.Automation.Commands.BeginContinousLoopCommand) || (command is Core.Automation.Commands.BeginListLoopCommand) || (command is Core.Automation.Commands.BeginIfCommand) || (command is Core.Automation.Commands.BeginMultiIfCommand) || (command is Core.Automation.Commands.BeginExcelDatasetLoopCommand) || (command is Core.Automation.Commands.TryCommand) || (command is Core.Automation.Commands.BeginLoopCommand) || (command is Core.Automation.Commands.BeginMultiLoopCommand))
                {
                    if (subCommands.Count == 0)  //if this is the first loop
                    {
                        //add command to root node
                        var newCommand = script.AddNewParentCommand(command);
                        //add to tracking for additional commands
                        subCommands.Add(newCommand);
                    }
                    else  //we are already looping so add to sub item
                    {
                        //get reference to previous node
                        var parentCommand = subCommands[subCommands.Count - 1];
                        //add as new node to previous node
                        var nextNodeParent = parentCommand.AddAdditionalAction(command);
                        //add to tracking for additional commands
                        subCommands.Add(nextNodeParent);
                    }
                }
                else if ((command is Core.Automation.Commands.EndLoopCommand) || (command is Core.Automation.Commands.EndIfCommand) || (command is Core.Automation.Commands.EndTryCommand))  //if current loop scenario is ending
                {
                    //get reference to previous node
                    var parentCommand = subCommands[subCommands.Count - 1];
                    //add to end command // DECIDE WHETHER TO ADD WITHIN LOOP NODE OR PREVIOUS NODE
                    parentCommand.AddAdditionalAction(command);
                    //remove last command since loop is ending
                    subCommands.RemoveAt(subCommands.Count - 1);
                }
                else if (subCommands.Count == 0) //add command as a root item
                {
                    script.AddNewParentCommand(command);
                }
                else //we are within a loop so add to the latest tracked loop item
                {
                    var parentCommand = subCommands[subCommands.Count - 1];
                    parentCommand.AddAdditionalAction(command);
                }

                //increment line number
                lineNumber++;
            }

            //output to xml file
            XmlSerializer serializer = new XmlSerializer(typeof(Script));
            var settings = new XmlWriterSettings
            {
                NewLineHandling = NewLineHandling.Entitize,
                Indent = true
            };

            //if output path was provided
            if (scriptFilePath != "")
            {
                //write to file
                System.IO.FileStream fs;
                using (fs = System.IO.File.Create(scriptFilePath))
                {
                    using (XmlWriter writer = XmlWriter.Create(fs, settings))
                    {
                        serializer.Serialize(writer, script);
                    }
                }  
            }


            return script;
        }
        /// <summary>
        /// Deserializes a valid XML file back into user-defined commands
        /// </summary>
        public static Script DeserializeFile(string scriptFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Script));

            System.IO.FileStream fs;

            //open file stream from file
            using (fs = new System.IO.FileStream(scriptFilePath, System.IO.FileMode.Open))
            {
                //read and return data
                XmlReader reader = XmlReader.Create(fs);
                Script deserializedData = (Script)serializer.Deserialize(reader);
                return deserializedData;
            }

          
            
        }
        /// <summary>
        /// Deserializes an XML string into user-defined commands (server sends a string to the client)
        /// </summary>
        public static Script DeserializeXML(string scriptXML)
        {
            System.IO.StringReader reader = new System.IO.StringReader(scriptXML);
            XmlSerializer serializer = new XmlSerializer(typeof(Script));
            Script deserializedData = (Script)serializer.Deserialize(reader);
            return deserializedData;
        }
    }

    public class ScriptAction
    {
        /// <summary>
        /// generic 'top-level' user-defined script command (ex. not nested)
        /// </summary>
        [XmlElement(Order = 1)]
        public Core.Automation.Commands.ScriptCommand ScriptCommand { get; set; }
        /// <summary>
        /// generic 'sub-level' commands (ex. nested commands within a loop)
        /// </summary>
        [XmlElement(Order = 2)]
        public List<ScriptAction> AdditionalScriptCommands { get; set; }
        /// <summary>
        /// adds a command as a nested command to a top-level command
        /// </summary>
        public ScriptAction AddAdditionalAction(Core.Automation.Commands.ScriptCommand scriptCommand)
        {
            if (AdditionalScriptCommands == null)
            {
                AdditionalScriptCommands = new List<ScriptAction>();
            }

            ScriptAction newExecutionCommand = new ScriptAction() { ScriptCommand = scriptCommand };
            AdditionalScriptCommands.Add(newExecutionCommand);
            return newExecutionCommand;
        }
    }
    [Serializable]
    public class ScriptVariable
    {
        /// <summary>
        /// name that will be used to identify the variable
        /// </summary>
        public string VariableName { get; set; }
        /// <summary>
        /// index/position tracking for complex variables (list)
        /// </summary>
        [XmlIgnore]
        public int CurrentPosition = 0;
        /// <summary>
        /// value of the variable or current index
        /// </summary>
        public object VariableValue { get; set; }
        /// <summary>
        /// retrieve value of the variable
        /// </summary>
        public string GetDisplayValue(string requiredProperty = "")
        {
           
            if (VariableValue is string)
            {
                switch (requiredProperty)
                {
                    case "type":
                    case "Type":
                    case "TYPE":
                        return "BASIC";
                    default:
                        return (string)VariableValue;
                }
              
            }
            else if(VariableValue is DataTable)
            {
                DataTable dataTable = (DataTable)VariableValue;              
                var dataRow = dataTable.Rows[CurrentPosition];
                return Newtonsoft.Json.JsonConvert.SerializeObject(dataRow.ItemArray);            
            }
            else
            {
                List<string> requiredValue = (List<string>)VariableValue;
                switch(requiredProperty)
                {
                    case "count":
                    case "Count":
                    case "COUNT":
                        return requiredValue.Count.ToString();
                    case "index":
                    case "Index":
                    case "INDEX":
                        return CurrentPosition.ToString();
                    case "tojson":
                    case "ToJson":                
                    case "toJson":
                    case "TOJSON":
                        return Newtonsoft.Json.JsonConvert.SerializeObject(requiredValue);
                    case "topipe":
                    case "ToPipe":
                    case "toPipe":                
                    case "TOPIPE":
                        return String.Join("|", requiredValue);
                    case "first":
                    case "First":
                    case "FIRST":
                        return requiredValue.FirstOrDefault();
                    case "last":
                    case "Last":
                    case "LAST":
                        return requiredValue.Last();
                    case "type":
                    case "Type":
                    case "TYPE":
                        return "LIST";
                    default:
                        return requiredValue[CurrentPosition];
                }
            }
           
        }
    }

    #endregion Script and Variables
}
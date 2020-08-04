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

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using taskt.Core.Command;
using Formatting = Newtonsoft.Json.Formatting;

namespace taskt.Core.Script
{
    public class Script
    {
        public string ProjectName { get; set; }
        /// <summary>
        /// Contains user-defined variables
        /// </summary>
        public List<ScriptVariable> Variables { get; set; }
        /// <summary>
        /// Contains user-defined elements
        /// </summary>
        public List<ScriptElement> Elements { get; set; }
        /// <summary>
        /// Contains user-selected commands
        /// </summary>
        public List<ScriptAction> Commands;
        private string FileName { get; set; }

        public Script()
        {
            Variables = new List<ScriptVariable>();
            Commands = new List<ScriptAction>();
        }

        /// <summary>
        /// Returns a new 'Top-Level' command.
        /// </summary>
        public ScriptAction AddNewParentCommand(ScriptCommand scriptCommand)
        {
            ScriptAction newExecutionCommand = new ScriptAction() { ScriptCommand = scriptCommand };
            Commands.Add(newExecutionCommand);
            return newExecutionCommand;
        }

        /// <summary>
        /// Converts and serializes the user-defined commands into an JSON file
        /// </summary>
        public static Script SerializeScript(
            ListView.ListViewItemCollection scriptCommands,
            List<ScriptVariable> scriptVariables,
            List<ScriptElement> scriptElements,
            string scriptFilePath = "",
            string projectName = ""
            )
        {
            var script = new Script();

            script.FileName = System.IO.Path.GetFileName(scriptFilePath);

            script.ProjectName = projectName;

            //save variables to file
            script.Variables = scriptVariables;

            //save elements to file
            script.Elements = scriptElements;

            //save listview tags to command list
            int lineNumber = 1;
            List<ScriptAction> subCommands = new List<ScriptAction>();

            foreach (ListViewItem commandItem in scriptCommands)
            {
                var command = (ScriptCommand)commandItem.Tag;
                command.LineNumber = lineNumber;

                if ((command.CommandName == "LoopNumberOfTimesCommand") || (command.CommandName == "LoopContinuouslyCommand") ||
                    (command.CommandName == "LoopCollectionCommand") || (command.CommandName == "BeginIfCommand") ||
                    (command.CommandName == "BeginMultiIfCommand") || (command.CommandName == "BeginTryCommand") ||
                    (command.CommandName == "BeginLoopCommand") || (command.CommandName == "BeginMultiLoopCommand") ||
                    (command.CommandName == "BeginRetryCommand") || (command.CommandName == "BeginSwitchCommand"))
                {
                    //if this is the first loop
                    if (subCommands.Count == 0)
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
                //if current loop scenario is ending
                else if ((command.CommandName == "EndLoopCommand") || (command.CommandName == "EndIfCommand") ||
                         (command.CommandName == "EndTryCommand") || (command.CommandName == "EndRetryCommand") ||
                         (command.CommandName == "EndSwitchCommand"))
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

            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
            JsonSerializer serializer = JsonSerializer.Create(serializerSettings);

            //output to json file
            //if output path was provided
            if (scriptFilePath != "")
            {
                //write to file
                using (StreamWriter sw = new StreamWriter(scriptFilePath))
                using (JsonWriter writer = new JsonTextWriter(sw){ Formatting = Formatting.Indented })
                {
                    serializer.Serialize(writer, script, typeof(Script));
                }
            }

            return script;
        }
        /// <summary>
        /// Deserializes a valid JSON file back into user-defined commands
        /// </summary>
        public static Script DeserializeFile(string scriptFilePath)
        {
            using (StreamReader file = File.OpenText(scriptFilePath))
            {
                var serializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };

                JsonSerializer serializer = JsonSerializer.Create(serializerSettings);
                Script deserializedData = (Script)serializer.Deserialize(file, typeof(Script));

                return deserializedData;
            }
        }

        /// <summary>
        /// Deserializes an json string into user-defined commands (server sends a string to the client)
        /// </summary>
        public static Script DeserializeJsonString(string jsonScript)
        {
            return JsonConvert.DeserializeObject<Script>(jsonScript);
        }
    }
}

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
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Xml;
using taskt.Core.Automation.Commands;

namespace taskt.Core.Script
{
    public class Script
    {
        public string FileName { get; set; }
        public string ProjectName { get; set; }
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
        /// Converts and serializes the user-defined commands into an XML file
        /// </summary>
        public static Script SerializeScript(
            ListView.ListViewItemCollection scriptCommands,
            List<ScriptVariable> scriptVariables,
            string scriptFilePath = "",
            string projectName = ""
            )
        {
            var script = new Script();

            script.FileName = System.IO.Path.GetFileName(scriptFilePath);

            script.ProjectName = projectName;

            //save variables to file
            script.Variables = scriptVariables;

            //save listview tags to command list
            int lineNumber = 1;
            List<ScriptAction> subCommands = new List<ScriptAction>();

            foreach (ListViewItem commandItem in scriptCommands)
            {
                var command = (ScriptCommand)commandItem.Tag;
                command.LineNumber = lineNumber;

                if ((command is LoopNumberOfTimesCommand) || (command is LoopContinuouslyCommand) ||
                    (command is LoopListCommand) || (command is BeginIfCommand) ||
                    (command is BeginMultiIfCommand) || (command is TryCommand) || 
                    (command is BeginLoopCommand) || (command is BeginMultiLoopCommand))
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
                else if ((command is EndLoopCommand) || (command is EndIfCommand) ||(command is EndTryCommand))
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
}

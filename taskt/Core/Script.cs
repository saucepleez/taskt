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
using System.Xml.Linq;
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

        public ScriptInformation Info;

        public Script()
        {
            //initialize
            Variables = new List<ScriptVariable>();
            Commands = new List<ScriptAction>();
            Info = new ScriptInformation();
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
        public static Script SerializeScript(ListView.ListViewItemCollection scriptCommands, List<ScriptVariable> scriptVariables, ScriptInformation info, Core.EngineSettings engineSettings, string scriptFilePath = "")
        {
            var script = new Core.Script.Script();

            //save variables to file

            script.Variables = scriptVariables;
            script.Info = info;

            //save listview tags to command list

            int lineNumber = 1;

            List<Core.Script.ScriptAction> subCommands = new List<Core.Script.ScriptAction>();

            foreach (ListViewItem commandItem in scriptCommands)
            {
                var srcCommand = (Core.Automation.Commands.ScriptCommand)commandItem.Tag;
                srcCommand.IsDontSavedCommand = false;

                var command = srcCommand.Clone();
                command.LineNumber = lineNumber;

                if ((command is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (command is Core.Automation.Commands.BeginContinousLoopCommand) || (command is Core.Automation.Commands.BeginListLoopCommand) || (command is Core.Automation.Commands.BeginIfCommand) || (command is Core.Automation.Commands.BeginMultiIfCommand) || (command is Core.Automation.Commands.TryCommand) || (command is Core.Automation.Commands.BeginLoopCommand) || (command is Core.Automation.Commands.BeginMultiLoopCommand))
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

            // Convert Intermediate
            if (engineSettings.ExportIntermediateXML)
            {
                foreach (var cmd in script.Commands)
                {
                    cmd.ConvertToIntermediate(engineSettings, scriptVariables);
                }
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
                using (System.IO.FileStream fs = System.IO.File.Create(scriptFilePath))
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
        public static Script DeserializeFile(string scriptFilePath, Core.EngineSettings engineSettings)
        {
            XDocument xmlScript = XDocument.Load(scriptFilePath);

            // pre-convert
            convertOldScript(xmlScript);

            using (var reader = xmlScript.Root.CreateReader())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Script));
                Script des = (Script)serializer.Deserialize(reader);

                // release
                serializer = null;

                foreach (var cmd in des.Commands)
                {
                    cmd.ConvertToRaw(engineSettings);
                }

                xmlScript = null;

                return des;
            }

            //open file stream from file
            //using (System.IO.FileStream fs = new System.IO.FileStream(scriptFilePath, System.IO.FileMode.Open))
            //{
            //    //read and return data
            //    XmlReader reader = XmlReader.Create(fs);
            //    Script deserializedData = (Script)serializer.Deserialize(reader);

            //    // release
            //    serializer = null;

            //    foreach (var cmd in deserializedData.Commands)
            //    {
            //        cmd.ConvertToRaw(engineSettings);
            //    }

            //    return deserializedData;
            //}
        }
        /// <summary>
        /// Deserializes an XML string into user-defined commands (server sends a string to the client)
        /// </summary>
        public static Script DeserializeXML(string scriptXML)
        {
            try
            {
                using (System.IO.StringReader reader = new System.IO.StringReader(scriptXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Script));
                    Script deserializedData = (Script)serializer.Deserialize(reader);
                    return deserializedData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string SerializeScript(List<Core.Automation.Commands.ScriptCommand> commands)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Script));

            var actions = new Script();
            foreach (taskt.Core.Automation.Commands.ScriptCommand cmd in commands)
            {
                actions.AddNewParentCommand(cmd);
            }
            using (var writer = new System.IO.StringWriter())
            {
                serializer.Serialize(writer, actions);
                actions = null;
                return writer.ToString();
            }
        }

        public static Script DeserializeScript(string scriptXML)
        {
            using (var reader = new System.IO.StringReader(scriptXML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Script));
                var ret = (Script)serializer.Deserialize(reader);
                return ret;
            }
        }

        private static XDocument convertOldScript(XDocument doc)
        {
            //IEnumerable<XElement> cmds = from el in doc.Descendants("ScriptCommand")
            //                             where (string)el.Attribute("CommandName") == "ActivateWindowCommand"
            //                             select el;

            //foreach (var cmd in cmds)
            //{
            //    if (((string)cmd.Attribute("v_SearchMethod")).ToLower() == "start with")
            //    {
            //        cmd.Attribute("v_SearchMethod").SetValue("Starts with");
            //    }
            //}

            convertTo3_5_0_45(doc);
            convertTo3_5_0_46(doc);
            convertTo3_5_0_47(doc);
            convertTo3_5_0_50(doc);
            convertTo3_5_0_51(doc);
            convertTo3_5_0_52(doc);
            convertTo3_5_0_57(doc);
            convertTo3_5_0_67(doc);
            convertTo3_5_0_69(doc);
            convertTo3_5_0_73(doc);
            convertTo3_5_0_74(doc);
            convertTo3_5_0_78(doc);

            return doc;
        }

        private static XDocument convertTo3_5_0_45(XDocument doc)
        {
            // change "Start with" -> "Starts with", "End with" -> "Ends with"
            IEnumerable<XElement> cmdWindowNames = doc.Descendants("ScriptCommand").Where(el => (
                    (string)el.Attribute("CommandName") == "ActivateWindowCommand" ||
                    (string)el.Attribute("CommandName") == "CheckWindowNameExistsCommand" ||
                    (string)el.Attribute("CommandName") == "CloseWindowCommand" ||
                    (string)el.Attribute("CommandName") == "GetWindowNamesCommand" ||
                    (string)el.Attribute("CommandName") == "GetWindowPositionCommand" ||
                    (string)el.Attribute("CommandName") == "GetWindowStateCommand" ||
                    (string)el.Attribute("CommandName") == "MoveWindowCommand" ||
                    (string)el.Attribute("CommandName") == "ResizeWindowCommand" ||
                    (string)el.Attribute("CommandName") == "SetWindowStateCommand" ||
                    (string)el.Attribute("CommandName") == "WaitForWindowCommand" ||
                    (string)el.Attribute("CommandName") == "SendAdvancedKeyStrokesCommand" ||
                    (string)el.Attribute("CommandName") == "SendHotkeyCommand" ||
                    (string)el.Attribute("CommandName") == "SendKeysCommand" ||
                    (string)el.Attribute("CommandName") == "UIAutomationCommand"
                )
            );
            foreach (var cmd in cmdWindowNames)
            {
                if (cmd.Attribute("v_SearchMethod") != null)
                {
                    if (((string)cmd.Attribute("v_SearchMethod")).ToLower() == "start with")
                    {
                        cmd.SetAttributeValue("v_SearchMethod", "Starts with");
                    }
                    if (((string)cmd.Attribute("v_SearchMethod")).ToLower() == "end with")
                    {
                        cmd.SetAttributeValue("v_SearchMethod", "Ends with");
                    }
                }
            }

            // ExcelCreateDataset -> LoadDataTable
            IEnumerable<XElement> cmdExcels = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "ExcelCreateDatasetCommand"));
            XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            foreach (var cmd in cmdExcels)
            {
                cmd.SetAttributeValue("CommandName", "LoadDataTableCommand");
                cmd.SetAttributeValue(ns + "type", "LoadDataTableCommand");
                cmd.SetAttributeValue("SelectionName", "Load DataTable");
            }

            return doc;
        }

        private static XDocument convertTo3_5_0_46(XDocument doc)
        {
            // AddToVariable -> AddListItem
            IEnumerable<XElement> cmdAddList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "AddToVariableCommand"));
            XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            foreach (var cmd in cmdAddList)
            {
                cmd.SetAttributeValue("CommandName", "AddListItemCommand");
                cmd.SetAttributeValue(ns + "type", "AddListItemCommand");
                cmd.SetAttributeValue("SelectionName", "Add List Item");
            }

            // SetVariableIndex -> SetListIndex
            IEnumerable<XElement> cmdListIndex = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "SetVariableIndexCommand"));
            foreach (var cmd in cmdListIndex)
            {
                cmd.SetAttributeValue("CommandName", "SetListIndexCommand");
                cmd.SetAttributeValue(ns + "type", "SetListIndexCommand");
                cmd.SetAttributeValue("SelectionName", "Set List Index");
            }

            return doc;
        }
        private static XDocument convertTo3_5_0_47(XDocument doc)
        {
            // AddListItem.v_userVariableName, SetListIndex.v_userVariableName -> *.v_ListName
            IEnumerable<XElement> cmdIndex = doc.Descendants("ScriptCommand")
                .Where(el =>
                    ((string)el.Attribute("CommandName") == "AddListItemCommand") ||
                    ((string)el.Attribute("CommandName") == "SetListIndexCommand")
            );
            //XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            foreach (var cmd in cmdIndex)
            {
                var listNameAttr = cmd.Attribute("v_userVariableName");
                if (listNameAttr != null)
                {
                    cmd.SetAttributeValue("v_ListName", listNameAttr.Value);
                    listNameAttr.Remove();
                }
            }

            return doc;
        }
        private static XDocument convertTo3_5_0_50(XDocument doc)
        {
            // ParseJSONArray -> ConvertJSONToList
            IEnumerable<XElement> cmdAddList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "ParseJSONArrayCommand"));
            XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            foreach (var cmd in cmdAddList)
            {
                cmd.SetAttributeValue("CommandName", "ConvertJSONToListCommand");
                cmd.SetAttributeValue(ns + "type", "ConvertJSONToListCommand");
                cmd.SetAttributeValue("SelectionName", "Convert JSON To List");
            }

            return doc;
        }
        private static XDocument convertTo3_5_0_51(XDocument doc)
        {
            // GetDataCountRowCommand -> GetDataTableRowCountCommand
            IEnumerable<XElement> cmdAddList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "GetDataRowCountCommand"));
            XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            foreach (var cmd in cmdAddList)
            {
                cmd.SetAttributeValue("CommandName", "GetDataTableRowCountCommand");
                cmd.SetAttributeValue(ns + "type", "GetDataTableRowCountCommand");
                cmd.SetAttributeValue("SelectionName", "Get DataTable Row Count");
            }

            // AddDataRow -> AddDataTableRow
            IEnumerable<XElement> cmdAddDataRow = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "AddDataRowCommand"));
            foreach (var cmd in cmdAddDataRow)
            {
                cmd.SetAttributeValue("CommandName", "AddDataTableRowCommand");
                cmd.SetAttributeValue(ns + "type", "AddDataTableRowCommand");
                cmd.SetAttributeValue("SelectionName", "Add DataTable Row");
            }

            return doc;
        }
        private static XDocument convertTo3_5_0_52(XDocument doc)
        {
            // change "Start with" -> "Starts with", "End with" -> "Ends with"
            IEnumerable<XElement> cmdWindowNames = doc.Descendants("ScriptCommand").Where(el => (
                    (string)el.Attribute("CommandName") == "GetFilesCommand" ||
                    (string)el.Attribute("CommandName") == "GetFoldersCommand" ||
                    (string)el.Attribute("CommandName") == "CheckStringCommand"
                )
            );
            foreach (var cmd in cmdWindowNames)
            {
                if (cmd.Attribute("v_SearchMethod") != null)
                {
                    if (((string)cmd.Attribute("v_SearchMethod")).ToLower() == "start with")
                    {
                        cmd.SetAttributeValue("v_SearchMethod", "Starts with");
                    }
                    if (((string)cmd.Attribute("v_SearchMethod")).ToLower() == "end with")
                    {
                        cmd.SetAttributeValue("v_SearchMethod", "Ends with");
                    }
                }
            }
            return doc;
        }
        private static XDocument convertTo3_5_0_57(XDocument doc)
        {
            // StringCheckTextCommand -> CheckTextCommand
            IEnumerable<XElement> chkTextList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "CheckStringCommand"));
            XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            foreach (var cmd in chkTextList)
            {
                cmd.SetAttributeValue("CommandName", "CheckTextCommand");
                cmd.SetAttributeValue(ns + "type", "CheckTextCommand");
                cmd.SetAttributeValue("SelectionName", "Check Text");
            }

            // ModifyVariableCommand -> ModifyTextCommand
            IEnumerable<XElement> modifyTextList = doc.Descendants("ScriptCommand")
                .Where(el => (
                    ((string)el.Attribute("CommandName") == "ModifyVariableCommand") ||
                    ((string)el.Attribute("CommandName") == "StringCaseCommand"))
                );
            foreach (var cmd in modifyTextList)
            {
                cmd.SetAttributeValue("CommandName", "ModifyTextCommand");
                cmd.SetAttributeValue(ns + "type", "ModifyTextCommand");
                cmd.SetAttributeValue("SelectionName", "Modify Text");
            }

            // RegExExtractorCommand -> RegExExtractionText
            IEnumerable<XElement> regExList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "RegExExtractorCommand"));
            foreach (var cmd in regExList)
            {
                cmd.SetAttributeValue("CommandName", "RegExExtractionTextCommand");
                cmd.SetAttributeValue(ns + "type", "RegExExtractionTextCommand");
                cmd.SetAttributeValue("SelectionName", "RegEx Extraction Text");
            }

            // StringReplaceCommand -> ReplaceTextCommand
            IEnumerable<XElement> replaceList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "StringReplaceCommand"));
            foreach (var cmd in replaceList)
            {
                cmd.SetAttributeValue("CommandName", "ReplaceTextCommand");
                cmd.SetAttributeValue(ns + "type", "ReplaceTextCommand");
                cmd.SetAttributeValue("SelectionName", "Replace Text");
            }

            // StringSplitCommand -> SplitTextCommand
            IEnumerable<XElement> splitList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "StringSplitCommand"));
            foreach (var cmd in splitList)
            {
                cmd.SetAttributeValue("CommandName", "SplitTextCommand");
                cmd.SetAttributeValue(ns + "type", "SplitTextCommand");
                cmd.SetAttributeValue("SelectionName", "Split Text");
            }

            // StringSubstringCommand -> SubstringTextCommand
            IEnumerable<XElement> substrList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "StringSubstringCommand"));
            foreach (var cmd in substrList)
            {
                cmd.SetAttributeValue("CommandName", "SubstringTextCommand");
                cmd.SetAttributeValue(ns + "type", "SubstringTextCommand");
                cmd.SetAttributeValue("SelectionName", "Substring Text");
            }

            // TextExtractorCommand -> ExtractionTextCommand
            IEnumerable<XElement> extList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "TextExtractorCommand"));
            foreach (var cmd in extList)
            {
                cmd.SetAttributeValue("CommandName", "ExtractionTextCommand");
                cmd.SetAttributeValue(ns + "type", "ExtractionTextCommand");
                cmd.SetAttributeValue("SelectionName", "Extraction Text");
            }

            return doc;
        }

        private static XDocument convertTo3_5_0_67(XDocument doc)
        {
            // GetAElement -> GetAnElement
            IEnumerable<XElement> getList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "SeleniumBrowserGetAElementValuesAsListCommand"));
            XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            foreach (var cmd in getList)
            {
                cmd.SetAttributeValue("CommandName", "SeleniumBrowserGetAnElementValuesAsListCommand");
                cmd.SetAttributeValue(ns + "type", "SeleniumBrowserGetAnElementValuesAsListCommand");
                cmd.SetAttributeValue("SelectionName", "Get An Element Values As List");
            }

            IEnumerable<XElement> getDic = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "SeleniumBrowserGetAElementValuesAsDictionaryCommand"));
            foreach (var cmd in getDic)
            {
                cmd.SetAttributeValue("CommandName", "SeleniumBrowserGetAnElementValuesAsDictionaryCommand");
                cmd.SetAttributeValue(ns + "type", "SeleniumBrowserGetAnElementValuesAsDictionaryCommand");
                cmd.SetAttributeValue("SelectionName", "Get An Element Values As Dictionary");
            }

            IEnumerable<XElement> getDT = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "SeleniumBrowserGetAElementValuesAsDataTableCommand"));
            foreach (var cmd in getDT)
            {
                cmd.SetAttributeValue("CommandName", "SeleniumBrowserGetAnElementValuesAsDataTableCommand");
                cmd.SetAttributeValue(ns + "type", "SeleniumBrowserGetAnElementValuesAsDataTableCommand");
                cmd.SetAttributeValue("SelectionName", "Get An Element Values As DataTable");
            }

            return doc;
        }

        private static XDocument convertTo3_5_0_69(XDocument doc)
        {
            // UI Automation Boolean Fix
            IEnumerable<XElement> getList = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "UIAutomationCommand"));
            XNamespace ns = "urn:schemas-microsoft-com:xml-diffgram-v1";
            foreach (var cmd in getList)
            {
                XElement tableParams = cmd.Element("v_UIASearchParameters").Element(ns + "diffgram").Element("DocumentElement");
                var elems = tableParams.Elements();
                foreach (XElement elem in elems)
                {
                    XElement elemEnabled = elem.Element("Enabled");
                    switch (elemEnabled.Value.ToLower())
                    {
                        case "true":
                        case "false":
                            break;

                        default:
                            elemEnabled.SetValue("False");
                            break;
                    }
                }
            }
            return doc;
        }

        private static XDocument convertTo3_5_0_73(XDocument doc)
        {
            // BeginIf Selenium -> WebBrowser
            IEnumerable<XElement> getIf = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "BeginIfCommand"));
            XNamespace ns = "urn:schemas-microsoft-com:xml-diffgram-v1";
            foreach(XElement cmd in getIf)
            {
                if ((string)cmd.Attribute("v_IfActionType") == "Web Element Exists")
                {
                    XElement tableParams = cmd.Element("v_IfActionParameterTable").Element(ns + "diffgram").Element("DocumentElement");
                    var elems = tableParams.Elements();
                    foreach(XElement elem in elems)
                    {
                        if (elem.Element("Parameter_x0020_Name").Value == "Selenium Instance Name")
                        {
                            elem.Element("Parameter_x0020_Name").Value = "WebBrowser Instance Name";
                            break;
                        }
                    }
                }
            }

            // BeginLoop Selenium -> WebBrowser
            IEnumerable<XElement> getLoop = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "BeginLoopCommand"));
            foreach (XElement cmd in getLoop)
            {
                if ((string)cmd.Attribute("v_LoopActionType") == "Web Element Exists")
                {
                    XElement tableParams = cmd.Element("v_LoopActionParameterTable").Element(ns + "diffgram").Element("DocumentElement");
                    var elems = tableParams.Elements();
                    foreach (XElement elem in elems)
                    {
                        if (elem.Element("Parameter_x0020_Name").Value == "Selenium Instance Name")
                        {
                            elem.Element("Parameter_x0020_Name").Value = "WebBrowser Instance Name";
                            break;
                        }
                    }
                }
            }

            return doc;
        }
        private static XDocument convertTo3_5_0_74(XDocument doc)
        {
            // BeginIf Value, Variable Compare -> Numeric Compare, Text Compare
            IEnumerable<XElement> getIf = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "BeginIfCommand"));
            foreach (XElement cmd in getIf)
            {
                switch ((string)cmd.Attribute("v_IfActionType"))
                {
                    case "Value":
                        cmd.Attribute("v_IfActionType").Value = "Numeric Compare";
                        break;

                    case "Variable Compare":
                        cmd.Attribute("v_IfActionType").Value = "Text Compare";
                        break;
                }
            }

            // BeginLoop Value, Variable Compare -> Numeric Compare, Text Compare
            IEnumerable<XElement> getLoop = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "BeginLoopCommand"));
            foreach (XElement cmd in getLoop)
            {
                switch ((string)cmd.Attribute("v_LoopActionType"))
                {
                    case "Value":
                        cmd.Attribute("v_LoopActionType").Value = "Numeric Compare";
                        break;

                    case "Variable Compare":
                        cmd.Attribute("v_LoopActionType").Value = "Text Compare";
                        break;
                }
            }

            return doc;
        }
        private static XDocument convertTo3_5_0_78(XDocument doc)
        {
            // BeginIf Window Search Method
            IEnumerable<XElement> getIf = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "BeginIfCommand"));
            XNamespace diffNs = "urn:schemas-microsoft-com:xml-diffgram-v1";
            XNamespace msNs = "urn:schemas-microsoft-com:xml-msdata";
            foreach (XElement cmd in getIf)
            {
                switch ((string)cmd.Attribute("v_IfActionType"))
                {
                    case "Window Name Exists":
                    case "Active Window Name Is":
                        XElement tableParams = cmd.Element("v_IfActionParameterTable").Element(diffNs + "diffgram").Element("DocumentElement");
                        var elems = tableParams.Elements();
                        bool isExists = false;
                        foreach (XElement elem in elems)
                        {
                            if (elem.Element("Parameter_x0020_Name").Value == "Search Method")
                            {
                                isExists = true;
                                break;
                            }
                        }
                        if (!isExists)
                        {
                            var elem = elems.ElementAt(0);
                            var newElem = new XElement(elem.Name);
                            newElem.Add(new XAttribute(diffNs + "id", elem.Name + (elems.Count() + 1).ToString()));    // diffgr:id
                            newElem.Add(new XAttribute(msNs + "rowOrder", elems.Count().ToString()));   // msdata:rowOrder

                            // name
                            var nameElem = new XElement("Parameter_x0020_Name");
                            nameElem.Value = "Search Method";

                            // value
                            var valueElem = new XElement("Parameter_x0020_Value");
                            valueElem.Value = "Contains";

                            newElem.Add(nameElem);
                            newElem.Add(valueElem);

                            tableParams.Add(newElem);
                        }
                        break;
                }
            }

            // BeginLoop Window Search Method
            IEnumerable<XElement> getLoop = doc.Descendants("ScriptCommand")
                .Where(el => ((string)el.Attribute("CommandName") == "BeginLoopCommand"));
            foreach (XElement cmd in getLoop)
            {
                switch ((string)cmd.Attribute("v_LoopActionType"))
                {
                    case "Window Name Exists":
                    case "Active Window Name Is":
                        XElement tableParams = cmd.Element("v_LoopActionParameterTable").Element(diffNs + "diffgram").Element("DocumentElement");
                        var elems = tableParams.Elements();
                        bool isExists = false;
                        foreach (XElement elem in elems)
                        {
                            if (elem.Element("Parameter_x0020_Name").Value == "Search Method")
                            {
                                isExists = true;
                                break;
                            }
                        }
                        if (!isExists)
                        {
                            var elem = elems.ElementAt(0);
                            var newElem = new XElement(elem.Name);
                            newElem.Add(new XAttribute(diffNs + "id", elem.Name + (elems.Count() + 1).ToString()));    // diffgr:id
                            newElem.Add(new XAttribute(msNs + "rowOrder", elems.Count().ToString()));   // msdata:rowOrder

                            // name
                            var nameElem = new XElement("Parameter_x0020_Name");
                            nameElem.Value = "Search Method";

                            // value
                            var valueElem = new XElement("Parameter_x0020_Value");
                            valueElem.Value = "Contains";

                            newElem.Add(nameElem);
                            newElem.Add(valueElem);

                            tableParams.Add(newElem);
                        }
                        break;
                }
            }

            return doc;
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

        public void ConvertToIntermediate(Core.EngineSettings settings, List<ScriptVariable> variables)
        {
            ScriptCommand.convertToIntermediate(settings, variables);
            if (AdditionalScriptCommands != null && AdditionalScriptCommands.Count > 0)
            {
                foreach (var cmd in AdditionalScriptCommands)
                {
                    cmd.ConvertToIntermediate(settings, variables);
                }
            }
        }

        public void ConvertToRaw(Core.EngineSettings settings)
        {
            ScriptCommand.convertToRaw(settings);
            if (AdditionalScriptCommands != null && AdditionalScriptCommands.Count > 0)
            {
                foreach (var cmd in AdditionalScriptCommands)
                {
                    cmd.ConvertToRaw(settings);
                }
            }
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

            if (VariableValue is DataTable)
            {
                DataTable dataTable = (DataTable)VariableValue;
                //switch (requiredProperty)
                //{
                //    case "rows":
                //    case "Rows":
                //    case "ROWS":
                //        return dataTable.Rows.ToString();
                //    case "cols":
                //    case "Cols":
                //    case "COLS":
                //    case "columns":
                //    case "Columns":
                //    case "COLUMNS":
                //        return dataTable.Columns.ToString();
                //    case "type":
                //    case "Type":
                //    case "TYPE":
                //        return "DATATABLE";
                //    case "index":
                //    case "Index":
                //    case "INDEX":
                //        return CurrentPosition.ToString();
                //    default:
                //        var dataRow = dataTable.Rows[CurrentPosition];
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(dataRow.ItemArray);
                //}
                return GetDisplayValue(dataTable, requiredProperty);
            }
            else if (VariableValue is Dictionary<string, string>)
            {
                Dictionary<string, string> trgDic = (Dictionary<string, string>)VariableValue;
                //switch (requiredProperty)
                //{
                //    case "count":
                //    case "Count":
                //    case "COUNT":
                //        return trgDic.Values.Count.ToString();
                //    case "type":
                //    case "Type":
                //    case "TYPE":
                //        return "DICTIONARY";
                //    case "index":
                //    case "Index":
                //    case "INDEX":
                //        return CurrentPosition.ToString();
                //    default:
                //        return (trgDic.Values.ToArray())[CurrentPosition];
                //}
                return GetDisplayValue(trgDic, requiredProperty);
            }
            else if (VariableValue is List<string>)
            {
                List<string> requiredValue = (List<string>)VariableValue;
                //switch (requiredProperty)
                //{
                //    case "count":
                //    case "Count":
                //    case "COUNT":
                //        return requiredValue.Count.ToString();
                //    case "index":
                //    case "Index":
                //    case "INDEX":
                //        return CurrentPosition.ToString();
                //    case "tojson":
                //    case "ToJson":
                //    case "toJson":
                //    case "TOJSON":
                //        return Newtonsoft.Json.JsonConvert.SerializeObject(requiredValue);
                //    case "topipe":
                //    case "ToPipe":
                //    case "toPipe":
                //    case "TOPIPE":
                //        return String.Join("|", requiredValue);
                //    case "first":
                //    case "First":
                //    case "FIRST":
                //        return requiredValue.FirstOrDefault();
                //    case "last":
                //    case "Last":
                //    case "LAST":
                //        return requiredValue.Last();
                //    case "type":
                //    case "Type":
                //    case "TYPE":
                //        return "LIST";
                //    default:
                //        return requiredValue[CurrentPosition];
                //}
                return GetDisplayValue(requiredValue, requiredProperty);
            }
            else if (VariableValue is System.Windows.Automation.AutomationElement)
            {
                System.Windows.Automation.AutomationElement elem = (System.Windows.Automation.AutomationElement)VariableValue;
                return GetDisplayValue(elem, requiredProperty);
            }
            else if (VariableValue is string)
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
            else
            {
                return "UNKNOWN";
            }
        }
        private string GetDisplayValue(DataTable myDT, string requiredProperty = "")
        {
            switch (requiredProperty)
            {
                case "rows":
                case "Rows":
                case "ROWS":
                    return myDT.Rows.ToString();
                case "cols":
                case "Cols":
                case "COLS":
                case "columns":
                case "Columns":
                case "COLUMNS":
                    return myDT.Columns.ToString();
                case "type":
                case "Type":
                case "TYPE":
                    return "DATATABLE";
                case "index":
                case "Index":
                case "INDEX":
                    return CurrentPosition.ToString();
                default:
                    if (requiredProperty == "")
                    {
                        var dataRow = myDT.Rows[CurrentPosition];
                        return Newtonsoft.Json.JsonConvert.SerializeObject(dataRow.ItemArray);
                    }
                    else
                    {
                        int idx = 0;
                        if (int.TryParse(requiredProperty, out idx))
                        {
                            return myDT.Rows[CurrentPosition][idx].ToString();

                        }
                        else
                        {
                            return myDT.Rows[CurrentPosition][requiredProperty].ToString();
                        }
                    }
            }
        }

        private string GetDisplayValue(Dictionary<string, string> myDic, string requiredProperty)
        {
            Dictionary<string, string> trgDic = (Dictionary<string, string>)VariableValue;
            switch (requiredProperty)
            {
                case "count":
                case "Count":
                case "COUNT":
                    return trgDic.Values.Count.ToString();
                case "type":
                case "Type":
                case "TYPE":
                    return "DICTIONARY";
                case "index":
                case "Index":
                case "INDEX":
                    return CurrentPosition.ToString();
                default:
                    if (requiredProperty == "")
                    {
                        return (trgDic.Values.ToArray())[CurrentPosition];
                    }
                    else
                    {
                        int idx;
                        if (int.TryParse(requiredProperty, out idx))
                        {
                            return (trgDic.Values.ToArray())[idx];
                        }
                        else
                        {
                            return trgDic[requiredProperty];
                        }
                    }
            }
        }

        private string GetDisplayValue(List<string> myList, string requiredProperty)
        {
            switch (requiredProperty)
            {
                case "count":
                case "Count":
                case "COUNT":
                    return myList.Count.ToString();
                case "index":
                case "Index":
                case "INDEX":
                    return CurrentPosition.ToString();
                case "tojson":
                case "ToJson":
                case "toJson":
                case "TOJSON":
                    return Newtonsoft.Json.JsonConvert.SerializeObject(myList);
                case "topipe":
                case "ToPipe":
                case "toPipe":
                case "TOPIPE":
                    return String.Join("|", myList);
                case "first":
                case "First":
                case "FIRST":
                    return myList.FirstOrDefault();
                case "last":
                case "Last":
                case "LAST":
                    return myList.Last();
                case "type":
                case "Type":
                case "TYPE":
                    return "LIST";
                default:
                    return myList[CurrentPosition];
            }
        }
        private string GetDisplayValue(System.Windows.Automation.AutomationElement element, string requiredProperty)
        {
            switch (requiredProperty)
            {
                case "type":
                case "Type":
                case "TYPE":
                    return "AUTOMATIONELEMENT";
                default:
                    return "Name: " + element.Current.Name + ", LocalizedControlType: " + element.Current.LocalizedControlType + ", ControlType: " + taskt.Core.Automation.Commands.AutomationElementControls.GetControlTypeText(element.Current.ControlType);
            }
        }
    }

    #endregion Script and Variables

    [Serializable]
    public class ScriptInformation
    {
        public string TasktVersion { get; set; }
        public string Author { get; set; }
        public DateTime LastRunTime { get; set; }
        public int RunTimes { get; set; }
        public string ScriptVersion { get; set; }
        public string Description { get; set; }
        public ScriptInformation()
        {
            this.TasktVersion = "";
            this.Author = "";
            this.LastRunTime = DateTime.Parse("1990-01-01T00:00:00");
            this.RunTimes = 0;
            this.ScriptVersion = "0.0.0";
            this.Description = "";
        }
    }
}
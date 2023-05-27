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
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using taskt.Core.Automation.Commands;

namespace taskt.Core.Script
{
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
        public ScriptAction AddNewParentCommand(ScriptCommand scriptCommand)
        {
            ScriptAction newExecutionCommand = new ScriptAction() { ScriptCommand = scriptCommand };
            Commands.Add(newExecutionCommand);
            return newExecutionCommand;
        }

        /// <summary>
        /// Converts and serializes the user-defined commands into an XML file  
        /// </summary>
        public static Script SerializeScript(ListView.ListViewItemCollection scriptCommands, List<ScriptVariable> scriptVariables, ScriptInformation info, EngineSettings engineSettings, XmlSerializer serializer = null, string scriptFilePath = "")
        {
            var script = new Script
            {
                //save variables to file
                Variables = scriptVariables,
                Info = info
            };

            //save listview tags to command list

            int lineNumber = 1;

            List<ScriptAction> subCommands = new List<ScriptAction>();

            foreach (ListViewItem commandItem in scriptCommands)
            {
                var srcCommand = (ScriptCommand)commandItem.Tag;
                srcCommand.IsDontSavedCommand = false;

                var command = srcCommand.Clone();
                command.LineNumber = lineNumber;

                if ((command is BeginNumberOfTimesLoopCommand) || (command is BeginContinousLoopCommand) || (command is BeginListLoopCommand) || (command is BeginIfCommand) || (command is BeginMultiIfCommand) || (command is TryCommand) || (command is BeginLoopCommand) || (command is BeginMultiLoopCommand))
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
                else if ((command is EndLoopCommand) || (command is EndIfCommand) || (command is EndTryCommand))  //if current loop scenario is ending
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
            //XmlSerializer serializer = new XmlSerializer(typeof(Script));
            if (serializer == null)
            {
                serializer = CreateSerializer();
            }
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
        public static Script DeserializeFile(string scriptFilePath, EngineSettings engineSettings, XmlSerializer serializer = null)
        {
            XDocument xmlScript = XDocument.Load(scriptFilePath);

            // pre-convert
            convertOldScript(xmlScript);

            using (var reader = xmlScript.Root.CreateReader())
            {
                //XmlSerializer serializer = new XmlSerializer(typeof(Script));
                if (serializer == null)
                {
                    serializer = CreateSerializer();
                }
                Script des = (Script)serializer.Deserialize(reader);

                foreach (var cmd in des.Commands)
                {
                    cmd.ConvertToRaw(engineSettings);
                }

                return des;
            }
        }

        /// <summary>
        /// Deserializes an XML string into user-defined commands (server sends a string to the client)
        /// </summary>
        public static Script DeserializeXML(string scriptXML, XmlSerializer serializer = null)
        {
            try
            {
                using (System.IO.StringReader reader = new System.IO.StringReader(scriptXML))
                {
                    //XmlSerializer serializer = new XmlSerializer(typeof(Script));
                    if (serializer == null)
                    {
                        serializer = CreateSerializer();
                    }
                    Script deserializedData = (Script)serializer.Deserialize(reader);
                    return deserializedData;
                }
            }
            catch
            {
                return null;
            }
        }

        public static string SerializeScript(List<ScriptCommand> commands, XmlSerializer serializer)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(Script));
            if (serializer == null)
            {
                serializer = CreateSerializer();
            }

            var actions = new Script();
            foreach (ScriptCommand cmd in commands)
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

        public static Script DeserializeScript(string scriptXML, XmlSerializer serializer = null)
        {
            using (var reader = new System.IO.StringReader(scriptXML))
            {
                //XmlSerializer serializer = new XmlSerializer(typeof(Script));
                if (serializer == null)
                {
                    serializer = CreateSerializer();
                }
                var ret = (Script)serializer.Deserialize(reader);
                return ret;
            }
        }

        /// <summary>
        /// Create Script XML Serializer
        /// </summary>
        /// <returns></returns>
        public static XmlSerializer CreateSerializer()
        {
            var subClasses = System.Reflection.Assembly.GetAssembly(typeof(ScriptCommand))
                                .GetTypes()
                                .Where(x => x.IsSubclassOf(typeof(ScriptCommand)) && !x.IsAbstract)
                                .ToArray();

            return new XmlSerializer(typeof(Script), subClasses);
        }

        /// <summary>
        /// script xml converter
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static XDocument convertOldScript(XDocument doc)
        {
            // very important!
            // ** DO NOT USE nameof to change command name **
            convertTo3_5_0_45(doc);
            convertTo3_5_0_46(doc);
            convertTo3_5_0_47(doc);
            convertTo3_5_0_50(doc);
            convertTo3_5_0_51(doc);
            convertTo3_5_0_52(doc);
            convertTo3_5_0_57(doc);
            convertTo3_5_0_67(doc);
            fixUIAutomationCommandEnableParameterValue(doc);
            convertTo3_5_0_73(doc);
            convertTo3_5_0_74(doc);
            convertTo3_5_0_78(doc);
            fixUIAutomationGroupEnableParameterValue(doc);
            convertTo3_5_0_83(doc);
            convertTo3_5_1_16(doc);
            convertTo3_5_1_30(doc);
            convertTo3_5_1_31(doc);
            convertTo3_5_1_33(doc);
            convertTo3_5_1_34(doc);
            convertTo3_5_1_35(doc);
            convertTo3_5_1_36(doc);
            convertTo3_5_1_38(doc);
            convertTo3_5_1_39(doc);
            fixUIAutomationGroupEnableParameterValue_3_5_1_39(doc);
            convertTo3_5_1_40(doc);
            convertTo3_5_1_41(doc);
            convertTo3_5_1_42(doc);
            convertTo3_5_1_44(doc);
            convertTo3_5_1_45(doc);
            convertTo3_5_1_46(doc);

            return doc;
        }

        private static XDocument convertTo3_5_0_45(XDocument doc)
        {
            // change "Start with" -> "Starts with", "End with" -> "Ends with"
            ChangeAttributeValue(doc, new Func<XElement, bool>( el => {
                switch (el.Attribute("CommandName").Value)
                {
                    case "ActivateWindowCommand":
                    case "CheckWindowNameExistsCommand":
                    case "CloseWindowCommand":
                    case "GetWindowNamesCommand":
                    case "GetWindowPositionCommand":
                    case "GetWindowStateCommand":
                    case "MoveWindowCommand":
                    case "ResizeWindowCommand":
                    case "SetWindowStateCommand":
                    case "WaitForWindowCommand":
                    case "SendAdvancedKeyStrokesCommand":
                    case "SendHotkeyCommand":
                    case "SendKeysCommand":
                    case "UIAutomationCommand":
                        return true;
                    default:
                        return false;
                }
            }), "v_SearchMethod", new Action<XAttribute>( attr => {
                switch (attr?.Value.ToLower() ?? "")
                {
                    case "start with":
                        attr.Value = "Starts With";
                        break;
                    case "end with":
                        attr.Value = "Ends With";
                        break;
                }
            }));

            // ExcelCreateDataset -> LoadDataTable
            ChangeCommandName(doc, "ExcelCreateDatasetCommand", "LoadDataTableCommand", "Load DataTable");

            return doc;
        }

        private static XDocument convertTo3_5_0_46(XDocument doc)
        {
            // AddToVariable -> AddListItem
            ChangeCommandName(doc, "AddToVariableCommand", "AddListItemCommand", "Add List Item");

            // SetVariableIndex -> SetListIndex
            // stop v3.5.1.40
            //ChangeCommandName(doc, "SetVariableIndexCommand", "SetListIndexCommand", "Set List Index");

            return doc;
        }
        private static XDocument convertTo3_5_0_47(XDocument doc)
        {
            // AddListItem.v_userVariableName, SetListIndex.v_userVariableName -> *.v_ListName
            ChangeAttributeName(doc, new Func<XElement, bool>(el =>
            {
                switch (el.Attribute("CommandName").Value)
                {
                    case "AddListItemCommand":
                    case "SetListIndexCommand":
                        return true;
                    default:
                        return false;
                }
            }), "v_userVariableName", "v_ListName");

            return doc;
        }
        private static XDocument convertTo3_5_0_50(XDocument doc)
        {
            // ParseJSONArray -> ConvertJSONToList
            ChangeCommandName(doc, "ParseJSONArrayCommand", "ConvertJSONToListCommand", "Convert JSON To List");
            return doc;
        }
        private static XDocument convertTo3_5_0_51(XDocument doc)
        {
            // GetDataCountRowCommand -> GetDataTableRowCountCommand
            ChangeCommandName(doc, "GetDataRowCountCommand", "GetDataTableRowCountCommand", "Get DataTable Row Count");

            // AddDataRow -> AddDataTableRow
            ChangeCommandName(doc, "AddDataRowCommand", "AddDataTableRowCommand", "Add DataTable Row");

            return doc;
        }
        private static XDocument convertTo3_5_0_52(XDocument doc)
        {
            // change "Start with" -> "Starts with", "End with" -> "Ends with"
            ChangeAttributeValue(doc, new Func<XElement, bool>(el =>
            {
                switch(el.Attribute("CommandName").Value)
                {
                    case "GetFilesCommand":
                    case "GetFoldersCommand":
                    case "CheckStringCommand":
                        return true;
                    default:
                        return false;
                }
            }), "v_SearchMethod", new Action<XAttribute>(attr => {
                switch (attr?.Value.ToLower() ?? "")
                {
                    case "start with":
                        attr.Value = "Starts With";
                        break;
                    case "end with":
                        attr.Value = "Ends With";
                        break;
                }
            }));
            return doc;
        }
        private static XDocument convertTo3_5_0_57(XDocument doc)
        {
            // StringCheckTextCommand -> CheckTextCommand
            ChangeCommandName(doc, "CheckStringCommand", "CheckTextCommand", "Check Text");

            // ModifyVariableCommand -> ModifyTextCommand
            ChangeCommandName(doc, new Func<XElement, bool>( el => 
            {
                switch (el.Attribute("CommandName").Value)
                {
                    case "ModifyVariableCommand":
                    case "StringCaseCommand":
                        return true;
                    default:
                        return false;
                }
            }), "ModifyTextCommand", "Modify Text");

            // RegExExtractorCommand -> RegExExtractionText
            ChangeCommandName(doc, "RegExExtractorCommand", "RegExExtractionTextCommand", "RegEx Extraction Text");

            // StringReplaceCommand -> ReplaceTextCommand
            ChangeCommandName(doc, "StringReplaceCommand", "ReplaceTextCommand", "Replace Text");

            // StringSplitCommand -> SplitTextCommand
            ChangeCommandName(doc, "StringSplitCommand", "SplitTextCommand", "Split Text");

            // StringSubstringCommand -> SubstringTextCommand
            ChangeCommandName(doc, "StringSubstringCommand", "SubstringTextCommand", "Substring Text");

            // TextExtractorCommand -> ExtractionTextCommand
            ChangeCommandName(doc, "TextExtractorCommand", "ExtractionTextCommand", "Extraction Text");

            return doc;
        }

        private static XDocument convertTo3_5_0_67(XDocument doc)
        {
            // GetAElement -> GetAnElement
            ChangeCommandName(doc, "SeleniumBrowserGetAElementValuesAsListCommand", "SeleniumBrowserGetAnElementValuesAsListCommand", "Get An Element Values As List");

            ChangeCommandName(doc, "SeleniumBrowserGetAElementValuesAsDictionaryCommand", "SeleniumBrowserGetAnElementValuesAsDictionaryCommand", "Get An Element Values As Dictionary");

            ChangeCommandName(doc, "SeleniumBrowserGetAElementValuesAsDataTableCommand", "SeleniumBrowserGetAnElementValuesAsDataTableCommand", "Get An Element Values As DataTable");

            return doc;
        }

        private static XDocument fixUIAutomationCommandEnableParameterValue(XDocument doc)
        {
            // UI Automation Boolean Fix
            ChangeTableCellValue(doc, "UIAutomationCommand", "v_UIASearchParameters", "Enabled", new Action<XElement>(c =>
            {
                switch(c.Value.ToLower() ?? "")
                {
                    case "true":
                    case "false":
                        break;
                    default:
                        c.SetValue("False");
                        break;
                }
            }));

            return doc;
        }

        private static XDocument convertTo3_5_0_73(XDocument doc)
        {
            var changeFunc = new Action<XElement>(c =>
            {
                if (c.Value == "Selenium Instance Name")
                {
                    c.SetValue("WebBrowser Instance Name");
                }
            });

            // BeginIf Selenium -> WebBrowser
            ChangeTableCellValue(doc, new Func<XElement, bool>(el =>
            {
                return (
                        (el.Attribute("CommandName").Value == "BeginIfCommand") &&
                        ((el.Attribute("v_IfActionType")?.Value ?? "") == "Web Element Exists")
                    );
            }), "v_IfActionParameterTable", "Parameter_x0020_Name", changeFunc);

            // BeginLoop Selenium -> WebBrowser
            ChangeTableCellValue(doc, new Func<XElement, bool>(el =>
            {
                return (
                    (el.Attribute("CommandName").Value == "BeginLoopCommand") &&
                    ((el.Attribute("v_LoopActionType")?.Value ?? "") == "Web Element Exists")
                );
            }), "v_LoopActionParameterTable", "Parameter_x0020_Name", changeFunc);

            return doc;
        }

        private static XDocument convertTo3_5_0_74(XDocument doc)
        {
            var changeFunc = new Action<XAttribute>(attr =>
            {
                switch (attr?.Value.ToLower() ?? "")
                {
                    case "value":
                        attr.Value = "Numeric Compare";
                        break;
                    case "variable compare":
                        attr.Value = "Text Compare";
                        break;
                }
            });

            // BeginIf Value, Variable Compare -> Numeric Compare, Text Compare
            ChangeAttributeValue(doc, "BeginIfCommand", "v_IfActionType", changeFunc);

            // BeginLoop Value, Variable Compare -> Numeric Compare, Text Compare
            ChangeAttributeValue(doc, "BeginLoopCommand", "v_LoopActionType", changeFunc);

            return doc;
        }
        private static XDocument convertTo3_5_0_78(XDocument doc)
        {
            var modFunc = new Action<XElement, List<XElement>>((table, rows) =>
            {
                var isExists = rows.Any(r => r.Element("Parameter_x0020_Name").Value == "Search Method");
                if (!isExists)
                {
                    AddTableRow(table, rows, new Dictionary<string, string>
                    {
                        { "Parameter_x0020_Name", "Search Method" },
                        { "Parameter_x0020_Value", "Contains" },
                    });
                }
            });

            // BeginIf add Window Search Method cell parameter
            ModifyTable(doc, new Func<XElement, bool>(el =>
            {
                if (el.Attribute("CommandName").Value == "BeginIfCommand")
                {
                    switch (el.Attribute("v_IfActionType")?.Value.ToLower() ?? "")
                    {
                        case "window name exists":
                        case "active window name is":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }), "v_IfActionParameterTable", modFunc);

            // BeginLoop add Window Search Method parameter
            ModifyTable(doc, new Func<XElement, bool>(el =>
            {
                if (el.Attribute("CommandName").Value == "BeginLoopCommand")
                {
                    switch (el.Attribute("v_LoopActionType")?.Value.ToLower() ?? "")
                    {
                        case "window name exists":
                        case "active window name is":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }), "v_LoopActionParameterTable", modFunc);


            return doc;
        }
        private static XDocument fixUIAutomationGroupEnableParameterValue(XDocument doc)
        {
            // UI Automation Boolean Fix
            ChangeTableCellValue(doc, new Func<XElement, bool>(el =>
            {
                switch (el.Attribute("CommandName").Value)
                {
                    case "UIAutomationGetChidrenElementsInformationCommand":
                    case "UIAutomationGetChildElementCommand":
                    case "UIAutomationGetElementFromElementCommand":
                        return true;
                    default:
                        return false;
                }
            }), "v_SearchParameters", "Enabled", new Action<XElement>(c =>
            {
                switch (c.Value.ToLower())
                {
                    case "true":
                    case "false":
                        break;
                    default:
                        c.SetValue("False");
                        break;
                }
            }));

            return doc;
        }

        private static XDocument convertTo3_5_0_83(XDocument doc)
        {
            // SMTPSendEmail -> MailKitSendEmail
            ChangeCommandName(doc, "SMTPCommand", "MailKitSendEmailCommand", "Send Email");

            return doc;
        }

        private static XDocument convertTo3_5_1_16(XDocument doc)
        {
            // Parse Json -> Get JSON Value List
            ChangeCommandName(doc, "ParseJsonCommand", "GetJSONValueListCommand", "Get JSON Value List");

            // Parse Json Model -> Get Multi JSON Value List
            ChangeCommandName(doc, "ParseJsonModelCommand", "GetMultiJSONValueListCommand", "Get Multi JSON Value List");

            return doc;
        }

        private static XDocument convertTo3_5_1_30(XDocument doc)
        {
            // AddDataTableRowByDataTableCommand -> AddDataTableRowsByDataTableCommand
            ChangeCommandName(doc, "AddDataTableRowByDataTableCommand", "AddDataTableRowsByDataTableCommand", "Add DataTable Rows By DataTable");

            // SetDataTableColumnByDataTableCommand -> SetDataTableColumnValuesByDataTableCommand
            ChangeCommandName(doc, "SetDataTableColumnByDataTableCommand", "SetDataTableColumnValuesByDataTableCommand", "Set DataTable Column Values By DataTable");

            // SetDataTableColumnByListCommand -> SetDataTableColumnValuesByListCommand
            ChangeCommandName(doc, "SetDataTableColumnByListCommand", "SetDataTableColumnValuesByListCommand", "Set DataTable Column Values By List");

            // AddDictionaryCommand -> AddDictionaryItemCommand
            ChangeCommandName(doc, "AddDictionaryCommand", "AddDictionaryItemCommand", "Add Dictionary Item");

            // ConvertDictionaryToDataTableCommand (fix typo)
            ChangeCommandName(doc, "ConvertDictionaryToDataTableCommand", "ConvertDictionaryToDataTableCommand", "Convert Dictionary To DataTable");

            // GetDictionaryValueCommand (fix Display Command Name)
            ChangeCommandName(doc, "GetDictionaryValueCommand", "GetDictionaryValueCommand", "Get Dictionary Value");

            // MailKitGetEMailFromMailListCommand -> MailKitGetEMailFromEMailListCommand
            ChangeCommandName(doc, "MailKitGetEMailFromMailListCommand", "MailKitGetEMailFromEMailListCommand", "Get EMail From EMailList");

            return doc;
        }

        private static XDocument convertTo3_5_1_31(XDocument doc)
        {
            // AddJSONArrayItem -> AddJSONArrayItemCommand
            ChangeCommandName(doc, "AddJSONArrayItem", "AddJSONArrayItemCommand", "Add JSON Array Item");

            // AddJSONObjectProperty -> AddJSONObjectPropertyCommand
            ChangeCommandName(doc, "AddJSONObjectProperty", "AddJSONObjectPropertyCommand", "Add JSON Object Property");

            // CreateJSONVariable -> CreateJSONVariableCommand
            ChangeCommandName(doc, "CreateJSONVariable", "CreateJSONVariableCommand", "Create JSON Variable");

            // InsertJSONArrayItem -> InsertJSONArrayItemCommand
            ChangeCommandName(doc, "InsertJSONArrayItem", "InsertJSONArrayItemCommand", "Insert JSON Array Item");

            // InsertJSONObjectProperty -> InsertJSONObjectPropertyCommand
            ChangeCommandName(doc, "InsertJSONObjectProperty", "InsertJSONObjectPropertyCommand", "Insert JSON Object Property");

            // RemoveJSONArrayItem -> RemoveJSONArrayItemCommand
            ChangeCommandName(doc, "RemoveJSONArrayItem", "RemoveJSONArrayItemCommand", "Remove JSON Array Item");

            // Remove JSON Property -> RemoveJSONPropertyCommand
            ChangeCommandName(doc, "Remove JSON Property", "RemoveJSONPropertyCommand", "Remove JSON Property");

            // SetJSONValue -> SetJSONValueCommand
            ChangeCommandName(doc, "SetJSONValue", "SetJSONValueCommand", "Set JSON Value");

            // CheckWordInstanceExistsCommand -> WordCheckWordInstanceExistsCommand
            ChangeCommandName(doc, "CheckWordInstanceExistsCommand", "WordCheckWordInstanceExistsCommand", "Check Word Instance Exists");

            // WordSaveAsCommand -> WordSaveDocumentAs
            ChangeCommandName(doc, "WordSaveAsCommand", "WordSaveDocumentAsCommand", "Save Document As");

            // WordSaveCommand -> WordSaveDocumentCommand
            ChangeCommandName(doc, "WordSaveCommand", "WordSaveDocumentCommand", "Save Document");

            // WebBrowserNavigateCommand -> SeleniumBrowserNavigateForwardCommand
            ChangeCommandName(doc, "WebBrowserNavigateCommand", "SeleniumBrowserNavigateForwardCommand", "Navigate Forward");

            // SeleniumBrowserResizeBrowser -> SeleniumBrowserResizeBrowserCommand
            ChangeCommandName(doc, "SeleniumBrowserResizeBrowserCommand", "SeleniumBrowserResizeBrowserCommand", "Resize Browser");

            // CheckExcelInstanceExistsCommand -> ExcelCheckExcelInstanceExistsCommand
            ChangeCommandName(doc, "CheckExcelInstanceExistsCommand", "ExcelCheckExcelInstanceExistsCommand", "Check Excel Instance Exists");

            // ExcelWorksheetInfoCommand -> ExcelGetWorksheetInfoCommand
            ChangeCommandName(doc, "ExcelWorksheetInfoCommand", "ExcelGetWorksheetInfoCommand", "Get Worksheet Info");

            return doc;
        }

        private static XDocument convertTo3_5_1_33(XDocument doc)
        {
            // AddVariableCommand -> NewVariableCommand
            ChangeCommandName(doc, "AddVariableCommand", "NewVariableCommand", "New Variable");

            // VariableCommand -> SetVariableValueCommand
            ChangeCommandName(doc, "VariableCommand", "SetVariableValueCommand", "Set Variable Value");

            return doc;
        }

        private static XDocument convertTo3_5_1_34(XDocument doc)
        {
            // GetLengthCommand -> GetWordLengthCommand
            ChangeCommandName(doc, "GetLengthCommand", "GetWordLengthCommand", "Get Word Length");

            // CreateNumberVariableCommand -> CreateNumericalVariableCommand
            ChangeCommandName(doc, "CreateNumberVariableCommand", "CreateNumericalVariableCommand", "Create Numerical Variable");

            return doc;
        }

        private static XDocument convertTo3_5_1_35(XDocument doc)
        {
            // EnvironmentVariableCommand -> GetEnvironmentVariableCommand
            ChangeCommandName(doc, "EnvironmentVariableCommand", "GetEnvironmentVariableCommand", "Get Environment Variable");

            // OSVariableCommand -> GetOSVariableCommand
            ChangeCommandName(doc, "OSVariableCommand", "GetOSVariableCommand", "Get OS Variable");

            // RemoteDesktopCommand -> LaunchRemoteDesktopCommand
            ChangeCommandName(doc, "RemoteDesktopCommand", "LaunchRemoteDesktopCommand", "Launch Remote Desktop");

            // LoadTaskCommand -> LoadScriptFileCommand
            ChangeCommandName(doc, "LoadTaskCommand", "LoadScriptFileCommand", "Load Script File");

            // RunTaskCommand -> RunScriptFileCommand
            ChangeCommandName(doc, "RunTaskCommand", "RunScriptFileCommand", "Run Script File");
            // RunScriptFileCommand.v_AssingVariables
            ChangeAttributeValue(doc, "RunScriptFileCommand", "v_AssignVariables", new Action<XAttribute>(attr =>
            {
                switch (attr?.Value.ToLower() ?? "")
                {
                    case "true":
                        attr.Value = "Yes";
                        break;

                    default:
                        attr.Value = "No";
                        break;
                }
            }));

            // StopTaskCommand -> StopCurrentScriptFileCommand
            ChangeCommandName(doc, "StopTaskCommand", "StopCurrentScriptFileCommand", "Stop Current Script File");

            // UnloadTaskCommand -> UnloadScriptFileCommand
            ChangeCommandName(doc, "UnloadTaskCommand", "UnloadScriptFileCommand", "Unload Script File");

            return doc;
        }

        private static XDocument convertTo3_5_1_36(XDocument doc)
        {
            // RunCustomCodeCommand -> RunCSharpCodeCommand
            ChangeCommandName(doc, "RunCustomCodeCommand", "RunCSharpCodeCommand", "Run CSharp Code");

            // RunPowershellCommand -> RunPowerShellScriptFileCommand
            ChangeCommandName(doc, "RunPowershellCommand", "RunPowerShellScriptFileCommand", "Run PowerShell Script File");

            // RunScriptCommand -> RunBatchScriptFileCommand
            ChangeCommandName(doc, "RunScriptCommand", "RunBatchScriptFileCommand", "Run Batch Script File");

            // StartProcessCommand -> StartApplicationCommand
            ChangeCommandName(doc, "StartProcessCommand", "StartApplicationCommand", "Start Application");

            // StopProgramCommand -> StopApplicationCommand
            ChangeCommandName(doc, "StopProgramCommand", "StopApplicationCommand", "Stop Application");

            // ClipboardClearTextCommand -> ClearClipboardTextCommand
            ChangeCommandName(doc, "ClipboardClearTextCommand", "ClearClipboardTextCommand", "Clear Clipboard Text");

            // ClipboardCommand -> GetClipboardTextCommand
            ChangeCommandName(doc, "ClipboardCommand", "GetClipboardTextCommand", "Get Clipboard Text");

            // ClipboardSetTextCommand -> SetClipboardTextCommand
            ChangeCommandName(doc, "ClipboardSetTextCommand", "SetClipboardTextCommand", "Set Clipboard Text");

            // CommentCommand (Display text only)
            ChangeCommandName(doc, "CommentCommand", "CommentCommand", "Comment");

            // EncryptionCommand -> EncryptTextCommand
            ChangeCommandName(doc, "EncryptionCommand", "EncryptDecryptTextCommand", "Encrypt Decrypt Text");

            // MessageBoxCommand -> ShowMessgeCommand
            ChangeCommandName(doc, "MessageBoxCommand", "ShowMessageCommand", "Show Message");

            // PingCommand (Display text only)
            ChangeCommandName(doc, "PingCommand", "PingCommand", "Ping");

            // SequenceCommand (Display text only)
            ChangeCommandName(doc, "SequenceCommand", "SequenceCommand", "Sequence");

            return doc;
        }

        private static XDocument convertTo3_5_1_38(XDocument doc)
        {
            // GetDataCommand -> GetBotStoreDataCommand
            ChangeCommandName(doc, "GetDataCommand", "GetBotStoreDataCommand", "Get BotStore Data");

            // PauseCommand -> PauseScriptCommand
            ChangeCommandName(doc, "PauseCommand", "PauseScriptCommand", "Pause Script");

            // UploadDataCommand -> UploadBotStoreDataCommand
            ChangeCommandName(doc, "UploadDataCommand", "UploadBotStoreDataCommand", "Upload BotStore Data");

            // Format File PathCommand -> FormatFilePathCommand
            ChangeCommandName(doc, "Format File PathCommand", "FormatFilePathCommand", "Format File Path");

            // ExtractFileCommand -> ExtractZipFileCommand
            ChangeCommandName(doc, "ExtractFileCommand", "ExtractZipFileCommand", "Extract Zip File");

            return doc;
        }

        private static XDocument convertTo3_5_1_39(XDocument doc)
        {
            // Format Folder PathCommand -> FormatFolderPathCommand
            ChangeCommandName(doc, "Format Folder PathCommand", "FormatFolderPathCommand", "Format Folder Path");

            return doc;
        }

        private static XDocument fixUIAutomationGroupEnableParameterValue_3_5_1_39(XDocument doc)
        {
            ChangeTableCellValue(doc, new Func<XElement, bool>(el =>
            {
                switch (el.Attribute("CommandName").Value)
                {
                    case "UIAutomationCheckElementExistCommand":
                    case "UIAutomationWaitForElementExistCommand":
                        return true;
                    default:
                        return false;
                }
            }), "v_SearchParameters", "Enabled", new Action<XElement>(c =>
            {
                switch (c.Value.ToLower())
                {
                    case "true":
                    case "false":
                        break;
                    default:
                        c.SetValue("False");
                        break;
                }
            }));
            return doc;
        }

        private static XDocument convertTo3_5_1_40(XDocument doc)
        {
            // OCRCommand -> ExecuteOCR
            ChangeCommandName(doc, "OCRCommand", "ExecuteOCRCommand", "Execute OCR");

            // ScreenShotCommand -> TakeScreenshotCommand
            ChangeCommandName(doc, "ScreenshotCommand", "TakeScreenshotCommand", "Take Screenshot");

            // FileDialogCommand -> ShowFileDialogCommand
            ChangeCommandName(doc, "FileDialogCommand", "ShowFileDialogCommand", "Show File Dialog");

            // FolderDialogCommand -> ShowFolderDialogCommand
            ChangeCommandName(doc, "FolderDialogCommand", "ShowFolderDialogCommand", "Show Folder Dialog");

            // HTMLInputCommand -> ShowHTMLInputDialogCommand
            ChangeCommandName(doc, "HTMLInputCommand", "ShowHTMLInputDialogCommand", "Show HTML Input Dialog");

            // SendHotkeyCommand -> EnterShortcutKeyCommand
            ChangeCommandName(doc, "SendHotkeyCommand", "EnterShortcutKeyCommand", "Enter Shortcut Key");

            // SendKeysCommand -> EnterKeysCommand
            ChangeCommandName(doc, "SendKeysCommand", "EnterKeysCommand", "Enter Keys");

            // SendMouseClickCommand -> ClickMouseCommand
            ChangeCommandName(doc, "SendMouseClickCommand", "ClickMouseCommand", "Click Mouse");

            // SendMouseMoveCommand -> MoveMouseCommand
            ChangeCommandName(doc, "SendMouseMoveCommand", "MoveMouseCommand", "Move Mouse");

            return doc;
        }

        private static XDocument convertTo3_5_1_41(XDocument doc)
        {
            // UserInputCommand -> ShowUserInputDialogCommand
            ChangeCommandName(doc, "UserInputCommand", "ShowUserInputDialogCommand", "Show User Input Dialog");

            // UIAutomationGetChildElementCommand -> UIAutomationSearchChildElementCommand
            ChangeCommandName(doc, "UIAutomationGetChildElementCommand", "UIAutomationSearchChildElementCommand", "Search Child Element");

            // UIAutomationGetElementFromElementByXPathCommand -> UIAutomationSearchElementFromElementByXPathCommand
            ChangeCommandName(doc, "UIAutomationGetElementFromElementByXPathCommand", "UIAutomationSearchElementFromElementByXPathCommand", "Search Element From Element By XPath");

            // UIAutomationGetElementFromElementCommand -> UIAutomationSearchElementFromElementCommand
            ChangeCommandName(doc, "UIAutomationGetElementFromElementCommand", "UIAutomationSearchElementFromElementCommand", "Search Element From Element");

            // UIAutomationGetElementFromTableElementCommand -> UIAutomationSearchElementFromTableElementCommand
            ChangeCommandName(doc, "UIAutomationGetElementFromTableElementCommand", "UIAutomationSearchElementFromTableElementCommand", "Search Element From Table Element");

            // UIAutomationGetElementFromWindowByXPathCommand -> UIAutomationSearchElementFromWindowByXPathCommand
            ChangeCommandName(doc, "UIAutomationGetElementFromWindowByXPathCommand", "UIAutomationSearchElementFromWindowByXPathCommand", "Search Element From Window By XPath");

            // UIAutomationGetElementFromWindowCommand -> UIAutomationSearchElementFromWindowCommand
            ChangeCommandName(doc, "UIAutomationGetElementFromWindowCommand", "UIAutomationSearchElementFromWindowCommand", "Search Element From Window");

            // UIAutomationGetParentElementCommand -> UIAutomationSearchParentElementCommand
            ChangeCommandName(doc, "UIAutomationGetParentElementCommand", "UIAutomationSearchParentElementCommand", "Search Parent Element");

            // NLGCreateInstanceCommand -> NLGCreateNLGInstanceCommand
            ChangeCommandName(doc, "NLGCreateInstanceCommand", "NLGCreateNLGInstanceCommand", "Create NLG Instance");

            // NLGGeneratePhraseCommand -> NLGGenerateNLGPhraseCommand
            ChangeCommandName(doc, "NLGGeneratePhraseCommand", "NLGGenerateNLGPhraseCommand", "Generate NLG Phrase");

            // NLGSetParameterCommand -> NLGSetNLGParameterCommand
            ChangeCommandName(doc, "NLGSetParameterCommand", "NLGSetNLGParameterCommand", "Set NLG Parameter");

            return doc;
        }

        private static XDocument convertTo3_5_1_42(XDocument doc)
        {
            // HTTPRequestQueryCommand -> HTMLGetHTMLTextByXPath
            ChangeCommandName(doc, "HTTPRequestQueryCommand", "HTMLGetHTMLTextByXPath", "Get HTML Text By XPath");

            // HTTPRequestCommand -> HTTPSendHTTPRequestCommand
            ChangeCommandName(doc, "HTTPRequestCommand", "HTTPSendHTTPRequestCommand", "Send HTTP Request");

            // RESTCommand -> HTTPExecuteRESTAPICommand
            ChangeCommandName(doc, "RESTCommand", "HTTPExecuteRESTAPICommand", "Execute REST API");

            return doc;
        }

        private static XDocument convertTo3_5_1_44(XDocument doc)
        {
            // Move/Copy File command -> Move File, Copy File command
            var commands = doc.Descendants("ScriptCommand").Where(el => {
                return (el.Attribute("CommandName").Value == "MoveFileCommand") &&
                    (el.Attribute("v_OperationType") != null);
            });
            var moveCommands = commands.Where(el => (el.Attribute("v_OperationType").Value.ToLower() != "copy file")).ToList();
            var copyCommands = commands.Where(el => (el.Attribute("v_OperationType").Value.ToLower() == "copy file")).ToList();
            ChangeCommandName(moveCommands, "MoveFileCommand", "Move File");
            ChangeCommandName(copyCommands, "CopyFileCommand", "Copy File");
            foreach(var cmd in commands)
            {
                cmd.Attribute("v_OperationType").Remove();
            }

            // reset (release)
            commands = null;
            moveCommands = null;
            copyCommands = null;

            // Move/Copy Folder command -> Move Folder, Copy Folder command
            commands = doc.Descendants("ScriptCommand").Where(el => {
                return (el.Attribute("CommandName").Value == "MoveFolderCommand") &&
                    (el.Attribute("v_OperationType") != null);
            });
            moveCommands = commands.Where(el => (el.Attribute("v_OperationType").Value.ToLower() != "copy folder")).ToList();
            copyCommands = commands.Where(el => (el.Attribute("v_OperationType").Value.ToLower() == "copy folder")).ToList();
            ChangeCommandName(moveCommands, "MoveFolderCommand", "Move Folder");
            ChangeCommandName(copyCommands, "CopyFolderCommand", "Copy Folder");
            foreach (var cmd in commands)
            {
                cmd.Attribute("v_OperationType").Remove();
            }

            // UIAutomationSearchElementFromWindowByXPathCommand -> UIAutomationSearchElementAndWindowByXPathCommand
            ChangeCommandName(doc, "UIAutomationSearchElementFromWindowByXPathCommand", "UIAutomationSearchElementAndWindowByXPathCommand", "Search Element And Window By XPath");

            return doc;
        }

        private static XDocument convertTo3_5_1_45(XDocument doc)
        {
            // FormatFilePathCommand -> ExtractionFilePathCommand
            ChangeCommandName(doc, "FormatFilePathCommand", "ExtractionFilePathCommand", "Extraction File Path");

            // FormatFolderPathCommnad -> ExtractionFolderPathCommand
            ChangeCommandName(doc, "FormatFolderPathCommnad", "ExtractionFolderPathCommand", "Extraction Folder Path");

            // FormatColorCommand -> ConvertColorCommand
            ChangeCommandName(doc, "FormatColorCommand", "ConvertColorCommand", "Convert Color");

            // UIAutomationClickElementCommand clickType, xOffset, yOffset parameters to attributes
            var commands = doc.Descendants("ScriptCommand").Where(el =>
            {
                return ((el.Attribute("CommandName").Value == "UIAutomationClickElementCommand") &&
                            (el.Attribute("v_ClickType") == null));
            });
            XNamespace ns = "urn:schemas-microsoft-com:xml-diffgram-v1";
            foreach (var cmd in commands)
            {
                string clickType = "";
                string xOffset = "";
                string yOffset = "";
                XElement tableParams = cmd.Element("v_ActionParameters").Element(ns + "diffgram").Element("DocumentElement");
                var table = tableParams?.Elements() ?? new List<XElement>();
                foreach (XElement row in table)
                {
                    var n = row.Element("ParameterName").Value;
                    var v = row.Element("ParameterValue").Value;
                    switch (n)
                    {
                        case "Click Type":
                            clickType = v;
                            break;
                        case "X Adjustment":
                            xOffset = v;
                            break;
                        case "Y Adjustment":
                            yOffset = v;
                            break;
                    }
                }
                cmd.SetAttributeValue("v_ClickType", clickType);
                cmd.SetAttributeValue("v_XOffset", xOffset);
                cmd.SetAttributeValue("v_YOffset", yOffset);
                cmd.Element("v_ActionParameters").Remove();
            }

            commands = null;    // release

            // UIAutomationSearchElementAndWindowByXPathCommand v_SearchXPath to attributes
            commands = doc.Descendants("ScriptCommand").Where(el => (el.Attribute("CommandName").Value == "UIAutomationSearchElementAndWindowByXPathCommand"));
            foreach (var cmd in commands)
            {
                var xpath = cmd.Element("v_SearchXPath");
                if (xpath != null)
                {
                    cmd.SetAttributeValue("v_SearchXPath", xpath.Value);
                    xpath.Remove();
                }
            }

            return doc;
        }

        private static XDocument convertTo3_5_1_46(XDocument doc)
        {
            // MoveFolderCommand change dipslay command name
            ChangeCommandName(doc, "MoveFolderCommand", "MoveFolderCommand", "Move Folder");

            return doc;
        }

        public static XDocument convertTo3_5_1_48(XDocument doc)
        {
            // SeleniumBrowserGetAnElementValuesAsDataTableCommand -> SeleniumBrowserGetAnWebElementValuesAsDataTableCommand
            ChangeCommandName(doc, "SeleniumBrowserGetAnElementValuesAsDataTableCommand", "SeleniumBrowserGetAnWebElementValuesAsDataTableCommand", "Get An WebElement Values As DataTable");

            // SeleniumBrowserGetAnElementValuesAsDictionaryCommand -> SeleniumBrowserGetAnWebElementValuesAsDictionaryCommand
            ChangeCommandName(doc, "SeleniumBrowserGetAnElementValuesAsDictionaryCommand", "SeleniumBrowserGetAnWebElementValuesAsDictionaryCommand", "Get An WebElement Values As Dictionary");

            // SeleniumBrowserGetAnElementValuesAsListCommand -> SeleniumBrowserGetAnWebElementValuesAsListCommand
            ChangeCommandName(doc, "SeleniumBrowserGetAnElementValuesAsListCommand", "SeleniumBrowserGetAnWebElementValuesAsListCommand", "Get An WebElement Values As List");

            // SeleniumBrowserGetElementsValueAsDataTableCommand -> SeleniumBrowserGetWebElementsValueAsDataTableCommand
            ChangeCommandName(doc, "SeleniumBrowserGetElementsValueAsDataTableCommand", "SeleniumBrowserGetWebElementsValueAsDataTableCommand", "Get WebElements Value As DataTable");

            // SeleniumBrowserGetElementsValueAsDictionaryCommand -> SeleniumBrowserGetWebElementsValueAsDictionaryCommand
            ChangeCommandName(doc, "SeleniumBrowserGetElementsValueAsDictionaryCommand", "SeleniumBrowserGetWebElementsValueAsDictionaryCommand", "Get WebElements Value As Dictionary");

            // SeleniumBrowserGetElementsValueAsListCommand -> SeleniumBrowserGetWebElementsValueAsListCommand
            ChangeCommandName(doc, "SeleniumBrowserGetElementsValueAsListCommand", "SeleniumBrowserGetWebElementsValueAsListCommand", "Get WebElements Value As List");

            // SeleniumBrowserGetElementsValuesAsDataTableCommand -> SeleniumBrowserGetWebElementsValuesAsDataTableCommand
            ChangeCommandName(doc, "SeleniumBrowserGetElementsValuesAsDataTableCommand", "SeleniumBrowserGetWebElementsValuesAsDataTableCommand", "Get WebElements Values As DataTable");

            return doc;
        }

        /// <summary>
        /// change command name to specified commands
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="newName"></param>
        /// <param name="newSelectioName"></param>
        private static void ChangeCommandName(IEnumerable<XElement> commands, string newName, string newSelectioName)
        {
            XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            foreach (var cmd in commands)
            {
                cmd.SetAttributeValue("CommandName", newName);
                cmd.SetAttributeValue(ns + "type", newName);
                cmd.SetAttributeValue("SelectionName", newSelectioName);
            }
        }

        /// <summary>
        /// change command name. target commands are searched by specified Func<>
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="searchFunc"></param>
        /// <param name="newName"></param>
        /// <param name="newSelectioName"></param>
        /// <returns></returns>
        private static XDocument ChangeCommandName(XDocument doc, Func<XElement, bool> searchFunc, string newName, string newSelectioName)
        {
            IEnumerable<XElement> commands = doc.Descendants("ScriptCommand").Where(searchFunc);
            //XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            //foreach (var cmd in commands)
            //{
            //    cmd.SetAttributeValue("CommandName", newName);
            //    cmd.SetAttributeValue(ns + "type", newName);
            //    cmd.SetAttributeValue("SelectionName", newSelectioName);
            //}
            ChangeCommandName(commands, newName, newSelectioName);
            return doc;
        }

        /// <summary>
        /// change command name. a target command is specified command name.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="targetName"></param>
        /// <param name="newName"></param>
        /// <param name="newSelectioName"></param>
        /// <returns></returns>
        private static XDocument ChangeCommandName(XDocument doc, string targetName, string newName, string newSelectioName)
        {
            return ChangeCommandName(doc, new Func<XElement, bool>(el =>
            {
                return (el.Attribute("CommandName").Value == targetName);
            }), newName, newSelectioName);
        }

        /// <summary>
        /// change attribute value. target commands are searched by specified Func<>
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="searchFunc"></param>
        /// <param name="targetAttribute"></param>
        /// <param name="changeFunc"></param>
        /// <returns></returns>
        private static XDocument ChangeAttributeValue(XDocument doc, Func<XElement, bool> searchFunc, string targetAttribute, Action<XAttribute> changeFunc)
        {
            IEnumerable<XElement> commands = doc.Descendants("ScriptCommand")
                .Where(searchFunc);
            foreach(var cmd in commands)
            {
                changeFunc(cmd.Attribute(targetAttribute));
            }
            return doc;
        }

        /// <summary>
        /// change attribute value. target commands are specified command name
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="targetCommand"></param>
        /// <param name="targetAttribute"></param>
        /// <param name="changeFunc"></param>
        /// <returns></returns>
        private static XDocument ChangeAttributeValue(XDocument doc, string targetCommand, string targetAttribute, Action<XAttribute> changeFunc)
        {
            return ChangeAttributeValue(doc, new Func<XElement, bool>(el =>
            {
                return (el.Attribute("CommandName").Value == targetCommand);
            }), targetAttribute, changeFunc);
        }

        /// <summary>
        /// change table cell value. target commands are specified Func<>
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="searchFunc"></param>
        /// <param name="tableParameterName"></param>
        /// <param name="tableCellName"></param>
        /// <param name="changeFunc"></param>
        /// <returns></returns>
        private static XDocument ChangeTableCellValue(XDocument doc, Func<XElement, bool> searchFunc, string tableParameterName, string tableCellName, Action<XElement> changeFunc)
        {
            IEnumerable<XElement> commands = doc.Descendants("ScriptCommand").Where(searchFunc);

            XNamespace ns = "urn:schemas-microsoft-com:xml-diffgram-v1";
            foreach (var cmd in commands)
            {
                XElement tableParams = cmd.Element(tableParameterName).Element(ns + "diffgram").Element("DocumentElement");
                var table = tableParams?.Elements() ?? new List<XElement>();
                foreach (XElement row in table)
                {
                    XElement targetCell = row.Element(tableCellName);
                    changeFunc(targetCell);
                }
            }
            return doc;
        }

        /// <summary>
        /// change table cell value. target commands are specified command name
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="commandName"></param>
        /// <param name="tableParameterName"></param>
        /// <param name="tableCellName"></param>
        /// <param name="changeFunc"></param>
        /// <returns></returns>
        private static XDocument ChangeTableCellValue(XDocument doc, string commandName, string tableParameterName, string tableCellName, Action<XElement> changeFunc)
        {
            ChangeTableCellValue(doc, new Func<XElement, bool>(el =>
            {
                return el.Attribute("CommandName").Value == commandName;
            }), tableParameterName, tableCellName, changeFunc);
            return doc;
        }

        /// <summary>
        /// modify table
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="searchFunc"></param>
        /// <param name="tableParameterName"></param>
        /// <param name="modifyFunc"></param>
        /// <returns></returns>
        private static XDocument ModifyTable(XDocument doc, Func<XElement, bool> searchFunc, string tableParameterName, Action<XElement, List<XElement>> modifyFunc)
        {
            IEnumerable<XElement> commands = doc.Descendants("ScriptCommand").Where(searchFunc);

            XNamespace ns = "urn:schemas-microsoft-com:xml-diffgram-v1";
            foreach (var cmd in commands)
            {
                XElement tableParams = cmd.Element(tableParameterName).Element(ns + "diffgram").Element("DocumentElement");
                var rows = tableParams?.Elements().ToList() ?? null;
                if (rows != null)
                {
                    modifyFunc(tableParams, rows);
                }
            }
            return doc;
        }

        /// <summary>
        /// modify table
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="targetCommand"></param>
        /// <param name="tableParameterName"></param>
        /// <param name="modifyFunc"></param>
        /// <returns></returns>
        private static XDocument ModifyTable(XDocument doc, string targetCommand, string tableParameterName, Action<XElement, List<XElement>> modifyFunc)
        {
            ModifyTable(doc, new Func<XElement, bool>(el =>
            {
                return el.Attribute("CommandName").Value == targetCommand;
            }), tableParameterName, modifyFunc);
            return doc;
        }

        /// <summary>
        /// add table row
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        private static void AddTableRow(XElement table, List<XElement> rows, Dictionary<string, string> cols)
        {
            XNamespace diffNs = "urn:schemas-microsoft-com:xml-diffgram-v1";
            XNamespace msNs = "urn:schemas-microsoft-com:xml-msdata";

            var row = rows[0];
            var newElem = new XElement(row.Name);
            newElem.Add(new XAttribute(diffNs + "id", row.Name + (rows.Count() + 1).ToString()));    // diffgr:id
            newElem.Add(new XAttribute(msNs + "rowOrder", rows.Count().ToString()));   // msdata:rowOrder

            foreach(var col in cols)
            {
                newElem.Add(new XElement(col.Key)
                {
                    Value = col.Value
                });
            }

            table.Add(newElem);
        }

        /// <summary>
        /// change attribute name. target command is specified Func<>
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="searchFunc"></param>
        /// <param name="targetAttribute"></param>
        /// <param name="newAttribute"></param>
        /// <returns></returns>
        private static XDocument ChangeAttributeName(XDocument doc, Func<XElement, bool> searchFunc, string targetAttribute, string newAttribute)
        {
            IEnumerable<XElement> commands = doc.Descendants("ScriptCommand")
                .Where(searchFunc);

            foreach(var cmd in commands)
            {
                var trgAttr = cmd.Attribute(targetAttribute);
                var newAttr = cmd.Attribute(newAttribute);
                if (trgAttr != null)
                {
                    if (newAttr == null)
                    {
                        cmd.SetAttributeValue(newAttribute, trgAttr.Value);
                    }
                    trgAttr.Remove();
                }
            }

            return doc;
        }

        /// <summary>
        /// change attribute name. target command is specified command name
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="targetCommand"></param>
        /// <param name="targetAttribute"></param>
        /// <param name="newAttribute"></param>
        /// <returns></returns>
        private static XDocument ChangeAttributeName(XDocument doc, string targetCommand, string targetAttribute, string newAttribute)
        {
            return ChangeAttributeName(doc, new Func<XElement, bool>(el =>
            {
                return el.Attribute("CommandName").Value == targetAttribute;
            }), targetAttribute, newAttribute);
        }
    }
}
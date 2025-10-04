using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using taskt.Core;
using taskt.Core.Automation.Commands;
using taskt.Core.Script;

namespace taskt.UI.Forms.ScriptBuilder
{
    using InstanceCounterData = Dictionary<string, Dictionary<string, Dictionary<string, int>>>;
    using LineStatesData = List<(bool IsNewInsertedLine, bool IsDontSaveLine)>;

    public partial class frmScriptBuilder
    {
        /*
         * this file has script file processes
         */

        /// <summary>
        /// begin open script file
        /// </summary>
        private void BeginOpenScriptProcess()
        {
            if (!CheckAndSaveScriptIfForget())
            {
                return;
            }

            // show ofd
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Core.IO.Folders.GetScriptsFolderPath();
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Xml (*.xml)|*.xml";

                // if user selected file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // open file
                    OpenFile(openFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// show check and save dialog
        /// </summary>
        /// <returns>true is allow to continue next process, false is stop process</returns>
        private bool CheckAndSaveScriptIfForget()
        {
            if (this.dontSaveFlag)
            {
                var ret = MessageBox.Show("This script has not been saved yet.\nDo you save it?", "taskt", MessageBoxButtons.YesNoCancel);
                switch (ret)
                {
                    case DialogResult.Yes:
                        BeginSaveScriptProcess((ScriptFilePath == ""));
                        return true;
                    case DialogResult.No:
                        return true;

                    case DialogResult.Cancel:
                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// open file core
        /// </summary>
        /// <param name="filePath"></param>
        private void OpenFile(string filePath)
        {
            // check file exists
            if (!File.Exists(filePath))
            {
                using (var fm = new General.frmDialog($"{filePath} does not exits.", "Open Error", General.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                    return;
                }
            }

            try
            {
                // reinitialize
                lstScriptActions.Items.Clear();
                scriptVariables = new List<ScriptVariable>();
                scriptInfo = null;
                instanceList = new Core.InstanceCounter();

                // get deserialized script
                var deserializedScript = Script.DeserializeFile(filePath, appSettings.EngineSettings, scriptSerializer);

                // check script created taskt version
                var myVer = new Version(Application.ProductVersion);
                var scriptVer = new Version(deserializedScript.Info.TasktVersion);
                if (myVer < scriptVer)
                {
                    using (var fm = new General.frmDialog($"This script file was created with version {deserializedScript.Info.TasktVersion}. It may not work correctly with the taskt version {Application.ProductVersion} that you are currently using.", "Script Warning", General.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }

                if (deserializedScript.Commands.Count == 0)
                {
                    Notify("Error Parsing File: Commands not found!");
                }

                // update file path and reflect in title bar
                this.ScriptFilePath = filePath;

                // assign variables
                scriptVariables = deserializedScript.Variables;

                // script information
                scriptInfo = deserializedScript.Info;
                if (scriptInfo == null)
                {
                    scriptInfo = new ScriptInformation();
                }

                lstScriptActions.BeginUpdate();

                // populate commands
                PopulateExecutionCommands(deserializedScript.Commands);

                // validate
                CheckValidateCommands(deserializedScript.Commands.Select(t => t.ScriptCommand).ToList());

                lstScriptActions.EndUpdate();

                // check indent
                IndentListViewItems();

                // validate
                //CheckValidateScriptFile();

                // Instance Count
                AddInstanceName(deserializedScript.Commands.Select(t => t.ScriptCommand).ToList());

                // format listview
                ChangeSaveState(false);

                // notify
                Notify("Script Loaded Successfully!");

                // release
                deserializedScript = null;
            }
            catch (Exception ex)
            {
                // DBG
                //MessageBox.Show(ex.Message);

                // signal an error has happened
                Notify($"An Error Occured: {ex.Message}");
            }
        }

        /// <summary>
        /// begin script file import process
        /// </summary>
        private void BeginImportProcess()
        {
            // show ofd
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Core.IO.Folders.GetScriptsFolderPath();
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Xml (*.xml)|*.xml";

                // if user selected file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // import
                    Cursor.Current = Cursors.WaitCursor;
                    Import(openFileDialog.FileName);
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// import process core
        /// </summary>
        /// <param name="filePath"></param>
        private void Import(string filePath)
        {
            // check file exists
            if (!File.Exists(filePath))
            {
                using (var fm = new General.frmDialog($"{filePath}_ does not exits.", "Open Error", General.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                    return;
                }
            }

            try
            {
                // deserialize file      
                var deserializedScript = Script.DeserializeFile(filePath, appSettings.EngineSettings, scriptSerializer);

                if (deserializedScript.Commands.Count == 0)
                {
                    Notify("Error Parsing File: Commands not found!");
                }
                // change command ids
                deserializedScript.ReGenerateCommandID();

                // variables for comments
                var fileName = new System.IO.FileInfo(filePath).Name;
                var dateTimeNow = DateTime.Now.ToString();

                lstScriptActions.BeginUpdate();

                // comment
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = $"Imported From {fileName} @ {dateTimeNow}" }));
                // import
                PopulateExecutionCommands(deserializedScript.Commands, false);
                foreach (ScriptVariable var in deserializedScript.Variables)
                {
                    if (scriptVariables.Find(alreadyExists => alreadyExists.VariableName == var.VariableName) == null)
                    {
                        scriptVariables.Add(var);
                    }
                }

                // validate imported commands
                CheckValidateCommands(deserializedScript.Commands.Select(t => t.ScriptCommand).ToList());

                // comment
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = $"End Import From {fileName} @ {dateTimeNow}" }));

                lstScriptActions.EndUpdate();

                // count instance name
                AddInstanceName(deserializedScript.Commands.Select(t => t.ScriptCommand).ToList());

                ChangeSaveState(true);

                // check indent
                IndentListViewItems();

                // format listview

                // notify
                Notify("Script Imported Successfully!");
            }
            catch (Exception ex)
            {
                // signal an error has happened
                Notify($"An Error Occured: {ex.Message}");
            }
        }

        /// <summary>
        /// populate (insert) script commands to lstScriptActions last position
        /// </summary>
        /// <param name="commandDetails"></param>
        /// <param name="isOpen"></param>
        public void PopulateExecutionCommands(List<ScriptAction> commandDetails, bool isOpen = true)
        {
            foreach (ScriptAction item in commandDetails)
            {
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(item.ScriptCommand, isOpen));
                if ((item.AdditionalScriptCommands?.Count ?? 0) > 0)
                {
                    PopulateExecutionCommands(item.AdditionalScriptCommands);
                }
            }

            if (pnlCommandHelper.Visible)
            {
                pnlCommandHelper.Hide();
            }
        }

        /// <summary>
        /// insert script commands to lstScriptActions. this commands can specify insert position
        /// </summary>
        /// <param name="commandDetails"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public int InsertExecutionCommands(List<ScriptAction> commandDetails, int position = -1)
        {
            if (position < 0)
            {
                if (lstScriptActions.SelectedIndices.Count == 0)
                {
                    position = lstScriptActions.Items.Count - 1;
                }
                else
                {
                    position = lstScriptActions.SelectedIndices[0];
                }
            }
            foreach (ScriptAction item in commandDetails)
            {
                lstScriptActions.Items.Insert(position + 1, CreateScriptCommandListViewItem(item.ScriptCommand));
                position++;
                if (item.AdditionalScriptCommands.Count > 0)
                {
                    position = InsertExecutionCommands(item.AdditionalScriptCommands, position);
                }
            }
            return position;
        }

        /// <summary>
        /// begin script file save procee
        /// </summary>
        /// <param name="SaveAs"></param>
        private void BeginSaveScriptProcess(bool SaveAs)
        {
            // clear selected items
            ClearSelectedListViewItems();
            SaveToFile(SaveAs);
        }

        private List<ScriptCommand> GetConfiguredCommands()
        {
            var ConfiguredCommands = new List<ScriptCommand>();
            foreach (ListViewItem item in lstScriptActions.Items)
            {
                ConfiguredCommands.Add(item.Tag as ScriptCommand);
            }

            return ConfiguredCommands;
        }

        /// <summary>
        /// script file save core
        /// </summary>
        /// <param name="saveAs"></param>
        private void SaveToFile(bool saveAs)
        {
            if (lstScriptActions.Items.Count == 0)
            {
                Notify("You must have at least 1 automation command to save.");
                return;
            }

            int beginLoopValidationCount = 0;
            int beginIfValidationCount = 0;
            int tryCatchValidationCount = 0;
            foreach (ListViewItem item in lstScriptActions.Items)
            {
                var cmd = (ScriptCommand)item.Tag;

                if (cmd is IHaveLoopAdditionalCommands)
                {
                    beginLoopValidationCount++;
                }
                else if (cmd is EndLoopCommand)
                {
                    beginLoopValidationCount--;
                }
                else if (cmd is IHaveErrorAdditionalCommands)
                {
                    tryCatchValidationCount++;
                }
                else if (cmd is EndTryCommand)
                {
                    tryCatchValidationCount--;
                }
                else if (cmd is IHaveIfAdditionalCommands)
                {
                    beginIfValidationCount++;
                }
                else if (cmd is EndIfCommand)
                {
                    beginIfValidationCount--;
                }

                // try-catch
                if (tryCatchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your try/catch blocks.");
                    return;
                }

                // end loop was found first
                if (beginLoopValidationCount < 0)
                {
                    Notify("Please verify the ordering of your loops.");
                    return;
                }

                // end if was found first
                if (beginIfValidationCount < 0)
                {
                    Notify("Please verify the ordering of your ifs.");
                    return;
                }
            }

            // extras were found
            if (beginLoopValidationCount != 0)
            {
                Notify("Please verify the ordering of your loops.");
                return;
            }
            // extras were found
            if (beginIfValidationCount != 0)
            {
                Notify("Please verify the ordering of your ifs.");
                return;
            }

            if (tryCatchValidationCount != 0)
            {
                Notify("Please verify the ordering of your try/catch blocks.");
                return;
            }

            // define default output path
            if ((this.ScriptFilePath == null) || (saveAs))
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = Core.IO.Folders.GetScriptsFolderPath();
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.Filter = "Xml (*.xml)|*.xml";

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    this.ScriptFilePath = saveFileDialog.FileName;
                }
            }

            // check file path is only filename
            if (Path.GetFileName(this.ScriptFilePath) == this.ScriptFilePath)
            {
                this.ScriptFilePath =  Path.Combine(appSettings.ClientSettings.RootFolder, this.ScriptFilePath);
            }

            // serialize script
            try
            {
                scriptInfo.TasktVersion = Application.ProductVersion;
                scriptInfo.Revision++;
                //var exportedScript = Script.SerializeScript(lstScriptActions.Items, scriptVariables, scriptInfo, appSettings.EngineSettings, scriptSerializer, this.ScriptFilePath);
                GetSerializedScript(this.ScriptFilePath);
                // show success dialog
                Notify("File has been saved successfully!");
                ChangeSaveState(false);
            }
            catch (Exception ex)
            {
                Notify($"Save Error: {ex}");
            }
        }

        /// <summary>
        /// get serialized script and save
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="enableConvertIntermediate"></param>
        /// <returns></returns>
        private Script GetSerializedScript(string fileName = "", bool enableConvertIntermediate = true, bool changeCommandSaveState = true)
        {
            return Script.SerializeScript(lstScriptActions.Items, this.scriptVariables, this.scriptInfo, appSettings.EngineSettings, scriptSerializer, fileName, enableConvertIntermediate, changeCommandSaveState);
        }

        private void CheckValidateCommands(List<ScriptCommand> commands)
        {
            using (var fm = new CommandEditor.frmCommandEditor(automationCommands, GetConfiguredCommands()))
            {
                fm.appSettings = this.appSettings;
                fm.scriptVariables = this.scriptVariables;
                foreach (var cmd in commands)
                {
                    cmd.IsValidate(fm);
                }
            }
        }

        public bool ImportScriptFromFilePath(string filePath)
        {
            Import(filePath);
            return true;
        }

        public bool OpenScriptFromFilePath(string filePath, bool normalFileOpen = false)
        {
            if (!CheckAndSaveScriptIfForget())
            {
                return false;
            }

            OpenFile(filePath);
            if (normalFileOpen)
            {
                this.ScriptFilePath = filePath;
            }
            else
            {
                this.ScriptFilePath = null;
            }
            ChangeSaveState(!normalFileOpen);
            return true;
        }


        private void BeginRunScriptProcess()
        {
            if (lstScriptActions.Items.Count == 0)
            {
                Notify("You must first build the script by adding commands!");
                return;
            }


            if (ScriptFilePath == null)
            {
                Notify("You must first save your script before you can run it! Or use 'Run w/o Saving'.");
                return;
            }

            // clear selected items
            ClearSelectedListViewItems();

            Notify("Running Script.");

            var newEngine = new Forms.ScriptEngine.frmScriptEngine(ScriptFilePath, this);
            newEngine.callBackForm = this;
            newEngine.Show();
        }


        /// <summary>
        /// begin undo/redo process
        /// </summary>
        /// <param name="overrideScript"></param>
        /// <param name="overrideInstanceCounter"></param>
        /// <param name="isUndo"></param>
        private void BeginUndoRedoProcess(Script overrideScript, InstanceCounterData overrideInstanceCounter, LineStatesData overrideLineStates, bool isUndo = true)
        {
            if ((overrideInstanceCounter != null) && (overrideInstanceCounter != null) && (overrideLineStates != null))
            {
                if (isUndo)
                {
                    CreateRedoSnapshot();
                }
                else
                {
                    CreateUndoSnapshot();
                }

                UndoRedoProcess(overrideScript, overrideInstanceCounter, overrideLineStates, isUndo);
            }
            else
            {
                Notify($"Unable to {(isUndo ? "Undo" : "Redo")}");
            }
        }

        /// <summary>
        /// undo/redo process core
        /// </summary>
        /// <param name="overrideScript"></param>
        /// <param name="overrideInstanceCounter"></param>
        /// <param name="isUndo"></param>
        private void UndoRedoProcess(Script overrideScript, InstanceCounterData overrideInstanceCounter, LineStatesData overrideLineStates, bool isUndo = true)
        {
            lstScriptActions.BeginUpdate();

            // clear all commands in lstScriptActions
            lstScriptActions.Items.Clear();

            // populate commands
            PopulateExecutionCommands(overrideScript.Commands);

            // set line states
            for (int i = lstScriptActions.Items.Count - 1; i >= 0; i--) 
            {
                var cmd = (ScriptCommand)lstScriptActions.Items[i].Tag;
                cmd.IsNewInsertedCommand = overrideLineStates[i].IsNewInsertedLine;
                cmd.IsDontSavedCommand = overrideLineStates[i].IsDontSaveLine;
            }

            // check indent
            IndentListViewItems();

            lstScriptActions.EndUpdate();

            // assign variables
            this.scriptVariables = overrideScript.Variables;

            // script information
            this.scriptInfo = overrideScript.Info ?? new ScriptInformation();

            // Instance Count
            this.instanceList = new InstanceCounter(overrideInstanceCounter);

            // format listview
            ChangeSaveState(true);

            // notify
            Notify($"{(isUndo ? "Undo" : "Redo")} is completed successfully.");
        }
    }
}

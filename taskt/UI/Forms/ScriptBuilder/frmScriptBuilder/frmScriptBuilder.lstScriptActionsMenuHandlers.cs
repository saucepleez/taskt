using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has lstScriptAction context menu event handlers
         */

        private void disableSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(true);
        }

        private void enableSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(false);
        }

        private void pauseBeforeExecutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPauseBeforeExecution();
        }

        private void dontPauseBeforeExecutionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetDontPauseBeforeExecution();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllRows();
        }

        private void cutSelectedActionssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CutRows();
        }

        private void copySelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRows();
        }

        private void pasteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteRows();
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRows();
        }

        private void aboveHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstScriptActions.SelectedIndices.Count > 0)
            {
                int index = lstScriptActions.SelectedIndices[0];
                ClearSelectedListViewItems();

                if (index >= 1)
                {
                    index--;
                }
                lstScriptActions.Items[index].Selected = true;

                bool currentInsertMode = appSettings.ClientSettings.InsertCommandsInline;
                appSettings.ClientSettings.InsertCommandsInline = true;
                AddNewCommand("Script Commands - Comment");
                appSettings.ClientSettings.InsertCommandsInline = currentInsertMode;
            }
        }

        private void belowHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstScriptActions.SelectedIndices.Count > 0)
            {
                int index = lstScriptActions.SelectedIndices[0];
                ClearSelectedListViewItems();
                lstScriptActions.Items[index].Selected = true;

                bool currentInsertMode = appSettings.ClientSettings.InsertCommandsInline;
                appSettings.ClientSettings.InsertCommandsInline = true;
                AddNewCommand("Script Commands - Comment");
                appSettings.ClientSettings.InsertCommandsInline = currentInsertMode;
            }
        }

        private void lstScriptActionsContextStrip_Opened(object sender, EventArgs e)
        {
            //if (lstScriptActions.SelectedIndices.Count == 1)
            //{
            //    var idx = lstScriptActions.SelectedIndices[0];
            //    if (idx == 0)
            //    {
            //        moveUpToolStripMenu.Visible = false;
            //        moveDownToolStripMenu.Visible = true;
            //    }
            //    else if (idx == lstScriptActions.Items.Count - 1)
            //    {
            //        moveUpToolStripMenu.Visible = true;
            //        moveDownToolStripMenu.Visible = false;
            //    }
            //    else
            //    {
            //        moveUpToolStripMenu.Visible = true;
            //        moveDownToolStripMenu.Visible = true;
            //    }
            //}
            if (lstScriptActions.SelectedIndices.Count >= 1)
            {
                var rg = lstScriptActions.SelectedIndices[lstScriptActions.SelectedIndices.Count - 1] - lstScriptActions.SelectedIndices[0];
                if (rg + 1 == lstScriptActions.SelectedIndices.Count)
                {
                    moveUpToolStripMenu.Visible = true;
                    moveDownToolStripMenu.Visible = true;
                    if (lstScriptActions.SelectedIndices[0] == 0)
                    {
                        moveUpToolStripMenu.Visible = false;
                    }
                    if (lstScriptActions.SelectedIndices[lstScriptActions.SelectedIndices.Count - 1] == lstScriptActions.Items.Count - 1)
                    {
                        moveDownToolStripMenu.Visible = false;
                    }
                }
                else
                {
                    moveUpToolStripMenu.Visible = false;
                    moveDownToolStripMenu.Visible = false;
                }
            }
            else
            {
                moveUpToolStripMenu.Visible = false;
                moveDownToolStripMenu.Visible = false;
            }
        }

        private void moveUpToolStripMenu_Click(object sender, EventArgs e)
        {
            //if (lstScriptActions.SelectedIndices.Count > 1)
            //{
            //    return;
            //}
            int idx = lstScriptActions.SelectedIndices[0];
            if (idx == 0)
            {
                return;
            }
            //SwapScriptCommandListViewItems(idx, idx - 1);
            MoveScriptCommandListViewItems(idx - 1);
        }

        private void moveDownToolStripMenu_Click(object sender, EventArgs e)
        {
            //if (lstScriptActions.SelectedIndices.Count > 1)
            //{
            //    return;
            //}
            int idx = lstScriptActions.SelectedIndices[lstScriptActions.SelectedIndices.Count - 1];
            if (idx == lstScriptActions.Items.Count - 1)
            {
                return;
            }
            //SwapScriptCommandListViewItems(idx, idx + 1);
            MoveScriptCommandListViewItems(idx + 2);
        }

        private void moveToParentToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // create command list
            var commandList = new List<ScriptCommand>();

            // loop each
            for (int i = lstScriptActions.SelectedItems.Count - 1; i >= 0; i--)
            {
                // add to list and remove existing
                commandList.Add((ScriptCommand)lstScriptActions.SelectedItems[i].Tag);
                lstScriptActions.Items.Remove(lstScriptActions.SelectedItems[i]);
            }

            // reverse commands only if not inserting inline
            if (!appSettings.ClientSettings.InsertCommandsInline)
            {
                commandList.Reverse();
            }

            // add to parent
            commandList.ForEach(x => parentBuilder.AddCommandToListView(x));
        }

        private void editThisCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstScriptActions_DoubleClick(null, null);
        }

        private void multiSendKeystrokesEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var commands = Supplemental.frmMultiEnterKeys.GetConsecutiveSendKeystrokesCommands(lstScriptActions, appSettings);
            if (commands.Count == 0)
            {
                return;
            }

            using (var fm = new Supplemental.frmMultiEnterKeys(appSettings, scriptVariables, commands))
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    ChangeSaveState(true);

                    var newCommands = fm.SendKeystrokesCommands();

                    // remove current
                    int selectedIndex = lstScriptActions.SelectedIndices[0];
                    for (int i = selectedIndex + commands.Count - 1; i >= selectedIndex; i--)
                    {
                        lstScriptActions.Items.RemoveAt(i);
                    }

                    lstScriptActions.BeginUpdate();

                    int idx = selectedIndex;
                    foreach (var cmd in newCommands)
                    {
                        var lstCommand = CreateScriptCommandListViewItem(cmd);
                        lstScriptActions.Items.Insert(idx, lstCommand);
                        idx++;
                    }

                    lstScriptActions.EndUpdate();
                }
            }
        }

        private void whereThisCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchSelectedCommand();
        }

        private void ViewJSONCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentCommand = lstScriptActions.SelectedItems[0].Tag;

            var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(currentCommand, new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All });

            using (var dialog = new General.frmDialog(jsonText, "JSON Command Code", General.frmDialog.DialogType.OkOnly, 0))
            {
                dialog.ShowDialog();
            }
        }

        private void ViewXMLCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentCommand = (ScriptCommand)lstScriptActions.SelectedItems[0].Tag;

            string scriptXML = Core.Script.Script.SerializeScript(new List<ScriptCommand>() { currentCommand }, scriptSerializer);

            int startIdx = scriptXML.IndexOf("<ScriptCommand ");

            int endIdx = scriptXML.IndexOf("</ScriptCommand>");
            if (endIdx < 0)
            {
                endIdx = scriptXML.IndexOf("</ScriptAction>");
            }

            string commandXML = scriptXML.Substring(startIdx, endIdx - startIdx - 1).Trim();

            using (var dialog = new General.frmDialog(commandXML, "XML Command Code", General.frmDialog.DialogType.OkOnly, 0))
            {
                dialog.ShowDialog();
            }
        }
        private void searchThisCommnadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HighlightAllCurrentSelectedCommand();
        }

        private void clearHighlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearHighlightListViewItem();
        }

        private void helpThisCommandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BeginShowThisCommandHelpProcess();
        }

        private void showScriptInfoMenuItem_Click(object sender, EventArgs e)
        {
            ShowScriptInformationForm();
        }

        private void variableManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowVariableManager();
        }
    }
}

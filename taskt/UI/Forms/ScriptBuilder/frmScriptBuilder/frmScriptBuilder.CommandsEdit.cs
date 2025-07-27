using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;
using taskt.Core.Script;

namespace taskt.UI.Forms.ScriptBuilder
{
    using LineStatesData = List<(bool IsNewInsertedLine, bool IsDontSaveLine)>;

    public partial class frmScriptBuilder
    {
        /*
         * this file has ScriptCommand edit process to lstScriptActions
         */

        #region edit, cut, copy, paste, etc.

        private void EditSelectedCommand()
        {
            if (lstScriptActions.SelectedItems.Count != 1)
            {
                return;
            }

            // bring up edit mode to edit the action
            var selectedCommandItem = lstScriptActions.SelectedItems[0];

            // set selected command from the listview item tag object which was assigned to the command
            var currentCommand = (ScriptCommand)selectedCommandItem.Tag;

            // check if editing a sequence
            if (currentCommand is SequenceCommand sequence)
            {
                if (editMode)
                {
                    MessageBox.Show("Embedding Sequence Commands within Sequence Commands not yet supported.");
                    return;
                }

                //get sequence events
                //SequenceCommand sequence = (SequenceCommand)currentCommand;

                using (var newBuilder = new frmScriptBuilder())
                {
                    // add variables
                    newBuilder.scriptVariables = new List<ScriptVariable>();
                    newBuilder.instanceList = instanceList;

                    foreach (var variable in this.scriptVariables)
                    {
                        newBuilder.scriptVariables.Add(variable);
                    }

                    // append to new builder
                    foreach (var cmd in sequence.v_scriptActions)
                    {
                        newBuilder.lstScriptActions.Items.Add(CreateScriptCommandListViewItem(cmd));
                    }

                    // apply editor style format
                    newBuilder.ApplyEditorFormat();

                    newBuilder.parentBuilder = this;

                    // if data has been changed
                    if (newBuilder.ShowDialog() == DialogResult.OK)
                    {
                        ChangeSaveState(true);

                        // create updated list
                        var updatedList = new List<ScriptCommand>();

                        // update to list
                        for (int i = 0; i < newBuilder.lstScriptActions.Items.Count; i++)
                        {
                            var command = (ScriptCommand)newBuilder.lstScriptActions.Items[i].Tag;
                            updatedList.Add(command);
                        }

                        // apply new list to existing sequence
                        sequence.v_scriptActions = updatedList;
                        sequence.IsDontSavedCommand = true;

                        // update label
                        selectedCommandItem.Text = sequence.GetDisplayValue();
                    }
                }
            }
            else
            {
                var cloneCommand = currentCommand.Clone();
                currentCommand.RemoveInstance(instanceList);

                // create new command editor form
                using (var editCommand = new CommandEditor.frmCommandEditor(automationCommands, GetConfiguredCommands(), this.bufferedCommandList, this.bufferedCommandTreeImages))
                {
                    // creation mode edit locks form to current command
                    editCommand.creationMode = CommandEditor.frmCommandEditor.CreationMode.Edit;

                    editCommand.editingCommand = currentCommand;

                    // create clone of current command so databinding does not affect if changes are not saved
                    //editCommand.originalCommand = Core.Common.Clone(currentCommand);
                    editCommand.originalCommand = currentCommand.Clone();

                    // set variables
                    editCommand.scriptVariables = this.scriptVariables;

                    // set taskt settings
                    editCommand.appSettings = this.appSettings;

                    // set instance counter
                    editCommand.instanceList = this.instanceList;

                    // set size, position
                    if ((lastEditorSize.Width != 0) && (lastEditorSize.Height != 0))
                    {
                        editCommand.Size = lastEditorSize;
                        editCommand.StartPosition = FormStartPosition.Manual;
                        editCommand.Location = lastEditorPosition;
                    }

                    // show edit command form and save changes on OK result
                    if (editCommand.ShowDialog(this) == DialogResult.OK)
                    {
                        CreateUndoSnapshot();

                        ChangeSaveState(true);

                        selectedCommandItem.Tag = editCommand.selectedCommand;
                        selectedCommandItem.Text = editCommand.selectedCommand.GetDisplayValue(); //+ "(" + cmdDetails.SelectedVariables() + ")";
                        selectedCommandItem.SubItems.Add(editCommand.selectedCommand.GetDisplayValue());

                        editCommand.selectedCommand.IsDontSavedCommand = true;

                        editCommand.selectedCommand.AddInstance(instanceList);

                        //CreateUndoSnapshot();
                    }
                    else
                    {
                        cloneCommand.AddInstance(instanceList);
                    }
                }
            }
        }

        private void SelectAllRows()
        {
            lstScriptActions.BeginUpdate();
            foreach (ListViewItem itm in lstScriptActions.Items)
            {
                itm.Selected = true;
            }
            lstScriptActions.EndUpdate();

            Notify($"{lstScriptActions.Items.Count} command(s) selected!");
        }

        private void DeleteRows()
        {
            CreateUndoSnapshot();

            lstScriptActions.BeginUpdate();

            var indices = new int[lstScriptActions.SelectedItems.Count];
            lstScriptActions.SelectedIndices.CopyTo(indices, 0);

            // remove instance name
            var removeCommands = new List<ScriptCommand>();
            foreach (int i in indices)
            {
                removeCommands.Add((ScriptCommand)lstScriptActions.Items[i].Tag);
            }
            RemoveInstanceName(removeCommands);

            for (var i = indices.Length - 1; i >= 0; i--)
            {
                lstScriptActions.Items.RemoveAt(indices[i]);
            }
            lstScriptActions.EndUpdate();
            ChangeSaveState(true);

            Notify($"{indices.Count()} command(s) deleted!");

            //CreateUndoSnapshot();

            // check indent
            IndentListViewItems();

            lstScriptActions.Invalidate();
        }

        private void CutRows()
        {
            // copy into list for all selected            
            if (lstScriptActions.SelectedItems.Count >= 1)
            {
                CreateUndoSnapshot();

                ChangeSaveState(true);

                var commands = new List<ScriptCommand>();

                lstScriptActions.BeginUpdate();
                int[] indices = new int[lstScriptActions.SelectedItems.Count];
                lstScriptActions.SelectedIndices.CopyTo(indices, 0);

                for (int i = 0; i < indices.Length; i++)
                {
                    commands.Add((ScriptCommand)lstScriptActions.Items[indices[i]].Tag);
                }
                for (int i = indices.Length - 1; i >= 0; i--)
                {
                    lstScriptActions.Items.RemoveAt(indices[i]);
                }
                lstScriptActions.EndUpdate();

                // set clipborad xml string
                Clipboard.SetText(Core.Script.Script.SerializeScript(commands, scriptSerializer));

                // remove instance name
                RemoveInstanceName(commands);

                Notify($"{commands.Count} command(s) cut to clipboard!");

                // release
                commands.Clear();

                //CreateUndoSnapshot();

                // check indent
                IndentListViewItems();
            }
        }

        private void CopyRows()
        {
            // copy into list for all selected            
            if (lstScriptActions.SelectedItems.Count >= 1)
            {
                ChangeSaveState(true);

                var commands = new List<ScriptCommand>();

                foreach (ListViewItem item in lstScriptActions.SelectedItems)
                {
                    var newCommand = ((ScriptCommand)item.Tag).Clone();
                    newCommand.GenerateID();
                    commands.Add(newCommand);
                    //commands.Add((ScriptCommand)item.Tag);
                }

                // set clipborad xml string
                Clipboard.SetText(Core.Script.Script.SerializeScript(commands, scriptSerializer));

                Notify($"{commands.Count} commands(s) copied to clipboard!");

                // release
                commands.Clear();
            }
        }

        private void PasteRows()
        {
            var sc = Script.DeserializeXML(Clipboard.GetText(), scriptSerializer);
            if (sc != null)
            {
                CreateUndoSnapshot();

                ChangeSaveState(true);

                lstScriptActions.BeginUpdate();
                InsertExecutionCommands(sc.Commands);
                lstScriptActions.EndUpdate();

                // add instance name
                AddInstanceName(sc.Commands.Select(t => t.ScriptCommand).ToList());

                Notify($"{sc.Commands.Count} commands(s) pasted!");
                // release
                sc = null;

                //CreateUndoSnapshot();

                // check indent
                IndentListViewItems();
            }
            else
            {
                Notify("Error! can not paste commands(s).");
            }
        }

        private void UndoChange()
        {
            (var script, var instances, var lineStates) = undoRedo.Undo();
            BeginUndoRedoProcess(script, instances, lineStates);

            undoSplitMenuItem.Enabled = undoRedo.CanUndo;
        }

        private void RedoChange()
        {
            (var script, var instances, var lineStates) = undoRedo.Redo();
            BeginUndoRedoProcess(script, instances, lineStates, false);

            redoSplitMenuItem.Enabled = undoRedo.CanRedo;
        }

        private void SetSelectedCodeToCommented(bool setCommented)
        {
            // warn if nothing was selected
            if (lstScriptActions.SelectedItems.Count == 0)
            {
                Notify("No code was selected!");
            }

            CreateUndoSnapshot();

            // get each item and set appropriately
            foreach (ListViewItem item in lstScriptActions.SelectedItems)
            {
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.IsCommented = setCommented;
            }

            ChangeSaveState(true);

            //CreateUndoSnapshot();

            // recolor
            lstScriptActions.Invalidate();
        }

        private void SetPauseBeforeExecution()
        {
            // warn if nothing was selected
            if (lstScriptActions.SelectedItems.Count == 0)
            {
                Notify("No code was selected!");
            }

            CreateUndoSnapshot();

            // get each item and set appropriately
            foreach (ListViewItem item in lstScriptActions.SelectedItems)
            {
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.PauseBeforeExeucution = true;
            }

            ChangeSaveState(true);

            //CreateUndoSnapshot();

            lstScriptActions.Invalidate();
        }

        private void SetDontPauseBeforeExecution()
        {
            // warn if nothing was selected
            if (lstScriptActions.SelectedItems.Count == 0)
            {
                Notify("No code was selected!");
            }

            CreateUndoSnapshot();

            // get each item and set appropriately
            foreach (ListViewItem item in lstScriptActions.SelectedItems)
            {
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.PauseBeforeExeucution = false;
            }

            ChangeSaveState(true);

            //CreateUndoSnapshot();

            lstScriptActions.Invalidate();
        }

        ///// <summary>
        ///// swap lstScriptActions items
        ///// </summary>
        ///// <param name="a"></param>
        ///// <param name="b"></param>
        //private void SwapScriptCommandListViewItems(int a, int b)
        //{
        //    // out of range
        //    if (a < 0 || a >= lstScriptActions.Items.Count || b < 0 || b >= lstScriptActions.Items.Count)
        //    {
        //        return;
        //    }
        //    else if (a == b)
        //    {
        //        return;
        //    }

        //    lstScriptActions.BeginUpdate();

        //    CreateUndoSnapshot();

        //    var itemA = CloneScriptCommandListViewItem(lstScriptActions.Items[a]);
        //    var itemB = CloneScriptCommandListViewItem(lstScriptActions.Items[b]);

        //    if (a > b)
        //    {
        //        lstScriptActions.Items.Insert(a, itemB);
        //        lstScriptActions.Items.RemoveAt(a + 1);
        //        lstScriptActions.Items.Insert(b, itemA);
        //        lstScriptActions.Items.RemoveAt(b + 1);
        //    }
        //    else
        //    {
        //        lstScriptActions.Items.Insert(b, itemA);
        //        lstScriptActions.Items.RemoveAt(b + 1);
        //        lstScriptActions.Items.Insert(a, itemB);
        //        lstScriptActions.Items.RemoveAt(a + 1);
        //    }

        //    ChangeSaveState(true);

        //    lstScriptActions.EndUpdate();
        //}

        /// <summary>
        /// move some script commands to specifiy row
        /// </summary>
        /// <param name="insertIndex"></param>
        private void MoveScriptCommandListViewItems(int insertIndex)
        {
            if (lstScriptActions.SelectedIndices.Count == 0)
            {
                return;
            }

            var startIndex = lstScriptActions.SelectedIndices[0];
            var endIndex = lstScriptActions.SelectedIndices[lstScriptActions.SelectedIndices.Count - 1]; 

            // strange insert index
            if ((startIndex <= insertIndex) && (insertIndex <= endIndex))
            {
                return;
            }

            var indices = new int[lstScriptActions.SelectedIndices.Count];
            lstScriptActions.SelectedIndices.CopyTo(indices, 0);

            var newItems = new ListViewItem[lstScriptActions.SelectedIndices.Count];
            for (int i = lstScriptActions.SelectedIndices.Count - 1 ; i >= 0; i--)
            {
                newItems[i] = CloneScriptCommandListViewItem(lstScriptActions.Items[lstScriptActions.SelectedIndices[i]]);
            }

            if (insertIndex < startIndex)
            {
                for (int i = indices.Length - 1 ; i >= 0 ; i--)
                {
                    lstScriptActions.Items.RemoveAt(indices[i]);
                }
                for (int i = indices.Length - 1; i >= 0; i--)
                {
                    lstScriptActions.Items.Insert(insertIndex, newItems[i]);
                }
            }
            else // if (insertIndex > endIndex) 
            {
                for (int i = indices.Length - 1; i >= 0; i--)
                {
                    lstScriptActions.Items.Insert(insertIndex, newItems[i]);
                }
                for (int i = indices.Length - 1; i >= 0; i--)
                {
                    lstScriptActions.Items.RemoveAt(indices[i]);
                }
            }
        }


        /// <summary>
        /// clone lstScriptActions listview Item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isNewCommand"></param>
        /// <returns></returns>
        private ListViewItem CloneScriptCommandListViewItem(ListViewItem item, bool isNewCommand = true)
        {
            ListViewItem newItem = (ListViewItem)item.Clone();
            if (isNewCommand)
            {
                var cmd = (ScriptCommand)item.Tag;
                cmd.IsNewInsertedCommand = true;
                cmd.IsDontSavedCommand = true;
            }
            return newItem;
        }

        private void ClearSelectedListViewItems()
        {
            if (lstScriptActions.FocusedItem != null)
            {
                lstScriptActions.FocusedItem.Focused = false;
            }

            lstScriptActions.SelectedItems.Clear();
            selectedIndex = -1;
            lstScriptActions.Invalidate();
        }

        private void ClearHighlightListViewItem()
        {
            this.currentScriptEditorMode = CommandEditorState.Normal;
            lstScriptActions.Invalidate();
        }

        /// <summary>
        /// create undo snapshot
        /// </summary>
        private void CreateUndoSnapshot()
        {
            undoRedo.AddUndoSnapshot(GetSerializedScript("", false, false), instanceList.GetInstancesCounterClone(), CreateLineStates());

            undoSplitMenuItem.Enabled = true;
        }

        /// <summary>
        /// create redo snapshot
        /// </summary>
        private void CreateRedoSnapshot()
        {
            undoRedo.AddRedoSnapshot(GetSerializedScript("", false, false), instanceList.GetInstancesCounterClone(), CreateLineStates());

            redoSplitMenuItem.Enabled = true;
        }

        /// <summary>
        /// create line state
        /// </summary>
        /// <returns></returns>
        private LineStatesData CreateLineStates()
        {
            var lines = new LineStatesData(lstScriptActions.Items.Count);
            foreach (ListViewItem item in lstScriptActions.Items)
            {
                var cmd = (ScriptCommand)item.Tag;
                lines.Add((cmd.IsNewInsertedCommand, cmd.IsDontSavedCommand));
            }
            return lines;
        }
        #endregion

        #region create command

        private ListViewItem CreateScriptCommandListViewItem(ScriptCommand cmdDetails, bool isOpenFile = false)
        {
            var newCommand = new ListViewItem();

            string dispValue = cmdDetails.GetDisplayValue();

            if (!isOpenFile)
            {
                cmdDetails.IsDontSavedCommand = true;
                cmdDetails.IsNewInsertedCommand = true;
            }

            newCommand.Text = dispValue;
            newCommand.ToolTipText = dispValue;

            newCommand.SubItems.AddRange(new string[] { "", "" });

            cmdDetails.RenderedControls = null;
            newCommand.Tag = cmdDetails;

            newCommand.ImageIndex = Images.GetUIImageList(cmdDetails.GetType().Name);
            return newCommand;
        }

        public void AddCommandToListView(ScriptCommand selectedCommand)
        {
            if (pnlCommandHelper.Visible)
            {
                pnlCommandHelper.Hide();
            }

            CreateUndoSnapshot();

            var command = CreateScriptCommandListViewItem(selectedCommand);

            // count instance name
            AddInstanceName(new List<ScriptCommand>() { selectedCommand });

            // insert to end by default
            var insertionIndex = lstScriptActions.Items.Count;

            // verify setting to insert inline is selected and if an item is currently selected
            if ((appSettings.ClientSettings.InsertCommandsInline) && (lstScriptActions.SelectedItems.Count > 0))
            {
                // insert inline
                insertionIndex = lstScriptActions.SelectedItems[0].Index + 1;
            }

            // insert comment above if, loop, try
            if (appSettings.ClientSettings.InsertCommentIfLoopAbove)
            {
                //if ((selectedCommand is BeginLoopForComplexDataTypesCommand) || (selectedCommand is BeginContinousLoopCommand) || (selectedCommand is BeginNumberOfTimesLoopCommand) || (selectedCommand is BeginLoopCommand) || (selectedCommand is BeginMultiLoopCommand))
                if (selectedCommand is IHaveLoopAdditionalCommands)
                {
                    lstScriptActions.Items.Insert(insertionIndex, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Please enter a description of the loop here" }));
                    insertionIndex++;
                }
                //else if((selectedCommand is BeginIfCommand) || (selectedCommand is BeginMultiIfCommand))
                else if (selectedCommand is IHaveIfAdditionalCommands)
                {
                    lstScriptActions.Items.Insert(insertionIndex, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Please enter a description of the if here" }));
                    insertionIndex++;
                }
                //else if(selectedCommand is TryCommand)
                else if (selectedCommand is IHaveErrorAdditionalCommands)
                {
                    lstScriptActions.Items.Insert(insertionIndex, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Please enter a description of the error handling here" }));
                    insertionIndex++;
                }
            }

            // insert command
            lstScriptActions.Items.Insert(insertionIndex, command);

            var focusIndex = insertionIndex;

            // special types also get a following command and comment
            //if ((selectedCommand is BeginLoopForComplexDataTypesCommand) || (selectedCommand is BeginContinousLoopCommand) || (selectedCommand is BeginNumberOfTimesLoopCommand) || (selectedCommand is BeginLoopCommand) || (selectedCommand is BeginMultiLoopCommand))
            if (selectedCommand is IHaveLoopAdditionalCommands)
            {
                lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will run within the loop" }));
                lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new EndLoopCommand()));
                focusIndex++;
            }
            //else if ((selectedCommand is BeginIfCommand) || (selectedCommand is BeginMultiIfCommand))
            else if (selectedCommand is IHaveIfAdditionalCommands)
            {
                if (appSettings.ClientSettings.InsertElseAutomatically)
                {
                    lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will run if the statement is true" }));
                    lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new ElseCommand()));
                    lstScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will run if the statement is false" }));
                    lstScriptActions.Items.Insert(insertionIndex + 4, CreateScriptCommandListViewItem(new EndIfCommand()));
                }
                else
                {
                    lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will run if the statement is true" }));
                    lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new EndIfCommand()));
                }
                focusIndex++;
            }
            //else if (selectedCommand is TryCommand)
            else if (selectedCommand is IHaveErrorAdditionalCommands)
            {
                lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will be handled if error occurs" }));
                lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new CatchExceptionCommand() { v_Comment = "Items in this section will run if error occurs" }));
                lstScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "This section executes if error occurs above" }));
                lstScriptActions.Items.Insert(insertionIndex + 4, CreateScriptCommandListViewItem(new EndTryCommand()));
                focusIndex++;
            }

            // focus insert command
            if (lstScriptActions.SelectedItems.Count > 0)
            {
                lstScriptActions.MultiSelect = false;
                for (var i = lstScriptActions.SelectedItems.Count - 1; i >= 0; i--)
                {
                    var idx = lstScriptActions.SelectedItems[i].Index;
                    lstScriptActions.Items[idx].Focused = false;
                }
            }
            lstScriptActions.Items[focusIndex].Selected = true;
            lstScriptActions.MultiSelect = true;

            //CreateUndoSnapshot();

            // check indent
            IndentListViewItems();

            lstScriptActions.Invalidate();

            //AutoSizeLineNumberColumn();
        }


        #endregion
    }
}

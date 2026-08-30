using System;
using System.Windows.Forms;
using taskt.Core.Script;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has header context menu event handlers
         */

        #region File Actions menu event handler
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginNewScriptProcess();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginOpenScriptProcess();
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void sampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fm = new Supplemental.frmSample())
            {
                fm.ShowDialog(this);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginSaveScriptProcess(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginSaveScriptProcess(true);
        }

        private void restartApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAndSaveScriptIfForget())
            {
                return;
            }

            Application.Restart();
        }

        private void closeApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginFormCloseProcess();
            Application.Exit();
        }
        #endregion

        #region Edit menu items click handler

        private void undoSplitMenuItem_Click(object sender, EventArgs e)
        {
            UndoChange();
        }

        private void redoSplitMenuItem_Click(object sender, EventArgs e)
        {
            RedoChange();
        }

        private void editThisActionStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedCommand();
        }

        private void helpThisCommandStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginShowThisCommandHelpProcess();
        }

        private void whereThisCommandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SearchSelectedCommand();
        }

        private void enableSelectedActionsStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(false);
        }

        private void disableSelectedActionsStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(true);
        }

        private void pauseBeforeExeutionStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPauseBeforeExecution();
        }

        private void dontPauseBeforeExecutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDontPauseBeforeExecution();
        }

        private void SelectAllStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllRows();
        }
        private void CutScriptStripMenuItem_Click(object sender, EventArgs e)
        {
            CutRows();
        }

        private void CopyStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRows();
        }

        private void PasteStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteRows();
        }

        private void deleteStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRows();
        }

        private void SearchStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch.CurrentMode = Supplemental.frmSearchCommands.SearchReplaceMode.Search;
            frmSearch.variables = GetAllVariablesNames();
            frmSearch.Show();
        }

        private void ReplaceStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch.CurrentMode = Supplemental.frmSearchCommands.SearchReplaceMode.Replace;
            frmSearch.variables = GetAllVariablesNames();
            frmSearch.Show();
        }

        private void highlightThisCommandStripMenuItem_Click(object sender, EventArgs e)
        {
            HighlightAllCurrentSelectedCommand();
        }
        private void clearSearchHighlightsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ClearHighlightListViewItem();
        }

        #endregion

        #region Options menu event handler
        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowVariableManager();
        }
        private void scriptInformationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowScriptInformationForm();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettingForm();
        }

        private void newSettigsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowNewSettingForm();
        }

        private void showScriptFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowScriptFolderProcess();
        }


        private void showLogFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLogFolderProcess();
        }

        private void showSearchBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appSettings.ClientSettings.ShowCommandSearchBar = !appSettings.ClientSettings.ShowCommandSearchBar;
            SetCommandSearchBoxState();
        }

        private void guiInspectToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fm = new CommandEditor.Supplemental.frmGUIInspectTool();
            fm.Show();
        }

        private void jsonPathHelperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fm = new CommandEditor.Supplemental.frmJSONPathHelper();
            fm.Show();
        }

        private void showFormatCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fm = new CommandEditor.Supplemental.frmFormatChecker();
            fm.Show();
        }
        #endregion

        #region Script Actions menu event handler
        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();

            using (var sequenceRecorder = new Supplemental.frmSequenceRecorder())
            {
                sequenceRecorder.callBackForm = this;
                sequenceRecorder.ShowDialog();
            }

            pnlCommandHelper.Hide();

            this.Show();
            this.BringToFront();
        }

        private void scheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var scheduleManager = new Supplemental.frmScheduleManagement();
            scheduleManager.Show();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tempFilePath = Script.GetRunWithoutSavingScriptFilePath();
            var currentFilePath = this.ScriptFilePath;
            var currentDontSaveFlag = this.dontSaveFlag;

            this.ScriptFilePath = tempFilePath;
            saveAndRunToolStripMenuItem_Clicked(null, null);

            this.ScriptFilePath = currentFilePath;
            this.dontSaveFlag = currentDontSaveFlag;
            UpdateWindowTitle();
        }
        #endregion

        #region Save And Run menu event handler
        private void saveAndRunToolStripMenuItem_Clicked(object sender, EventArgs e)
        {
            if (scriptInfo.RunTimes != int.MaxValue)
            {
                scriptInfo.RunTimes++;
            }
            scriptInfo.LastRunTime = DateTime.Now;
            BeginSaveScriptProcess((this.ScriptFilePath == ""));
            BeginRunScriptProcess();
        }
        #endregion

        #region Help tool strip
        private void tasktProjectPageStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGitProjectPage();
        }

        private void tasktWikiStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWikiPage();
        }

        private void tasktGitterStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGitterPage();
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGitReleasePage();
        }

        private void issueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGitIssuePage();
        }

        private void aboutStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutForm();
        }
        #endregion

        #region command search
        private void tsSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SearchForItemInListView();
            }
        }

        #endregion
    }
}

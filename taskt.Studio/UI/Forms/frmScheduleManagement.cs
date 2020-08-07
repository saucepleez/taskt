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
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core.Enums;
using taskt.Core.IO;

namespace taskt.UI.Forms
{
    public partial class frmScheduleManagement : UIForm
    {
        private string _rpaScriptsFolder;

        public frmScheduleManagement()
        {
            InitializeComponent();
        }

        #region Form and Control Events

        private void frmScheduleManagement_Load(object sender, EventArgs e)
        {
            //get path to executing assembly
            txtAppPath.Text = Assembly.GetEntryAssembly().Location;

            //get path to scripts folder
            _rpaScriptsFolder = Folders.GetFolder(FolderType.ScriptsFolder);

            var files = Directory.GetFiles(_rpaScriptsFolder);

            foreach (var fil in files)
            {
                FileInfo newFileInfo = new FileInfo(fil);
                cboSelectedScript.Items.Add(newFileInfo.Name);
            }

            //set autosize mode
            colTaskName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //call bgw to pull schedule info
            RefreshTasks();
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtRecurCount.Text))
            {
                MessageBox.Show("Please indicate a recurrence value!");
                return;
            }

            if (String.IsNullOrEmpty(cboRecurType.Text))
            {
                MessageBox.Show("Please select a recurrence frequency!");
                return;
            }

            if (String.IsNullOrEmpty(cboSelectedScript.Text))
            {
                MessageBox.Show("Please select a script!");
                return;
            }

            var selectedFile = Path.Combine(_rpaScriptsFolder, cboSelectedScript.Text);

            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Scheduled task from taskt - " + selectedFile;
                var trigger = new TimeTrigger();
                ////   // Add a trigger that will fire the task at this time every other day
                //DailyTrigger dt = (DailyTrigger)td.Triggers.Add(new DailyTrigger(2));
                //dt.Repetition.Duration = TimeSpan.FromHours(4);
                //dt.Repetition.Interval = TimeSpan.FromHours()
                // Create a trigger that will execute very 2 minutes.

                trigger.StartBoundary = dtStartTime.Value;
                if (rdoEndByDate.Checked)
                {
                    trigger.EndBoundary = dtEndTime.Value;
                }

                double recurParsed;

                if (!double.TryParse(txtRecurCount.Text, out recurParsed))
                {
                    MessageBox.Show("Recur value must be a number type (double)!");
                    return;
                }

                switch (cboRecurType.Text)
                {
                    case "Minutes":
                        trigger.Repetition.Interval = TimeSpan.FromMinutes(recurParsed);
                        break;

                    case "Hours":
                        trigger.Repetition.Interval = TimeSpan.FromHours(recurParsed);
                        break;

                    case "Days":
                        trigger.Repetition.Interval = TimeSpan.FromDays(recurParsed);
                        break;

                    default:
                        break;
                }

                td.Triggers.Add(trigger);

                td.Actions.Add(new ExecAction(@"" + txtAppPath.Text + "", "\"" + selectedFile + "\"", null));
                ts.RootFolder.RegisterTaskDefinition(@"taskt-" + cboSelectedScript.Text, td);
            }
        }

        private void uiBtnShowScheduleManager_Click(object sender, EventArgs e)
        {
            using (TaskService ts = new TaskService())
                ts.StartSystemTaskSchedulerManager();
        }

        private void dgvScheduledTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvScheduledTasks.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                int row = this.dgvScheduledTasks.CurrentCell.RowIndex;
                using (TaskService ts = new TaskService())
                {
                    string taskName = (string)dgvScheduledTasks.Rows[row].Cells["colTaskName"].Value;
                    var updateTask = ts.FindTask(taskName);
                    updateTask.Enabled = !updateTask.Enabled;
                }  
            }
        }

        #endregion Form and Control Events

        #region BackgroundWorker, Timer

        //events for background worker and associated methods
        private void RefreshTasks()
        {
            bgwGetSchedulingInfo.RunWorkerAsync();
        }

        private void tmrGetSchedulingInfo_Tick(object sender, EventArgs e)
        {
            if (!bgwGetSchedulingInfo.IsBusy)
                bgwGetSchedulingInfo.RunWorkerAsync();
        }

        private void bgwGetSchedulingInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Object[]> scheduledTaskList = new List<Object[]>();

            using (TaskService ts = new TaskService())
            {
                foreach (Task task in ts.RootFolder.Tasks)
                {
                    if (task.Name.Contains("taskt"))
                    {
                        string currentState = "enable";
                        if (task.Enabled)
                            currentState = "disable";

                        var scheduleItem = new object[] 
                        { 
                            task.Name, 
                            task.LastRunTime, 
                            task.LastTaskResult, 
                            task.NextRunTime, 
                            task.IsActive, 
                            currentState 
                        };
                        scheduledTaskList.Add(scheduleItem);
                    }
                }
            }

            e.Result = scheduledTaskList;
        }

        private void bgwGetSchedulingInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgvScheduledTasks.Rows.Clear();
            List<object[]> datagridRows = (List<object[]>)e.Result;
            datagridRows.ForEach(itm => dgvScheduledTasks.Rows.Add(itm[0], itm[1], itm[2], itm[3], itm[4], itm[5]));
        }

        #endregion BackgroundWorker, Timer
    }
}
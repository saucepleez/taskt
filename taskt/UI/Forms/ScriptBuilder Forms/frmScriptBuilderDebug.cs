using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using taskt.Core.Script;
using taskt.Properties;

namespace taskt.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        private void CreateDebugTab()
        {
            TabPage debugTab = new TabPage();
            debugTab.Name = "DebugVariables";
            debugTab.Text = "Variables";
            uiPaneTabs.TabPages.Add(debugTab);
            uiPaneTabs.SelectedTab = debugTab;
            LoadDebugTab(debugTab);
        }

        //TODO: Studio Step Into
        public delegate void LoadDebugTabDelegate(TabPage debugTab);
        private void LoadDebugTab(TabPage debugTab)
        {
            if (InvokeRequired)
            {
                var d = new LoadDebugTabDelegate(LoadDebugTab);
                Invoke(d, new object[] { debugTab });
            }
            else
            {
                DataTable variableValues = new DataTable();
                variableValues.Columns.Add("Name");
                variableValues.Columns.Add("Type");
                variableValues.Columns.Add("Value");
                variableValues.TableName = "VariableValuesDataTable" + DateTime.Now.ToString("MMddyyhhmmss");

                DataGridView variablesGridViewHelper = new DataGridView();
                variablesGridViewHelper.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                variablesGridViewHelper.Dock = DockStyle.Fill;
                variablesGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                variablesGridViewHelper.AllowUserToAddRows = false;
                variablesGridViewHelper.AllowUserToDeleteRows = false;

                if (debugTab.Controls.Count != 0)
                    debugTab.Controls.RemoveAt(0);
                debugTab.Controls.Add(variablesGridViewHelper);

                List<ScriptVariable> engineVariables = _newEngine.EngineInstance.VariableList;
                foreach (var variable in engineVariables)
                {
                    DataRow[] foundVariables = variableValues.Select("Name = '" + variable.VariableName + "'");
                    if (foundVariables.Length == 0)
                    {
                        var type = variable.VariableValue.GetType().ToString();
                        switch (variable.VariableValue.GetType().ToString())
                        {
                            case "System.String":
                                variableValues.Rows.Add(variable.VariableName, variable.VariableValue.GetType().ToString(),
                                    variable.VariableValue);
                                break;
                            case "System.Data.DataTable":
                                variableValues.Rows.Add(variable.VariableName, variable.VariableValue.GetType().ToString(), 
                                    ConvertDataTableToString((DataTable)variable.VariableValue));
                                break;
                            case "System.Collections.Generic.List`1[System.String]":
                                variableValues.Rows.Add(variable.VariableName, variable.VariableValue.GetType().ToString(),
                                    ConvertListToString((List<string>)variable.VariableValue));
                                break;
                            default:
                                variableValues.Rows.Add(variable.VariableName, variable.VariableValue.GetType().ToString(), 
                                    variable.VariableValue.ToString());
                                break;
                        }                       
                    }
                }
                variablesGridViewHelper.DataSource = variableValues;
            }           
        }

        #region Debug Buttons
        private void stepOverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _newEngine.uiBtnStepOver_Click(sender, e);
            IsScriptSteppedOver = true;
        }

        private void stepIntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _newEngine.uiBtnStepInto_Click(sender, e);
            IsScriptSteppedInto = true;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _newEngine.uiBtnPause_Click(sender, e);
            if (pauseToolStripMenuItem.Tag.ToString() == "pause")
            {
                pauseToolStripMenuItem.Image = Resources.command_resume;
                pauseToolStripMenuItem.Tag = "resume";
            }

            else if (pauseToolStripMenuItem.Tag.ToString() == "resume")
            {
                stepIntoToolStripMenuItem.Visible = false;
                stepOverToolStripMenuItem.Visible = false;
                pauseToolStripMenuItem.Visible = true;
                cancelToolStripMenuItem.Visible = true;
                pauseToolStripMenuItem.Image = Resources.command_pause;
                pauseToolStripMenuItem.Tag = "pause";

                //When resuming, close debug tab if it's open
                TabPage debugTab = uiPaneTabs.TabPages.Cast<TabPage>().Where(t => t.Name == "DebugVariables")
                                                                              .FirstOrDefault();

                if (debugTab != null)
                    uiPaneTabs.TabPages.Remove(debugTab);

                IsScriptSteppedOver = false;
                IsScriptSteppedInto = false;
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _newEngine.uiBtnCancel_Click(sender, e);

            stepIntoToolStripMenuItem.Visible = false;
            stepOverToolStripMenuItem.Visible = false;
            pauseToolStripMenuItem.Visible = false;
            cancelToolStripMenuItem.Visible = false;
        }
        #endregion

        public string ConvertDataTableToString(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[[");

            for (int i = 0; i < dt.Columns.Count - 1; i++)
                stringBuilder.AppendFormat("{0}, ", dt.Columns[i].ColumnName);

            stringBuilder.AppendFormat("{0}]]", dt.Columns[dt.Columns.Count -1].ColumnName);
            stringBuilder.AppendLine();

            foreach (DataRow rows in dt.Rows)
            {
                stringBuilder.Append("[");

                for(int i = 0; i<dt.Columns.Count-1; i++)
                    stringBuilder.AppendFormat("{0}, ", rows[i]);

                stringBuilder.AppendFormat("{0}]", rows[dt.Columns.Count - 1]);
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public string ConvertListToString(List<string> list)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");

            for (int i = 0; i < list.Count - 1; i++)
                stringBuilder.AppendFormat("{0}, ", list[i]);

            stringBuilder.AppendFormat("{0}]", list[list.Count - 1]);
            return stringBuilder.ToString();
        }
    }
}

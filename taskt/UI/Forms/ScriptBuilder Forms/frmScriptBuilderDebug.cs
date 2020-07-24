using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using taskt.Core.Script;
using taskt.UI.Forms.Supplement_Forms;

namespace taskt.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        private void CreateDebugTab()
        {
            TabPage debugTab = uiPaneTabs.TabPages.Cast<TabPage>().Where(t => t.Name == "DebugVariables")
                                                                              .FirstOrDefault();

            if (debugTab == null)
            {
                debugTab = new TabPage();
                debugTab.Name = "DebugVariables";
                debugTab.Text = "Variables";
                uiPaneTabs.TabPages.Add(debugTab);
                uiPaneTabs.SelectedTab = debugTab;
            }
            LoadDebugTab(debugTab);
        }

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

                List<ScriptVariable> engineVariables = CurrentEngine.EngineInstance.VariableList;
                foreach (var variable in engineVariables)
                {
                    DataRow[] foundVariables = variableValues.Select("Name = '" + variable.VariableName + "'");
                    if (foundVariables.Length == 0)
                    {
                        var type = variable.VariableValue.GetType().ToString();
                        switch (variable.VariableValue.GetType().ToString())
                        {
                            case "System.String":
                                variableValues.Rows.Add(variable.VariableName, variable.VariableValue.GetType().FullName,
                                    variable.VariableValue);
                                break;
                            case "System.Data.DataTable":
                                variableValues.Rows.Add(variable.VariableName, variable.VariableValue.GetType().FullName, 
                                    ConvertDataTableToString((DataTable)variable.VariableValue));
                                break;
                            case "System.Data.DataRow":
                                variableValues.Rows.Add(variable.VariableName, variable.VariableValue.GetType().FullName,
                                    ConvertDataRowToString((DataRow)variable.VariableValue));
                                break;
                            case "System.__ComObject":
                                variableValues.Rows.Add(variable.VariableName, "Microsoft.Office.Interop.Outlook.MailItem",
                                    ConvertMailItemToString((MailItem)variable.VariableValue));
                                break;
                            case "System.Collections.Generic.List`1[System.String]":
                            case "System.Collections.Generic.List`1[Microsoft.Office.Interop.Outlook.MailItem]":
                                variableValues.Rows.Add(variable.VariableName, variable.VariableValue.GetType().FullName,
                                    ConvertListToString(variable.VariableValue));
                                break;
                            default:
                                variableValues.Rows.Add(variable.VariableName, variable.VariableValue.GetType().FullName, 
                                    "*Type Not Yet Supported*");
                                break;
                        }                       
                    }
                }
                variablesGridViewHelper.DataSource = variableValues;
                uiPaneTabs.SelectedTab = debugTab;
            }           
        }

        public delegate void RemoveDebugTabDelegate();
        public void RemoveDebugTab()
        {
            if (InvokeRequired)
            {
                var d = new RemoveDebugTabDelegate(RemoveDebugTab);
                Invoke(d, new object[] { });
            }
            else
            {
                TabPage debugTab = uiPaneTabs.TabPages.Cast<TabPage>().Where(t => t.Name == "DebugVariables")
                                                                              .FirstOrDefault();

                if (debugTab != null)
                    uiPaneTabs.TabPages.Remove(debugTab);
            }
        }

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

        public string ConvertDataRowToString(DataRow row)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");

            for (int i = 0; i < row.ItemArray.Length - 1; i++)
                stringBuilder.AppendFormat("{0}, ", row.ItemArray[i]);

            stringBuilder.AppendFormat("{0}]", row.ItemArray[row.ItemArray.Length - 1]);
            return stringBuilder.ToString();
        }

        public string ConvertListToString(object list)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            Type type = list.GetType().GetGenericArguments()[0];

            if (type == typeof(string))
            {
                List<string> stringList = (List<string>)list;
                stringBuilder.Append($"Count({stringList.Count}) [");

                for (int i = 0; i < stringList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, ", stringList[i]);

                stringBuilder.AppendFormat("{0}]", stringList[stringList.Count - 1]);
            }
            else if (type == typeof(MailItem))
            {
                List<MailItem> mailItemList = (List<MailItem>)list;
                stringBuilder.Append($"Count({mailItemList.Count}) [");

                for (int i = 0; i < mailItemList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertMailItemToString(mailItemList[i]));

                stringBuilder.AppendFormat("{0}]", ConvertMailItemToString(mailItemList[mailItemList.Count - 1]));
            }

            return stringBuilder.ToString();
        }

        public string ConvertMailItemToString(MailItem mail)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"[Subject: {mail.Subject}, \n" +
                                  $"Sender: {mail.SenderName}, \n" +
                                  $"Sent On: {mail.SentOn}, \n" +
                                  $"Unread: {mail.UnRead}, \n" +
                                  $"Attachments({mail.Attachments.Count})");
            if (mail.Attachments.Count > 0)
            {
                stringBuilder.Append(" [");
                foreach(Attachment attachment in mail.Attachments)
                {
                    stringBuilder.Append($"{attachment.FileName}, ");
                }
                //trim final comma
                stringBuilder.Length = stringBuilder.Length - 2;
                stringBuilder.Append("]");
            }

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        public delegate DialogResult LoadErrorFormDelegate(string errorMessage);
        public DialogResult LoadErrorForm(string errorMessage)
        {
            if (InvokeRequired)
            {
                var d = new LoadErrorFormDelegate(LoadErrorForm);
                return (DialogResult)Invoke(d, new object[] { errorMessage });
            }
            else
            {
                frmError errorForm = new frmError(errorMessage);
                errorForm.Owner = this;
                errorForm.ShowDialog();
                return errorForm.DialogResult;
            }          
        }
    }
}

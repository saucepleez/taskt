using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;
using taskt.UI.CustomControls;

namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    public partial class frmMultiEnterKeys : ThemedForm
    {
        private Core.ApplicationSettings appSetttings;
        private List<Core.Script.ScriptVariable> scriptVariables;
        private CreationMode mode;

        public enum CreationMode
        {
            New,
            Edit
        }

        #region form events
        public frmMultiEnterKeys(Core.ApplicationSettings appSetttings, List<Core.Script.ScriptVariable> scriptVariables, List<ScriptCommand> keyCommands = null)
        {
            InitializeComponent();
            this.appSetttings = appSetttings;
            this.scriptVariables = scriptVariables;

            cmbCompareMethod.Items.AddRange(
                new string[] { "", "Contains", "Starts with", "Ends with", "Exact Match"}
            );

            if ((keyCommands == null) || (keyCommands.Count == 0))
            {
                mode = CreationMode.New;
            }
            else
            {
                mode = CreationMode.Edit;

                EnterKeysCommand firstKeystroke = (EnterKeysCommand)keyCommands[0];
                cmbWindowName.Text = firstKeystroke.v_WindowName;
                cmbCompareMethod.Text = firstKeystroke.v_CompareMethod;
                txtWaitTimeAfter.Text = firstKeystroke.v_WaitTime;

                string keystrokes = "";
                foreach(var cmd in keyCommands)
                {
                    EnterKeysCommand c = (EnterKeysCommand)cmd;
                    keystrokes += c.v_TextToSend + "\r\n";
                }
                txtTextToSend.Text = keystrokes.Substring(0, keystrokes.Length - 2);
            }

            // Insert Variable
            lnkWindoNameVariable.Tag = cmbWindowName;
            lnkCompareMethodVariable.Tag = cmbCompareMethod;
            lnkTextToSend.Tag = txtTextToSend;
            lnkWaitTimeAfterVariable.Tag = txtWaitTimeAfter;

            // ComboBox events
            cmbWindowName.Click += combobox_Click;
            cmbWindowName.KeyUp += combobox_KeyUp;
            cmbCompareMethod.Click += combobox_Click;
            cmbCompareMethod.KeyUp += combobox_KeyUp;
        }

        private void frmMultiSendKeystrokes_Load(object sender, EventArgs e)
        {
            UpdateWindowNames();
        }
        #endregion

        #region Window Name events
        private void lnkWindowNameUpToDate_Click(object sender, EventArgs e)
        {
            UpdateWindowNames();
        }

        private void lnkWindoNameVariable_Click(object sender, EventArgs e)
        {
            using (var fm = CreateVariableSelectForm())
            {
                if ((fm.ShowDialog() == DialogResult.OK) && (fm.selectedItem != null))
                {
                    //string variableName = appSetttings.EngineSettings.wrapVariableMarker((string)fm.selectedItem);
                    string variableName = VariableNameControls.GetWrappedVariableName((string)fm.selectedItem, appSetttings);
                    if (appSetttings.ClientSettings.InsertVariableAtCursor)
                    {
                        string currentValue = cmbWindowName.Text;
                        int pos = int.Parse(cmbWindowName.Tag.ToString());
                        cmbWindowName.Text = currentValue.Substring(0, pos) + variableName + currentValue.Substring(pos);
                    }
                    else
                    {
                        cmbWindowName.Text += variableName;
                    }
                }
            }
        }

        private void cmbWindowName_KeyUp(object sender, KeyEventArgs e)
        {
            cmbWindowName.Tag = cmbWindowName.SelectionStart;
        }

        private void cmbWindowName_Click(object sender, EventArgs e)
        {
            cmbWindowName.Tag = cmbWindowName.SelectionStart;
        }
        #endregion

        #region compare method events

        private void lnkCompareMethodVariable_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region text To Send Events
        private void lnkKeyStrokesVariable_Click(object sender, EventArgs e)
        {
            using(var fm = CreateVariableSelectForm())
            {
                if ((fm.ShowDialog() == DialogResult.OK) && (fm.selectedItem != null))
                {
                    ConcatenateVariableName((string)fm.selectedItem, txtTextToSend, appSetttings);
                }
            }
        }
        #endregion

        #region wait time events
        private void lnkWaitTimeVariable_Click(object sender, EventArgs e)
        {
            using (var fm = CreateVariableSelectForm())
            {
                if ((fm.ShowDialog() == DialogResult.OK) && (fm.selectedItem != null))
                {
                    ConcatenateVariableName((string)fm.selectedItem, txtWaitTimeAfter, appSetttings);
                }
            }
        }
        #endregion

        #region footer buttons events
        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region General Events
        /// <summary>
        /// combobox key up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void combobox_KeyUp(object sender, KeyEventArgs e)
        {
            SetCursorPosition((ComboBox)sender);
        }

        /// <summary>
        /// combobox click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void combobox_Click(object sender, EventArgs e)
        {
            SetCursorPosition((ComboBox)sender);
        }

        /// <summary>
        /// set cursor position
        /// </summary>
        /// <param name="cmb"></param>
        private static void SetCursorPosition(ComboBox cmb)
        {
            cmb.Tag = cmb.SelectionStart;
        }

        /// <summary>
        /// lnkInsertVariable clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkInsertVariable_Click(object sender, EventArgs e)
        {
            using (var fm = CreateVariableSelectForm())
            {
                if ((fm.ShowDialog() == DialogResult.OK) && (fm.selectedItem != null))
                {
                    string variableName = VariableNameControls.GetWrappedVariableName((string)fm.selectedItem, appSetttings);

                    var ctrl = (Control)((CommandItemControl)sender).Tag;
                    if (ctrl is TextBox txt)
                    {
                        ConcatenateVariableName(variableName, txt, appSetttings);
                    }
                    else if (ctrl is ComboBox cmb)
                    {
                        ConcatenateVariableName(variableName, cmb, appSetttings);
                    }
                }
            }
        }

        /// <summary>
        /// create variable name selector form
        /// </summary>
        /// <returns></returns>
        private CommandEditor.Supplemental.frmItemSelector CreateVariableSelectForm()
        {
            var variableList = scriptVariables.Select(f => f.VariableName).ToList();
            variableList.AddRange(Core.Automation.Engine.SystemVariables.GetSystemVariablesName());

            return (new CommandEditor.Supplemental.frmItemSelector(variableList, "Select Variable", "Select Variable"));
        }

        /// <summary>
        /// concatenate variable name to TextBox
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="txt"></param>
        /// <param name="settings"></param>
        private static void ConcatenateVariableName(string variableName, TextBox txt, Core.ApplicationSettings settings)
        {
            variableName = VariableNameControls.GetWrappedVariableName(variableName, settings);
            if (settings.ClientSettings.InsertVariableAtCursor)
            {
                string current = txt.Text;
                txt.Text = current.Substring(0, txt.SelectionStart) + variableName + current.Substring(txt.SelectionStart);
            }
            else
            {
                txt.Text += variableName;
            }
        }

        /// <summary>
        /// concatenate variable name to ComboBox
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="cmb"></param>
        /// <param name="settings"></param>
        private static void ConcatenateVariableName(string variableName, ComboBox cmb, Core.ApplicationSettings settings)
        {
            variableName = VariableNameControls.GetWrappedVariableName(variableName, settings);
            if (settings.ClientSettings.InsertVariableAtCursor)
            {
                string currentValue = cmb.Text;
                int pos = int.Parse(cmb.Tag?.ToString() ?? "0");
                cmb.Text = currentValue.Substring(0, pos) + variableName + currentValue.Substring(pos);
            }
            else
            {
                cmb.Text += variableName;
            }
        }
        #endregion

        #region Properties
        public List<ScriptCommand> SendKeystrokesCommands()
        {
            string[] sendTexts = txtTextToSend.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            if (sendTexts.Length == 0)
            {
                return null;
            }

            List<ScriptCommand> commands = new List<ScriptCommand>();
            foreach (string s in sendTexts)
            {
                commands.Add(
                    new EnterKeysCommand()
                    {
                        v_WindowName = cmbWindowName.Text,
                        v_CompareMethod = cmbCompareMethod.Text,
                        v_TextToSend = s,
                        v_EncryptionOption = "",
                        v_WaitTime = txtWaitTimeAfter.Text
                    }
                );
            }

            return commands;
        }

        public CreationMode Mode
        {
            get
            {
                return this.mode;
            }
        }
        #endregion

        #region private methods
        private void UpdateWindowNames()
        {
            string currentWindow = cmbWindowName.Text;

            var windowNames = Core.Automation.Commands.WindowControls.GetAllWindowTitles();

            cmbWindowName.BeginUpdate();
            cmbWindowName.Items.Clear();

            //cmbWindowName.Items.Add(appSetttings.EngineSettings.CurrentWindowKeyword);
            cmbWindowName.Items.Add(VariableNameControls.GetWrappedVariableName(Core.Automation.Engine.SystemVariables.Window_CurrentWindowName.VariableName, appSetttings));
            cmbWindowName.Items.AddRange(windowNames.ToArray());

            if (cmbWindowName.Items.Contains(currentWindow))
            {
                cmbWindowName.Text = currentWindow;
            }

            cmbWindowName.EndUpdate();
        }
        #endregion

        #region public methods
        public static List<ScriptCommand> GetConsecutiveSendKeystrokesCommands(ListView lstCommands, Core.ApplicationSettings appSettings, int startIndex = -1) 
        {
            if (startIndex < 0)
            {
                if (lstCommands.SelectedIndices.Count > 0)
                {
                    startIndex = lstCommands.SelectedIndices[0];
                }
                else
                {
                    startIndex = 0;
                }
            }

            List<ScriptCommand> commands = new List<ScriptCommand>();

            int lastRow = lstCommands.Items.Count;
            

            ScriptCommand selectedCommand = (ScriptCommand)lstCommands.Items[startIndex].Tag;
            if (!(selectedCommand is EnterKeysCommand))
            {
                return commands;
            }

            // search to top
            EnterKeysCommand tryFirstKeystroke = (EnterKeysCommand)selectedCommand;
            int idx = startIndex;
            while (idx >= 1)
            {
                ScriptCommand cmd = (ScriptCommand)lstCommands.Items[idx-1].Tag;
                if (cmd is EnterKeysCommand)
                {
                    EnterKeysCommand currentSendKeys = (EnterKeysCommand)cmd;
                    if (isSameWindowToBottom(currentSendKeys, tryFirstKeystroke,  appSettings))
                    {
                        tryFirstKeystroke = currentSendKeys;
                        idx--;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            lstCommands.Items[idx].Selected = true;
            //Console.WriteLine(lstCommands.SelectedIndices[0]);

            // search to buttom, create SendKeystrokes list
            while (idx < lastRow)
            {
                ScriptCommand cmd = (ScriptCommand)lstCommands.Items[idx].Tag;
                if (cmd is EnterKeysCommand)
                {
                    EnterKeysCommand currentSendKeys = (EnterKeysCommand)cmd;
                    if (isSameWindowToBottom(tryFirstKeystroke, currentSendKeys, appSettings))
                    {
                        commands.Add(currentSendKeys);
                        idx++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return commands;
        }

        private static bool isSameWindowToBottom(EnterKeysCommand aboveCommand, EnterKeysCommand bottomCommand, Core.ApplicationSettings appSettings)
        {
            if (aboveCommand.v_WindowName == bottomCommand.v_WindowName)
            {
                return true;
            }
            else if ((bottomCommand.v_WindowName == appSettings.EngineSettings.CurrentWindowKeyword) || 
                    //(bottomCommand.v_WindowName == appSettings.EngineSettings.wrapVariableMarker("Env.ActiveWindowTitle")))
                    (bottomCommand.v_WindowName == VariableNameControls.GetWrappedVariableName(Core.Automation.Engine.SystemVariables.Env_ActiveWindowTitle.VariableName, appSettings)) ||
                    (bottomCommand.v_WindowName == VariableNameControls.GetWrappedVariableName(Core.Automation.Engine.SystemVariables.Window_CurrentWindowName.VariableName, appSettings)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}

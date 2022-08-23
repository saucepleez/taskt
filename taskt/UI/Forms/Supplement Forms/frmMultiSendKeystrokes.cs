using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmMultiSendKeystrokes : ThemedForm
    {
        private Core.ApplicationSettings appSetttings;
        private List<Core.Script.ScriptVariable> scriptVariables;

        #region form events
        public frmMultiSendKeystrokes(Core.ApplicationSettings appSetttings, List<Core.Script.ScriptVariable> scriptVariables)
        {
            InitializeComponent();
            this.appSetttings = appSetttings;
            this.scriptVariables = scriptVariables;

            cmbSearchMethod.Items.AddRange(
                new string[] { "", "Contains", "Starts with", "Ends with", "Exact Match"}
            );
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
                    string variableName = appSetttings.EngineSettings.wrapVariableMarker((string)fm.selectedItem);
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
                    ConcatenateVariableName((string)fm.selectedItem, txtWaitTime, appSetttings);
                }
            }
        }
        #endregion

        #region Properties
        public string WindowName
        {
            get
            {
                return this.cmbWindowName.Text;
            }
        }

        public string SearchMethod
        {
            get
            {
                return this.SearchMethod;
            }
        }

        public string TextToSend
        {
            get
            {
                return this.txtTextToSend.Text;
            }
        }

        public string WaitTime
        {
            get
            {
                return this.txtWaitTime.Text;
            }
        }
        #endregion

        private void UpdateWindowNames()
        {
            string currentWindow = cmbWindowName.Text;

            var windowNames = Core.Automation.Commands.WindowNameControls.GetAllWindowTitles();

            cmbWindowName.BeginUpdate();
            cmbWindowName.Items.Clear();

            cmbWindowName.Items.Add(appSetttings.EngineSettings.CurrentWindowKeyword);
            cmbWindowName.Items.AddRange(windowNames.ToArray());

            if (cmbWindowName.Items.Contains(currentWindow))
            {
                cmbWindowName.Text = currentWindow;
            }

            cmbWindowName.EndUpdate();
        }

        private Supplemental.frmItemSelector CreateVariableSelectForm()
        {
            var variableList = scriptVariables.Select(f => f.VariableName).ToList();
            variableList.AddRange(Core.Common.GenerateSystemVariables().Select(f => f.VariableName));

            return (new Supplemental.frmItemSelector(variableList, "Select Variable", "Select Variable"));
        }

        private static void ConcatenateVariableName(string variableName, TextBox txt, Core.ApplicationSettings settings)
        {
            variableName = settings.EngineSettings.wrapVariableMarker(variableName);
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
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;

namespace taskt.UI.Forms.ScriptEngine.Supplemental
{
    public partial class frmUserInput : UIForm
    {
        /// <summary>
        /// frmUserInput button state
        /// </summary>
        public enum ButtonState
        {
            OKOnly,
            OKCancel,
            AcceptOnly,
            AcceptCancel
        }

        /// <summary>
        /// called ShowUserInputCommand clone
        /// </summary>
        private readonly ShowUserInputDialogCommand inputCommand;

        /// <summary>
        /// controls list
        /// </summary>
        private readonly List<Control> inputControls;

        /// <summary>
        /// dialog result value
        /// </summary>
        public string DialogResultText { get; private set; }

        public frmUserInput(ShowUserInputDialogCommand command)
        {
            InitializeComponent();

            this.DialogResult = DialogResult.None;  // set empty
            this.inputControls = new List<Control>();
            this.inputCommand = command;

            // get presentation data from command
            this.lblHeader.Text = inputCommand.v_InputHeader;
            this.lblDirections.Text = inputCommand.v_InputDirections;

            // get input table
            var inputTable = inputCommand.v_UserInputConfig;

            // loop each data collection point
            foreach (DataRow rw in inputTable.Rows)
            {
                //get properties to render controls with
                var fieldType = rw["Type"] as string;
                var fieldLabel = rw["Label"] as string;
                var fieldSize = rw["Size"] as string;

                // attempt to parse custom width/height
                int fieldWidth, fieldHeight;
                try
                {
                    // format should be X,Y
                    var fieldSizeData = fieldSize.Split(',');
                    fieldWidth = int.Parse(fieldSizeData[0].Trim());
                    fieldHeight = int.Parse(fieldSizeData[1].Trim());
                }
                catch (Exception)
                {
                    // if something goes wrong just use defaults
                    fieldWidth = 500;
                    fieldHeight = 100;
                }

                // get default value
                var defaultFieldValue = rw["DefaultValue"] as string;

                var labelingFont = new Font("Segoe UI Bold", 12);

                var label = new Label();

                switch (fieldType)
                {
                    // add more cases here
                    case "ComboBox":
                        // add label
                        label.AutoSize = true;
                        label.ForeColor = Color.SteelBlue;
                        label.Font = labelingFont;
                        label.Text = fieldLabel;
                        flwInputControls.Controls.Add(label);

                        var combobox = new ComboBox();
                        try
                        {
                            var items = defaultFieldValue.Split(',');
                            foreach (var comboItem in items)
                            {
                                combobox.Items.Add(comboItem.Trim());
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error Generating Control: " + ex.ToString());
                            return;
                        }

                        combobox.Width = fieldWidth;
                        combobox.Height = fieldHeight;
                        combobox.Margin = new Padding(10, 5, 0, 0);
                        combobox.DropDownStyle = ComboBoxStyle.DropDownList;
                        combobox.SelectedIndex = -1;
                        combobox.Font = labelingFont;
                        combobox.ForeColor = Color.SteelBlue;
                        this.inputControls.Add(combobox);
                        flwInputControls.Controls.Add(combobox);
                        break;

                    case "CheckBox":
                        var checkBox = new CheckBox();
                        try
                        {
                            checkBox.Checked = bool.Parse(defaultFieldValue);
                        }
                        catch (Exception)
                        {
                            checkBox.Checked = false;
                        }

                        checkBox.Width = fieldWidth;
                        checkBox.Height = fieldHeight;
                        checkBox.Margin = new Padding(10, 5, 0, 0);
                        checkBox.Text = fieldLabel;
                        checkBox.Font = labelingFont;
                        checkBox.ForeColor = Color.SteelBlue;
                        checkBox.AutoSize = true;

                        this.inputControls.Add(checkBox);
                        flwInputControls.Controls.Add(checkBox);
                        break;

                    default:
                        //add label 
                        label.AutoSize = true;
                        label.ForeColor = Color.SteelBlue;
                        label.Font = labelingFont;
                        label.Text = fieldLabel;
                        flwInputControls.Controls.Add(label);

                        //add textbox
                        var textBox = new TextBox();
                        textBox.Multiline = true;
                        textBox.Width = fieldWidth;
                        textBox.Height = fieldHeight;
                        textBox.Margin = new Padding(10, 5, 0, 0);
                        textBox.Text = defaultFieldValue;
                        textBox.Font = labelingFont;
                        textBox.ForeColor = Color.SteelBlue;
                        this.inputControls.Add(textBox);
                        flwInputControls.Controls.Add(textBox);
                        break;
                }
            }
        }

        public frmUserInput(ShowUserInputDialogCommand command, ButtonState button) : this(command)
        {
            switch (button)
            {
                case ButtonState.OKOnly:
                    uiBtnCancel.Visible = false;
                    break;
                case ButtonState.AcceptOnly:
                    uiBtnCancel.Visible = false;
                    uiBtnOk.Text = "Accept";
                    break;
                case ButtonState.AcceptCancel:
                    uiBtnOk.Text = "Accept";
                    break;
            }
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.DialogResultText = uiBtnOk.Text;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.Cancel;
            //this.DialogResultText = "Cancel";
            CancelProcess();
        }

        private void frmUserInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelProcess();
        }

        private void CancelProcess()
        {
            if (this.DialogResult == DialogResult.None)
            {
                this.DialogResult = DialogResult.Cancel;
                this.DialogResultText = "Cancel";
            }
        }


        private void frmUserInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        /// <summary>
        /// get specified values
        /// </summary>
        /// <returns></returns>
        public List<string> GetSpecifiedValues()
        {
            var ret = new List<string>();
            if (this.inputControls != null)
            {
                foreach (var ctrl in this.inputControls) 
                {
                    if (ctrl is CheckBox chk)
                    {
                        ret.Add(chk.Checked.ToString());
                    }
                    else
                    {
                        ret.Add(ctrl.Text);
                    }
                }
            }
            return ret;
        }

    }

    // MEMO: what is this class ?
    //public class UserInput
    //{
    //    public Control RenderedControl { get; set; }
    //}
}

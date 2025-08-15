using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message")]
    [Attributes.ClassAttributes.CommandSettings("Show User Input Dialog")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ShowUserInputDialogCommand : ScriptCommand, IDialogResultProperties, IHaveDataTableElements
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Dialog Title")]
        [InputSpecification("Title", true)]
        [SampleUsage("**Please Provide Input**")]
        [PropertyFirstValue("Please Provide Input")]
        [PropertyValidationRule("Title", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(true, "Title")]
        [PropertyParameterOrder(1000)]
        public string v_InputHeader { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Input Directions")]
        [InputSpecification("Input Directions", true)]
        [SampleUsage("**Please fill in the following fields**")]
        [PropertyFirstValue("Directions: Please fill in the following fields")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Input Direction", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Input Directions")]
        [PropertyParameterOrder(2000)]
        public string v_InputDirections { get; set; }

        [XmlElement]
        [PropertyDescription("User Input Parameters")]
        [InputSpecification("User Input", true)]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, true, true, 400, 250, true, 2)]
        [PropertyDataGridViewColumnSettings("Type", "Input Type", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox, "TextBox\nComboBox\nCheckBox", "TextBox")]
        [PropertyDataGridViewColumnSettings("Label", "Input Label", false)]
        [PropertyDataGridViewColumnSettings("Size", "Input Size (X,Y)", false)]
        [PropertyDataGridViewColumnSettings("DefaultValue", "Default Value", false)]
        [PropertyDataGridViewColumnSettings("UserInput", "User Input", false)]
        [PropertyDataGridViewColumnSettings("ApplyToVariable", "Apply To Variable", false)]
        [PropertyCustomUIHelper("Add Input Parameter", nameof(lnkAddInputParameter_Click), "addrow")]
        [PropertyCustomUIHelper("Delete Row", nameof(lnkDeleteInputParameter_Click), "deleterow")]
        [PropertyCustomUIHelper("Move Up Row", nameof(lnkUpInputParameter_Click), "uprow")]
        [PropertyCustomUIHelper("Move Down Row", nameof(lnkDownInputParameter_Click), "downrow")]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyParameterOrder(3000)]
        public DataTable v_UserInputConfig { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Dialog Buttons")]
        [PropertyUISelectionOption("OKCancel")]
        [PropertyUISelectionOption("OKOnly")]
        [PropertyUISelectionOption("AcceptCancel")]
        [PropertyUISelectionOption("AcceptOnly")]
        [PropertyIsOptional(true, "OKCancel")]
        [PropertyValidationRule("Dialog Buttons", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(11000)]
        public string v_DialogButtons { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ShowDialogControls), nameof(ShowDialogControls.v_WhenCancel))]
        [PropertyParameterOrder(12000)]
        public string v_WhenCancel { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ShowDialogControls), nameof(ShowDialogControls.v_DialogResult))]
        [PropertyParameterOrder(13000)]
        public string v_DialogResult { get; set; }

        public ShowUserInputDialogCommand()
        {
            //this.CommandName = "UserInputCommand";
            //this.SelectionName = "Prompt for Input";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("UserInput Supported With UI Only");
                MessageBox.Show("UserInput Supported With UI Only", "UserInput Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // create clone of original
            var clonedCommand = (ShowUserInputDialogCommand)this.Clone();

            // expand variables
            clonedCommand.v_InputHeader = clonedCommand.v_InputHeader.ExpandValueOrUserVariable(engine);
            clonedCommand.v_InputDirections = clonedCommand.v_InputDirections.ExpandValueOrUserVariable(engine);

            // expand variables for each label
            foreach (DataRow rw in clonedCommand.v_UserInputConfig.Rows)
            {
                rw["DefaultValue"] = (rw.Field<string>("DefaultValue") ?? "").ExpandValueOrUserVariable(engine);

                var targetVariable = rw["ApplyToVariable"] as string;

                if (string.IsNullOrEmpty(targetVariable))
                {
                    var newMessage = new ShowMessageCommand
                    {
                        v_Message = $"User Input question '{rw["Label"]}' is missing variables to apply results to! Results for the item will not be tracked.  To fix this, assign a variable in the designer!",
                        v_AutoCloseAfter = "10"
                    };
                    newMessage.RunCommand(engine);
                }
            }

            //// invoke ui for data collection
            //var result = engine.tasktEngineUI.Invoke(new Action(() =>
            //    {

            //        //get input from user
            //        var userInputs =  engine.tasktEngineUI.ShowInput(clonedCommand);

            //        //check if user provided input
            //        if (userInputs != null)
            //        {
            //            //loop through each input and assign
            //            for (int i = 0; i < userInputs.Count; i++)
            //            {
            //                var targetVariable = VariableNameControls.GetVariableName(v_UserInputConfig.Rows[i].Field<string>("ApplyToVariable") ?? "", engine);

            //                //store user data in variable
            //                if (!string.IsNullOrEmpty(targetVariable))
            //                {
            //                    userInputs[i].StoreInUserVariable(engine, targetVariable);
            //                }
            //            }
            //        }
            //    }
            //));

            var dialogButtons = (UI.Forms.ScriptEngine.Supplemental.frmUserInput.ButtonState)Enum.Parse(typeof(UI.Forms.ScriptEngine.Supplemental.frmUserInput.ButtonState), this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_DialogButtons), engine), true);

            var whenCancel = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenCancel), engine);

            void SetVariableValues(List<string> values)
            {
                for (int i = values.Count - 1; i >= 0; i--)
                {
                    var targetVariable = VariableNameControls.GetVariableName(v_UserInputConfig.Rows[i].Field<string>("ApplyToVariable") ?? "", engine);

                    // store user data in variable
                    if (!string.IsNullOrEmpty(targetVariable))
                    {
                        values[i].StoreInUserVariable(engine, targetVariable);
                    }
                }
            }

            engine.tasktEngineUI.Invoke(new Action(() =>
            {
                //var responses = new List<string>();
                using (var fm = new UI.Forms.ScriptEngine.Supplemental.frmUserInput(clonedCommand, dialogButtons))
                {
                    if (whenCancel == "show dialog again")
                    {
                        bool isAgain = true;
                        do
                        {
                            //if (fm.ShowDialog() == DialogResult.OK)
                            //{
                            //    isAgain = false;
                            //    SetVariableValues(fm.GetSpecifiedValues());
                            //}
                            var r = fm.ShowDialog();
                            if (r == DialogResult.OK)
                            {
                                isAgain = false;
                                SetVariableValues(fm.GetSpecifiedValues());
                            }
                        } while (isAgain);
                        this.StoreDialogResultInUserVariable(fm.DialogResultText, engine);
                    }
                    else
                    {
                        if (fm.ShowDialog() == DialogResult.OK)
                        {
                            SetVariableValues(fm.GetSpecifiedValues());
                            this.StoreDialogResultInUserVariable(fm.DialogResultText, engine);
                        }
                        else
                        {
                            switch (whenCancel)
                            {
                                case "error":
                                    throw new Exception("Error. UserInput Dialog is Clicked Cancel.");
                                    
                                case "ignore":
                                    break;

                                case "set empty":
                                    for (int i = v_UserInputConfig.Rows.Count - 1; i >= 0; i--)
                                    {
                                        var targetVariable = VariableNameControls.GetVariableName(v_UserInputConfig.Rows[i].Field<string>("ApplyToVariable") ?? "", engine);
                                        "".StoreInUserVariable(engine, targetVariable);
                                    }
                                    break;
                            }
                            this.StoreDialogResultInUserVariable("Cancel", engine);
                        }
                    }
                }
            }));
        }

        private void lnkAddInputParameter_Click(object sender, EventArgs e)
        {
            var newRow = v_UserInputConfig.NewRow();
            newRow["Size"] = "500,100";
            v_UserInputConfig.Rows.Add(newRow);
        }

        private void lnkUpInputParameter_Click(object sender, EventArgs e)
        {
            var dgv = this.ControlsList.GetPropertyControl<DataGridView>(nameof(v_UserInputConfig));
            var row = dgv.CurrentCell.RowIndex;
            if (row > 0)
            {
                SwapUserInputParameterRows(row, row - 1);
                dgv.CurrentCell = dgv[dgv.CurrentCell.ColumnIndex, row - 1];
            }
        }

        private void lnkDownInputParameter_Click(object sender, EventArgs e)
        {
            var dgv = this.ControlsList.GetPropertyControl<DataGridView>(nameof(v_UserInputConfig));
            var row = dgv.CurrentCell.RowIndex;
            if (row < dgv.Rows.Count - 1)
            {
                SwapUserInputParameterRows(row, row + 1);
                dgv.CurrentCell = dgv[dgv.CurrentCell.ColumnIndex, row + 1];
            }
        }

        private void lnkDeleteInputParameter_Click(object sender, EventArgs e)
        {
            var dgv = this.ControlsList.GetPropertyControl<DataGridView>(nameof(v_UserInputConfig));
            var row = dgv.CurrentCell.RowIndex;
            v_UserInputConfig.Rows[row].Delete();
        }

        /// <summary>
        /// swap v_UserInputConfig parameters value
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void SwapUserInputParameterRows(int a, int b)
        {
            //var dgv = this.ControlsList.GetPropertyControl<DataGridView>(nameof(v_UserInputConfig));
            
            for (int i = v_UserInputConfig.Columns.Count - 1; i > 0; i--) 
            {
                var va = v_UserInputConfig.Rows[a][i]?.ToString() ?? "";
                var vb = v_UserInputConfig.Rows[b][i]?.ToString() ?? "";
                v_UserInputConfig.Rows[a][i] = vb;
                v_UserInputConfig.Rows[b][i] = va;
            }
        }

        public override bool IsValidate(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.IsValidate(editor);

            
            int dgvRows;
            DataGridView dgv = null;
            try
            {
                dgv = this.ControlsList.GetPropertyControl<DataGridView>(nameof(v_UserInputConfig));
                dgvRows = dgv.Rows.Count;
            }
            catch
            {
                dgvRows = v_UserInputConfig.Rows.Count;
            }

            //var rows = (dgvRows < v_UserInputConfig.Rows.Count) ? dgvRows : v_UserInputConfig.Rows.Count;

            DataTable targetDT;
            int rows;
            if (dgvRows < v_UserInputConfig.Rows.Count)
            {
                rows = dgvRows;
                targetDT = new DataTable();
                targetDT.Columns.Add("Type");
                targetDT.Columns.Add("Label");
                targetDT.Columns.Add("Size");
                targetDT.Columns.Add("DefaultValue");
                targetDT.Columns.Add("UserInput");
                targetDT.Columns.Add("ApplyToVariable");
                int cols = dgv.Columns.Count;
                for (int i = 0; i < rows; i++)
                {
                    targetDT.Rows.Add();
                    for (int j = 0; j < cols; j++)
                    {
                        targetDT.Rows[i][j] = dgv[j, i].Value?.ToString() ?? "";
                    }
                }
            }
            else
            {
                rows = v_UserInputConfig.Rows.Count;
                targetDT = v_UserInputConfig;
            }

            for (int i = 0; i < rows; i++)
            {
                var row = targetDT.Rows[i];
                var showIndex = i + 1;

                if (string.IsNullOrEmpty(row.Field<string>("Type")))
                {
                    this.validationResult += $"Input Type #{showIndex} is empty.\n";
                    this.IsValid = false;
                }
                if (string.IsNullOrEmpty(row.Field<string>("Size")))
                {
                    this.validationResult += $"Input Size #{showIndex} is empty.\n";
                    this.IsValid = false;
                }
            }

            return this.IsValid;
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            DataTableControls.BeforeValidate_NoRowAdding((DataGridView)ControlsList[nameof(v_UserInputConfig)], v_UserInputConfig);
        }
    }
}
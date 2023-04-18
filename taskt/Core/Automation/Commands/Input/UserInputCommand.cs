using System;
using System.Xml.Serialization;
using System.Data;
using taskt.UI.Forms;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.CommandSettings("Prompt for Input")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UserInputCommand : ScriptCommand
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
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_UserInputConfig { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView UserInputGridViewHelper;

        //[XmlIgnore]
        //[NonSerialized]
        //private CommandItemControl AddRowControl;

        public UserInputCommand()
        {
            //this.CommandName = "UserInputCommand";
            //this.SelectionName = "Prompt for Input";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //v_UserInputConfig = new DataTable();
            //v_UserInputConfig.TableName = DateTime.Now.ToString("UserInputParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            //v_UserInputConfig.Columns.Add("Type");
            //v_UserInputConfig.Columns.Add("Label");
            //v_UserInputConfig.Columns.Add("Size");
            //v_UserInputConfig.Columns.Add("DefaultValue");
            //v_UserInputConfig.Columns.Add("UserInput");
            //v_UserInputConfig.Columns.Add("ApplyToVariable");

            //this.v_InputHeader = "Please Provide Input";
            //this.v_InputDirections = "Directions: Please fill in the following fields";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
               
            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("UserInput Supported With UI Only");
                MessageBox.Show("UserInput Supported With UI Only", "UserInput Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            //create clone of original
            var clonedCommand = Common.Clone(this);

            //translate variable
            clonedCommand.v_InputHeader = clonedCommand.v_InputHeader.ConvertToUserVariable(engine);
            clonedCommand.v_InputDirections = clonedCommand.v_InputDirections.ConvertToUserVariable(engine);

            //translate variables for each label
            foreach (DataRow rw in clonedCommand.v_UserInputConfig.Rows)
            {
                //rw["DefaultValue"] = rw["DefaultValue"].ToString().ConvertToUserVariable(engine);
                rw["DefaultValue"] = (rw.Field<string>("DefaultValue") ?? "").ConvertToUserVariable(engine);

                var targetVariable = rw["ApplyToVariable"] as string;

                if (string.IsNullOrEmpty(targetVariable))
                {
                    var newMessage = new ShowMessageCommand
                    {
                        v_Message = "User Input question '" + rw["Label"] + "' is missing variables to apply results to! Results for the item will not be tracked.  To fix this, assign a variable in the designer!",
                        v_AutoCloseAfter = "10"
                    };
                    newMessage.RunCommand(engine);
                }
            }


            //invoke ui for data collection
            var result = engine.tasktEngineUI.Invoke(new Action(() =>
                {

                    //get input from user
                    var userInputs =  engine.tasktEngineUI.ShowInput(clonedCommand);

                    //check if user provided input
                    if (userInputs != null)
                    {
                        //loop through each input and assign
                        for (int i = 0; i < userInputs.Count; i++)
                        {
                            //get target variable
                            //var targetVariable = v_UserInputConfig.Rows[i]["ApplyToVariable"] as string;


                            ////if engine is expected to create variables, the user will not expect them to contain start/end markers
                            ////ex. {vAge} should not be created, vAge should be created and then called by doing {vAge}
                            //if ((!string.IsNullOrEmpty(targetVariable)) && (engine.engineSettings.CreateMissingVariablesDuringExecution))
                            //{
                            //    //remove start markers
                            //    if (targetVariable.StartsWith(engine.engineSettings.VariableStartMarker))
                            //    {
                            //        targetVariable = targetVariable.TrimStart(engine.engineSettings.VariableStartMarker.ToCharArray());
                            //    }

                            //    //remove end markers
                            //    if (targetVariable.EndsWith(engine.engineSettings.VariableEndMarker))
                            //    {
                            //        targetVariable = targetVariable.TrimEnd(engine.engineSettings.VariableEndMarker.ToCharArray());
                            //    }
                            //}

                            var targetVariable = VariableNameControls.GetVariableName(v_UserInputConfig.Rows[i].Field<string>("ApplyToVariable") ?? "", engine);
                            
                            //store user data in variable
                            if (!string.IsNullOrEmpty(targetVariable))
                            {
                                userInputs[i].StoreInUserVariable(engine, targetVariable);
                            }
                        }
                    }
                }
            ));
        }

        private void lnkAddInputParameter_Click(object sender, EventArgs e)
        {
            var newRow = v_UserInputConfig.NewRow();
            newRow["Size"] = "500,100";
            v_UserInputConfig.Rows.Add(newRow);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //UserInputGridViewHelper = new DataGridView();
        //    //UserInputGridViewHelper.DataBindings.Add("DataSource", this, "v_UserInputConfig", false, DataSourceUpdateMode.OnPropertyChanged);
        //    //UserInputGridViewHelper = CommandControls.CreateDataGridView(this, "v_UserInputConfig", true, true, false, 400, 250, true, 2);
        //    //UserInputGridViewHelper.CellClick += UserInputGridViewHelper_CellClick;

        //    //var typefield = new DataGridViewComboBoxColumn();
        //    //typefield.Items.Add("TextBox");
        //    //typefield.Items.Add("CheckBox");
        //    //typefield.Items.Add("ComboBox");
        //    //typefield.HeaderText = "Input Type";
        //    //typefield.DataPropertyName = "Type";
        //    //UserInputGridViewHelper.Columns.Add(typefield);

        //    //var field = new DataGridViewTextBoxColumn();
        //    //field.HeaderText = "Input Label";
        //    //field.DataPropertyName = "Label";
        //    //UserInputGridViewHelper.Columns.Add(field);


        //    //field = new DataGridViewTextBoxColumn();
        //    //field.HeaderText = "Input Size (X,Y)";
        //    //field.DataPropertyName = "Size";
        //    //UserInputGridViewHelper.Columns.Add(field);

        //    //field = new DataGridViewTextBoxColumn();
        //    //field.HeaderText = "Default Value";
        //    //field.DataPropertyName = "DefaultValue";
        //    //UserInputGridViewHelper.Columns.Add(field);

        //    //field = new DataGridViewTextBoxColumn();
        //    //field.HeaderText = "Assigned Variable";
        //    //field.DataPropertyName = "ApplyToVariable";
        //    //UserInputGridViewHelper.Columns.Add(field);


        //    //UserInputGridViewHelper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        //    //UserInputGridViewHelper.AllowUserToAddRows = false;
        //    //UserInputGridViewHelper.AllowUserToDeleteRows = false;


        //    //AddRowControl = new CommandItemControl();
        //    //AddRowControl.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
        //    //AddRowControl.ForeColor = Color.AliceBlue;
        //    //AddRowControl.Font = new Font("Segoe UI Semilight", 10);
        //    //AddRowControl.CommandImage = UI.Images.GetUIImage("ExecuteDLLCommand");
        //    //AddRowControl.CommandDisplay = "Add Input Parameter";
        //    //AddRowControl.Click += (sender, e) => AddInputParameter(sender, e, editor);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputHeader", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputDirections", this, editor));
        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UserInputConfig", this));
        //    //RenderedControls.Add(AddRowControl);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_UserInputConfig", this, new Control[] { UserInputGridViewHelper }, editor));
        //    //RenderedControls.Add(UserInputGridViewHelper);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    UserInputGridViewHelper = (DataGridView)ctrls.Where(t => (t.Name == "v_UserInputConfig")).FirstOrDefault();
        //    //UserInputGridViewHelper.CellClick += UserInputGridViewHelper_CellClick;
        //    UserInputGridViewHelper.CellClick += DataTableControls.AllEditableDataGridView_CellClick;

        //    return RenderedControls;

        //}

        //private void AddInputParameter(object sender, EventArgs e, frmCommandEditor editor)
        //{
        //    var newRow = v_UserInputConfig.NewRow();
        //    newRow["Size"] = "500,100";
        //    v_UserInputConfig.Rows.Add(newRow);
        //}



        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [" + v_InputHeader + "]";
        //}


        //private void UserInputGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (UserInputGridViewHelper.Rows.Count == 0)
        //    {
        //        return;
        //    }
        //    if (e.ColumnIndex >= 0)
        //    {
        //        if (e.ColumnIndex == 0)
        //        {
        //            if (UserInputGridViewHelper.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
        //            {
        //                SendKeys.Send("{F4}");
        //            }
        //        }
        //        else
        //        {
        //            UserInputGridViewHelper.BeginEdit(false);
        //        }
        //    }
        //    else
        //    {
        //        UserInputGridViewHelper.EndEdit();
        //    }
        //}

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            for (int i = 0; i < v_UserInputConfig.Rows.Count; i++)
            {
                var row = v_UserInputConfig.Rows[i];
                if (String.IsNullOrEmpty(row["Type"].ToString()))
                {
                    this.validationResult += "Input Type #" + (i + 1) + " is empty.\n";
                    this.IsValid = false;
                }
                if (String.IsNullOrEmpty(row["Size"].ToString()))
                {
                    this.validationResult += "Input Size #" + (i + 1) + " is empty.\n";
                    this.IsValid = false;
                }
            }

            return this.IsValid;
        }

    }
}
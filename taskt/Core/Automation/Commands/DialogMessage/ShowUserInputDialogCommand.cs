using System;
using System.Xml.Serialization;
using System.Data;
using taskt.UI.Forms;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message Commands")]
    [Attributes.ClassAttributes.CommandSettings("Show User Input Dialog")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ShowUserInputDialogCommand : ScriptCommand
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

        public ShowUserInputDialogCommand()
        {
            //this.CommandName = "UserInputCommand";
            //this.SelectionName = "Prompt for Input";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
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

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            for (int i = 0; i < v_UserInputConfig.Rows.Count; i++)
            {
                var row = v_UserInputConfig.Rows[i];
                
                if (String.IsNullOrEmpty(row.Field<string>("Type")))
                {
                    this.validationResult += "Input Type #" + (i + 1) + " is empty.\n";
                    this.IsValid = false;
                }
                if (String.IsNullOrEmpty(row.Field<string>("Size")))
                {
                    this.validationResult += "Input Size #" + (i + 1) + " is empty.\n";
                    this.IsValid = false;
                }
            }

            return this.IsValid;
        }
    }
}
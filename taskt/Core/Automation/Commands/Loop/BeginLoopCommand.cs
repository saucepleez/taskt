using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to evaluate a logical statement to determine if the statement is true. The following actions will repeat continuously until that statement becomes false")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check if a statement is 'true' or 'false' and subsequently loop actions based on either condition. Any 'BeginLoop' command must have a following 'EndLoop' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command evaluates supplied arguments and if evaluated to true runs sub elements")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    public class BeginLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select type of Loop Command")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Numeric Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Date Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Text Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Variable Has Value")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Variable Is Numeric")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Window Name Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Active Window Name Is")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Folder Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Web Element Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("GUI Element Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error Occured")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error Did Not Occur")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Boolean")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Boolean Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("List Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Dictionary Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("DataTable Compare")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary comparison type.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Value**, **Window Name Exists**, **Active Window Name Is**, **File Exists**, **Folder Exists**, **Web Element Exists**, **Error Occured**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_LoopActionType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select the required comparison parameters.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_LoopActionParameterTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView LoopGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox ActionDropdown;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> ParameterControls;

        //[XmlIgnore]
        //[NonSerialized]
        //CommandItemControl RecorderControl;

        [XmlIgnore]
        [NonSerialized]
        CommandItemControl lnkBrowserInstanceSelector;

        [XmlIgnore]
        [NonSerialized]
        CommandItemControl lnkWindowNameSelector;

        [XmlIgnore]
        [NonSerialized]
        CommandItemControl lnkBooleanSelector;

        public BeginLoopCommand()
        {
            this.CommandName = "BeginLoopCommand";
            this.SelectionName = "Begin Loop";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //define parameter table
            this.v_LoopActionParameterTable = new System.Data.DataTable
            {
                TableName = DateTime.Now.ToString("LoopActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };
            this.v_LoopActionParameterTable.Columns.Add("Parameter Name");
            this.v_LoopActionParameterTable.Columns.Add("Parameter Value");
        }

        private void LoopGridViewHelper_MouseEnter(object sender, EventArgs e, UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            loopAction_SelectionChangeCommitted(null, null, editor);
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Core.Script.ScriptAction parentCommand)
        {
            string actionType = v_LoopActionType.ExpandValueOrUserVariable(engine);

            bool loopResult = ConditionControls.DetermineStatementTruth(actionType, v_LoopActionParameterTable, engine);
            engine.ReportProgress("Starting Loop"); 

            while (loopResult)
            {
                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop"); 
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress("Continuing Next Loop"); 
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }
                //loopResult = DetermineStatementTruth(sender);
                loopResult = ConditionControls.DetermineStatementTruth(actionType, v_LoopActionParameterTable, engine);
            }
        }

        public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.Render(editor);

            //LoopGridViewHelper = new DataGridView();
            //LoopGridViewHelper.AllowUserToAddRows = true;
            //LoopGridViewHelper.AllowUserToDeleteRows = true;
            //LoopGridViewHelper.Size = new Size(400, 250);
            //LoopGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //LoopGridViewHelper.DataBindings.Add("DataSource", this, "v_LoopActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            //LoopGridViewHelper.AllowUserToAddRows = false;
            //LoopGridViewHelper.AllowUserToDeleteRows = false;
            LoopGridViewHelper = CommandControls.CreateDefaultDataGridViewFor("v_LoopActionParameterTable", this, false, false);
            LoopGridViewHelper.MouseEnter += (sender, e) => LoopGridViewHelper_MouseEnter(sender, e, editor);
            //LoopGridViewHelper.CellBeginEdit += LoopGridViewHelper_CellBeginEdit;
            //LoopGridViewHelper.CellClick += LoopGridViewHelper_CellClick;
            LoopGridViewHelper.CellBeginEdit += DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit;
            LoopGridViewHelper.CellClick += DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick;

            //var helperTheme = editor.Theme.UIHelper;
            //RecorderControl = new taskt.UI.CustomControls.CommandItemControl();
            //RecorderControl.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            ////RecorderControl.ForeColor = Color.AliceBlue;
            //RecorderControl.Font = new Font(helperTheme.Font, helperTheme.FontSize, helperTheme.Style);
            //RecorderControl.ForeColor = helperTheme.FontColor;
            //RecorderControl.BackColor = helperTheme.BackColor;
            //RecorderControl.Name = "guirecorder_helper";
            //RecorderControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
            //RecorderControl.CommandDisplay = "Element Recorder";
            //RecorderControl.Click += ShowLoopElementRecorder;
            //RecorderControl.Hide();

            ActionDropdown = (ComboBox)CommandControls.CreateDefaultDropdownFor("v_LoopActionType", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_LoopActionType", this));
            RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_LoopActionType", this, ActionDropdown, editor));
            ActionDropdown.SelectionChangeCommitted += (sender, e) => loopAction_SelectionChangeCommitted(sender, e, editor);

            RenderedControls.Add(ActionDropdown);

            ParameterControls = new List<Control>();
            ParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_LoopActionParameterTable", this));
            //ParameterControls.Add(RecorderControl);

            var helpers = CommandControls.CreateDefaultUIHelpersFor("v_LoopActionParameterTable", this, LoopGridViewHelper, editor);

            lnkBrowserInstanceSelector = CommandControls.CreateSimpleUIHelper(nameof(v_LoopActionParameterTable) + "_customhelper_0", LoopGridViewHelper);
            lnkBrowserInstanceSelector.Name = "v_LoopActionParameterTable_helper_WebBrowser";
            lnkBrowserInstanceSelector.CommandDisplay = "Select WebBrowser Instance";
            lnkBrowserInstanceSelector.Click += (sender, e) => linkWebBrowserInstanceSelector_Click(sender, e, editor);
            helpers.Add(lnkBrowserInstanceSelector);

            lnkWindowNameSelector = CommandControls.CreateSimpleUIHelper(nameof(v_LoopActionParameterTable) + "_customhelper_1", LoopGridViewHelper);
            lnkWindowNameSelector.Name = "v_LoopActionParameterTable_helper_WindowName";
            lnkWindowNameSelector.CommandDisplay = "Select Window Name";
            lnkWindowNameSelector.Click += (sender, e) => linkWindowNameSelector_Click(sender, e, editor);
            helpers.Add(lnkWindowNameSelector);

            lnkBooleanSelector = CommandControls.CreateSimpleUIHelper(nameof(v_LoopActionParameterTable) + "_customhelper_2", LoopGridViewHelper);
            lnkBooleanSelector.Name = "v_LoopActionParameterTable_helper_Boolean";
            lnkBooleanSelector.CommandDisplay = "Select Boolean Instance";
            lnkBooleanSelector.Click += (sender, e) => linkBooleanInstanceSelector_Click(sender, e, editor);
            helpers.Add(lnkBooleanSelector);

            ParameterControls.AddRange(helpers);
            ParameterControls.Add(LoopGridViewHelper);

            RenderedControls.AddRange(ParameterControls);

            return RenderedControls;
        }

        private void loopAction_SelectionChangeCommitted(object sender, EventArgs e, UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            ComboBox loopAction = (ComboBox)ActionDropdown;
            //DataGridView loopActionParameterBox = (DataGridView)LoopGridViewHelper;

            Core.Automation.Commands.BeginLoopCommand cmd = (Core.Automation.Commands.BeginLoopCommand)this;
            DataTable actionParameters = cmd.v_LoopActionParameterTable;

            //sender is null when command is updating
            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }

            lnkBrowserInstanceSelector.Hide();
            lnkWindowNameSelector.Hide();

            switch (loopAction.SelectedItem)
            {
                //case "Value":
                case "Numeric Compare":
                case "Date Compare":
                    ConditionControls.RenderNumericCompare(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;


                //case "Variable Compare":
                case "Text Compare":
                    ConditionControls.RenderTextCompare(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;


                case "Variable Has Value":
                case "Variable Is Numeric":
                    ConditionControls.RenderVariableIsHas(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;

                case "Error Occured":
                case "Error Did Not Occur":
                    ConditionControls.RenderErrorOccur(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;

                case "Window Name Exists":
                case "Active Window Name Is":
                    ConditionControls.RenderWindowName(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    lnkWindowNameSelector.Show();
                    break;

                case "File Exists":
                    ConditionControls.RenderFileExists(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;

                case "Folder Exists":
                    ConditionControls.RenderFolderExists(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;

                case "Web Element Exists":
                    ConditionControls.RenderWebElement(sender, LoopGridViewHelper, v_LoopActionParameterTable, editor.appSettings);
                    lnkBrowserInstanceSelector.Show();
                    break;

                case "GUI Element Exists":
                    ConditionControls.RenderGUIElement(sender, LoopGridViewHelper, v_LoopActionParameterTable, editor.appSettings);
                    lnkWindowNameSelector.Show();
                    break;

                case "Boolean":
                    ConditionControls.RenderBoolean(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;

                case "Boolean Compare":
                    ConditionControls.RenderBooleanCompare(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;

                case "List Compare":
                    ConditionControls.RenderListCompare(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;

                case "Dictionary Compare":
                    ConditionControls.RenderDictionaryCompare(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;

                case "DataTable Compare":
                    ConditionControls.RenderDataTableCompare(sender, LoopGridViewHelper, v_LoopActionParameterTable);
                    break;

                default:
                    break;
            }
        }

        private void linkWebBrowserInstanceSelector_Click(object sender, EventArgs e, UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var instances = editor.instanceList.getInstanceClone(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);

            using (var frm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmItemSelector(instances.Keys.ToList<string>()))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string selectedItem = frm.selectedItem.ToString();
                    if (!DataTableControls.SetParameterValue(v_LoopActionParameterTable, selectedItem, "Selenium Instance Name", "Parameter Name", "Parameter Value"))
                    {
                        throw new Exception("Fail update Selenium Instance Name");
                    }
                }
            }
        }

        private void linkWindowNameSelector_Click(object sender, EventArgs e, UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            List<string> windowNames = new List<string>
            {
                //editor.appSettings.EngineSettings.CurrentWindowKeyword
                VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Window_CurrentWindowName.VariableName, editor.appSettings),
            };

            System.Diagnostics.Process[] processlist = System.Diagnostics.Process.GetProcesses();
            //pull the main window title for each
            foreach (var process in processlist)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //add to the control list of available windows
                    windowNames.Add(process.MainWindowTitle);
                }
            }

            using (var frm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmItemSelector(windowNames))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string selectedItem = frm.selectedItem.ToString();
                    if (!DataTableControls.SetParameterValue(v_LoopActionParameterTable, selectedItem, "Window Name", "Parameter Name", "Parameter Value"))
                    {
                        throw new Exception("Fail update Window Name");
                    }
                }
            }
        }

        private void linkBooleanInstanceSelector_Click(object sender, EventArgs e, UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var instances = editor.instanceList.getInstanceClone(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Boolean);

            using (var frm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmItemSelector(instances.Keys.ToList<string>()))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string selectedItem = frm.selectedItem.ToString();
                    //int currentRow = IfGridViewHelper.CurrentRow.Index;
                    string parameterName = DataTableControls.GetFieldValue(v_LoopActionParameterTable, LoopGridViewHelper.CurrentRow.Index, "Parameter Name");

                    switch (v_LoopActionType.ToLower())
                    {
                        case "boolean":
                            DataTableControls.SetParameterValue(v_LoopActionParameterTable, selectedItem, "Variable Name", "Parameter Name", "Parameter Value");
                            break;

                        case "boolean compare":
                            switch (parameterName)
                            {
                                case "Value1":
                                case "Value2":
                                    DataTableControls.SetParameterValue(v_LoopActionParameterTable, selectedItem, parameterName, "Parameter Name", "Parameter Value");
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        public override string GetDisplayValue()
        {
            return ConditionControls.GetDisplayValue("Loop While", v_LoopActionType, v_LoopActionParameterTable);
        }

        public override bool IsValidate(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_LoopActionType))
            {
                this.validationResult += "Type is empty.";
                this.IsValid = false;
            }
            else
            {
                string message;
                bool res = true;
                switch (this.v_LoopActionType)
                {
                    //case "Value":
                    case "Numeric Compare":
                    case "Date Compare":
                    //case "Variable Compare":
                    case "Text Compare":
                        res = ConditionControls.ValueValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "Variable Has Value":
                    case "Variable Is Numeric":
                        res = ConditionControls.VariableValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "Window Name Exists":
                    case "Active Window Name Is":
                        res = ConditionControls.WindowValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "File Exists":
                        res = ConditionControls.FileValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "Folder Exists":
                        res = ConditionControls.FolderValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "Web Element Exists":
                        res = ConditionControls.WebValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "GUI Element Exists":
                        res = ConditionControls.GUIValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "Error Occured":
                    case "Error Did Not Occur":
                        res = ConditionControls.ErrorValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "Boolean":
                        res = ConditionControls.BooleanValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "Boolean Compare":
                        res = ConditionControls.BooleanCompareValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "List Compare":
                        res = ConditionControls.ListCompareValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "Dictionary Compare":
                        res = ConditionControls.DictionaryCompareValidate(v_LoopActionParameterTable, out message);
                        break;

                    case "DataTable Compare":
                        res = ConditionControls.DataTableCompareValidate(v_LoopActionParameterTable, out message);
                        break;

                    default:
                        message = "Strange Action Parameter";
                        res = false;
                        break;
                }

                if (!res)
                {
                    this.validationResult += message;
                    this.IsValid = false;
                }
            }

            return this.IsValid;
        }

        public override void ConvertToIntermediate(EngineSettings settings, List<Core.Script.ScriptVariable> variables)
        {
            if (this.v_LoopActionType == "GUI Element Exists")
            {
                var cnv = new Dictionary<string, string>();
                cnv.Add("v_LoopActionParameterTable", "convertToIntermediateWindowName");
                ConvertToIntermediate(settings, cnv, variables);
            }
            else
            {
                base.ConvertToIntermediate(settings, variables);
            }
        }

        public override void ConvertToRaw(EngineSettings settings)
        {
            if (this.v_LoopActionType == "GUI Element Exists")
            {
                var cnv = new Dictionary<string, string>();
                cnv.Add("v_LoopActionParameterTable", "convertToRawWindowName");
                ConvertToRaw(settings, cnv);
            }
            else
            {
                base.ConvertToRaw(settings);
            }
        }
    }
}

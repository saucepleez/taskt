﻿using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to evaluate a logical statement to determine if the statement is true.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check if a statement is 'true' or 'false' and subsequently take an action based on either condition. Any 'BeginIf' command must have a following 'EndIf' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command evaluates supplied arguments and if evaluated to true runs sub elements")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_begin_if))]
    public class BeginIfCommand : ScriptCommand, IHaveDataTableElements
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select type of If Command")]
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
        [Attributes.PropertyAttributes.SampleUsage("Select **Value**, **Window Name Exists**, **Active Window Name Is**, **File Exists**, **Folder Exists**, **Web Element Exists**, **Error Occured**, **Boolean**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_IfActionType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select the required comparison parameters.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_IfActionParameterTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView IfGridViewHelper;

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

        public BeginIfCommand()
        {
            this.CommandName = "BeginIfCommand";
            this.SelectionName = "Begin If";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //define parameter table
            this.v_IfActionParameterTable = new DataTable
            {
                TableName = DateTime.Now.ToString("IfActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };
            this.v_IfActionParameterTable.Columns.Add("Parameter Name");
            this.v_IfActionParameterTable.Columns.Add("Parameter Value");
        }

        private void IfGridViewHelper_MouseEnter(object sender, EventArgs e, UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            ifAction_SelectionChangeCommitted(null, null, editor);
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            bool ifResult = ConditionControls.DetermineStatementTruth(v_IfActionType, v_IfActionParameterTable, engine);

            int startIndex, endIndex, elseIndex;
            if (parentCommand.AdditionalScriptCommands.Any(item => item.ScriptCommand is ElseCommand))
            {
                elseIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is ElseCommand);

                if (ifResult)
                {
                    startIndex = 0;
                    endIndex = elseIndex;
                }
                else
                {
                    startIndex = elseIndex + 1;
                    endIndex = parentCommand.AdditionalScriptCommands.Count;
                }
            }
            else if (ifResult)
            {
                startIndex = 0;
                endIndex = parentCommand.AdditionalScriptCommands.Count;
            }
            else
            {
                return;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                if ((engine.IsCancellationPending) || (engine.CurrentLoopCancelled) || (engine.CurrentLoopContinuing))
                    return;

                engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[i]);
            }
        }

        public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.Render(editor);

            //IfGridViewHelper = new DataGridView();
            //IfGridViewHelper.AllowUserToAddRows = true;
            //IfGridViewHelper.AllowUserToDeleteRows = true;
            //IfGridViewHelper.Size = new Size(400, 250);
            //IfGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //IfGridViewHelper.DataBindings.Add("DataSource", this, "v_IfActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            //IfGridViewHelper.AllowUserToAddRows = false;
            //IfGridViewHelper.AllowUserToDeleteRows = false;
            IfGridViewHelper = CommandControls.CreateDefaultDataGridViewFor("v_IfActionParameterTable", this, false, false, false, 400, 200);
            IfGridViewHelper.MouseEnter += (sender, e) => IfGridViewHelper_MouseEnter(sender,e, editor);
            //IfGridViewHelper.CellClick += IfGridViewHelper_CellClick;
            //IfGridViewHelper.CellBeginEdit += IfGridViewHelper_CellBeginEdit;
            IfGridViewHelper.CellClick += DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick;
            IfGridViewHelper.CellBeginEdit += DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit;

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
            //RecorderControl.Click += ShowIfElementRecorder;
            //RecorderControl.Hide();

            ActionDropdown = (ComboBox)CommandControls.CreateDefaultDropdownFor("v_IfActionType", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_IfActionType", this));
            RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_IfActionType", this, ActionDropdown, editor));
            ActionDropdown.SelectionChangeCommitted += (sender, e) => ifAction_SelectionChangeCommitted(sender, e, editor);

            RenderedControls.Add(ActionDropdown);

            ParameterControls = new List<Control>();
            ParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_IfActionParameterTable", this));
            //ParameterControls.Add(RecorderControl);

            var helpers = CommandControls.CreateDefaultUIHelpersFor("v_IfActionParameterTable", this, IfGridViewHelper, editor);

            lnkBrowserInstanceSelector = CommandControls.CreateSimpleUIHelper(nameof(v_IfActionParameterTable) + "_customhelper_0", IfGridViewHelper);
            lnkBrowserInstanceSelector.Name = "v_IfActionParameterTable_helper_WebBrowser";
            lnkBrowserInstanceSelector.CommandDisplay = "Select WebBrowser Instance";
            lnkBrowserInstanceSelector.Click += (sender, e) => linkWebBrowserInstanceSelector_Click(sender, e, editor);
            //RenderedControls.Add(lnkBrowserInstance);
            helpers.Add(lnkBrowserInstanceSelector);

            lnkWindowNameSelector = CommandControls.CreateSimpleUIHelper(nameof(v_IfActionParameterTable) + "_customhelper_1", IfGridViewHelper);
            lnkWindowNameSelector.Name = "v_IfActionParameterTable_helper_WindowName";
            lnkWindowNameSelector.CommandDisplay = "Select Window Name";
            lnkWindowNameSelector.Click += (sender, e) => linkWindowNameSelector_Click(sender, e, editor);
            helpers.Add(lnkWindowNameSelector);

            lnkBooleanSelector = CommandControls.CreateSimpleUIHelper(nameof(v_IfActionParameterTable) + "_customhelper_2", IfGridViewHelper);
            lnkBooleanSelector.Name = "v_IfActionParameterTable_helper_Boolean";
            lnkBooleanSelector.CommandDisplay = "Select Boolean Instance";
            lnkBooleanSelector.Click += (sender, e) => linkBooleanInstanceSelector_Click(sender, e, editor);
            helpers.Add(lnkBooleanSelector);

            ParameterControls.AddRange(helpers);
            ParameterControls.Add(IfGridViewHelper);

            RenderedControls.AddRange(ParameterControls);

            return RenderedControls;
        }

        private void linkWebBrowserInstanceSelector_Click(object sender, EventArgs e, UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var instances = editor.instanceList.getInstanceClone(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);

            using (var frm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmItemSelector(instances.Keys.ToList()))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string selectedItem = frm.selectedItem.ToString();
                    if (!DataTableControls.SetParameterValue(v_IfActionParameterTable, selectedItem, "Selenium Instance Name", "Parameter Name", "Parameter Value"))
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
                    if (!DataTableControls.SetParameterValue(v_IfActionParameterTable, selectedItem, "Window Name", "Parameter Name", "Parameter Value"))
                    {
                        throw new Exception("Fail update Window Name");
                    }
                }
            }
        }

        private void linkBooleanInstanceSelector_Click(object sender, EventArgs e, UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var instances = editor.instanceList.getInstanceClone(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Boolean);

            using (var frm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmItemSelector(instances.Keys.ToList()))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string selectedItem = frm.selectedItem.ToString();
                    //int currentRow = IfGridViewHelper.CurrentRow.Index;
                    string parameterName = DataTableControls.GetFieldValue(v_IfActionParameterTable, IfGridViewHelper.CurrentRow.Index, "Parameter Name");

                    switch (v_IfActionType.ToLower())
                    {
                        case "boolean":
                            DataTableControls.SetParameterValue(v_IfActionParameterTable, selectedItem, "Variable Name", "Parameter Name", "Parameter Value");
                            break;

                        case "boolean compare":
                            switch (parameterName)
                            {
                                case "Value1":
                                case "Value2":
                                    DataTableControls.SetParameterValue(v_IfActionParameterTable, selectedItem, parameterName, "Parameter Name", "Parameter Value");
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void ifAction_SelectionChangeCommitted(object sender, EventArgs e, UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            ComboBox ifAction = (ComboBox)ActionDropdown;
            DataGridView ifActionParameterBox = (DataGridView)IfGridViewHelper;

            BeginIfCommand cmd = (BeginIfCommand)this;
            DataTable actionParameters = cmd.v_IfActionParameterTable;

            //sender is null when command is updating
            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }

            lnkBrowserInstanceSelector.Hide();
            lnkWindowNameSelector.Hide();
            lnkBooleanSelector.Hide();

            switch (ifAction.SelectedItem)
            {
                //case "Value":
                case "Numeric Compare":
                case "Date Compare":
                    ConditionControls.RenderNumericCompare(sender, ifActionParameterBox, actionParameters);
                    break;

                //case "Variable Compare":
                case "Text Compare":
                    ConditionControls.RenderTextCompare(sender, ifActionParameterBox, actionParameters);
                    break;


                case "Variable Has Value":
                case "Variable Is Numeric":
                    ConditionControls.RenderVariableIsHas(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Error Occured":
                case "Error Did Not Occur":
                    ConditionControls.RenderErrorOccur(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Window Name Exists":
                case "Active Window Name Is":
                    ConditionControls.RenderWindowName(sender, ifActionParameterBox, actionParameters);
                    lnkWindowNameSelector.Show();
                    break;

                case "File Exists":
                    ConditionControls.RenderFileExists(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Folder Exists":
                    ConditionControls.RenderFolderExists(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Web Element Exists":
                    ConditionControls.RenderWebElement(sender, ifActionParameterBox, actionParameters, editor.appSettings);
                    lnkBrowserInstanceSelector.Show();
                    break;

                case "GUI Element Exists":
                    ConditionControls.RenderGUIElement(sender, ifActionParameterBox, actionParameters, editor.appSettings);
                    lnkWindowNameSelector.Show();
                    break;

                case "Boolean":
                    ConditionControls.RenderBoolean(sender, ifActionParameterBox, actionParameters);
                    lnkBooleanSelector.Show();
                    break;

                case "Boolean Compare":
                    ConditionControls.RenderBooleanCompare(sender, ifActionParameterBox, actionParameters);
                    lnkBooleanSelector.Show();
                    break;

                case "List Compare":
                    ConditionControls.RenderListCompare(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Dictionary Compare":
                    ConditionControls.RenderDictionaryCompare(sender, ifActionParameterBox, actionParameters);
                    break;

                case "DataTable Compare":
                    ConditionControls.RenderDataTableCompare(sender, ifActionParameterBox, actionParameters);
                    break;

                default:
                    break;
            }
        }

        public override string GetDisplayValue()
        {
            return ConditionControls.GetDisplayValue("If", v_IfActionType, v_IfActionParameterTable);
        }

        public override bool IsValidate(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_IfActionType))
            {
                this.validationResult += "Type is empty.";
                this.IsValid = false;
            }
            else
            {
                string message;
                bool res = true;
                switch (this.v_IfActionType)
                {
                    //case "Value":
                    case "Numeric Compare":
                    case "Date Compare":
                    //case "Variable Compare":
                    case "Text Compare":
                        res = ConditionControls.ValueValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Variable Has Value":
                    case "Variable Is Numeric":
                        res = ConditionControls.VariableValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Window Name Exists":
                    case "Active Window Name Is":
                        res = ConditionControls.WindowValidate(v_IfActionParameterTable, out message);
                        break;

                    case "File Exists":
                        res = ConditionControls.FileValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Folder Exists":
                        res = ConditionControls.FolderValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Web Element Exists":
                        res = ConditionControls.WebValidate(v_IfActionParameterTable, out message);
                        break;

                    case "GUI Element Exists":
                        res = ConditionControls.GUIValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Error Occured":
                    case "Error Did Not Occur":
                        res = ConditionControls.ErrorValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Boolean":
                        res = ConditionControls.BooleanValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Boolean Compare":
                        res = ConditionControls.BooleanCompareValidate(v_IfActionParameterTable, out message);
                        break;

                    case "List Compare":
                        res = ConditionControls.ListCompareValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Dictionary Compare":
                        res = ConditionControls.DictionaryCompareValidate(v_IfActionParameterTable, out message);
                        break;

                    case "DataTable Compare":
                        res = ConditionControls.DataTableCompareValidate(v_IfActionParameterTable, out message);
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

        public override void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            if (this.v_IfActionType == "GUI Element Exists")
            {
                var cnv = new Dictionary<string, string>();
                cnv.Add("v_IfActionParameterTable", "convertToIntermediateWindowName");
                ConvertToIntermediate(settings, cnv, variables);
            }
            else
            {
                base.ConvertToIntermediate(settings, variables);
            }
        }

        public override void ConvertToRaw(EngineSettings settings)
        {
            if (this.v_IfActionType == "GUI Element Exists")
            {
                var cnv = new Dictionary<string, string>();
                cnv.Add("v_IfActionParameterTable", "convertToRawWindowName");
                ConvertToRaw(settings, cnv);
            }
            else
            {
                base.ConvertToRaw(settings);
            }
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            //var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_IfActionParameterTable));
            DataTableControls.BeforeValidate_NoRowAdding(IfGridViewHelper, v_IfActionParameterTable);
        }
    }
}
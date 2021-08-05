using SHDocVw;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using taskt.Core;

namespace taskt.UI.CustomControls
{
    public static class CommandControls
    {
        public static UI.Forms.frmCommandEditor CurrentEditor { get; set; }

        public static List<Control> MultiCreateInferenceDefaultControlGroupFor(Core.Automation.Commands.ScriptCommand parent, Forms.frmCommandEditor editor)
        {
            var controlList = new List<string>();
            var parentProps = parent.GetType().GetProperties();
            foreach(var prop in parentProps)
            {
                if (prop.Name.StartsWith("v_") && prop.Name != "v_Comment")
                {
                    controlList.Add(prop.Name);
                }
            }

            return MultiCreateInferenceDefaultControlGroupFor(controlList, parent, editor);
        }

        public static List<Control> MultiCreateInferenceDefaultControlGroupFor(List<string> parameterNames, Core.Automation.Commands.ScriptCommand parent, Forms.frmCommandEditor editor)
        {
            var controlList = new List<Control>();

            foreach(var parameterName in parameterNames)
            {
                controlList.AddRange(CreateInferenceDefaultControlGroupFor(parameterName, parent, editor));
            }

            return controlList;
        }

        public static List<Control> CreateInferenceDefaultControlGroupFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, Forms.frmCommandEditor editor, List<Control> additionalLinks = null)
        {
            var controlList = new List<Control>();

            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var label = CreateDefaultLabelFor(parameterName, parent, variableProperties);

            Control input;
            
            var rct = (Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl));
            if (rct != null)
            {
                switch (rct.recommendedControl)
                {
                    case Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.TextBox:
                        input = CreateDefaultInputFor(parameterName, parent, variableProperties);
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox:
                        input = CreateDropdownFor(parameterName, parent, variableProperties);
                        var varList = (Core.Automation.Attributes.PropertyAttributes.PropertyIsVariablesList)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyIsVariablesList));
                        if ((varList != null) && varList.isVariablesList)
                        {
                            ((ComboBox)input).AddVariableNames(editor);
                        }
                        else
                        {
                            var winList = (Core.Automation.Attributes.PropertyAttributes.PropertyIsWindowNamesList)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyIsWindowNamesList));
                            if ((winList != null) && (winList.isWindowNamesList))
                            {
                                ((ComboBox)input).AddWindowNames(editor);
                            }
                        }
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView:
                        input = CreateDataGridView(parent, parameterName, variableProperties);
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox:
                        input = CreateDefaultInputFor(parameterName, parent, variableProperties);
                        break;
                    default:
                        input = CreateDefaultInputFor(parameterName, parent, variableProperties);
                        break;
                }
            }
            else
            {
                // check combobox
                var varList = (Core.Automation.Attributes.PropertyAttributes.PropertyIsVariablesList)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyIsVariablesList));
                var winList = (Core.Automation.Attributes.PropertyAttributes.PropertyIsWindowNamesList)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyIsWindowNamesList));
                var uiList = variableProperties.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption), true);

                if (uiList.Length > 0)
                {
                    input = CreateDropdownFor(parameterName, parent, variableProperties);
                }
                else if (varList != null && varList.isVariablesList)
                {
                    input = CreateDropdownFor(parameterName, parent, variableProperties);
                    ((ComboBox)input).AddVariableNames(editor);
                }
                else if (winList != null && winList.isWindowNamesList)
                {
                    input = CreateDropdownFor(parameterName, parent, variableProperties);
                    ((ComboBox)input).AddVariableNames(editor);
                }
                else
                {
                    input = CreateDefaultInputFor(parameterName, parent, variableProperties);
                }
            }

            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { input }, editor, variableProperties);

            controlList.Add(label);

            var secondaryLabel = (Core.Automation.Attributes.PropertyAttributes.PropertySecondaryLabel)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertySecondaryLabel));
            if (secondaryLabel != null && secondaryLabel.useSecondaryLabel)
            {
                var label2 = CreateSimpleLabel();
                label2.Name = "lbl2_" + parameterName;
                controlList.Add(label2);
            }

            controlList.AddRange(helpers);
            if (additionalLinks != null)
            {
                controlList.AddRange(additionalLinks);
            }
            controlList.Add(input);

            return controlList;
        }

        public static List<Control> CreateDefaultInputGroupFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, Forms.frmCommandEditor editor, List<Control> additionalLinks = null)
        {
            //Todo: Test
            var controlList = new List<Control>();

            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();

            var label = CreateDefaultLabelFor(parameterName, parent, variableProperties);
            var input = CreateDefaultInputFor(parameterName, parent, variableProperties);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { input }, editor, variableProperties);

            controlList.Add(label);

            var secondaryLabel = (Core.Automation.Attributes.PropertyAttributes.PropertySecondaryLabel)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertySecondaryLabel));
            if (secondaryLabel != null && secondaryLabel.useSecondaryLabel)
            {
                var label2 = CreateSimpleLabel();
                label2.Name = "lbl2_" + parameterName;
                controlList.Add(label2);
            }

            controlList.AddRange(helpers);
            if (additionalLinks != null)
            {
                controlList.AddRange(additionalLinks);
            }
            controlList.Add(input);

            return controlList;
            
        }

        public static List<Control> CreateDefaultDropdownGroupFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, Forms.frmCommandEditor editor, List<Control> additionalLinks = null)
        {
            //Todo: Test
            var controlList = new List<Control>();

            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();

            var label = CreateDefaultLabelFor(parameterName, parent, variableProperties);
            var input = CreateDropdownFor(parameterName, parent, variableProperties);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { input }, editor, variableProperties);

            controlList.Add(label);

            var secondaryLabel = (Core.Automation.Attributes.PropertyAttributes.PropertySecondaryLabel)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertySecondaryLabel));
            if (secondaryLabel != null && secondaryLabel.useSecondaryLabel)
            {
                var label2 = CreateSimpleLabel();
                label2.Name = "lbl2_" + parameterName;
                controlList.Add(label2);
            }

            controlList.AddRange(helpers);
            if (additionalLinks != null)
            {
                controlList.AddRange(additionalLinks);
            }
            controlList.Add(input);

            return controlList;
            
        }

        public static List<Control> CreateDataGridViewGroupFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, Forms.frmCommandEditor editor, List<Control> additionalLinks = null)
        {
            var controlList = new List<Control>();

            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();

            var label = CreateDefaultLabelFor(parameterName, parent, variableProperties);
            var gridview = CreateDataGridView(parent, parameterName, variableProperties);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { gridview }, editor, variableProperties);

            controlList.Add(label);

            var secondaryLabel = (Core.Automation.Attributes.PropertyAttributes.PropertySecondaryLabel)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertySecondaryLabel));
            if (secondaryLabel != null && secondaryLabel.useSecondaryLabel)
            {
                var label2 = CreateSimpleLabel();
                label2.Name = "lbl2_" + parameterName;
                controlList.Add(label2);
            }

            controlList.AddRange(helpers);
            if (additionalLinks != null)
            {
                controlList.AddRange(additionalLinks);
            }
            controlList.Add(gridview);

            return controlList;
        }

        public static Label CreateSimpleLabel()
        {
            Label newLabel = new Label();
            var theme = CurrentEditor.Theme.Label;
            newLabel.AutoSize = true;
            newLabel.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            newLabel.ForeColor = theme.FontColor;
            newLabel.BackColor = theme.BackColor;
            return newLabel;
        }
        public static Control CreateDefaultLabelFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, PropertyInfo pInfo = null)
        {
            PropertyInfo variableProperties;
            if (pInfo == null)
            {
                variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            }
            else
            {
                variableProperties = pInfo;
            }

            var propertyAttributesAssigned = variableProperties.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyDescription), true);

            // var settings = (new Core.ApplicationSettings().GetOrCreateApplicationSettings()).EngineSettings;
            var settings = CurrentEditor.appSettings.EngineSettings;

            Label inputLabel = CreateSimpleLabel();
            if (propertyAttributesAssigned.Length > 0)
            {
                var desc = (Core.Automation.Attributes.PropertyAttributes.PropertyDescription)propertyAttributesAssigned[0];
                var labelText = desc.propertyDescription.replaceEngineKeyword();

                if (CurrentEditor.appSettings.ClientSettings.ShowSampleUsageInDescription)
                {
                    var showSample = (Core.Automation.Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription));
                    if (showSample != null && showSample.showSampleUsage)
                    {
                        if (!labelText.Contains("(ex."))
                        {
                            var sampleAttr = (Core.Automation.Attributes.PropertyAttributes.SampleUsage)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.SampleUsage));
                            var sampleText = (sampleAttr == null) ? "" : sampleAttr.sampleUsage.getTextMDFormat().replaceEngineKeyword().Replace(" or ", ", ");
                            if (sampleText.Length > 0)
                            {
                                labelText += " (ex. " + sampleText + ")";
                            }
                        }
                    }
                }

                var attrIsOpt = (Core.Automation.Attributes.PropertyAttributes.PropertyIsOptional)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyIsOptional));
                if (attrIsOpt != null && attrIsOpt.isOptional)
                {
                    if (!labelText.Contains("Optional"))
                    {
                        labelText = "Optional - " + labelText;
                    }
                }

                inputLabel.Text = labelText;
            }
            else
            {
                inputLabel.Text = parameterName;
            }

            //var theme = CurrentEditor.Theme.Label;
            //inputLabel.AutoSize = true;
            //inputLabel.Font = new Font("Segoe UI Light", 12);
            //inputLabel.ForeColor = Color.White;
            //inputLabel.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            //inputLabel.ForeColor = theme.FontColor;
            //inputLabel.BackColor = theme.BackColor;

            inputLabel.Name = "lbl_" + parameterName;
            return inputLabel;
        }
        public static Control CreateDefaultInputFor(string parameterName, Core.Automation.Commands.ScriptCommand parent)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            return CreateDefaultInputFor(parameterName, parent, variableProperties);
        }
        public static Control CreateDefaultInputFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, PropertyInfo pInfo = null)
        {
            PropertyInfo variableProperties;
            if (pInfo == null)
            {
                variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            }
            else
            {
                variableProperties = pInfo;
            }

            var setting = (Core.Automation.Attributes.PropertyAttributes.PropertyTextBoxSetting)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyTextBoxSetting));
            var rcmCtl = (Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl)variableProperties.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl));
            if (setting == null)
            {
                if (rcmCtl == null || rcmCtl.recommendedControl != Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)
                {
                    return CreateDefaultInputFor(parameterName, parent, 30, 300, true);
                }
                else
                {
                    return CreateDefaultInputFor(parameterName, parent, 90, 300, true);
                }
            }
            else
            {
                if (rcmCtl != null && rcmCtl.recommendedControl == Core.Automation.Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)
                {
                    int height = (setting.height < 2) ? 3 : setting.height;
                    return CreateDefaultInputFor(parameterName, parent, height * 30, 300, setting.allowNewLine);
                }
                else
                {
                    return CreateDefaultInputFor(parameterName, parent, setting.height * 30, 300, setting.allowNewLine);
                }
            }
        }
        public static Control CreateDefaultInputFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, int height = 30, int width = 300, bool allowNewLine = true)
        {
            var inputBox = new TextBox();
            //inputBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            var theme = CurrentEditor.Theme.Input;
            inputBox.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            inputBox.ForeColor = theme.FontColor;
            inputBox.BackColor = theme.BackColor;
            inputBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            
            if (!allowNewLine)
            {
                inputBox.KeyDown += (sender, e) => TextBoxKeyDown(sender, e);
                inputBox.ScrollBars = ScrollBars.None;
            }
            else
            {
                inputBox.Multiline = true;
                inputBox.ScrollBars = ScrollBars.Vertical;
            }
            inputBox.Height = height;
            inputBox.Width = width;

            inputBox.Name = parameterName;
            return inputBox;

        }
        public static Control CreateDropdownFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, PropertyInfo pInfo = null)
        {
            var inputBox = new ComboBox();
            //inputBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            var theme = CurrentEditor.Theme.Dropdown;
            inputBox.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            inputBox.ForeColor = theme.FontColor;
            inputBox.BackColor = theme.BackColor;
            inputBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Height = 30;
            inputBox.Width = 300;
            inputBox.Name = parameterName;

            PropertyInfo variableProperties;
            if (pInfo == null)
            {
                variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            }
            else
            {
                variableProperties = pInfo;
            }

            var propertyAttributesAssigned = variableProperties.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption), true);

            foreach (Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption option in propertyAttributesAssigned)
            {
                inputBox.Items.Add(option.uiOption);
            }

            inputBox.KeyUp += (sender, e) => ComboBoxKeyUp(sender, e);
            inputBox.Click += (sender, e) => ComboBoxClick(sender, e);

            return inputBox;

        }
        public static ComboBox CreateStandardComboboxFor(string parameterName, Core.Automation.Commands.ScriptCommand parent)
        {
            var inputBox = new ComboBox();
            //inputBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            var theme = CurrentEditor.Theme.Combobox;
            inputBox.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            inputBox.ForeColor = theme.FontColor;
            inputBox.BackColor = theme.BackColor;
            inputBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Height = 30;
            inputBox.Width = 300;
            inputBox.Name = parameterName;

            inputBox.KeyUp += (sender, e) => ComboBoxKeyUp(sender, e);
            inputBox.Click += (sender, e) => ComboBoxClick(sender, e);

            return inputBox;

        }

        public static CommandItemControl CreateUIHelper()
        {
            taskt.UI.CustomControls.CommandItemControl helperControl = new taskt.UI.CustomControls.CommandItemControl();
            helperControl.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            var theme = CurrentEditor.Theme.UIHelper;
            helperControl.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            helperControl.ForeColor = theme.FontColor;
            helperControl.BackColor = theme.BackColor;

            return helperControl;
        }

        public static List<Control> CreateUIHelpersFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, Control[] targetControls, UI.Forms.frmCommandEditor editor, PropertyInfo pInfo = null)
        {
            PropertyInfo variableProperties;
            if (pInfo == null)
            {
                variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            }
            else
            {
                variableProperties = pInfo;
            }
            
            var propertyUIHelpers = variableProperties.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper), true);
            var controlList = new List<Control>();

            if (propertyUIHelpers.Count() == 0)
            {
                return controlList;
            }

            int count = 0;
            foreach (Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper attrib in propertyUIHelpers)
            {
                taskt.UI.CustomControls.CommandItemControl helperControl = new taskt.UI.CustomControls.CommandItemControl();
                helperControl.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
                //helperControl.ForeColor = Color.AliceBlue;
                //helperControl.Font = new Font("Segoe UI Semilight", 10);
                var theme = CurrentEditor.Theme.UIHelper;
                helperControl.Font = new Font(theme.Font, theme.FontSize, theme.Style);
                helperControl.ForeColor = theme.FontColor;
                helperControl.BackColor = theme.BackColor;
                helperControl.Name = parameterName + "_helper_" + count.ToString(); ;
                helperControl.Tag = targetControls.FirstOrDefault();
                helperControl.HelperType = attrib.additionalHelper;

                switch (attrib.additionalHelper)
                {
                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                        //show variable selector
                        helperControl.CommandImage = UI.Images.GetUIImage("VariableCommand");
                        helperControl.CommandDisplay = "Insert Variable";
                        helperControl.DrawIcon = taskt.Properties.Resources.taskt_variable_helper;
                        helperControl.Click += (sender, e) => ShowVariableSelector(sender, e, editor);
                        break;

                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper:
                        //show file selector
                        helperControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
                        helperControl.CommandDisplay = "Select a File";
                        helperControl.DrawIcon = taskt.Properties.Resources.taskt_file_helper;
                        helperControl.Click += (sender, e) => ShowFileSelector(sender, e, editor);
                        break;

                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper:
                        //show file selector
                        helperControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
                        helperControl.CommandDisplay = "Select a Folder";
                        helperControl.DrawIcon = taskt.Properties.Resources.taskt_folder_helper;
                        helperControl.Click += (sender, e) => ShowFolderSelector(sender, e, editor);
                        break;

                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowImageRecogitionHelper:
                        //show file selector
                        helperControl.CommandImage = UI.Images.GetUIImage("OCRCommand");
                        helperControl.CommandDisplay = "Capture Reference Image";
                        helperControl.DrawIcon = taskt.Properties.Resources.taskt_element_helper;
                        helperControl.Click += (sender, e) => ShowImageCapture(sender, e, editor);

                        taskt.UI.CustomControls.CommandItemControl testRun = new taskt.UI.CustomControls.CommandItemControl();
                        testRun.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
                        testRun.ForeColor = Color.AliceBlue;

                        testRun.CommandImage = UI.Images.GetUIImage("OCRCommand");
                        testRun.CommandDisplay = "Run Image Recognition Test";
                        testRun.ForeColor = Color.AliceBlue;
                        testRun.Tag = targetControls.FirstOrDefault();
                        testRun.Click += (sender, e) => RunImageCapture(sender, e);
                        controlList.Add(testRun);
                        break;

                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowCodeBuilder:
                        //show variable selector
                        helperControl.CommandImage = UI.Images.GetUIImage("RunScriptCommand");
                        helperControl.CommandDisplay = "Code Builder";
                        helperControl.Click += (sender, e) => ShowCodeBuilder(sender, e, editor);
                        break;

                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper:
                        helperControl.CommandImage = UI.Images.GetUIImage("SendMouseMoveCommand");
                        helperControl.CommandDisplay = "Capture Mouse Position";
                        //helperControl.ForeColor = Color.AliceBlue;
                        helperControl.DrawIcon = taskt.Properties.Resources.taskt_element_helper;
                        helperControl.Click += (sender, e) => ShowMouseCaptureForm(sender, e);
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowElementRecorder:
                        //show variable selector
                        helperControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
                        helperControl.CommandDisplay = "Element Recorder";
                        helperControl.Click += (sender, e) => ShowElementRecorder(sender, e, editor);
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.GenerateDLLParameters:
                        //show variable selector
                        helperControl.CommandImage = UI.Images.GetUIImage("ExecuteDLLCommand");
                        helperControl.CommandDisplay = "Generate Parameters";
                        helperControl.Click += (sender, e) => GenerateDLLParameters(sender, e);
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowDLLExplorer:
                        //show variable selector
                        helperControl.CommandImage = UI.Images.GetUIImage("ExecuteDLLCommand");
                        helperControl.CommandDisplay = "Launch DLL Explorer";
                        helperControl.Click += (sender, e) => ShowDLLExplorer(sender, e);
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.AddInputParameter:
                        //show variable selector
                        helperControl.CommandImage = UI.Images.GetUIImage("ExecuteDLLCommand");
                        helperControl.CommandDisplay = "Add Input Parameter";
                        helperControl.Click += (sender, e) => AddInputParameter(sender, e, editor);
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowHTMLBuilder:
                        helperControl.CommandImage = UI.Images.GetUIImage("ExecuteDLLCommand");
                        helperControl.CommandDisplay = "Launch HTML Builder";
                        helperControl.Click += (sender, e) => ShowHTMLBuilder(sender, e, editor);
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowIfBuilder:
                        //show variable selector
                        helperControl.CommandImage = UI.Images.GetUIImage("VariableCommand");
                        helperControl.CommandDisplay = "Add New If Statement";
                        break;
                    case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowLoopBuilder:
                        //show variable selector
                        helperControl.CommandImage = UI.Images.GetUIImage("VariableCommand");
                        helperControl.CommandDisplay = "Add New Loop Statement";
                        break;

                        //default:
                        //    MessageBox.Show("Command Helper does not exist for: " + attrib.additionalHelper.ToString());
                        //    break;
                }

                controlList.Add(helperControl);
                count++;
            }

            return controlList;

        }

        public static DataGridView CreateDataGridView(object sourceCommand, string dataSourceName, PropertyInfo prop)
        {
            var dgvProp = (Core.Automation.Attributes.PropertyAttributes.PropertyDataGridViewSetting)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyDataGridViewSetting));
            if (dgvProp == null)
            {
                return CreateDataGridView(sourceCommand, dataSourceName);
            }
            else
            {
                return CreateDataGridView(sourceCommand, dataSourceName, dgvProp.allowAddRow, dgvProp.allowDeleteRow, dgvProp.allowResizeRow, dgvProp.width, dgvProp.height, dgvProp.autoGenerateColumns, dgvProp.headerRowHeight);
            }
        }

        public static DataGridView CreateDataGridView(object sourceCommand, string dataSourceName, bool AllowAddRows = true, bool AllowDeleteRows = true, bool AllowResizeRows = false, int width = 400, int height = 250, bool AutoGenerateColumns = true, int headerRowHeight = 1)
        {
            if (width < 100)
            {
                width = 400;
            }
            if (height < 100)
            {
                height = 250;
            }

            var gridView = new DataGridView();
            gridView.Name = dataSourceName;
            gridView.AllowUserToAddRows = AllowAddRows;
            gridView.AllowUserToDeleteRows = AllowDeleteRows;
            gridView.AutoGenerateColumns = AutoGenerateColumns;
            gridView.Size = new Size(width, height);
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridView.DataBindings.Add("DataSource", sourceCommand, dataSourceName, false, DataSourceUpdateMode.OnPropertyChanged);
            gridView.AllowUserToResizeRows = AllowResizeRows;

            var theme = CurrentEditor.Theme.Datagridview;
            gridView.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            gridView.ForeColor = theme.FontColor;
            gridView.ColumnHeadersHeight = Convert.ToInt32(theme.FontSize) + 20;
            gridView.RowTemplate.Height = Convert.ToInt32(theme.FontSize) + 20;

            if (headerRowHeight > 1)
            {
                gridView.ColumnHeadersHeight = ((Convert.ToInt32(theme.FontSize) + 15) * headerRowHeight);
            }

            return gridView;
        }
        private static void ShowCodeBuilder(object sender, EventArgs e, UI.Forms.frmCommandEditor editor)
        {
            //get textbox text
            CustomControls.CommandItemControl commandItem = (CustomControls.CommandItemControl)sender;
            TextBox targetTextbox = (TextBox)commandItem.Tag;

            using (UI.Forms.Supplemental.frmCodeBuilder codeBuilder = new Forms.Supplemental.frmCodeBuilder(targetTextbox.Text))
            {
                if (codeBuilder.ShowDialog() == DialogResult.OK)
                {

                    targetTextbox.Text = codeBuilder.rtbCode.Text;
                }
            }
        }
        private static void ShowMouseCaptureForm(object sender, EventArgs e)
        {
            using (taskt.UI.Forms.Supplemental.frmShowCursorPosition frmShowCursorPos = new taskt.UI.Forms.Supplemental.frmShowCursorPosition())
            {
                //if user made a successful selection
                if (frmShowCursorPos.ShowDialog() == DialogResult.OK)
                {
                    //Todo - ideally one function to add to textbox which adds to class

                    //add selected variables to associated control text
                    CurrentEditor.flw_InputVariables.Controls["v_XMousePosition"].Text = frmShowCursorPos.xPos.ToString();
                    CurrentEditor.flw_InputVariables.Controls["v_YMousePosition"].Text = frmShowCursorPos.yPos.ToString();

                    //find current command and add to underlying class
                    Core.Automation.Commands.SendMouseMoveCommand cmd = (Core.Automation.Commands.SendMouseMoveCommand)CurrentEditor.selectedCommand;
                    cmd.v_XMousePosition = frmShowCursorPos.xPos.ToString();
                    cmd.v_YMousePosition = frmShowCursorPos.yPos.ToString();
                }
            }  
        }
        public static void ShowVariableSelector(object sender, EventArgs e, taskt.UI.Forms.frmCommandEditor editor)
        {
            //create variable selector form
            UI.Forms.Supplemental.frmItemSelector newVariableSelector = new Forms.Supplemental.frmItemSelector();

       
            //get copy of user variables and append system variables, then load to combobox
            var variableList = CurrentEditor.scriptVariables.Select(f => f.VariableName).ToList();
            variableList.AddRange(Core.Common.GenerateSystemVariables().Select(f => f.VariableName));
            newVariableSelector.lstVariables.Items.AddRange(variableList.ToArray());

            //if user pressed "OK"
            if (newVariableSelector.ShowDialog() == DialogResult.OK)
            {
                //ensure that a variable was actually selected
                if (newVariableSelector.lstVariables.SelectedItem == null)
                {
                    //return out as nothing was selected
                    MessageBox.Show("There were no variables selected!");
                    return;
                }

                //grab the referenced input assigned to the 'insert variable' button instance
                CustomControls.CommandItemControl inputBox = (CustomControls.CommandItemControl)sender;
                //currently variable insertion is only available for simply textboxes

                //load settings
                //var settings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
                var settings = CurrentEditor.appSettings.EngineSettings;

                if (inputBox.Tag is TextBox)
                {
                    TextBox targetTextbox = (TextBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    if (editor.appSettings.ClientSettings.InsertVariableAtCursor)
                    {
                        string str = targetTextbox.Text;
                        int cursorPos = targetTextbox.SelectionStart;
                        string ins = string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                        targetTextbox.Text = str.Substring(0, cursorPos) + ins + str.Substring(cursorPos);
                        targetTextbox.Focus();
                        targetTextbox.SelectionStart = cursorPos + ins.Length;
                        targetTextbox.SelectionLength = 0;
                    }
                    else
                    {
                        targetTextbox.Text = targetTextbox.Text + string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                        targetTextbox.Focus();
                        targetTextbox.SelectionStart = targetTextbox.Text.Length;
                        targetTextbox.SelectionLength = 0;
                    }
                }
                else if (inputBox.Tag is ComboBox)
                {
                    ComboBox targetCombobox = (ComboBox)inputBox.Tag;
                    if (editor.appSettings.ClientSettings.InsertVariableAtCursor)
                    {
                        string str = targetCombobox.Text;
                        int cursorPos;
                        if (!int.TryParse(targetCombobox.Tag.ToString(), out cursorPos))
                        {
                            cursorPos = str.Length;
                        }
                        string ins = string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                        targetCombobox.Text = str.Substring(0, cursorPos) + ins + str.Substring(cursorPos);
                        targetCombobox.Focus();
                        targetCombobox.SelectionStart = cursorPos + ins.Length;
                        targetCombobox.SelectionLength = 0;
                    }
                    else
                    {
                        //concat variable name with brackets [vVariable] as engine searches for the same
                        targetCombobox.Text = targetCombobox.Text + string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                        targetCombobox.Focus();
                        targetCombobox.SelectionStart = targetCombobox.Text.Length;
                        targetCombobox.SelectionLength = 0;
                    }
                }
                else if (inputBox.Tag is DataGridView)
                {
                    DataGridView targetDGV = (DataGridView)inputBox.Tag;

                    if (targetDGV.SelectedCells.Count == 0)
                    {
                        MessageBox.Show("Please make sure you have selected an action and selected a cell before attempting to insert a variable!", "No Cell Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!(targetDGV.SelectedCells[0] is DataGridViewTextBoxCell))
                    {
                        MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (targetDGV.SelectedCells[0].ColumnIndex == 0)
                    {
                        if (targetDGV.Tag == null)
                        {
                            MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (targetDGV.Tag.ToString() != "column-a-editable")
                        {
                            MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    var source = (DataTable)targetDGV.DataSource;
                    var rowIndex = targetDGV.SelectedCells[0].RowIndex;
                    var colIndex = targetDGV.SelectedCells[0].ColumnIndex;
                    if (source.Rows.Count == targetDGV.SelectedCells[0].RowIndex)
                    {
                        source.Rows.Add(source.NewRow());
                    }
                    var targetCell = targetDGV.Rows[rowIndex].Cells[colIndex];
                    targetCell.Value = targetCell.Value + string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                }


            }
        }
        private static void ShowFileSelector(object sender, EventArgs e, UI.Forms.frmCommandEditor editor)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    CustomControls.CommandItemControl inputBox = (CustomControls.CommandItemControl)sender;
                    //currently variable insertion is only available for simply textboxes
                    TextBox targetTextbox = (TextBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetTextbox.Text = ofd.FileName;
                }
            }     
        }
        private static void ShowFolderSelector(object sender, EventArgs e, UI.Forms.frmCommandEditor editor)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    CustomControls.CommandItemControl inputBox = (CustomControls.CommandItemControl)sender;
                    TextBox targetTextBox = (TextBox)inputBox.Tag;
                    targetTextBox.Text = fbd.SelectedPath;
                }
            }
        }
        private static void ShowImageCapture(object sender, EventArgs e, UI.Forms.frmCommandEditor editor)
        {
            //ApplicationSettings settings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
            var settings = editor.appSettings;
            var minimizePreference = settings.ClientSettings.MinimizeToTray;

            if (minimizePreference)
            {
                settings.ClientSettings.MinimizeToTray = false;
                settings.Save(settings);
            }

            HideAllForms();

            var userAcceptance = MessageBox.Show("The image capture process will now begin and display a screenshot of the current desktop in a custom full-screen window.  You may stop the capture process at any time by pressing the 'ESC' key, or selecting 'Close' at the top left. Simply create the image by clicking once to start the rectangle and clicking again to finish. The image will be cropped to the boundary within the red rectangle. Shall we proceed?", "Image Capture", MessageBoxButtons.YesNo);

            if (userAcceptance == DialogResult.Yes)
            {

                using (Forms.Supplement_Forms.frmImageCapture imageCaptureForm = new Forms.Supplement_Forms.frmImageCapture())
                {
                    if (imageCaptureForm.ShowDialog() == DialogResult.OK)
                    {
                        CustomControls.CommandItemControl inputBox = (CustomControls.CommandItemControl)sender;
                        UIPictureBox targetPictureBox = (UIPictureBox)inputBox.Tag;
                        targetPictureBox.Image = imageCaptureForm.userSelectedBitmap;
                        var convertedImage = Core.Common.ImageToBase64(imageCaptureForm.userSelectedBitmap);
                        var convertedLength = convertedImage.Length;
                        targetPictureBox.EncodedImage = convertedImage;
                        //imageCaptureForm.Show();
                    }
                }
            }

            ShowAllForms();

            if (minimizePreference)
            {
                settings.ClientSettings.MinimizeToTray = true;
                settings.Save(settings);
            }
        }
        private static void RunImageCapture(object sender, EventArgs e)
        {

            //get input control
            CustomControls.CommandItemControl inputBox = (CustomControls.CommandItemControl)sender;
            UIPictureBox targetPictureBox = (UIPictureBox)inputBox.Tag;
            string imageSource = targetPictureBox.EncodedImage;

            if (string.IsNullOrEmpty(imageSource))
            {
                MessageBox.Show("Please capture an image before attempting to test!");
                return;
            }

            //hide all
            HideAllForms();



            try
            {
                //run image recognition
                Core.Automation.Commands.ImageRecognitionCommand imageRecognitionCommand = new Core.Automation.Commands.ImageRecognitionCommand();
                imageRecognitionCommand.v_ImageCapture = imageSource;
                imageRecognitionCommand.TestMode = true;
                imageRecognitionCommand.RunCommand(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            //show all forms
            ShowAllForms();


        }
        private static void ShowElementRecorder(object sender, EventArgs e, UI.Forms.frmCommandEditor editor)
        {
            //get command reference
            Core.Automation.Commands.UIAutomationCommand cmd = (Core.Automation.Commands.UIAutomationCommand)editor.selectedCommand;

            //create recorder
            UI.Forms.Supplemental.frmThickAppElementRecorder newElementRecorder = new UI.Forms.Supplemental.frmThickAppElementRecorder();
            newElementRecorder.searchParameters = cmd.v_UIASearchParameters;

            //show form
            newElementRecorder.ShowDialog();

            ComboBox txtWindowName = (ComboBox)editor.flw_InputVariables.Controls["v_WindowName"];
            txtWindowName.Text = newElementRecorder.cboWindowTitle.Text;

            editor.WindowState = FormWindowState.Normal;
            editor.BringToFront();
        }
        private static void GenerateDLLParameters(object sender, EventArgs e)
        {


            Core.Automation.Commands.ExecuteDLLCommand cmd = (Core.Automation.Commands.ExecuteDLLCommand)CurrentEditor.selectedCommand;

            var filePath = CurrentEditor.flw_InputVariables.Controls["v_FilePath"].Text;
            var className = CurrentEditor.flw_InputVariables.Controls["v_ClassName"].Text;
            var methodName = CurrentEditor.flw_InputVariables.Controls["v_MethodName"].Text;
            DataGridView parameterBox = (DataGridView)CurrentEditor.flw_InputVariables.Controls["v_MethodParameters"];

            //clear all rows
            cmd.v_MethodParameters.Rows.Clear();

            //Load Assembly
            try
            {
                Assembly requiredAssembly = Assembly.LoadFrom(filePath);

                //get type
                Type t = requiredAssembly.GetType(className);

                //verify type was found
                if (t == null)
                {
                    MessageBox.Show("The class '" + className + "' was not found in assembly loaded at '" + filePath + "'", "Class Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get method
                MethodInfo m = t.GetMethod(methodName);

                //verify method found
                if (m == null)
                {
                    MessageBox.Show("The method '" + methodName + "' was not found in assembly loaded at '" + filePath + "'", "Method Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get parameters
                var reqdParams = m.GetParameters();

                if (reqdParams.Length > 0)
                {
                    cmd.v_MethodParameters.Rows.Clear();
                    foreach (var param in reqdParams)
                    {
                        cmd.v_MethodParameters.Rows.Add(param.Name, "");
                    }
                }
                else
                {
                    MessageBox.Show("There are no parameters required for this method!", "No Parameters Required", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error generating the parameters: " + ex.ToString());
            }



        }
        private static void ShowDLLExplorer(object sender, EventArgs e)
        {
            //create form
            using (UI.Forms.Supplemental.frmDLLExplorer dllExplorer = new UI.Forms.Supplemental.frmDLLExplorer())
            {
                //show dialog
                if (dllExplorer.ShowDialog() == DialogResult.OK)
                {
                    //user accepted the selections
                    //declare command
                    Core.Automation.Commands.ExecuteDLLCommand cmd = (Core.Automation.Commands.ExecuteDLLCommand)CurrentEditor.selectedCommand;

                    //add file name
                    if (!string.IsNullOrEmpty(dllExplorer.FileName))
                    {
                        CurrentEditor.flw_InputVariables.Controls["v_FilePath"].Text = dllExplorer.FileName;
                    }

                    //add class name
                    if (dllExplorer.lstClasses.SelectedItem != null)
                    {
                        CurrentEditor.flw_InputVariables.Controls["v_ClassName"].Text = dllExplorer.lstClasses.SelectedItem.ToString();
                    }

                    //add method name
                    if (dllExplorer.lstMethods.SelectedItem != null)
                    {
                        CurrentEditor.flw_InputVariables.Controls["v_MethodName"].Text = dllExplorer.lstMethods.SelectedItem.ToString();
                    }

                    cmd.v_MethodParameters.Rows.Clear();

                    //add parameters
                    if ((dllExplorer.lstParameters.Items.Count > 0) && (dllExplorer.lstParameters.Items[0].ToString() != "This method requires no parameters!"))
                    {
                        foreach (var param in dllExplorer.SelectedParameters)
                        {
                            cmd.v_MethodParameters.Rows.Add(param, "");
                        }
                    }
                }
            }
        }

        private static void TextBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private static void ComboBoxKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            ComboBox trg = (ComboBox)sender;
            trg.Tag = trg.SelectionStart;
            Console.WriteLine(trg.Tag);
        }
        private static void ComboBoxClick(object sender, System.EventArgs e)
        {
            ComboBox trg = (ComboBox)sender;
            trg.Tag = trg.SelectionStart;
            Console.WriteLine(trg.Tag);
        }

        private static void AddInputParameter(object sender, EventArgs e, UI.Forms.frmCommandEditor editor)
        {

            DataGridView inputControl = (DataGridView)CurrentEditor.flw_InputVariables.Controls["v_UserInputConfig"];
            var inputTable = (DataTable)inputControl.DataSource;
            var newRow = inputTable.NewRow();
            newRow["Size"] = "500,100";
            inputTable.Rows.Add(newRow);

        }
        private static void ShowHTMLBuilder(object sender, EventArgs e, UI.Forms.frmCommandEditor editor)
        {
            using (var htmlForm = new UI.Forms.Supplemental.frmHTMLBuilder())
            {
                RichTextBox inputControl = (RichTextBox)editor.flw_InputVariables.Controls["v_InputHTML"];
                htmlForm.rtbHTML.Text = inputControl.Text;

                if (htmlForm.ShowDialog() == DialogResult.OK)
                {
                    inputControl.Text = htmlForm.rtbHTML.Text;
                }
            }
        }

        public static void ShowAllForms()
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.WindowState = FormWindowState.Normal;
            }
        }
        public static void HideAllForms()
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.WindowState = FormWindowState.Minimized;
            }
        }


        public static List<AutomationCommand> GenerateCommandsandControls()
        {
            var commandList = new List<AutomationCommand>();

            var commandClasses = Assembly.GetExecutingAssembly().GetTypes()
                                 .Where(t => t.Namespace == "taskt.Core.Automation.Commands")
                                 .Where(t => t.Name != "ScriptCommand")
                                 .Where(t => t.IsAbstract == false)
                                 .Where(t => t.BaseType.Name == "ScriptCommand")
                                 .ToList();


            var userPrefs = new ApplicationSettings().GetOrCreateApplicationSettings();

            //Loop through each class
            foreach (var commandClass in commandClasses)
            {
                var groupingAttribute = commandClass.GetCustomAttributes(typeof(Core.Automation.Attributes.ClassAttributes.Group), true);
                string groupAttribute = "";
                if (groupingAttribute.Length > 0)
                {
                    var attributeFound = (Core.Automation.Attributes.ClassAttributes.Group)groupingAttribute[0];
                    groupAttribute = attributeFound.groupName;
                }
                var subGroupAttr = (Core.Automation.Attributes.ClassAttributes.SubGruop)commandClass.GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.SubGruop));
                string subGroupName = (subGroupAttr != null) ? subGroupAttr.subGruopName : "";
                    
                //Instantiate Class
                Core.Automation.Commands.ScriptCommand newCommand = (Core.Automation.Commands.ScriptCommand)Activator.CreateInstance(commandClass);

                //If command is enabled, pull for display and configuration
                if (newCommand.CommandEnabled)
                {
                    var newAutomationCommand = new AutomationCommand();
                    newAutomationCommand.CommandClass = commandClass;
                    newAutomationCommand.Command = newCommand;
                    newAutomationCommand.DisplayGroup = groupAttribute;
                    newAutomationCommand.DisplaySubGroup = subGroupName;
                    newAutomationCommand.FullName = string.Join(" - ", groupAttribute, newCommand.SelectionName);
                    newAutomationCommand.ShortName = newCommand.SelectionName;

                    if (userPrefs.ClientSettings.PreloadBuilderCommands)
                    {
                        //newAutomationCommand.RenderUIComponents();
                    }

                    //call RenderUIComponents to render UI controls              
                    commandList.Add(newAutomationCommand);

                }
            }

            return commandList;

        }
        public static ComboBox AddWindowNames(this ComboBox cbo, UI.Forms.frmCommandEditor editor = null)
        {
            if (cbo == null)
                return null;

            cbo.BeginUpdate();

            cbo.Items.Clear();

            if (editor != null)
            {
                cbo.Items.Add(editor.appSettings.EngineSettings.CurrentWindowKeyword);
            }
            else
            {
                cbo.Items.Add("Current Window");
            }

            Process[] processlist = Process.GetProcesses();
            //pull the main window title for each
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //add to the control list of available windows
                    cbo.Items.Add(process.MainWindowTitle);
                }
            }

            cbo.EndUpdate();

            return cbo;
        }
        public static ComboBox AddVariableNames(this ComboBox cbo, UI.Forms.frmCommandEditor editor)
        {
            if (cbo == null)
                return null;

            if (editor != null)
            {
                cbo.BeginUpdate();

                cbo.Items.Clear();

                foreach (var variable in editor.scriptVariables)
                {
                    cbo.Items.Add(variable.VariableName);
                }

                cbo.EndUpdate();

            }

            return cbo;
        }

        public static string GetDataGridViewRowInfoText(Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo rowInfo)
        {
            if (rowInfo == null)
            {
                return "";
            }

            string ret = rowInfo.description.replaceEngineKeyword();
            if (CurrentEditor.appSettings.ClientSettings.ShowSampleUsageInDescription)
            {
                if (!ret.Contains("(ex."))
                {
                    string smp = rowInfo.sampleUsage.getTextMDFormat().replaceEngineKeyword().Replace(" or ", ", ");
                    if (smp.Length > 0)
                    {
                        ret += " (ex. " + smp + ")";
                    }
                }
            }

            return ret;
        }

        private static string replaceEngineKeyword(this string targetString)
        {
            var settings = CurrentEditor.appSettings.EngineSettings;
            return targetString.Replace("{{{", settings.VariableStartMarker).Replace("}}}", settings.VariableEndMarker)
                    .Replace("%kwd_current_window%", settings.CurrentWindowKeyword);
        }

        private static string getTextMDFormat(this string targetString)
        {
            int idxAster, idxTable;
            string ret = "";
            while(targetString.Length > 0)
            {
                idxAster = targetString.IndexOf("\\*");
                idxTable = targetString.IndexOf("\\|");
                if (idxAster >= 0 || idxTable >= 0)
                {
                    if ((idxAster >= 0) && (idxTable < 0))
                    {
                        ret += targetString.Substring(0, idxAster).removeMDFormat() + "*";
                        targetString = targetString.Substring(idxAster + 1);
                    }
                    else if ((idxTable >= 0) && (idxAster < 0))
                    {
                        ret += targetString.Substring(0, idxTable).removeMDFormat() + "|";
                        targetString = targetString.Substring(idxTable + 1);
                    }
                    else if (idxAster < idxTable)
                    {
                        ret += targetString.Substring(0, idxAster).removeMDFormat() + "*";
                        targetString = targetString.Substring(idxAster + 1);
                    }
                    else if (idxTable < idxAster)
                    {
                        ret += targetString.Substring(0, idxTable).removeMDFormat() + "|";
                        targetString = targetString.Substring(idxTable + 1);
                    }
                }
                else
                {
                    ret += targetString.removeMDFormat();
                    targetString = "";
                }
            }
            return ret;
        }

        private static string removeMDFormat(this string targetString)
        {
            return targetString.Replace("*", "").Replace("**", "").Replace("|", "");
        }

    }



public class AutomationCommand
    {
        public Type CommandClass { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string DisplayGroup { get; set; }
        public string DisplaySubGroup { get; set; }
        public Core.Automation.Commands.ScriptCommand Command { get; set; }
        public List<Control> UIControls { get; set; }
        public void RenderUIComponents(taskt.UI.Forms.frmCommandEditor editorForm)
        {
            if (Command == null)
            {
                throw new InvalidOperationException("Command cannot be null!");
            }

            UIControls = new List<Control>();
            if (Command.CustomRendering)
            {
   
                var renderedControls = Command.Render(editorForm);

                if (renderedControls.Count == 0)
                {
                    var label = new Label();
                    var theme = editorForm.Theme.ErrorLabel;
                    //label.ForeColor = Color.Red;
                    //label.AutoSize = true;
                    //label.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                    label.Font = new Font(theme.Font, theme.FontSize, theme.Style);
                    label.AutoSize = true;
                    label.ForeColor = theme.FontColor;
                    label.BackColor = theme.BackColor;
                    label.Text = "No Controls are defined for rendering!  If you intend to override with custom controls, you must handle the Render() method of this command!  If you do not wish to override with your own custom controls then set 'CustomRendering' to False.";
                    UIControls.Add(label);
                }
                else
                {
                    foreach (var ctrl in renderedControls)
                    {
                        UIControls.Add(ctrl);
                    }

                    //generate comment command if user did not generate it
                    var commentControlExists = renderedControls.Any(f => f.Name == "v_Comment");

                    if (!commentControlExists)
                    {
                        UIControls.Add(CommandControls.CreateDefaultLabelFor("v_Comment", Command));
                        UIControls.Add(CommandControls.CreateDefaultInputFor("v_Comment", Command, 100, 300));                      
                    }

                }


            }
            else
            {

                var label = new Label();
                var theme = editorForm.Theme.ErrorLabel;
                //label.ForeColor = Color.Red;
                //label.AutoSize = true;
                //label.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                label.Font = new Font(theme.Font, theme.FontSize, theme.Style);
                label.AutoSize = true;
                label.ForeColor = theme.FontColor;
                label.BackColor = theme.BackColor;
                label.Text = "Command not enabled for custom rendering!";
                UIControls.Add(label);
            }
        }  
        public void Bind(UI.Forms.frmCommandEditor editor)
        {
            //preference to preload is false
            //if (UIControls is null)
            //{
            this.RenderUIComponents(editor);
            //}

            foreach (var ctrl in UIControls)
            {

                if (ctrl.DataBindings.Count > 0)
                {
                    var newBindingList = new List<Binding>();
                    foreach (Binding binding in ctrl.DataBindings)
                    {
                        newBindingList.Add(new Binding(binding.PropertyName, Command, binding.BindingMemberInfo.BindingField, false, DataSourceUpdateMode.OnPropertyChanged));
                    }

                    ctrl.DataBindings.Clear();

                    foreach (var newBinding in newBindingList)
                    {
                        ctrl.DataBindings.Add(newBinding);
                    }
                }

                if (ctrl is CommandItemControl)
                {
                    var control = (CommandItemControl)ctrl;
                    switch (control.HelperType)
                    {
                        case Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                            control.DataSource = editor.scriptVariables;
                            break;
                        default:
                            break;
                    }
                }

                //if (ctrl is UIPictureBox)
                //{

                //    var typedControl = (UIPictureBox)InputControl;
                
                //}

                //Todo: helper for loading variables, move to attribute
                if ((ctrl.Name == "v_userVariableName") && (ctrl is ComboBox))
                {
                    var variableCbo = (ComboBox)ctrl;
                    variableCbo.Items.Clear();
                    foreach (var var in editor.scriptVariables)
                    {
                        variableCbo.Items.Add(var.VariableName);
                    }
                }
              
            }
        }
    }
}

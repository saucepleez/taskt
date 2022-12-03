using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.Engine;
using taskt.UI.Forms;

namespace taskt.UI.CustomControls
{
    public static class CommandControls
    {
        public static Forms.frmCommandEditor CurrentEditor { get; set; }

        public enum CommandControlType
        {
            Body,
            Label,
            Helpers,
            CunstomHelpers,
            SecondLabel
        }

        #region create multi group for
        /// <summary>
        /// create Controls for Render. This method automatically creates all properties controls except "v_Comment". This method supports all attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static List<Control> MultiCreateInferenceDefaultControlGroupFor(Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor)
        {
            var controlList = command.GetType().GetProperties().Where(
                    prop => (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
                ).Select(prop => prop.Name).ToList();

            return MultiCreateInferenceDefaultControlGroupFor(controlList, command, editor);
        }

        /// <summary>
        /// create Controls for Render specified by List&lt;string&gt;. This method supports all attributes.
        /// </summary>
        /// <param name="propartiesName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static List<Control> MultiCreateInferenceDefaultControlGroupFor(List<string> propartiesName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor)
        {
            var controlList = new List<Control>();

            foreach (var propertyName in propartiesName)
            {
                controlList.AddRange(CreateInferenceDefaultControlGroupFor(propertyName, command, editor));
            }

            return controlList;
        }
        #endregion

        #region create inference default control group for
        /// <summary>
        /// create control group. this method use PropertyRecommendedUIControl attribute.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static List<Control> CreateInferenceDefaultControlGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor)
        {
            var propInfo = command.GetProperty(propertyName);

            var attrRecommended = propInfo.GetCustomAttribute<PropertyRecommendedUIControl>();
            if (attrRecommended != null)
            {
                switch (attrRecommended.recommendedControl)
                {
                    case PropertyRecommendedUIControl.RecommendeUIControlType.TextBox:
                    case PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox:
                        return CreateDefaultInputGroupFor(propertyName, command, editor, propInfo);
                        
                    case PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox:
                        return CreateDefaultDropdownGroupFor(propertyName, command, editor, propInfo);

                    case PropertyRecommendedUIControl.RecommendeUIControlType.CheckBox:
                        return CreateDefaultCheckBoxGroupFor(propertyName, command, editor, propInfo);

                    case PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView:
                        return CreateDataGridViewGroupFor(propertyName, command, editor, propInfo);

                    default:
                        return CreateDefaultInputGroupFor(propertyName, command, editor, propInfo);
                }
            }
            else
            {
                // check combobox
                var attrUIOpt = propInfo.GetCustomAttribute<PropertyUISelectionOption>();
                var attrIsWin = propInfo.GetCustomAttribute<PropertyIsWindowNamesList>();
                var attrIsVar = propInfo.GetCustomAttribute<PropertyIsVariablesList>();
                var attrInstance = propInfo.GetCustomAttribute<PropertyInstanceType>();
                if ((attrUIOpt != null) || (attrIsWin != null) || (attrIsVar != null) || (attrInstance != null))
                {
                    return CreateDefaultDropdownGroupFor(propertyName, command, editor, propInfo);
                }
                else
                {
                    return CreateDefaultInputGroupFor(propertyName, command, editor, propInfo);
                }
            }
        }
        #endregion

        #region create default control group for

        /// <summary>
        /// create input control group. this method use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static List<Control> CreateDefaultInputGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor, PropertyInfo propInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }
            
            return CreateDefaultControlGroupFor(propertyName, command, new Func<Control>(() =>
            {
                return CreateDefaultInputFor(propertyName, command, propInfo);
            }), propInfo, editor);
        }

        /// <summary>
        /// create combobox control group. this method use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static List<Control> CreateDefaultDropdownGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor, PropertyInfo propInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            return CreateDefaultControlGroupFor(propertyName, command, new Func<Control>(() => {
                return CreateDefaultDropdownFor(propertyName, command, propInfo, editor);
            }), propInfo, editor);
        }

        /// <summary>
        /// create checkbox control group. this method use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static List<Control> CreateDefaultCheckBoxGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor, PropertyInfo propInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            return CreateDefaultControlGroupFor(propertyName, command, new Func<Control>(() => {
                return CreateDefaultCheckBoxFor(propertyName, command, propInfo);
            }), propInfo, editor);
        }

        /// <summary>
        /// create datagridview control group. this method use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static List<Control> CreateDataGridViewGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor, PropertyInfo propInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            return CreateDefaultControlGroupFor(propertyName, command, new Func<Control>(() => {
                return CreateDefaultDataGridViewFor(propertyName, command, propInfo);
            }), propInfo, editor);
        }

        /// <summary>
        /// create control group. this method control body is created by method as argument. this method use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="createFunc"></param>
        /// <param name="propInfo"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        private static List<Control> CreateDefaultControlGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Func<Control> createFunc, PropertyInfo propInfo, Forms.frmCommandEditor editor)
        {
            var controlList = new List<Control>();

            // label
            var label = CreateDefaultLabelFor(propertyName, command, propInfo);
            controlList.Add(label);

            // 2nd label
            var attr2ndLabel = propInfo.GetCustomAttribute<PropertySecondaryLabel>();
            if (attr2ndLabel?.useSecondaryLabel ?? false)
            {
                var label2 = CreateSimpleLabel();
                label2.Name = "lbl2_" + propertyName;
                controlList.Add(label2);
            }

            var createdInput = createFunc();

            // ui helper
            controlList.AddRange(CreateDefaultUIHelpersFor(propertyName, command, createdInput, editor, propInfo));

            // custom ui helper
            controlList.AddRange(CreateCustomUIHelperFor(propertyName, command, new Control[] { createdInput }, editor, propInfo));

            // body
            controlList.Add(createdInput);

            return controlList;
        }
        #endregion

        #region create default controls

        #region label
        /// <summary>
        /// create Label from seveal attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="propInfo"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static Control CreateDefaultLabelFor(string propertyName, Core.Automation.Commands.ScriptCommand command, PropertyInfo propInfo = null, frmCommandEditor editor = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var setting = editor?.appSettings ?? CurrentEditor.appSettings;

            var labelText = GetLabelText(propertyName, propInfo, setting);

            // get addtional parameter info
            var attrAdditionalParams = (PropertyAddtionalParameterInfo[])propInfo.GetCustomAttributes(typeof(PropertyAddtionalParameterInfo));
            Dictionary<string, string> addParams = null;
            if (attrAdditionalParams.Length > 0)
            {
                Func<string, string> convFunc;
                if (editor == null)
                {
                    convFunc = new Func<string, string>((str) =>
                    {
                        return str;
                    });
                }
                else
                {
                    convFunc = new Func<string, string>((str) =>
                    {
                        return GetSampleUsageTextForLabel(str, editor.appSettings);
                    });
                }

                addParams = new Dictionary<string, string>();
                foreach (var p in attrAdditionalParams)
                {
                    addParams.Add(p.searchKey, convFunc(p.description));
                }
            }

            return CreateDefaultLabelFor(propertyName, command, labelText, addParams);
        }

        /// <summary>
        /// create Label. This method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="labelText"></param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        public static Control CreateDefaultLabelFor(string propertyName, Core.Automation.Commands.ScriptCommand command, string labelText, Dictionary<string, string> additionalParams = null)
        {
            var inputLabel = CreateSimpleLabel();

            inputLabel.Text = labelText;
            inputLabel.Name = "lbl_" + propertyName;

            if (additionalParams != null)
            {
                inputLabel.Tag = additionalParams;
            }

            return inputLabel;
        }
        #endregion

        #region textbox
        /// <summary>
        /// create TextBox from PropertyTextBoxSetting, PropertyRecommendedUIControl attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Control CreateDefaultInputFor(string propertyName, Core.Automation.Commands.ScriptCommand command, PropertyInfo propInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var textBoxSetting = propInfo.GetCustomAttribute<PropertyTextBoxSetting>() ?? new PropertyTextBoxSetting();
            var recommendedControl = propInfo.GetCustomAttribute<PropertyRecommendedUIControl>() ?? new PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox);

            TextBox newTextBox;
            
            int height = textBoxSetting.height;
            if (recommendedControl.recommendedControl == PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)
            {
                height = (height < 2) ? 3 : height; // multi line fix
            }
            newTextBox = (TextBox)CreateDefaultInputFor(propertyName, command, height * 30, 300, textBoxSetting.allowNewLine);

            return newTextBox;
        }

        /// <summary>
        /// create TextBox and binding property, this method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="allowNewLine"></param>
        /// <returns></returns>
        public static Control CreateDefaultInputFor(string propertyName, Core.Automation.Commands.ScriptCommand command, int height = 30, int width = 300, bool allowNewLine = true)
        {
            var inputBox = CreateStandardTextBoxFor(propertyName, command);

            // new line setting
            if (!allowNewLine)
            {
                inputBox.KeyDown += (sender, e) => TextBoxKeyDown_DenyEnterNewLine(sender, e);
                inputBox.ScrollBars = ScrollBars.None;
            }
            else
            {
                inputBox.Multiline = true;
                inputBox.ScrollBars = ScrollBars.Vertical;
            }

            inputBox.Height = height;
            inputBox.Width = width;

            return inputBox;
        }
        #endregion

        #region checkbox
        /// <summary>
        /// create CheckBox and binding property, this method try to use PropertyDescription attribute.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static Control CreateDefaultCheckBoxFor(string propertyName, Core.Automation.Commands.ScriptCommand command, PropertyInfo propInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var desc = propInfo.GetCustomAttribute<PropertyDescription>();
            return CreateDefaultCheckBoxFor(propertyName, command, desc?.propertyDescription ?? "");
        }

        /// <summary>
        /// create CheckBox and binding property, this method does not use attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Control CreateDefaultCheckBoxFor(string propertyName, Core.Automation.Commands.ScriptCommand command, string description)
        {
            var iputBox = CreateStandardCheckboxFor(propertyName, command);
            iputBox.Text = description;
            return iputBox;
        }
        #endregion

        #region combobox
        /// <summary>
        /// create ComboBox and binding property, some events, selection items. this method use PropertyIsWindowNamesList, PropertyIsVariableList, PropertyInstanceType, PropertyUISelectionOption, PropertySelectionChangeEvent attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="propInfo"></param>
        /// <param name="editor">if editor is null, does not support variable, window, instance selection items</param>
        /// <returns></returns>
        public static Control CreateDefaultDropdownFor(string propertyName, Core.Automation.Commands.ScriptCommand command, PropertyInfo propInfo = null, frmCommandEditor editor = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var uiOptions = new List<string>();

            // window names list
            var attrIsWin = propInfo.GetCustomAttribute<PropertyIsWindowNamesList>();
            if (attrIsWin?.isWindowNamesList ?? false)
            {
                uiOptions.AddRange(GetWindowNames(editor, attrIsWin.allowCurrentWindow, attrIsWin.allowCurrentWindow, attrIsWin.allowDesktop));
            }

            // variable names list
            var attrIsVar = propInfo.GetCustomAttribute<PropertyIsVariablesList>();
            if (attrIsVar?.isVariablesList ?? false)
            {
                uiOptions.AddRange(GetVariableNames(editor));
            }

            // instance name list
            var attrIsInstance = propInfo.GetCustomAttribute<PropertyInstanceType>();
            if ((attrIsInstance?.instanceType ?? PropertyInstanceType.InstanceType.none) != PropertyInstanceType.InstanceType.none)
            {
                uiOptions.AddRange(GetInstanceNames(editor, attrIsInstance.instanceType));
            }

            // ui options
            var opts = propInfo.GetCustomAttributes<PropertyUISelectionOption>(true);
            if (opts.Count() > 0)
            {
                uiOptions.AddRange(opts.Select(opt => opt.uiOption).ToList());
            }

            var changeEvent = propInfo.GetCustomAttribute<PropertySelectionChangeEvent>();

            return CreateDefaultDropdownFor(propertyName, command, uiOptions, changeEvent?.methodName ?? "");
        }

        /// <summary>
        /// create ComboBox and binding property, some events, selection items. this method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="uiOptions"></param>
        /// <param name="selectionChangeEventName"></param>
        /// <returns></returns>
        public static Control CreateDefaultDropdownFor(string propertyName, Core.Automation.Commands.ScriptCommand command, List<string> uiOptions, string selectionChangeEventName = "")
        {
            var inputBox = CreateStandardComboboxFor(propertyName, command);
            inputBox.Items.AddRange(uiOptions.ToArray());

            if (selectionChangeEventName != "")
            {
                (var trgMethod, var useOuterClassEvent) = GetMethodInfo(selectionChangeEventName, command);

                inputBox.SelectionChangeCommitted += useOuterClassEvent ?
                    (EventHandler)trgMethod.CreateDelegate(typeof(EventHandler)) :
                    (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), command, trgMethod);
            }

            return inputBox;
        }
        #endregion

        #region DataGridView
        /// <summary>
        /// create DataGridView and binding property, this method use PropertyDataGridViewSetting, PropertyDataGridViewColumnSettings, PropertyDataGridViewCellEditEvent attribute.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static DataGridView CreateDefaultDataGridViewFor(string propertyName, Core.Automation.Commands.ScriptCommand command, PropertyInfo propInfo = null)
        {
            //return CreateDataGridView(sourceCommand, dataSourceName, prop);
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            // DataGridView setting
            var dgvSetting = propInfo.GetCustomAttribute<PropertyDataGridViewSetting>() ?? new PropertyDataGridViewSetting();
            // columns setting
            var columnSetting = propInfo.GetCustomAttributes<PropertyDataGridViewColumnSettings>()?.ToList() ?? null;
            // events
            var events = propInfo.GetCustomAttributes<PropertyDataGridViewCellEditEvent>()?.ToList() ?? null;

            return CreateDefaultDataGridViewFor(propertyName, command, dgvSetting.allowAddRow, dgvSetting.allowDeleteRow, dgvSetting.allowResizeRow,
                    dgvSetting.width, dgvSetting.height,
                    dgvSetting.autoGenerateColumns, dgvSetting.headerRowHeight,
                    false, columnSetting, events);
        }


        /// <summary>
        /// create DataGridView and binding property, some events, selection items. this method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="allowAddRows"></param>
        /// <param name="allowDeleteRows"></param>
        /// <param name="allowResizeRows"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="autoGenerateColumns"></param>
        /// <param name="headerRowHeight"></param>
        /// <param name="allowSort"></param>
        /// <param name="columns"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        public static DataGridView CreateDefaultDataGridViewFor(string propertyName, Core.Automation.Commands.ScriptCommand command, bool allowAddRows = true, bool allowDeleteRows = true, bool allowResizeRows = false, int width = 400, int height = 250, bool autoGenerateColumns = true, int headerRowHeight = 1, bool allowSort = false, List<PropertyDataGridViewColumnSettings> columns = null, List<PropertyDataGridViewCellEditEvent> events = null)
        {
            var propInfo = command.GetProperty(propertyName);

            var dgv = CreateStandardDataGridViewFor(propertyName, command);

            // create DataTable
            if ((columns?.Count ?? 0) > 0)
            {
                var table = CreateDataTable(propertyName, command, dgv, columns);
                propInfo.SetValue(command, table);
            }

            // behavior
            dgv.AllowUserToAddRows = allowAddRows;
            dgv.AllowUserToDeleteRows = allowDeleteRows;
            dgv.AllowUserToResizeRows = allowResizeRows;
            dgv.AutoGenerateColumns = autoGenerateColumns;

            // looks
            if (width < 100)
            {
                width = 400;
            }
            if (height < 100)
            {
                height = 250;
            }

            dgv.Size = new Size(width, height);

            if (headerRowHeight > 1)
            {
                dgv.ColumnHeadersHeight = ((Convert.ToInt32(CurrentEditor.Theme.Datagridview.FontSize) + 15) * headerRowHeight);
            }

            // sort mode
            if ((dgv.Columns.Count > 0) && !allowSort)
            {
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }

            // set events
            if (events != null)
            {
                foreach (var ev in events)
                {
                    (var trgMethod, var useOuterClass) = GetMethodInfo(ev.methodName, command);

                    switch (ev.eventRaise)
                    {
                        case PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick:
                            DataGridViewCellEventHandler clickMethod = (useOuterClass) ?
                                (DataGridViewCellEventHandler)trgMethod.CreateDelegate(typeof(DataGridViewCellEventHandler)) :
                                (DataGridViewCellEventHandler)Delegate.CreateDelegate(typeof(DataGridViewCellEventHandler), command, trgMethod);
                            dgv.CellClick += clickMethod;
                            break;
                        case PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit:
                            DataGridViewCellCancelEventHandler beginEditMethod = (useOuterClass) ?
                                (DataGridViewCellCancelEventHandler)trgMethod.CreateDelegate(typeof(DataGridViewCellCancelEventHandler)) :
                                (DataGridViewCellCancelEventHandler)Delegate.CreateDelegate(typeof(DataGridViewCellCancelEventHandler), command, trgMethod);
                            dgv.CellBeginEdit += beginEditMethod;
                            break;
                    }
                }
            }

            return dgv;
        }
        #endregion

        #endregion

        #region create standard controls

        /// <summary>
        /// create ComboBox and binding property. This methods does not use attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static TextBox CreateStandardTextBoxFor(string propertyName, Core.Automation.Commands.ScriptCommand command)
        {
            var inputBox = new TextBox();
            var theme = CurrentEditor.Theme.Input;
            inputBox.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            inputBox.ForeColor = theme.FontColor;
            inputBox.BackColor = theme.BackColor;
            inputBox.DataBindings.Add("Text", command, propertyName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Name = propertyName;
            return inputBox;
        }

        /// <summary>
        /// create ComboBox and binding property and binding some events for cursor position. This methods does not use attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ComboBox CreateStandardComboboxFor(string propertyName, Core.Automation.Commands.ScriptCommand command)
        {
            var inputBox = new ComboBox();
            var theme = CurrentEditor.Theme.Combobox;
            inputBox.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            inputBox.ForeColor = theme.FontColor;
            inputBox.BackColor = theme.BackColor;
            inputBox.DataBindings.Add("Text", command, propertyName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Height = 30;
            inputBox.Width = 300;
            inputBox.Name = propertyName;

            // cursor position events
            inputBox.KeyUp += (sender, e) => ComboBoxKeyUp_SaveCursorPosition(sender, e);
            inputBox.Click += (sender, e) => ComboBoxClick_SaveCursorPosition(sender, e);

            return inputBox;
        }

        /// <summary>
        /// create CheckBox and binding property. This method does not use attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static CheckBox CreateStandardCheckboxFor(string propertyName, Core.Automation.Commands.ScriptCommand command)
        {
            var checkBox = new CheckBox();

            var theme = CurrentEditor.Theme.Checkbox;
            checkBox.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            checkBox.ForeColor = theme.FontColor;
            checkBox.BackColor = theme.BackColor;
            checkBox.DataBindings.Add("Checked", command, propertyName, false, DataSourceUpdateMode.OnPropertyChanged);
            checkBox.Name = propertyName;

            return checkBox;
        }

        /// <summary>
        ///  create DataGridView and binding property. This method does not use attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static DataGridView CreateStandardDataGridViewFor(string propertyName, Core.Automation.Commands.ScriptCommand command)
        {
            var dgv = new DataGridView();

            var theme = CurrentEditor.Theme.Datagridview;
            dgv.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            dgv.ForeColor = theme.FontColor;
            dgv.ColumnHeadersHeight = Convert.ToInt32(theme.FontSize) + 20;
            dgv.RowTemplate.Height = Convert.ToInt32(theme.FontSize) + 20;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DataBindings.Add("DataSource", command, propertyName, false, DataSourceUpdateMode.OnPropertyChanged);

            dgv.Name = propertyName;

            return dgv;
        }

        /// <summary>
        /// create Label, this method does not use attributes.
        /// </summary>
        /// <returns></returns>
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
        #endregion

        #region create Control support methods

        #region DataGridView methods
        /// <summary>
        /// create/init DataTable and DataGridView columns to specified arguments
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="dgv"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static DataTable CreateDataTable(string propertyName, Core.Automation.Commands.ScriptCommand command, DataGridView dgv, List<PropertyDataGridViewColumnSettings> columns = null)
        {
            var table = new DataTable
            {
                TableName = propertyName.Replace("v_", "") + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            // add column
            foreach (var colSetting in columns)
            {
                // DataTable Column
                table.Columns.Add(colSetting.columnName);
                table.Columns[table.Columns.Count - 1].DefaultValue = colSetting.defaultValue;

                // DataGridView Column
                DataGridViewColumn newDGVColumn;
                switch (colSetting.type)
                {
                    case PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox:
                        newDGVColumn = new DataGridViewTextBoxColumn();
                        break;
                    case PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox:
                        newDGVColumn = new DataGridViewComboBoxColumn();
                        var so = colSetting.comboBoxItems.Split('\n');
                        ((DataGridViewComboBoxColumn) newDGVColumn).Items.AddRange(so);
                        break;
                    case PropertyDataGridViewColumnSettings.DataGridViewColumnType.CheckBox:
                        newDGVColumn = new DataGridViewCheckBoxColumn();
                        break;
                    default:
                        newDGVColumn = new DataGridViewTextBoxColumn();
                        break;
                }
                newDGVColumn.HeaderText = colSetting.headerText;
                newDGVColumn.DataPropertyName = colSetting.columnName;
                newDGVColumn.ReadOnly = colSetting.readOnly;
                dgv.Columns.Add(newDGVColumn);
            }

            return table;
        }
        #endregion

        #region Label methods
        /// <summary>
        /// get text for Label. This method use PropertyDescription, PropertyShowSampleUsageInDescription, PropertyIsOptional attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propInfo"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static string GetLabelText(string propertyName, PropertyInfo propInfo, ApplicationSettings setting)
        {
            var attrDescription = propInfo.GetCustomAttribute<PropertyDescription>() ?? new PropertyDescription(propertyName);

            string labelText = setting.replaceApplicationKeyword(attrDescription.propertyDescription);

            // show sample usage
            if (setting.ClientSettings.ShowSampleUsageInDescription)
            {
                var attrShowSample = propInfo.GetCustomAttribute<PropertyShowSampleUsageInDescription>();
                if (attrShowSample?.showSampleUsage ?? false)
                {
                    if (!labelText.Contains("(ex."))
                    {
                        var attrSample = propInfo.GetCustomAttribute<SampleUsage>();
                        var sampleText = GetSampleUsageTextForLabel(attrSample?.sampleUsage ?? "", setting);
                        if (sampleText.Length > 0)
                        {
                            labelText += " (ex. " + sampleText + ")";
                        }
                    }
                }
            }

            // show optional
            var attrIsOpt = propInfo.GetCustomAttribute<PropertyIsOptional>();
            if (attrIsOpt?.isOptional ?? false)
            {
                if (!labelText.Contains("Optional"))
                {
                    labelText = "Optional - " + labelText;
                }

                if ((attrIsOpt.setBlankToValue != "") && (!labelText.Contains("Default is")))
                {
                    labelText += " (Default is " + attrIsOpt.setBlankToValue + ")";
                }
            }

            return labelText;
        }

        #region keyword md format

        /// <summary>
        /// get SampleUsage text for Label (Description)
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        private static string GetSampleUsageTextForLabel(string sample, ApplicationSettings setting)
        {
            return setting.replaceApplicationKeyword(GetTextMDFormat(sample)).Replace(" or ", ", ");
        }

        private static string GetTextMDFormat(this string targetString)
        {
            int idxAster, idxTable;
            string ret = "";
            while (targetString.Length > 0)
            {
                idxAster = targetString.IndexOf("\\*");
                idxTable = targetString.IndexOf("\\|");
                if (idxAster >= 0 || idxTable >= 0)
                {
                    if ((idxAster >= 0) && (idxTable < 0))
                    {
                        ret += targetString.Substring(0, idxAster).RemoveMDFormat() + "*";
                        targetString = targetString.Substring(idxAster + 1);
                    }
                    else if ((idxTable >= 0) && (idxAster < 0))
                    {
                        ret += targetString.Substring(0, idxTable).RemoveMDFormat() + "|";
                        targetString = targetString.Substring(idxTable + 1);
                    }
                    else if (idxAster < idxTable)
                    {
                        ret += targetString.Substring(0, idxAster).RemoveMDFormat() + "*";
                        targetString = targetString.Substring(idxAster + 1);
                    }
                    else if (idxTable < idxAster)
                    {
                        ret += targetString.Substring(0, idxTable).RemoveMDFormat() + "|";
                        targetString = targetString.Substring(idxTable + 1);
                    }
                }
                else
                {
                    ret += targetString.RemoveMDFormat();
                    targetString = "";
                }
            }
            return ret;
        }

        private static string RemoveMDFormat(this string targetString)
        {
            return targetString.Replace("*", "").Replace("**", "").Replace("|", "");
        }
        #endregion
        #endregion

        #region ComboBox methods
        
        /// <summary>
        /// add windows names to specified ComboBox
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="editor"></param>
        /// <param name="addCurrentWindow"></param>
        /// <param name="addAllWindows"></param>
        /// <param name="addDesktop"></param>
        /// <returns></returns>
        public static ComboBox AddWindowNames(this ComboBox cbo, Forms.frmCommandEditor editor = null, bool addCurrentWindow = true, bool addAllWindows = false, bool addDesktop = false)
        {
            if (cbo == null)
                return null;

            cbo.BeginUpdate();

            cbo.Items.Clear();

            cbo.Items.AddRange(GetWindowNames(editor, addCurrentWindow, addAllWindows, addDesktop).ToArray());

            cbo.EndUpdate();

            return cbo;
        }

        /// <summary>
        /// add variable names to specified ComboBox
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static ComboBox AddVariableNames(this ComboBox cbo, Forms.frmCommandEditor editor)
        {
            if (cbo == null)
                return null;

            cbo.BeginUpdate();

            cbo.Items.Clear();

            cbo.Items.AddRange(GetVariableNames(editor).ToArray());

            cbo.EndUpdate();

            return cbo;
        }

        
        /// <summary>
        /// add Instace names to specified ComboBox
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="editor"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static ComboBox AddInstanceNames(this ComboBox cbo, Forms.frmCommandEditor editor, PropertyInstanceType.InstanceType tp)
        {
            if ((cbo == null) || (editor == null))
            {
                return null;
            }

            cbo.BeginUpdate();

            cbo.Items.Clear();

            cbo.Items.AddRange(GetInstanceNames(editor, tp).ToArray());

            cbo.EndUpdate();

            return cbo;
        }

        /// <summary>
        /// get Window Names list
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="addCurrentWindow"></param>
        /// <param name="addAllWindows"></param>
        /// <param name="addDesktop"></param>
        /// <returns></returns>
        public static List<string> GetWindowNames(Forms.frmCommandEditor editor = null, bool addCurrentWindow = true, bool addAllWindows = false, bool addDesktop = false)
        {
            var lst = new List<string>();

            if (addCurrentWindow)
            {
                lst.Add(editor?.appSettings.EngineSettings.CurrentWindowKeyword ?? "Current Window");
            }

            if (addAllWindows)
            {
                lst.Add(editor?.appSettings.EngineSettings.AllWindowsKeyword ?? "All Windows");
            }
            if (addDesktop)
            {
                lst.Add(editor.appSettings.EngineSettings.DesktopKeyword ?? "Desktop");
            }
            lst.AddRange(WindowNameControls.GetAllWindowTitles());

            return lst;
        }

        #region create ComboBox items list
        /// <summary>
        /// get variable names list
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static List<string> GetVariableNames(Forms.frmCommandEditor editor)
        {
            return editor?.scriptVariables.Select(v => v.VariableName).ToList() ?? new List<string>();
        }

        /// <summary>
        /// get instance names list
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static List<string> GetInstanceNames(Forms.frmCommandEditor editor, PropertyInstanceType.InstanceType tp)
        {
            if (editor == null)
            {
                return new List<string>();
            }

            string sortOrder = editor.appSettings.ClientSettings.InstanceNameOrder.ToLower();

            Dictionary<string, int> instanceList = editor.instanceList.getInstanceClone(tp, (sortOrder == "frequency of use"));

            string defInstanceName = "";
            switch (tp)
            {
                case PropertyInstanceType.InstanceType.DataBase:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultDBInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.Excel:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.IE:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.NLG:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultNLGInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.WebBrowser:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.StopWatch:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultStopWatchInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.Word:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultWordInstanceName;
                    break;
            }
            if ((defInstanceName != "") && !instanceList.ContainsKey(defInstanceName))
            {
                instanceList.Add(defInstanceName, 0);
            }

            List<string> sortedInstance;
            switch (editor.appSettings.ClientSettings.InstanceNameOrder.ToLower())
            {
                case "no sorting":
                    sortedInstance = instanceList.Keys.ToList();
                    break;
                case "by name":
                    sortedInstance = instanceList.OrderBy(t => t.Key).Select(v => v.Key).ToList();
                    break;
                case "creation frequently":
                case "frequency of use":
                default:
                    sortedInstance = instanceList.OrderByDescending(t => t.Value).Select(v => v.Key).ToList();
                    break;
            }

            return sortedInstance;
        }

        #endregion

        /// <summary>
        /// get MethodInfo from name. If methodName contains "+", it means use outer class method.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static (MethodInfo, bool) GetMethodInfo(string methodName, ScriptCommand command)
        {
            bool useOuterClassEvent = methodName.Contains("+");
            MethodInfo trgMethod;
            if (useOuterClassEvent)
            {
                int idx = methodName.IndexOf("+");
                string className = methodName.Substring(0, idx);
                string shortMethodName = methodName.Substring(idx + 1);
                var tp = Type.GetType("taskt.Core.Automation.Commands." + className);
                trgMethod = tp.GetMethod(shortMethodName, BindingFlags.Public | BindingFlags.Static);
            }
            else
            {
                trgMethod = command.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }

            if (trgMethod == null)
            {
                throw new Exception("Method '" + methodName + "' does not exists. Command: " + command.CommandName);
            }

            return (trgMethod, useOuterClassEvent);
        }
        #endregion

        #region Control event handlers

        /// <summary>
        /// deny create new line when type Enter in TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TextBoxKeyDown_DenyEnterNewLine(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// remember cursor position in ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ComboBoxKeyUp_SaveCursorPosition(object sender, KeyEventArgs e)
        {
            ComboBox trg = (ComboBox)sender;
            trg.Tag = trg.SelectionStart;
        }
        /// <summary>
        /// remember cursor position in ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ComboBoxClick_SaveCursorPosition(object sender, EventArgs e)
        {
            ComboBox trg = (ComboBox)sender;
            trg.Tag = trg.SelectionStart;
        }
        #endregion

        #endregion

        
        public static CommandItemControl CreateSimpleUIHelper(string controlName, Control targetControl)
        {
            var theme = CurrentEditor.Theme.UIHelper;
            CommandItemControl helperControl = new CommandItemControl
            {
                Padding = new Padding(10, 0, 0, 0),
                Font = new Font(theme.Font, theme.FontSize, theme.Style),
                ForeColor = theme.FontColor,
                BackColor = theme.BackColor,
                Name = controlName,
                Tag = targetControl
            };

            return helperControl;
        }

        public static CommandItemControl CreateDefaultUIHelperFor(string propertyName, PropertyUIHelper setting, int num, Control targetControl, Forms.frmCommandEditor editor)
        {
            var uiHelper = CreateSimpleUIHelper(propertyName + "_helper_" + num, targetControl);
            uiHelper.HelperType = setting.additionalHelper;
            switch (setting.additionalHelper)
            {
                case PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                    //show variable selector
                    uiHelper.CommandImage = Images.GetUIImage("VariableCommand");
                    uiHelper.CommandDisplay = "Insert Variable";
                    uiHelper.DrawIcon = Properties.Resources.taskt_variable_helper;
                    uiHelper.Click += (sender, e) => ShowVariableSelector(sender, e, editor);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper:
                    //show file selector
                    uiHelper.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                    uiHelper.CommandDisplay = "Select a File";
                    uiHelper.DrawIcon = Properties.Resources.taskt_file_helper;
                    uiHelper.Click += (sender, e) => ShowFileSelector(sender, e, editor);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper:
                    //show folder selector
                    uiHelper.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                    uiHelper.CommandDisplay = "Select a Folder";
                    uiHelper.DrawIcon = Properties.Resources.taskt_folder_helper;
                    uiHelper.Click += (sender, e) => ShowFolderSelector(sender, e, editor);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowCodeBuilder:
                    //show code builder
                    uiHelper.CommandImage = Images.GetUIImage("RunScriptCommand");
                    uiHelper.CommandDisplay = "Code Builder";
                    uiHelper.Click += (sender, e) => ShowCodeBuilder(sender, e, editor);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper:
                    // show mouse position
                    uiHelper.CommandImage = Images.GetUIImage("SendMouseMoveCommand");
                    uiHelper.CommandDisplay = "Capture Mouse Position";
                    uiHelper.DrawIcon = Properties.Resources.taskt_element_helper;
                    uiHelper.Click += (sender, e) => ShowMouseCaptureForm(sender, e);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowElementRecorder:
                    // show ui element recorder
                    uiHelper.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                    uiHelper.CommandDisplay = "Element Recorder";
                    uiHelper.Click += (sender, e) => ShowElementRecorder(sender, e, editor);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.GenerateDLLParameters:
                    // show dll parameters
                    uiHelper.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                    uiHelper.CommandDisplay = "Generate Parameters";
                    uiHelper.Click += (sender, e) => GenerateDLLParameters(sender, e);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowDLLExplorer:
                    // show dll explorer
                    uiHelper.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                    uiHelper.CommandDisplay = "Launch DLL Explorer";
                    uiHelper.Click += (sender, e) => ShowDLLExplorer(sender, e);
                    break;
                case PropertyUIHelper.UIAdditionalHelperType.AddInputParameter:
                    // show input parameter
                    uiHelper.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                    uiHelper.CommandDisplay = "Add Input Parameter";
                    uiHelper.Click += (sender, e) => AddInputParameter(sender, e, editor);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowHTMLBuilder:
                    // show html builder
                    uiHelper.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                    uiHelper.CommandDisplay = "Launch HTML Builder";
                    uiHelper.Click += (sender, e) => ShowHTMLBuilder(sender, e, editor);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowIfBuilder:
                    // show if builder
                    uiHelper.CommandImage = Images.GetUIImage("VariableCommand");
                    uiHelper.CommandDisplay = "Add New If Statement";
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowLoopBuilder:
                    // show loop builder
                    uiHelper.CommandImage = Images.GetUIImage("VariableCommand");
                    uiHelper.CommandDisplay = "Add New Loop Statement";
                    break;
            }
            return uiHelper;
        }

        public static List<Control> CreateDefaultUIHelpersFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Control targetControl, Forms.frmCommandEditor editor, PropertyInfo propInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var propertyUIHelpers = propInfo.GetCustomAttributes<PropertyUIHelper>().ToList();
            
            var controlList = new List<Control>();
            if (propertyUIHelpers.Count() == 0)
            {
                return controlList;
            }

            int count = 0;
            foreach (PropertyUIHelper uiHelper in propertyUIHelpers)
            {
                //CommandItemControl helperControl = new CommandItemControl();
                //helperControl.Padding = new Padding(10, 0, 0, 0);
                ////helperControl.ForeColor = Color.AliceBlue;
                ////helperControl.Font = new Font("Segoe UI Semilight", 10);
                //var theme = CurrentEditor.Theme.UIHelper;
                //helperControl.Font = new Font(theme.Font, theme.FontSize, theme.Style);
                //helperControl.ForeColor = theme.FontColor;
                //helperControl.BackColor = theme.BackColor;
                //helperControl.Name = propertyName + "_helper_" + count.ToString();
                //helperControl.Tag = targetControls.FirstOrDefault();
                //helperControl.HelperType = attrib.additionalHelper;

                //switch (attrib.additionalHelper)
                //{
                //    case PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                //        //show variable selector
                //        helperControl.CommandImage = Images.GetUIImage("VariableCommand");
                //        helperControl.CommandDisplay = "Insert Variable";
                //        helperControl.DrawIcon = Properties.Resources.taskt_variable_helper;
                //        helperControl.Click += (sender, e) => ShowVariableSelector(sender, e, editor);
                //        break;

                //    case PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper:
                //        //show file selector
                //        helperControl.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                //        helperControl.CommandDisplay = "Select a File";
                //        helperControl.DrawIcon = Properties.Resources.taskt_file_helper;
                //        helperControl.Click += (sender, e) => ShowFileSelector(sender, e, editor);
                //        break;

                //    case PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper:
                //        //show file selector
                //        helperControl.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                //        helperControl.CommandDisplay = "Select a Folder";
                //        helperControl.DrawIcon = Properties.Resources.taskt_folder_helper;
                //        helperControl.Click += (sender, e) => ShowFolderSelector(sender, e, editor);
                //        break;

                //    case PropertyUIHelper.UIAdditionalHelperType.ShowImageRecogitionHelper:
                //        //show file selector
                //        helperControl.CommandImage = Images.GetUIImage("OCRCommand");
                //        helperControl.CommandDisplay = "Capture Reference Image";
                //        helperControl.DrawIcon = Properties.Resources.taskt_element_helper;
                //        helperControl.Click += (sender, e) => ShowImageCapture(sender, e, editor);

                //        CommandItemControl testRun = new CommandItemControl();
                //        testRun.Padding = new Padding(10, 0, 0, 0);
                //        testRun.ForeColor = Color.AliceBlue;

                //        testRun.CommandImage = Images.GetUIImage("OCRCommand");
                //        testRun.CommandDisplay = "Run Image Recognition Test";
                //        testRun.ForeColor = Color.AliceBlue;
                //        testRun.Tag = targetControls.FirstOrDefault();
                //        testRun.Click += (sender, e) => RunImageCapture(sender, e);
                //        controlList.Add(testRun);
                //        break;

                //    case PropertyUIHelper.UIAdditionalHelperType.ShowCodeBuilder:
                //        //show variable selector
                //        helperControl.CommandImage = Images.GetUIImage("RunScriptCommand");
                //        helperControl.CommandDisplay = "Code Builder";
                //        helperControl.Click += (sender, e) => ShowCodeBuilder(sender, e, editor);
                //        break;

                //    case PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper:
                //        helperControl.CommandImage = Images.GetUIImage("SendMouseMoveCommand");
                //        helperControl.CommandDisplay = "Capture Mouse Position";
                //        //helperControl.ForeColor = Color.AliceBlue;
                //        helperControl.DrawIcon = Properties.Resources.taskt_element_helper;
                //        helperControl.Click += (sender, e) => ShowMouseCaptureForm(sender, e);
                //        break;
                //    case PropertyUIHelper.UIAdditionalHelperType.ShowElementRecorder:
                //        //show variable selector
                //        helperControl.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                //        helperControl.CommandDisplay = "Element Recorder";
                //        helperControl.Click += (sender, e) => ShowElementRecorder(sender, e, editor);
                //        break;
                //    case PropertyUIHelper.UIAdditionalHelperType.GenerateDLLParameters:
                //        //show variable selector
                //        helperControl.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                //        helperControl.CommandDisplay = "Generate Parameters";
                //        helperControl.Click += (sender, e) => GenerateDLLParameters(sender, e);
                //        break;
                //    case PropertyUIHelper.UIAdditionalHelperType.ShowDLLExplorer:
                //        //show variable selector
                //        helperControl.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                //        helperControl.CommandDisplay = "Launch DLL Explorer";
                //        helperControl.Click += (sender, e) => ShowDLLExplorer(sender, e);
                //        break;
                //    case PropertyUIHelper.UIAdditionalHelperType.AddInputParameter:
                //        //show variable selector
                //        helperControl.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                //        helperControl.CommandDisplay = "Add Input Parameter";
                //        helperControl.Click += (sender, e) => AddInputParameter(sender, e, editor);
                //        break;
                //    case PropertyUIHelper.UIAdditionalHelperType.ShowHTMLBuilder:
                //        helperControl.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                //        helperControl.CommandDisplay = "Launch HTML Builder";
                //        helperControl.Click += (sender, e) => ShowHTMLBuilder(sender, e, editor);
                //        break;
                //    case PropertyUIHelper.UIAdditionalHelperType.ShowIfBuilder:
                //        //show variable selector
                //        helperControl.CommandImage = Images.GetUIImage("VariableCommand");
                //        helperControl.CommandDisplay = "Add New If Statement";
                //        break;
                //    case PropertyUIHelper.UIAdditionalHelperType.ShowLoopBuilder:
                //        //show variable selector
                //        helperControl.CommandImage = Images.GetUIImage("VariableCommand");
                //        helperControl.CommandDisplay = "Add New Loop Statement";
                //        break;

                //        //default:
                //        //    MessageBox.Show("Command Helper does not exist for: " + attrib.additionalHelper.ToString());
                //        //    break;
                //}

                
                switch (uiHelper.additionalHelper)
                {
                    case PropertyUIHelper.UIAdditionalHelperType.ShowImageRecogitionHelper:
                        //show file selector
                        CommandItemControl captureHelper = CreateSimpleUIHelper(propertyName + "_helper_" + count, targetControl);
                        captureHelper.CommandImage = Images.GetUIImage("OCRCommand");
                        captureHelper.CommandDisplay = "Capture Reference Image";
                        captureHelper.DrawIcon = Properties.Resources.taskt_element_helper;
                        captureHelper.Click += (sender, e) => ShowImageCapture(sender, e, editor);

                        count++;
                        CommandItemControl testHelper = CreateSimpleUIHelper(propertyName + "_helper_" + count, targetControl);
                        testHelper.CommandImage = Images.GetUIImage("OCRCommand");
                        testHelper.CommandDisplay = "Run Image Recognition Test";
                        testHelper.ForeColor = Color.AliceBlue;
                        testHelper.Click += (sender, e) => RunImageCapture(sender, e);

                        controlList.Add(captureHelper);
                        controlList.Add(testHelper);
                        break;

                    default:
                        controlList.Add(CreateDefaultUIHelperFor(propertyName, uiHelper, count, targetControl, editor));
                        break;
                }

                count++;
            }

            return controlList;
        }

        public static List<Control> CreateCustomUIHelperFor(string parameterName, Core.Automation.Commands.ScriptCommand parent, Control[] targetControls, Forms.frmCommandEditor editor, PropertyInfo pInfo)
        {
            List<Control> ctrls = new List<Control>();

            var attrs = (PropertyCustomUIHelper[])pInfo.GetCustomAttributes(typeof(PropertyCustomUIHelper));

            if (attrs.Length > 0)
            {
                int counter = 0;
                Type parentType = parent.GetType();
                foreach (var attr in attrs)
                {
                    var link = CreateSimpleUIHelper();
                    link.CommandDisplay = attr.labelText;
                    link.Name = parameterName + "_customhelper_" + ((attr.nameKey == "") ? counter.ToString() : attr.nameKey);
                    link.Tag = targetControls.FirstOrDefault();

                    bool useOuterMethod = attr.methodName.Contains("+");

                    MethodInfo trgMethod;
                    EventHandler dMethod;
                    if (useOuterMethod)
                    {
                        int idx = attr.methodName.IndexOf("+");
                        string className = attr.methodName.Substring(0, idx);
                        string methodName = attr.methodName.Substring(idx + 1);
                        var tp = Type.GetType("taskt.Core.Automation.Commands." + className);
                        trgMethod = tp.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
                        dMethod = (EventHandler)trgMethod.CreateDelegate(typeof(EventHandler));
                    }
                    else
                    {
                        trgMethod = parentType.GetMethod(attr.methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        dMethod = (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), parent, trgMethod);
                    }

                    link.Click += dMethod;

                    ctrls.Add(link);

                    counter++;
                }
            }
            return ctrls;
        }

        private static void ShowCodeBuilder(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {
            //get textbox text
            CommandItemControl commandItem = (CommandItemControl)sender;
            TextBox targetTextbox = (TextBox)commandItem.Tag;

            using (Forms.Supplemental.frmCodeBuilder codeBuilder = new Forms.Supplemental.frmCodeBuilder(targetTextbox.Text))
            {
                if (codeBuilder.ShowDialog() == DialogResult.OK)
                {

                    targetTextbox.Text = codeBuilder.rtbCode.Text;
                }
            }
        }
        private static void ShowMouseCaptureForm(object sender, EventArgs e)
        {
            using (Forms.Supplemental.frmShowCursorPosition frmShowCursorPos = new Forms.Supplemental.frmShowCursorPosition())
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
        public static void ShowVariableSelector(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {
            //get copy of user variables and append system variables, then load to combobox
            var variableList = CurrentEditor.scriptVariables.Select(f => f.VariableName).ToList();
            variableList.AddRange(Common.GenerateSystemVariables().Select(f => f.VariableName));

            //create variable selector form
            Forms.Supplemental.frmItemSelector newVariableSelector = new Forms.Supplemental.frmItemSelector(variableList);

            ////get copy of user variables and append system variables, then load to combobox
            //var variableList = CurrentEditor.scriptVariables.Select(f => f.VariableName).ToList();
            //variableList.AddRange(Core.Common.GenerateSystemVariables().Select(f => f.VariableName));
            //newVariableSelector.lstVariables.Items.AddRange(variableList.ToArray());

            //if user pressed "OK"
            if (newVariableSelector.ShowDialog() == DialogResult.OK)
            {
                //ensure that a variable was actually selected
                //if (newVariableSelector.lstVariables.SelectedItem == null)
                if (newVariableSelector.selectedItem == null)
                {
                    //return out as nothing was selected
                    MessageBox.Show("There were no variables selected!");
                    return;
                }

                //grab the referenced input assigned to the 'insert variable' button instance
                CommandItemControl inputBox = (CommandItemControl)sender;
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
                        //string ins = string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                        string ins = string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
                        targetTextbox.Text = str.Substring(0, cursorPos) + ins + str.Substring(cursorPos);
                        targetTextbox.Focus();
                        targetTextbox.SelectionStart = cursorPos + ins.Length;
                        targetTextbox.SelectionLength = 0;
                    }
                    else
                    {
                        //targetTextbox.Text = targetTextbox.Text + string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                        targetTextbox.Text = targetTextbox.Text + string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
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
                        if (targetCombobox.Tag == null)
                        {
                            targetCombobox.Tag = 0;
                        }
                        if (!int.TryParse(targetCombobox.Tag.ToString(), out cursorPos))
                        {
                            cursorPos = str.Length;
                        }
                        //string ins = string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                        string ins = string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
                        targetCombobox.Text = str.Substring(0, cursorPos) + ins + str.Substring(cursorPos);
                        targetCombobox.Focus();
                        targetCombobox.SelectionStart = cursorPos + ins.Length;
                        targetCombobox.SelectionLength = 0;
                    }
                    else
                    {
                        //concat variable name with brackets [vVariable] as engine searches for the same
                        //targetCombobox.Text = targetCombobox.Text + string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                        targetCombobox.Text = targetCombobox.Text + string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
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
                    //targetCell.Value = targetCell.Value + string.Concat(settings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.VariableEndMarker);
                    targetCell.Value = targetCell.Value + string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
                }
            }
        }
        private static void ShowFileSelector(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    CommandItemControl inputBox = (CommandItemControl)sender;
                    //currently variable insertion is only available for simply textboxes
                    TextBox targetTextbox = (TextBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetTextbox.Text = ofd.FileName;
                }
            }
        }
        private static void ShowFolderSelector(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    CommandItemControl inputBox = (CommandItemControl)sender;
                    TextBox targetTextBox = (TextBox)inputBox.Tag;
                    targetTextBox.Text = fbd.SelectedPath;
                }
            }
        }
        private static void ShowImageCapture(object sender, EventArgs e, Forms.frmCommandEditor editor)
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
                        CommandItemControl inputBox = (CommandItemControl)sender;
                        UIPictureBox targetPictureBox = (UIPictureBox)inputBox.Tag;
                        targetPictureBox.Image = imageCaptureForm.userSelectedBitmap;
                        var convertedImage = Common.ImageToBase64(imageCaptureForm.userSelectedBitmap);
                        var convertedLength = convertedImage.Length;
                        targetPictureBox.EncodedImage = convertedImage;

                        // force set property value
                        if (editor.selectedCommand.CommandName == "ImageRecognitionCommand")
                        {
                            ((Core.Automation.Commands.ImageRecognitionCommand)editor.selectedCommand).v_ImageCapture = convertedImage;
                        }
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
            CommandItemControl inputBox = (CommandItemControl)sender;
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
                imageRecognitionCommand.RunCommand(new Core.Automation.Engine.AutomationEngineInstance());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            //show all forms
            ShowAllForms();


        }
        private static void ShowElementRecorder(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {
            //get command reference
            Core.Automation.Commands.UIAutomationCommand cmd = (Core.Automation.Commands.UIAutomationCommand)editor.selectedCommand;

            //create recorder
            Forms.Supplemental.frmThickAppElementRecorder newElementRecorder = new Forms.Supplemental.frmThickAppElementRecorder();
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
            using (Forms.Supplemental.frmDLLExplorer dllExplorer = new Forms.Supplemental.frmDLLExplorer())
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
        private static void ShowHTMLBuilder(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {
            using (var htmlForm = new Forms.Supplemental.frmHTMLBuilder())
            {
                RichTextBox inputControl = (RichTextBox)editor.flw_InputVariables.Controls["v_InputHTML"];
                htmlForm.rtbHTML.Text = inputControl.Text;

                if (htmlForm.ShowDialog() == DialogResult.OK)
                {
                    inputControl.Text = htmlForm.rtbHTML.Text;
                }
            }
        }


        private static void AddInputParameter(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {

            DataGridView inputControl = (DataGridView)CurrentEditor.flw_InputVariables.Controls["v_UserInputConfig"];
            var inputTable = (DataTable)inputControl.DataSource;
            var newRow = inputTable.NewRow();
            newRow["Size"] = "500,100";
            inputTable.Rows.Add(newRow);

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

        #region search control methods
        public static List<Control> GetControlsByName(this List<Control> ctrls, string parameterName, CommandControlType t = CommandControlType.Body)
        {
            List<Control> ret = new List<Control>();

            switch (t)
            {
                case CommandControlType.Body:
                    ret.Add(ctrls.Where(c => (c.Name == parameterName)).FirstOrDefault());
                    break;

                case CommandControlType.Label:
                    ret.Add(ctrls.Where(c => (c.Name == "lbl_" + parameterName)).FirstOrDefault());
                    break;

                case CommandControlType.SecondLabel:
                    ret.Add(ctrls.Where(c => (c.Name == "lbl2_" + parameterName)).FirstOrDefault());
                    break;

                case CommandControlType.Helpers:
                    ret.AddRange(ctrls.Where(c => (c.Name.StartsWith(parameterName + "_helper_"))).ToArray());
                    break;

                case CommandControlType.CunstomHelpers:
                    ret.AddRange(ctrls.Where(c => (c.Name.StartsWith(parameterName + "_customhelper_"))).ToArray());
                    break;
            }

            return ret;
        }

        public static List<Control> GetControlGroup(this List<Control> ctrls, string parameterName, string nextParameterName = "")
        {
            List<Control> ret = new List<Control>();

            int index = ctrls.FindIndex(t => (t.Name == "lbl_" + parameterName));
            int last = (nextParameterName == "") ? ctrls.Count : ctrls.FindIndex(t => (t.Name == "lbl_" + nextParameterName));

            for (int i = index; i < last; i++)
            {
                ret.Add(ctrls[i]);
            }

            return ret;
        }

        public static Control GetPropertyControl(this Dictionary<string, Control> controls, string propertyName)
        {
            if (controls.ContainsKey(propertyName))
            {
                return controls[propertyName];
            }
            else
            {
                throw new Exception("Control '" + propertyName + "' does not exists.");
            }
        }
        public static Label GetPropertyControlLabel(this Dictionary<string, Control> controls, string propertyName)
        {
            if (controls.ContainsKey("lbl_" + propertyName))
            {
                return (Label)controls["lbl_" + propertyName];
            }
            else
            {
                throw new Exception("Label 'lbl_" + propertyName + "' does not exists.");
            }
        }
        public static Label GetPropertyControl2ndLabel(this Dictionary<string, Control> controls, string propertyName)
        {
            if (controls.ContainsKey("lbl2_" + propertyName))
            {
                return (Label)controls["lbl2_" + propertyName];
            }
            else
            {
                throw new Exception("2nd Label 'lbl2_" + propertyName + "' does not exists.");
            }
        }
        public static (Control body, Label label, Label label2nd) GetAllPropertyControl(this Dictionary<string, Control> controls, string propertyName, bool throwWhenLabelNotExists = true, bool throwWhen2ndLabelNotExists = false)
        {
            Control body = controls.GetPropertyControl(propertyName);

            Label label;
            try
            {
                label = controls.GetPropertyControlLabel(propertyName);
            }
            catch (Exception ex)
            {
                if (throwWhenLabelNotExists)
                {
                    throw ex;
                }
                else
                {
                    label = null;
                }
            }
            Label label2nd;
            try
            {
                label2nd = controls.GetPropertyControl2ndLabel(propertyName);
            }
            catch (Exception ex)
            {
                if (throwWhen2ndLabelNotExists)
                {
                    throw ex;
                }
                else
                {
                    label2nd = null;
                }
            }
            return (body, label, label2nd);
        }
        #endregion
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
        public void RenderUIComponents(Forms.frmCommandEditor editorForm)
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
        public void Bind(Forms.frmCommandEditor editor)
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
                        case PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
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

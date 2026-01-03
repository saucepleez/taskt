using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class VP_UIElementControls
    {
        /// <summary>
        /// input UIElement
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_InputInstanceName))]
        [PropertyDescription("UIElement Variable Name")]
        [InputSpecification("UIElement Variable Name", true)]
        [PropertyDetailSampleUsage("**vElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.UIElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("UIElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "UIElement")]
        public static string v_InputUIElementName { get; }

        /// <summary>
        /// output UIElement property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store UIElement")]
        [InputSpecification("UIElement Variable Name", true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.UIElement, true)]
        [PropertyValidationRule("UIElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "UIElement")]
        public static string v_OutputUIElementName { get; }

        /// <summary>
        /// window UIElement variable name
        /// </summary>
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_OutputUIElementName))]
        [PropertyDescription("Variable Name to Store Window UIElement")]
        [InputSpecification("Window UIElement Variable Name", true)]
        [PropertyIsOptional(true, "")]
        [PropertyValidationRule("Window UIElement", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Window UIElement")]
        public static string v_WindowUIElementName { get; }

        /// <summary>
        /// New output UIElement name
        /// </summary>
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_OutputUIElementName))]
        [PropertyDescription("UIElement Variable Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**vNewElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vNewElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyValidationRule("New UIElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New UIElement")]
        public static string v_NewOutputUIElementName { get; }

        /// <summary>
        /// wait time before action
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time before Action (sec)")]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(false, "Wait Time Before", "s")]
        public static string v_WaitTimeBeforeAction { get; }

        /// <summary>
        /// wait time after action
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time after Action (sec)")]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(false, "Wait Time Before", "s")]
        public static string v_WaitTimeAfterAction { get; }

        /// <summary>
        /// activate window before action
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Activate Window before Action")]
        [PropertyIsOptional(true, "No")]
        public static string v_ActivateWindow { get; }

        /// <summary>
        /// when this action is not supported
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnore))]
        [PropertyDescription("When Action Is Not Supported")]
        [PropertyIsOptional(true, "Error")]
        public static string v_WhenActionIsNotSupported { get; }

        /// <summary>
        /// wait time for UIElement
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time for the UIElement to Exist (sec)")]
        [Remarks("Specify how long to Wait before an Error will occur because the UIElement is Not Found.")]
        [PropertyIsOptional(true, "10")]
        [PropertyFirstValue("10")]
        public static string v_WaitTimeForUIElement { get; }

        /// <summary>
        /// Search paramters property (DataTable)
        /// </summary>
        [PropertyDescription("Search Parameters")]
        [PropertyCustomUIHelper("GUI Inspect Tool", nameof(VP_UIElementControls) + "+" + nameof(lnkGUIInspectTool_UsedByInspectResult_Click))]
        [PropertyCustomUIHelper("Inspect Tool Parser", nameof(VP_UIElementControls) + "+" + nameof(lnkInspectToolParser_Click))]
        [PropertyCustomUIHelper("Add Empty Parameters", nameof(VP_UIElementControls) + "+" + nameof(lnkAddEmptyParameter_Click))]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Search Paramters", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true)]
        [PropertyDataGridViewColumnSettings("Enabled", "Enabled", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.CheckBox)]
        [PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewCellEditEvent(nameof(VP_UIElementControls) + "+" + nameof(UIElementSearchDGV_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(VP_UIElementControls) + "+" + nameof(UIElementSearchDGV_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDataGridViewInitMethod(nameof(VP_UIElementControls) + "+" + nameof(CreateEmptySearchParamters))]
        [PropertyParameterOrder(5000)]
        public static string v_SearchParameters { get; }

        /// <summary>
        /// Maxinum number of Sibling Node for Search UIElements
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Maxinum number of Sibling Nodes for Search UIElements")]
        [InputSpecification("Number Greater than or Equal 0")]
        [PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value, "Max Siblings")]
        [PropertyDetailSampleUsage("**{{{vMax}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Max Siblings")]
        [PropertyDetailSampleUsage("**0**", "Search All Sibling")]
        [PropertyIsOptional(true, "64")]
        [PropertyDisplayText(false, "Max Siblings")]
        [PropertyValidationRule("Max Siblings", PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public static string v_MaxSiblings { get; }

        /// <summary>
        /// Maxinum Depth for Search UIElements
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Maxinum Depth for Search UIElements")]
        [InputSpecification("Number Greater than or Equal 0")]
        [PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value, "Max Depth")]
        [PropertyDetailSampleUsage("**{{{vMax}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Max Depth")]
        [PropertyDetailSampleUsage("**0**", "Search to All Depth")]
        [PropertyIsOptional(true, "32")]
        [PropertyDisplayText(false, "Max Depth")]
        [PropertyValidationRule("Max Depth", PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public static string v_MaxDepth { get; }

        /// <summary>
        /// Maxinum Number of UIElements to Search
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Maxinum Number of UIElements to Search")]
        [InputSpecification("Number Greater than or Equal 0")]
        [PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value, "Max UIElements")]
        [PropertyDetailSampleUsage("**{{{vMax}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Max UIElements")]
        [PropertyDetailSampleUsage("**0**", "After checking all UIElements, return the result")]
        [PropertyIsOptional(true, "0")]
        [PropertyDisplayText(false, "Max UIElements")]
        [PropertyValidationRule("Max UIElements", PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public static string v_MaxNumberUIElements { get; }

        /// <summary>
        /// Search Siblings Direction
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Search Siblings Direction")]
        [PropertyUISelectionOption("First to Last")]
        [PropertyUISelectionOption("Last to First")]
        [PropertyIsOptional(true, "First to Last")]
        [PropertyDisplayText(false, "Direction")]
        public static string v_SiblingsDirection { get; }

        /// <summary>
        /// window index for match
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("UIElement Index")]
        [InputSpecification("UIElement Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specify the First Window")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "UIElement Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "UIElement Index")]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "UIElement Index")]
        public static string v_TargetUIElementIndex { get; }

        /// <summary>
        /// xpath property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Search XPath")]
        [InputSpecification("Search XPath", true)]
        [PropertyDetailSampleUsage("**//Button[@Name=\"OK\"]**", "Specify a Button whose **Name** Attribute is **OK** in descendant node of the criteria AutomationElement")]
        [PropertyDetailSampleUsage("**/Pane[1]/Button[2]**", "Specify the **second** Button of the **first** Pane child node of the child node of the criteria AutomationElement")]
        [PropertyDetailSampleUsage("**{{{vXPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "XPath")]
        [Remarks("XPath does not support to use parent, following-sibling, and preceding-sibling for root element.")]
        [PropertyValidationRule("XPath", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "XPath")]
        [PropertyCustomUIHelper("GUI Inspect Tool", nameof(VP_UIElementControls) + "+" + nameof(lnkGUIInspectTool_UsedByXPath_Click))]
        public static string v_SearchXPath { get; }

        /// <summary>
        /// UIElement Action
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("UIElement Action")]
        [PropertyUISelectionOption("Click UIElement")]
        [PropertyUISelectionOption("Expand Collapse Items In UIElement")]
        [PropertyUISelectionOption("Scroll UIElement")]
        [PropertyUISelectionOption("Scroll Percent UIElement")]
        [PropertyUISelectionOption("Select Item In UIElement")]
        [PropertyUISelectionOption("Select UIElement")]
        [PropertyUISelectionOption("Set Selected State To UIElement")]
        [PropertyUISelectionOption("Set Text To UIElement")]
        [PropertyUISelectionOption("Get Parent UIElement")]
        [PropertyUISelectionOption("Get Property Value From UIElement")]
        [PropertyUISelectionOption("Get ScrollBar Information From UIElement")]
        [PropertyUISelectionOption("Get Selected State From UIElement")]
        [PropertyUISelectionOption("Get Selection Items Value From UIElement")]
        [PropertyUISelectionOption("Get Table Information From UIElement")]
        [PropertyUISelectionOption("Get Text From Table UIElement")]
        [PropertyUISelectionOption("Get Text From UIElement")]
        [PropertyUISelectionOption("Get UIElement From Table UIElement")]
        [PropertyUISelectionOption("Get UIElement Position")]
        [PropertyUISelectionOption("Get UIElement Size")]
        [PropertyUISelectionOption("Get Window Handle From UIElement")]
        [PropertyUISelectionOption("Get Window Name From UIElement")]
        [PropertyUISelectionOption("Check UIElement Exists")]
        [PropertyUISelectionOption("Wait For UIElement To Exists")]
        //[PropertySelectionChangeEvent(nameof(cmbActionType_SelectedItemChange))]
        [PropertyDisplayText(true, "Action")]
        public static string v_AutomationType { get; }

        /// <summary>
        /// UIElement Action Parameters (actually data type is DataTable)
        /// </summary>
        [PropertyDescription("Action Parameters")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true, 400, 250)]
        [PropertyDataGridViewColumnSettings("Parameter Name", "Parameter Name", true)]
        [PropertyDataGridViewColumnSettings("Parameter Value", "Parameter Value", false)]
        public static string v_UIAActionParameters { get; }

        /// <summary>
        /// show GUI InspectTool and get InspectTool like result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkGUIInspectTool_UsedByInspectResult_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmGUIInspect())
            {
                if (fm.ShowDialog(((Control)sender).FindForm()) == DialogResult.OK)
                {
                    (var command, var dgv) = EM_UIElementChildrenSearchParametersPropertiesExtensionMethods.GetCommandAndSearchDataGridView((Control)sender);
                    command.SearchParametersUpdateProcess(dgv, new Action<DataTable>((tbl) =>
                    {
                        ParseInspectToolResult(fm.InspectResult, tbl);
                    }));
                }
            }
        }

        /// <summary>
        /// set Empty Paramter in DGV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkAddEmptyParameter_Click(object sender, EventArgs e)
        {
            (var command, var dgv) = EM_UIElementChildrenSearchParametersPropertiesExtensionMethods.GetCommandAndSearchDataGridView((Control)sender);
            command.SearchParametersUpdateProcess(dgv, new Action<DataTable>((tbl) =>
            {
                CreateEmptySearchParamters(tbl);
            }));
        }

        /// <summary>
        /// show InspectTool Parser and set result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkInspectToolParser_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmInspectParser())
            {
                if (fm.ShowDialog(((Control)sender).FindForm()) == DialogResult.OK)
                {
                    (var command, var dgv) = EM_UIElementChildrenSearchParametersPropertiesExtensionMethods.GetCommandAndSearchDataGridView((Control)sender);
                    command.SearchParametersUpdateProcess(dgv, new Action<DataTable>((tbl) =>
                    {
                        ParseInspectToolResult(fm.inspectResult, tbl);
                    }));
                }
            }
        }

        /// <summary>
        /// UIElement search DGV cell click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void UIElementSearchDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var myDGV = (DataGridView)sender;

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex != 1)
                {
                    var targetCell = myDGV.Rows[e.RowIndex].Cells[1];
                    if (targetCell is DataGridViewTextBoxCell)
                    {
                        myDGV.BeginEdit(false);
                    }
                    else if (targetCell is DataGridViewComboBoxCell)
                    {
                        SendKeys.Send("{F4}");
                    }
                }
            }
            else
            {
                myDGV.EndEdit();
            }
        }

        /// <summary>
        /// UIElement search DGV cell begin edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void UIElementSearchDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                e.Cancel = true;
            }
        }


        /// <summary>
        /// create empty UIElement search parameters
        /// </summary>
        /// <param name="table"></param>
        public static void CreateEmptySearchParamters(DataTable table)
        {
            table.Rows.Clear();
            foreach (var n in EM_UIElementChildrenSearchParametersPropertiesExtensionMethods.TargetControlProperties)
            {
                table.Rows.Add(false, n, "");
            }
        }

        /// <summary>
        /// parse inspect tool result for UIElement Search DGV
        /// </summary>
        /// <param name="result"></param>
        /// <param name="table"></param>
        /// <param name="windowNames"></param>
        private static void ParseInspectToolResult(string result, DataTable table, ComboBox windowNames = null)
        {
            var results = result.Split(new[] { "\r\n" }, StringSplitOptions.None);

            if ((results.Length >= 1) && (result != ""))
            {
                CreateEmptySearchParamters(table);

                var ancestors = new List<string>();
                string currentParam = "";
                foreach (string res in results)
                {
                    var spt = res.Split('\t');
                    string value = (spt.Length >= 2) ? spt[1] : "";
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                    if (spt[0] != "")
                    {
                        string name = spt[0].Substring(0, spt[0].Length - 1);
                        currentParam = name;

                        switch (name)
                        {
                            case "AcceleratorKey":
                            case "AccessKey":
                            case "AutomationId":
                            case "ClassName":
                            case "FrameworkId":
                            case "HasKeyboardFocus":
                            case "HelpText":
                            case "IsContentElement":
                            case "IsControlElement":
                            case "IsEnabled":
                            case "IsKeyboardFocusable":
                            case "IsOffscreen":
                            case "IsPassword":
                            case "IsRequiredForForm":
                            case "ItemStatus":
                            case "ItemType":
                            case "LocalizedControlType":
                            case "Name":
                            case "NativeWindowHandle":
                            case "ProcessId":
                                DataTableControls.SetParameterValue(table, value, name, "ParameterName", "ParameterValue");
                                break;

                            case "ControlType":
                                DataTableControls.SetParameterValue(table, ParseControlTypeInspectToolResult(value), name, "ParameterName", "ParameterValue");
                                break;

                            case "Ancestors":
                                ancestors.Add(value);
                                break;
                        }
                    }
                    else
                    {
                        if (currentParam == "Ancestors")
                        {
                            ancestors.Add(value);
                        }
                    }
                    if (windowNames != null)
                    {
                        SetComboBoxWindowNameFromInspectAncestors(ancestors, windowNames);
                    }
                }
            }
            else
            {
                var f = new UI.Forms.General.frmDialog("No Inspect Tool Results", "Fail Parse", UI.Forms.General.frmDialog.DialogType.OkOnly, 0);
                f.ShowDialog();
            }
        }

        /// <summary>
        /// parse ControlType in InspectTool result
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ParseControlTypeInspectToolResult(string value)
        {
            var spt = value.Split(' ');
            return spt[0].Replace("UIA_", "").Replace("ControlTypeId", "");
        }

        /// <summary>
        /// set combobox window name from Inspect Tool result Ancenstors value
        /// </summary>
        /// <param name="ancestors"></param>
        /// <param name="cmb"></param>
        private static void SetComboBoxWindowNameFromInspectAncestors(List<string> ancestors, ComboBox cmb)
        {
            if (ancestors.Count > 0)
            {
                var windows = new string[cmb.Items.Count];
                cmb.Items.CopyTo(windows, 0);

                bool isFound = false;
                foreach (string ancestor in ancestors)
                {
                    // get quoted " " text
                    int pos = ancestor.IndexOf("\"");
                    if (pos < 0)
                    {
                        continue;
                    }

                    string winName = ancestor.Substring(pos + 1);
                    pos = winName.IndexOf("\"");
                    if (pos < 0)
                    {
                        continue;
                    }
                    winName = winName.Substring(0, pos);

                    foreach (string win in windows)
                    {
                        if (winName == win)
                        {
                            cmb.Text = winName;
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// show GUI Inspect Tool and get XPath
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkGUIInspectTool_UsedByXPath_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmGUIInspect())
            {
                var trgCtrl = (Control)sender;
                if (fm.ShowDialog(trgCtrl.FindForm()) == DialogResult.OK)
                {
                    object ctrl = trgCtrl.Tag;
                    if (ctrl is TextBox txt)
                    {
                        txt.Text = fm.XPath;
                    }
                    else if (ctrl is ComboBox cmb)
                    {
                        cmb.Text = fm.XPath;
                    }
                    else if (ctrl is DataGridView dgv)
                    {
                        dgv.CurrentCell.Value = fm.XPath;
                    }
                }
            }
        }
    }
}

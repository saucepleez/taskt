using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Windows.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("UIElement Action")]
    [Attributes.ClassAttributes.CommandSettings("UIElement Action")]
    [Attributes.ClassAttributes.Description("Combined implementation of the ThickAppClick/GetText command but includes an advanced Window Recorder to record the required element.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a Variable Command to assign data and achieve automation")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationUIElementActionCommand : ScriptCommand
    {
        // todo: XPath command
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls),  nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("UIElement Action")]
        [PropertyUISelectionOption("Click UIElement")]
        [PropertyUISelectionOption("Expand Collapse Items In UIElement")]
        [PropertyUISelectionOption("Scroll UIElement")]
        [PropertyUISelectionOption("Select UIElement")]
        [PropertyUISelectionOption("Select Item In UIElement")]
        [PropertyUISelectionOption("Set Text To UIElement")]
        [PropertyUISelectionOption("Get Property Value From UIElement")]
        [PropertyUISelectionOption("Check UIElement Exists")]
        [PropertyUISelectionOption("Get Text From UIElement")]
        [PropertyUISelectionOption("Get Selected State From UIElement")]
        [PropertyUISelectionOption("Get Text From Table UIElement")]
        [PropertyUISelectionOption("Wait For UIElement To Exists")]
        [PropertySelectionChangeEvent(nameof(cmbActionType_SelectedItemChange))]
        [PropertyDisplayText(true, "Action")]
        public string v_AutomationType { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_SearchParameters))]
        public DataTable v_UIASearchParameters { get; set; }

        [XmlElement]
        [PropertyDescription("Action Parameters")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true, 400, 250)]
        [PropertyDataGridViewColumnSettings("Parameter Name", "Parameter Name", true)]
        [PropertyDataGridViewColumnSettings("Parameter Value", "Parameter Value", false)]
        public DataTable v_UIAActionParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_MatchMethod_Single))]
        [PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        public string v_MatchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_TargetWindowIndex))]
        public string v_TargetWindowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        public string v_WindowWaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        public string v_ElementWaitTime { get; set; }

        public UIAutomationUIElementActionCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var elemAction = this.GetUISelectionValue(nameof(v_AutomationType), engine);

            var winElemVar = VariableNameControls.GetInnerVariableName(0, engine);
            var winElem = new UIAutomationSearchUIElementFromWindowCommand()
            {
                v_WindowName = this.v_WindowName,
                v_SearchMethod = this.v_SearchMethod,
                v_MatchMethod = this.v_MatchMethod,
                v_TargetWindowIndex = this.v_TargetWindowIndex,
                v_WindowWaitTime = this.v_WindowWaitTime,
                v_AutomationElementVariable = winElemVar,
            };
            winElem.RunCommand(engine);

            var p = DataTableControls.GetFieldValues(v_UIAActionParameters, "Parameter Name", "Parameter Value", false, engine);
            var trgElemVar = VariableNameControls.GetInnerVariableName(1, engine);

            switch (elemAction)
            {
                case "check uielement exists":
                    var chkElem = new UIAutomationCheckUIElementExistCommand()
                    {
                        v_TargetElement = winElemVar,
                        v_SearchParameters = this.v_UIASearchParameters,
                        v_WaitTime = this.v_ElementWaitTime,
                        v_Result = p["Apply To Variable"],
                    };
                    chkElem.RunCommand(engine);
                    return;
                    
                default:
                    var trgElem = new UIAutomationSearchUIElementFromUIElementCommand()
                    {
                        v_TargetElement = winElemVar,
                        v_SearchParameters = this.v_UIASearchParameters,
                        v_WaitTime = this.v_ElementWaitTime,
                        v_AutomationElementVariable = trgElemVar,
                    };
                    trgElem.RunCommand(engine);
                    break;
            }

            switch (elemAction)
            {
                case "click uielement":
                    var clickCmd = new UIAutomationClickUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                        v_ClickType = p["Click Type"],
                        v_XOffset = p["X Offset"],
                        v_YOffset = p["Y Offset"],
                    };
                    clickCmd.RunCommand(engine);
                    break;
                case "expand collapse items in uielement":
                    var expandCmd = new UIAutomationExpandCollapseItemsInUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                        v_ItemsState = p["Items State"],
                    };
                    expandCmd.RunCommand(engine);
                    break;
                case "scroll uielement":
                    var scrollCmd = new UIAutomationScrollUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                        v_ScrollBarType = p["ScrollBar Type"],
                        v_DirectionAndAmount = p["Scroll Method"],
                    };
                    scrollCmd.RunCommand(engine);
                    break;
                case "select uielement":
                    var selectCmd = new UIAutomationSelectUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                    };
                    selectCmd.RunCommand(engine);
                    break;
                case "select item in uielement":
                    var selectItemCmd = new UIAutomationSelectItemInUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                        v_Item = p["Item Value"],
                    };
                    selectItemCmd.RunCommand(engine);
                    break;
                case "set text to uielement":
                    var setTextCmd = new UIAutomationSetTextToUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                        v_TextVariable = p["Text To Set"],
                    };
                    setTextCmd.RunCommand(engine);
                    break;
                case "get property value from uielement":
                    var propValueCmd = new UIAutomationGetPropertyValueFromUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                        v_PropertyName = p["Property Name"],
                        v_Result = p["Apply To Variable"],
                    };
                    propValueCmd.RunCommand(engine);
                    break;
                case "check uielement exists":
                    true.StoreInUserVariable(engine, p["Apply To Variable"]);
                    break;
                case "get text from uielement":
                    var getTextCmd = new UIAutomationGetTextFromUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                        v_TextVariable = p["Apply To Variable"],
                    };
                    getTextCmd.RunCommand(engine);
                    break;
                case "get selected state from uielement":
                    var getSelectedCmd = new UIAutomationGetSelectedStateFromUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                        v_ResultVariable = p["Apply To Variable"],
                    };
                    getSelectedCmd.RunCommand(engine);
                    break;
                case "get text from table uielement":
                    var getTableCmd = new UIAutomationGetTextFromTableUIElementCommand()
                    {
                        v_TargetElement = trgElemVar,
                        v_Row = p["Row"],
                        v_Column = p["Column"],
                        v_TextVariable = p["Apply To Variable"],
                    };
                    getTableCmd.RunCommand(engine);
                    break;
            }

            ////create variable window name
            //var variableWindowName = v_WindowName.ConvertToUserVariable(sender);

            //if (variableWindowName == engine.engineSettings.CurrentWindowKeyword)
            //{
            //    //variableWindowName = User32Functions.GetActiveWindowTitle();
            //    variableWindowName = WindowNameControls.GetActiveWindowTitle();
            //}
            //else
            //{
            //    // search and activate window
            //    var searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            //    if (String.IsNullOrEmpty(searchMethod))
            //    {
            //        searchMethod = "Contains";
            //    }

            //    if (searchMethod != "Exact match")
            //    {
            //        ActivateWindowCommand activateWindow = new ActivateWindowCommand
            //        {
            //            v_WindowName = variableWindowName,
            //            v_SearchMethod = searchMethod
            //        };
            //        activateWindow.RunCommand(sender);
            //        System.Threading.Thread.Sleep(500); // wait a bit
            //        //variableWindowName = User32Functions.GetActiveWindowTitle();
            //        variableWindowName = WindowNameControls.GetActiveWindowTitle();
            //    }
            //}

            //var requiredHandle =  SearchForGUIElement(sender, variableWindowName);

            ////if element exists type
            //if (v_AutomationType == "Check If Element Exists")
            //{
            //    //apply to variable
            //    var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
            //                           where rw.Field<string>("Parameter Name") == "Apply To Variable"
            //                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                
            //    //remove brackets from variable
            //    applyToVariable = applyToVariable.Replace(engine.engineSettings.VariableStartMarker, "").Replace(engine.engineSettings.VariableEndMarker, "");

            //    //declare search result
            //    string searchResult;

            //    //determine search result
            //    if (requiredHandle == null)
            //    {
            //        searchResult = "FALSE";
  
            //    }
            //    else
            //    {
            //        searchResult = "TRUE";
            //    }

            //    //store data
            //    searchResult.StoreInUserVariable(sender, applyToVariable);

            //}

            ////determine element click type
            //else if (v_AutomationType == "Click Element")
            //{

            //    //if handle was not found
            //    if (requiredHandle == null)
            //        throw new Exception("Element was not found in window '" + variableWindowName + "'");

            //    //create search params
            //    var clickType = (from rw in v_UIAActionParameters.AsEnumerable()
            //                     where rw.Field<string>("Parameter Name") == "Click Type"
            //                     select rw.Field<string>("Parameter Value")).FirstOrDefault();

            //    //get x adjust
            //    var xAdjust = (from rw in v_UIAActionParameters.AsEnumerable()
            //                   where rw.Field<string>("Parameter Name") == "X Adjustment"
            //                   select rw.Field<string>("Parameter Value")).FirstOrDefault();

            //    //get y adjust
            //    var yAdjust = (from rw in v_UIAActionParameters.AsEnumerable()
            //                   where rw.Field<string>("Parameter Name") == "Y Adjustment"
            //                   select rw.Field<string>("Parameter Value")).FirstOrDefault();

            //    //convert potential variable
            //    var xAdjustVariable = xAdjust.ConvertToUserVariable(sender);
            //    var yAdjustVariable = yAdjust.ConvertToUserVariable(sender);

            //    //parse to int
            //    var xAdjustInt = int.Parse(xAdjustVariable);
            //    var yAdjustInt = int.Parse(yAdjustVariable);

            //    //get clickable point
            //    var newPoint = requiredHandle.GetClickablePoint();

            //    //send mousemove command
            //    var newMouseMove = new MoveMouseCommand
            //    {
            //        v_XMousePosition = (newPoint.X + xAdjustInt).ToString(),
            //        v_YMousePosition = (newPoint.Y + yAdjustInt).ToString(),
            //        v_MouseClick = clickType
            //    };

            //    //run commands
            //    newMouseMove.RunCommand(sender);
            //}
            //else if (v_AutomationType == "Get Value From Element")
            //{

            //    //if handle was not found
            //    if (requiredHandle == null)
            //        throw new Exception("Element was not found in window '" + variableWindowName + "'");
            //    //get value from property
            //    var propertyName = (from rw in v_UIAActionParameters.AsEnumerable()
            //                        where rw.Field<string>("Parameter Name") == "Get Value From"
            //                        select rw.Field<string>("Parameter Value")).FirstOrDefault();
               
            //    //apply to variable
            //    var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
            //                           where rw.Field<string>("Parameter Name") == "Apply To Variable"
            //                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

            //    //remove brackets from variable
            //    applyToVariable = applyToVariable.Replace(engine.engineSettings.VariableStartMarker, "").Replace(engine.engineSettings.VariableEndMarker, "");

            //    //get required value
            //    var requiredValue = requiredHandle.Current.GetType().GetRuntimeProperty(propertyName)?.GetValue(requiredHandle.Current).ToString();

            //    //store into variable
            //    requiredValue.StoreInUserVariable(sender, applyToVariable);

            //}
            //else if (v_AutomationType == "Get Text Value From Element")
            //{
            //    //apply to variable
            //    var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
            //                           where rw.Field<string>("Parameter Name") == "Apply To Variable"
            //                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

            //    object patternObj;
            //    if (requiredHandle.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
            //    {
            //        // TextBox
            //        ((ValuePattern)patternObj).Current.Value.StoreInUserVariable(sender, applyToVariable);
            //    }
            //    else if (requiredHandle.TryGetCurrentPattern(TextPattern.Pattern, out patternObj))
            //    {
            //        // TextBox Multilune
            //        TextPattern tPtn = (TextPattern)patternObj;
            //        tPtn.DocumentRange.GetText(-1).StoreInUserVariable(sender, applyToVariable);
            //    }
            //    else if (requiredHandle.TryGetCurrentPattern(SelectionPattern.Pattern, out patternObj))
            //    {
            //        ((SelectionPattern)patternObj).Current.GetSelection()[0].GetCurrentPropertyValue(AutomationElement.NameProperty).ToString().StoreInUserVariable(sender, applyToVariable);
            //    }
            //    else
            //    {
            //        requiredHandle.Current.Name.StoreInUserVariable(sender, applyToVariable);
            //    }
            //}
            //else if (v_AutomationType == "Get Selected State From Element")
            //{
            //    //apply to variable
            //    var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
            //                           where rw.Field<string>("Parameter Name") == "Apply To Variable"
            //                           select rw.Field<string>("Parameter Value")).FirstOrDefault();
            //    object patternObj;
            //    bool checkState;
            //    if (requiredHandle.TryGetCurrentPattern(TogglePattern.Pattern, out patternObj))
            //    {
            //        checkState = (((TogglePattern)patternObj).Current.ToggleState == ToggleState.On);
            //    }
            //    else if (requiredHandle.TryGetCurrentPattern(SelectionItemPattern.Pattern, out patternObj))
            //    {
            //        checkState = ((SelectionItemPattern)patternObj).Current.IsSelected;
            //    }
            //    else
            //    {
            //        throw new Exception("Thie element is not CheckBox or RadioButton.");
            //    }
            //    (checkState ? "TRUE" : "FALSE").StoreInUserVariable(sender, applyToVariable);
            //}
            //else if (v_AutomationType == "Get Value From Table Element")
            //{
            //    // row, column
            //    var vTarget = (from rw in v_UIAActionParameters.AsEnumerable()
            //                where rw.Field<string>("Parameter Name") == "Target"
            //                select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);
            //    var vRow = (from rw in v_UIAActionParameters.AsEnumerable()
            //                where rw.Field<string>("Parameter Name") == "Row"
            //                select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);
            //    var vColumn = (from rw in v_UIAActionParameters.AsEnumerable()
            //                where rw.Field<string>("Parameter Name") == "Column"
            //                select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);
            //    //apply to variable
            //    var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
            //                           where rw.Field<string>("Parameter Name") == "Apply To Variable"
            //                           select rw.Field<string>("Parameter Value")).FirstOrDefault();
            //    int row, col;
                

            //    object dgvPattern;
            //    if (requiredHandle.TryGetCurrentPattern(TablePattern.Pattern, out dgvPattern))
            //    {
            //        row = int.Parse(vRow);
            //        col = int.Parse(vColumn);
            //        var cell = ((TablePattern)dgvPattern).GetItem(row, col);
            //        cell.Current.Name.StoreInUserVariable(sender, applyToVariable);
            //    }
            //    else
            //    {
            //        throw new Exception("This table is not supported.");
            //    }
                
            //}
            //else
            //{
            //    throw new NotImplementedException("Automation type '" + v_AutomationType + "' not supported.");
            //}
        }

        public override void AfterShown()
        {
            var cmb = PropertyControls.GetPropertyControl<ComboBox>(ControlsList, nameof(v_AutomationType));
            var dgv = PropertyControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_UIAActionParameters));
            actionParameterProcess(dgv, cmb.SelectedItem?.ToString() ?? "");
        }

        private void cmbActionType_SelectedItemChange(object sender, EventArgs e)
        {
            var a = ((ComboBox)sender).SelectedItem?.ToString() ?? "";

            var dgv = PropertyControls.GetPropertyControl<DataGridView>(this.ControlsList, nameof(v_UIAActionParameters));
            var table = v_UIAActionParameters;
            table.Rows.Clear();
            switch (a.ToLower())
            {
                case "click uielement":
                    table.Rows.Add(new string[] { "Click Type", "" });
                    table.Rows.Add(new string[] { "X Offset", "" });
                    table.Rows.Add(new string[] { "Y Offset", "" });

                    //var clickType = new DataGridViewComboBoxCell();
                    //clickType.Items.AddRange(new string[]
                    //{
                    //    "Left Click",
                    //    "Middle Click",
                    //    "Right Click",
                    //    "Left Down",
                    //    "Middle Down",
                    //    "Right Down",
                    //    "Left Up",
                    //    "Middle Up",
                    //    "Right Up",
                    //    "Double Left Click",
                    //    "None",
                    //});
                    //dgv.Rows[0].Cells[1] = clickType;
                    break;
                case "expand collapse items in uielement":
                    table.Rows.Add(new string[] { "Items State", "" });
                    //var itemState = new DataGridViewComboBoxCell();
                    //itemState.Items.AddRange(new string[]
                    //{
                    //    "Expand",
                    //    "Collapse"
                    //});
                    //dgv.Rows[0].Cells[1] = itemState;
                    break;
                case "scroll uielement":
                    table.Rows.Add(new string[] { "ScrollBar Type", "" });
                    table.Rows.Add(new string[] { "Scroll Method", "" });
                    //var barType = new DataGridViewComboBoxCell();
                    //barType.Items.AddRange(new string[]
                    //{
                    //    "Vertical",
                    //    "Horizonal",
                    //});
                    //var scrollMethod = new DataGridViewComboBoxCell();
                    //scrollMethod.Items.AddRange(new string[]
                    //{
                    //    "Scroll Small Down or Right",
                    //    "Scroll Large Down or Right",
                    //    "Scroll Small Up or Left",
                    //    "Scroll Large Up or Left",
                    //});
                    //dgv.Rows[0].Cells[1] = barType;
                    //dgv.Rows[1].Cells[1] = scrollMethod;
                    break;
                case "select item in uielement":
                    table.Rows.Add(new string[] { "Item Value", "" });
                    break;
                case "set text to uielement":
                    table.Rows.Add(new string[] { "Text To Set", "" });
                    break;
                case "get property value from uielement":
                    table.Rows.Add(new string[] { "Property Name", "" });
                    table.Rows.Add(new string[] { "Apply To Variable", "" });
                    //var propNames = new DataGridViewComboBoxCell();
                    //propNames.Items.AddRange(new string[]
                    //{
                    //    "Name",
                    //    "ControlType",
                    //    "LocalizedControlType",
                    //    "IsEnabled",
                    //    "IsOffscreen",
                    //    "IsKeyboardFocusable",
                    //    "HasKeyboardFocusable",
                    //    "AccessKey",
                    //    "ProcessId",
                    //    "AutomationId",
                    //    "FrameworkId",
                    //    "ClassName",
                    //    "IsContentElement",
                    //    "IsPassword",
                    //    "AcceleratorKey",
                    //    "HelpText",
                    //    "IsControlElement",
                    //    "IsRequiredForForm",
                    //    "ItemStatus",
                    //    "ItemType",
                    //    "NativeWindowHandle",
                    //});
                    //dgv.Rows[0].Cells[1] = propNames;
                    break;
                case "get text from table uielement":
                    table.Rows.Add(new string[] { "Row", "" });
                    table.Rows.Add(new string[] { "Column", "" });
                    table.Rows.Add(new string[] { "Apply To Variable", "" });
                    break;
                case "check uielement exists":
                case "get text from uielement":
                case "get selected state from uielement":
                    table.Rows.Add(new string[] { "Apply To Variable", "" });
                    break;
                case "select uielement":
                case "wait for uielement to exists":
                    // nothing
                    break;
            }

            actionParameterProcess(dgv, a);
        }

        private static void actionParameterProcess(DataGridView dgv, string actionType)
        {
            switch (actionType.ToLower())
            {
                case "click uielement":
                    var clickType = new DataGridViewComboBoxCell();
                    clickType.Items.AddRange(new string[]
                    {
                        "Left Click",
                        "Middle Click",
                        "Right Click",
                        "Left Down",
                        "Middle Down",
                        "Right Down",
                        "Left Up",
                        "Middle Up",
                        "Right Up",
                        "Double Left Click",
                        "None",
                    });
                    dgv.Rows[0].Cells[1] = clickType;
                    break;
                case "expand collapse items in uielement":
                    var itemState = new DataGridViewComboBoxCell();
                    itemState.Items.AddRange(new string[]
                    {
                        "Expand",
                        "Collapse"
                    });
                    dgv.Rows[0].Cells[1] = itemState;
                    break;
                case "scroll uielement":
                    var barType = new DataGridViewComboBoxCell();
                    barType.Items.AddRange(new string[]
                    {
                        "Vertical",
                        "Horizonal",
                    });
                    var scrollMethod = new DataGridViewComboBoxCell();
                    scrollMethod.Items.AddRange(new string[]
                    {
                        "Scroll Small Down or Right",
                        "Scroll Large Down or Right",
                        "Scroll Small Up or Left",
                        "Scroll Large Up or Left",
                    });
                    dgv.Rows[0].Cells[1] = barType;
                    dgv.Rows[1].Cells[1] = scrollMethod;
                    break;
                case "get property value from uielement":
                    var propNames = new DataGridViewComboBoxCell();
                    propNames.Items.AddRange(new string[]
                    {
                        "Name",
                        "ControlType",
                        "LocalizedControlType",
                        "IsEnabled",
                        "IsOffscreen",
                        "IsKeyboardFocusable",
                        "HasKeyboardFocusable",
                        "AccessKey",
                        "ProcessId",
                        "AutomationId",
                        "FrameworkId",
                        "ClassName",
                        "IsContentElement",
                        "IsPassword",
                        "AcceleratorKey",
                        "HelpText",
                        "IsControlElement",
                        "IsRequiredForForm",
                        "ItemStatus",
                        "ItemType",
                        "NativeWindowHandle",
                    });
                    dgv.Rows[0].Cells[1] = propNames;
                    break;
            }
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        public override void BeforeValidate()
        {
            var dgvAction = PropertyControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_UIAActionParameters));
            DataTableControls.BeforeValidate(dgvAction, v_UIAActionParameters);

            var dgvSearch = PropertyControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_UIASearchParameters));
            DataTableControls.BeforeValidate(dgvSearch, v_UIASearchParameters);
        }

        //public override string GetDisplayValue()
        //{
        //    if (v_AutomationType == "Click Element")
        //    {
        //        //create search params
        //        var clickType = (from rw in v_UIAActionParameters.AsEnumerable()
        //                         where rw.Field<string>("Parameter Name") == "Click Type"
        //                         select rw.Field<string>("Parameter Value")).FirstOrDefault();


        //        return base.GetDisplayValue() + " [" + clickType + " element in window '" + v_WindowName + "']";
        //    }
        //    else if(v_AutomationType == "Check If Element Exists")
        //    {

        //        //apply to variable
        //        var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
        //                               where rw.Field<string>("Parameter Name") == "Apply To Variable"
        //                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

        //        return base.GetDisplayValue() + " [Check for element in window '" + v_WindowName + "' and apply to '" + applyToVariable + "']";
        //    }
        //    else if (v_AutomationType == "Get Text Value From Element")
        //    {
        //        //apply to variable
        //        var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
        //                               where rw.Field<string>("Parameter Name") == "Apply To Variable"
        //                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

        //        return base.GetDisplayValue() + " [Text Value for element in window '" + v_WindowName + "' and apply to '" + applyToVariable + "']";
        //    }
        //    else if (v_AutomationType == "Get Selected State From Element")
        //    {
        //        //apply to variable
        //        var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
        //                               where rw.Field<string>("Parameter Name") == "Apply To Variable"
        //                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

        //        return base.GetDisplayValue() + " [Selected State for element in window '" + v_WindowName + "' and apply to '" + applyToVariable + "']";
        //    }
        //    else
        //    {
        //        //get value from property
        //        var propertyName = (from rw in v_UIAActionParameters.AsEnumerable()
        //                            where rw.Field<string>("Parameter Name") == "Get Value From"
        //                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

        //        //apply to variable
        //        var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
        //                               where rw.Field<string>("Parameter Name") == "Apply To Variable"
        //                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

        //        return base.GetDisplayValue() + " [Get value from '" + propertyName + "' in window '" + v_WindowName + "' and apply to '" + applyToVariable + "']";
        //    }
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_WindowName))
        //    {
        //        this.validationResult += "Window Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_AutomationType))
        //    {
        //        this.validationResult += "Action is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        switch (this.v_AutomationType)
        //        {
        //            case "Click Element":
        //                ClickElementValidate();
        //                break;
        //            case "Get Value From Element":
        //                GetValueFromElementValidate();
        //                break;
        //            case "Check If Element Exists":
        //            case "Get Text Value From Element":
        //            case "Get Selected State From Element":
        //                CheckIfElementExistsValidate();
        //                break;

        //            case "Get Value From Table Element":
        //                GetValueFromTableElement();
        //                break;

        //            default:
        //                break;
        //        }
        //    }

        //    return this.IsValid;
        //}

        private void ClickElementValidate()
        {
            var clickType = (from rw in v_UIAActionParameters.AsEnumerable()
                             where rw.Field<string>("Parameter Name") == "Click Type"
                             select rw.Field<string>("Parameter Value")).FirstOrDefault();
            if (String.IsNullOrEmpty(clickType))
            {
                this.validationResult += "Click Type is empty.\n";
                this.IsValid = false;
            }
            
            var x = (from rw in v_UIAActionParameters.AsEnumerable()
                     where rw.Field<string>("Parameter Name") == "X Adjustment"
                     select rw.Field<string>("Parameter Value")).FirstOrDefault();
            var y = (from rw in v_UIAActionParameters.AsEnumerable()
                     where rw.Field<string>("Parameter Name") == "Y Adjustment"
                     select rw.Field<string>("Parameter Value")).FirstOrDefault();

            if (String.IsNullOrEmpty(x))
            {
                this.validationResult += "X Adjustment is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(y))
            {
                this.validationResult += "Y Adjustment is empty.\n";
                this.IsValid = false;
            }
        }

        private void GetValueFromElementValidate()
        {
            var valueFrom = (from rw in v_UIAActionParameters.AsEnumerable()
                             where rw.Field<string>("Parameter Name") == "Get Value From"
                             select rw.Field<string>("Parameter Value")).FirstOrDefault();
            var variable = (from rw in v_UIAActionParameters.AsEnumerable()
                             where rw.Field<string>("Parameter Name") == "Apply To Variable"
                             select rw.Field<string>("Parameter Value")).FirstOrDefault();

            if (String.IsNullOrEmpty(valueFrom))
            {
                this.validationResult += "Get Value From is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(variable))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }
        }

        private void CheckIfElementExistsValidate()
        {
            var variable = (from rw in v_UIAActionParameters.AsEnumerable()
                            where rw.Field<string>("Parameter Name") == "Apply To Variable"
                            select rw.Field<string>("Parameter Value")).FirstOrDefault();
            if (String.IsNullOrEmpty(variable))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }
        }

        private void GetValueFromTableElement()
        {
            var row = (from rw in v_UIAActionParameters.AsEnumerable()
                            where rw.Field<string>("Parameter Name") == "Row"
                            select rw.Field<string>("Parameter Value")).FirstOrDefault();
            var column = (from rw in v_UIAActionParameters.AsEnumerable()
                            where rw.Field<string>("Parameter Name") == "Column"
                            select rw.Field<string>("Parameter Value")).FirstOrDefault();
            var variable = (from rw in v_UIAActionParameters.AsEnumerable()
                            where rw.Field<string>("Parameter Name") == "Apply To Variable"
                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

            if (String.IsNullOrEmpty(row))
            {
                this.validationResult += "Row is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(column))
            {
                this.validationResult += "Column is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(variable))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }
        }

        //public override void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        //{
        //    var cnv = new Dictionary<string, string>();
        //    cnv.Add("v_WindowName", "convertToIntermediateWindowName");
        //    ConvertToIntermediate(settings, cnv, variables);
        //}

        //public override void ConvertToRaw(EngineSettings settings)
        //{
        //    var cnv = new Dictionary<string, string>();
        //    cnv.Add("v_WindowName", "convertToRawWindowName");
        //    ConvertToRaw(settings, cnv);
        //}
    }
}
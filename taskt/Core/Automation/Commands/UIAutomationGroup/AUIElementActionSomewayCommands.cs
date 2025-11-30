using System;
using System.Data;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WindowGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for UIElement Action By/From someway commands
    /// </summary>
    public abstract class AUIElementActionSomewayCommands : ScriptCommand, IUIElementUIElementActionSomewayProperties, IFromWindowNameResultsProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_AutomationType))]
        [PropertySelectionChangeEvent(nameof(cmbActionType_SelectedItemChange))]
        public abstract string v_AutomationType { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_UIAActionParameters))]
        public abstract DataTable v_UIAActionParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeForUIElement))]
        [PropertyParameterOrder(7990)]
        public string v_WaitTimeForUIElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxSiblings))]
        [PropertyParameterOrder(7991)]
        public string v_MaxSiblings { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxDepth))]
        [PropertyParameterOrder(7992)]
        public string v_MaxDepth { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_OutputUIElementName))]
        [PropertyIsOptional(true, "")]
        [PropertyValidationRule("UIElement Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "UIElement Result")]
        [PropertyParameterOrder(10000)]
        public virtual string v_TargetUIElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowNameResult))]
        [PropertyParameterOrder(10100)]
        public virtual string v_WindowNameResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        [PropertyParameterOrder(10200)]
        public virtual string v_WindowHandleResult { get; set; }

        /// <summary>
        /// UIElement Action Process
        /// </summary>
        /// <param name="rootElemAction">arg1 is a Variable to Store root UIElement</param>
        /// <param name="existsElemAction">arg1 is a Varialbe has root UIElement, arg2 is Variable Name to Store Result</param>
        /// <param name="searchElemAction">arg1 is a Varialbe has root UIElement, arg2 is Variable to Store Target UIElement</param>
        /// <param name="engine"></param>
        protected void UIElementActionProcess(Engine.AutomationEngineInstance engine, Action<InnerScriptVariable> rootElemAction, Action<InnerScriptVariable, string> existsElemAction, Action<InnerScriptVariable, InnerScriptVariable> searchElemAction)
        {
            var elemAction = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_AutomationType), engine);

            void StoreUIElementResultInUserVarialbe(InnerScriptVariable v, Engine.AutomationEngineInstance egn)
            {
                if (!string.IsNullOrEmpty(v_TargetUIElement))
                {
                    if (v.VariableValue is AutomationElement elm)
                    {
                        elm.StoreInUserVariable(engine, v_TargetUIElement);
                    }
                    else
                    {
                        throw new Exception("Strange UIElement Result Value.");
                    }
                }
            }

            using (var myRootElem = new InnerScriptVariable(engine))
            {
                rootElemAction(myRootElem);

                var p = DataTableControls.GetFieldValues(v_UIAActionParameters, "Parameter Name", "Parameter Value", false, engine);

                using (var myTrgElem = new InnerScriptVariable(engine))
                {
                    switch (elemAction)
                    {
                        case "check uielement exists":
                            existsElemAction(myRootElem, p["Apply To Variable"]);
                            StoreUIElementResultInUserVarialbe(myTrgElem, engine);
                            return;

                        default:
                            searchElemAction(myRootElem, myTrgElem);
                            StoreUIElementResultInUserVarialbe(myTrgElem, engine);
                            break;
                    }

                    switch (elemAction)
                    {
                        case "click uielement":
                            var clickCmd = new UIAutomationClickUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_ClickType = p["Click Type"],
                                v_XOffset = p["X Offset"],
                                v_YOffset = p["Y Offset"],
                            };
                            clickCmd.RunCommand(engine);
                            break;
                        case "expand collapse items in uielement":
                            var expandCmd = new UIAutomationExpandCollapseItemsInUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_ItemsState = p["Items State"],
                            };
                            expandCmd.RunCommand(engine);
                            break;
                        case "scroll uielement":
                            var scrollCmd = new UIAutomationScrollUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_ScrollBarType = p["ScrollBar Type"],
                                v_DirectionAndAmount = p["Scroll Method"],
                            };
                            scrollCmd.RunCommand(engine);
                            break;
                        case "select item in uielement":
                            var selectItemCmd = new UIAutomationSelectItemInUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_Item = p["Item Value"],
                            };
                            selectItemCmd.RunCommand(engine);
                            break;
                        case "select uielement":
                            var selectCmd = new UIAutomationSelectUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                            };
                            selectCmd.RunCommand(engine);
                            break;
                        case "set selected state to uielement":
                            var selectStateCmd = new UIAutomationSetSelectedStateToUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_State = p["Selected State"],
                            };
                            selectStateCmd.RunCommand(engine);
                            break;
                        case "set text to uielement":
                            var setTextCmd = new UIAutomationSetTextToUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_TextToSet = p["Text To Set"],
                            };
                            setTextCmd.RunCommand(engine);
                            break;
                        case "get parent uielement":
                            var getPrentCmd = new UIAutomationGetParentUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_Result = p["Apply To Variable"],
                            };
                            getPrentCmd.RunCommand(engine);
                            break;
                        case "get property value from uielement":
                            var propValueCmd = new UIAutomationGetPropertyValueFromUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
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
                                v_TargetElement = myTrgElem.VariableName,
                                v_Result = p["Apply To Variable"],
                            };
                            getTextCmd.RunCommand(engine);
                            break;
                        case "get selected state from uielement":
                            var getSelectedCmd = new UIAutomationGetSelectedStateFromUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_Result = p["Apply To Variable"],
                            };
                            getSelectedCmd.RunCommand(engine);
                            break;
                        case "get selection items value from uielement":
                            var getSelectionItemsCmd = new UIAutomationGetSelectionItemsValueFromUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_Result = p["Apply To Variable"],
                            };
                            getSelectionItemsCmd.RunCommand(engine);
                            break;
                        case "get text from table uielement":
                            var getTableCmd = new UIAutomationGetTextFromTableUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_Row = p["Row"],
                                v_Column = p["Column"],
                                v_Result = p["Apply To Variable"],
                            };
                            getTableCmd.RunCommand(engine);
                            break;
                        case "get uielement from table uielement":
                            var getTableElemCmd = new UIAutomationGetUIElementFromTableUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_Row = p["Row"],
                                v_Column = p["Column"],
                                v_Result = p["Apply To Variable"],
                            };
                            getTableElemCmd.RunCommand(engine);
                            break;
                        case "get uielement position":
                            var getElemPosCmd = new UIAutomationGetUIElementPositionCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_XPosition = p["X Variable"],
                                v_YPosition = p["Y Variable"],
                                v_PositionBase = p["Base Position"],
                            };
                            getElemPosCmd.RunCommand(engine);
                            break;
                        case "get uielement size":
                            var getElemSizeCmd = new UIAutomationGetUIElementSizeCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_Width = p["Width Variable"],
                                v_Height = p["Height Variable"],
                            };
                            getElemSizeCmd.RunCommand(engine);
                            break;
                        case "get window handle from uielement":
                            var getWinHandleCmd = new UIAutomationGetWindowHandleFromUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_WindowHandleResult = p["Apply To Variable"],
                            };
                            getWinHandleCmd.RunCommand(engine);
                            break;
                        case "get window name from uielement":
                            var getWinNameCmd = new UIAutomationGetWindowNameFromUIElementCommand()
                            {
                                v_TargetElement = myTrgElem.VariableName,
                                v_WindowNameResult = p["Apply To Variable"],
                            };
                            getWinNameCmd.RunCommand(engine);
                            break;
                    }
                }
            }
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var cmb = FormUIControls.GetPropertyControl<ComboBox>(ControlsList, nameof(v_AutomationType));
            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_UIAActionParameters));
            actionParameterProcess(dgv, cmb.SelectedItem?.ToString() ?? "");
        }

        protected void cmbActionType_SelectedItemChange(object sender, EventArgs e)
        {
            var a = ((ComboBox)sender).SelectedItem?.ToString() ?? "";

            var dgv = FormUIControls.GetPropertyControl<DataGridView>(this.ControlsList, nameof(v_UIAActionParameters));
            var table = v_UIAActionParameters;
            table.Rows.Clear();
            switch (a.ToLower())
            {
                case "click uielement":
                    table.Rows.Add(new string[] { "Click Type", "" });
                    table.Rows.Add(new string[] { "X Offset", "" });
                    table.Rows.Add(new string[] { "Y Offset", "" });
                    break;

                case "expand collapse items in uielement":
                    table.Rows.Add(new string[] { "Items State", "" });
                    break;

                case "scroll uielement":
                    table.Rows.Add(new string[] { "ScrollBar Type", "" });
                    table.Rows.Add(new string[] { "Scroll Method", "" });
                    break;

                case "select item in uielement":
                    table.Rows.Add(new string[] { "Item Value", "" });
                    break;

                case "set selected state to uielement":
                    table.Rows.Add(new string[] { "Selected State", "" });
                    break;

                case "set text to uielement":
                    table.Rows.Add(new string[] { "Text To Set", "" });
                    break;

                case "get property value from uielement":
                    table.Rows.Add(new string[] { "Property Name", "" });
                    table.Rows.Add(new string[] { "Apply To Variable", "" });
                    break;

                case "get text from table uielement":
                case "get uielement from table uielement":
                    table.Rows.Add(new string[] { "Row", "" });
                    table.Rows.Add(new string[] { "Column", "" });
                    table.Rows.Add(new string[] { "Apply To Variable", "" });
                    break;

                case "get uielement position":
                    table.Rows.Add(new string[] { "X Variable", "" });
                    table.Rows.Add(new string[] { "Y Variable", "" });
                    table.Rows.Add(new string[] { "Base Position", "" });
                    break;

                case "get uielement size":
                    table.Rows.Add(new string[] { "Width Variable", "" });
                    table.Rows.Add(new string[] { "Height Variable", "" });
                    break;

                case "check uielement exists":
                case "get parent uielement":
                case "get selected state from uielement":
                case "get selection items value from uielement":
                case "get text from uielement":
                case "get window handle from uielement":
                case "get window name from uielement":
                    table.Rows.Add(new string[] { "Apply To Variable", "" });
                    break;

                case "select uielement":
                case "wait for uielement to exists":
                    // nothing
                    break;
            }

            actionParameterProcess(dgv, a);
        }

        protected static void actionParameterProcess(DataGridView dgv, string actionType)
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
                case "set selected state to uielement":
                    var selectedState = new DataGridViewComboBoxCell();
                    selectedState.Items.AddRange(new string[]
                    {
                        "Selected",
                        "Unselected"
                    });
                    dgv.Rows[0].Cells[1] = selectedState;
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
                case "get uielement position":
                    var positionName = new DataGridViewComboBoxCell();
                    positionName.Items.AddRange(new string[]
                    {
                        "Top Left",
                        "Bottom Right",
                        "Top Right",
                        "Bottom Left",
                        "Center",
                    });
                    dgv.Rows[2].Cells[1] = positionName;
                    break;
            }
        }

        public override void BeforeValidate()
        {
            var dgvAction = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_UIAActionParameters));
            DataTableControls.BeforeValidate(dgvAction, v_UIAActionParameters);
        }
    }
}

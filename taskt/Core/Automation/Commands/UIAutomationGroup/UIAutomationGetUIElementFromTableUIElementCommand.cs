using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get UIElement From Table UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement from Table UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement from Table UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetUIElementFromTableUIElementCommand : AGetFromUIElementCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        // TODO: table row/col interface
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDetailSampleUsage("**0**", "Specify the First Row Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Row Index")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Row Index")]
        [PropertyDescription("Row Index")]
        [InputSpecification("Row Index", true)]
        [PropertyValidationRule("Row", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Row")]
        [PropertyParameterOrder(6000)]
        public string v_Row { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDetailSampleUsage("**0**", "Specify the First Column Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Column Index")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column Index")]
        [PropertyDescription("Column Index")]
        [InputSpecification("Column Index", true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        [PropertyParameterOrder(6100)]
        public string v_Column { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_NewOutputUIElementName))]
        [PropertyParameterOrder(6200)]
        public string v_Result { get; set; }

        public UIAutomationGetUIElementFromTableUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            void ErrorAction()
            {
                this.ValueCanNotRetrievedProcess("Table UIElement", new Action(() =>
                {
                    "".StoreInUserVariable(engine, v_Result);
                }), engine);
            }

            this.UIElementAction(engine,
                new Action<AutomationElement>(targetElement =>
                {
                    int rowIndex = v_Row.ExpandValueOrUserVariableAsInteger("v_Row", engine);
                    int columnIndex = v_Column.ExpandValueOrUserVariableAsInteger("v_Column", engine);

                    AutomationElement ret = null;
                    if (targetElement.TryGetCurrentPattern(GridPattern.Pattern, out object gridObj))
                    {
                        var grid = (GridPattern)gridObj;
                        //var customRows = targetElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));
                        //var customRows = targetElement.FindAll(TreeScope.Children, EM_UIElementTableUIElement.GetRowSearchConditionToDataGridView());
                        var customRows = EM_UIElementTableUIElement.GetRowsFromDataGridView(targetElement);
                        if (customRows.Count > 0)
                        {
                            // DataGridView (.net)
                            try
                            {
                                var row = GetInRangeUIElement(customRows, rowIndex, v_Row, "Row");
                                //var cols = row.FindAll(TreeScope.Children, new OrCondition(
                                //            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Header),
                                //            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit)
                                //        )
                                //    );
                                //var cols = row.FindAll(TreeScope.Children, EM_UIElementTableUIElement.GetColumnSearchConditionToDataGridView());
                                var cols = EM_UIElementTableUIElement.GetColumnsFromDataGridView(row);
                                ret = GetInRangeUIElement(cols, columnIndex, v_Column, "Column");
                            }
                            catch
                            {
                                ErrorAction();
                                return;
                            }
                        }
                        else
                        {
                            // listView
                            try
                            {
                                //var rows = targetElement.FindAll(TreeScope.Children,
                                //        new OrCondition(
                                //            new Condition[]
                                //            {
                                //                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Header),
                                //                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.DataItem),
                                //                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
                                //            }
                                //        )
                                //    );
                                //var rows = targetElement.FindAll(TreeScope.Children, EM_UIElementTableUIElement.GetRowSearchConditionToGridPattern());
                                var rows = EM_UIElementTableUIElement.GetRowsFromGridPattern(targetElement);
                                var row = GetInRangeUIElement(rows, rowIndex, v_Row, "Row");
                                //var cols = row.FindAll(TreeScope.Children, new OrCondition(
                                //            new Condition[]
                                //            {
                                //                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.HeaderItem),
                                //                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text),
                                //                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit),
                                //            }
                                //        )
                                //    );
                                //var cols = row.FindAll(TreeScope.Children, EM_UIElementTableUIElement.GetColumnSearchConditionToGridPattern());
                                var cols = EM_UIElementTableUIElement.GetColumnsFromGridPattern(row);
                                ret = GetInRangeUIElement(cols, columnIndex, v_Column, "Column");
                            }
                            catch
                            {
                                ErrorAction();
                                return;
                            }
                        }
                    }
                    else if (targetElement.TryGetCurrentPattern(SelectionPattern.Pattern, out object selectObj))
                    {
                        var selPattern = (SelectionPattern)selectObj;
                        // foobar2000 like table
                        try
                        {
                            //var rows = targetElement.FindAll(TreeScope.Children, EM_UIElementTableUIElement.GetRowSearchConditionToSelectionTable());
                            var rows = EM_UIElementTableUIElement.GetRowsFromSelectionTable(targetElement);
                            var row = GetInRangeUIElement(rows, rowIndex, v_Row, "Row");
                            //var cols = row.FindAll(TreeScope.Children, EM_UIElementTableUIElement.GetColumnSearchConditionToSelectionTable());
                            var cols = EM_UIElementTableUIElement.GetColumnsFromSelectionTable(row);
                            if ((cols.Count == 0) && (columnIndex == 0))
                            {
                                ret = row;
                            }
                            else
                            {
                                ret = GetInRangeUIElement(cols, columnIndex, v_Column, "Column");
                            }
                        }
                        catch
                        {
                            ErrorAction();
                            return;
                        }
                    }
                    else
                    {
                        ErrorAction();
                        return;
                    }
                    ret.StoreInUserVariable(engine, v_Result);
                })
            );
        }

        /// <summary>
        /// get in range UIElement
        /// </summary>
        /// <param name="elems"></param>
        /// <param name="index"></param>
        /// <param name="rawValue"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static AutomationElement GetInRangeUIElement(AutomationElementCollection elems, int index, string rawValue, string elementType)
        {
            if (index < 0)
            {
                index += elems.Count;
            }
            if (index < 0 || index >= elems.Count)
            {
                throw new Exception($"Error. Table {elementType} is Out of Range. Value: '{rawValue}', Expand Value: {index}");
            }
            else
            {
                return elems[index];
            }
        }
    }
}
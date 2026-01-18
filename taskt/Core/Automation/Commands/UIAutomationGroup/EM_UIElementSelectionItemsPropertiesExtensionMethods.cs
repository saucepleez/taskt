using System;
using System.Collections.Generic;
using System.Windows.Automation;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementSelectionItemsPropertiesExtensionMethods
    {
        public static void SelectionItemsAction(this IUIElementSelectionItemsProperties command, AutomationEngineInstance engine, Action<List<AutomationElement>> actionFunc, Action errorFunc)
        {
            List<AutomationElement> GetListItems(AutomationElement element) 
            {
                var elems = element.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsSelectionItemPatternAvailableProperty, true));
                var ret = new List<AutomationElement>();
                foreach (AutomationElement elem in elems)
                {
                    ret.Add(elem);
                }
                return ret;
            }

            command.UIElementAction(engine,
                new Action<AutomationElement>(targetElement =>
                {
                    List<AutomationElement> items;
                    ExpandCollapsePattern ecPtn = null;

                    if ((bool)targetElement.GetCurrentPropertyValue(AutomationElement.IsGridPatternAvailableProperty) ||
                        (bool)targetElement.GetCurrentPropertyValue(AutomationElement.IsSelectionPatternAvailableProperty))
                    {
                        // DataGridView-ComboBox, ListBox
                        items = GetListItems(targetElement);
                    }
                    else
                    {
                        // ComboBox, TreeView
                        AutomationElement curElement = targetElement;
                        bool isCmb = (bool)curElement.GetCurrentPropertyValue(AutomationElement.IsExpandCollapsePatternAvailableProperty);

                        if (!isCmb)
                        {
                            curElement = EM_CanHandleUIElementExtentionMethods.GetParentUIElement(curElement);
                            isCmb = (bool)curElement.GetCurrentPropertyValue(AutomationElement.IsExpandCollapsePatternAvailableProperty);
                        }

                        if (isCmb)
                        {
                            if (curElement.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out object selPtn))
                            {
                                ecPtn = (ExpandCollapsePattern)selPtn;

                                command.ExpandAndActivateWindowProcess(curElement, ecPtn, engine);

                                items = GetListItems(curElement);

                                if (items.Count == 0)
                                {
                                    // selection window is other window
                                    var pid = targetElement.Current.ProcessId;
                                    var con = new AndCondition(
                                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.List),
                                            new PropertyCondition(AutomationElement.ProcessIdProperty, pid)
                                        );
                                    var popupListElem = AutomationElement.RootElement.FindFirst(TreeScope.Children, con);
                                    if (popupListElem != null)
                                    {
                                        // DBG
                                        //Console.WriteLine($"#!# {popupListElem.Current.Name}");

                                        items = GetListItems(popupListElem);
                                    }
                                }
                            }
                            else
                            {
                                errorFunc();
                                return;
                            }
                        }
                        else
                        {
                            //throw new Exception("This UIElement does not have Selection Items");
                            errorFunc();
                            return;
                        }
                    }

                    actionFunc(items);

                    // collapse after action (when targetElement is expanded)
                    if (ecPtn != null)
                    {
                        ecPtn.Collapse();
                    }
                })
            );
        }

        /// <summary>
        /// expand selection items and activate window process
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        private static void ExpandAndActivateWindowProcess(this IUIElementSelectionItemsProperties command, AutomationElement targetElement, ExpandCollapsePattern expandPattern, AutomationEngineInstance engine)
        {
            var cmd = command.ToScriptCommand();
            if (cmd.ExpandValueOrUserVariableAsYesNo(nameof(command.v_ExpandWhenItemsNotFound), engine))
            {
                using (var elemVar = new InnerScriptVariable(engine))
                {
                    elemVar.VariableValue = targetElement;
                    using (var winVar = new InnerScriptVariable(engine))
                    {
                        // get window handle
                        var getHandle = new UIAutomationGetWindowHandleFromUIElementCommand()
                        {
                            v_TargetElement = elemVar.VariableName,
                            v_WindowHandleResult = winVar.VariableName,
                        };
                        getHandle.RunCommand(engine);

                        var activateWin = new ActivateWindowByWindowHandleCommand()
                        {
                            v_WindowHandle = VariableNameControls.GetWrappedVariableName(winVar.VariableName, engine),
                        };
                        activateWin.RunCommand(engine);

                        // expand and wait
                        if (string.IsNullOrEmpty(command.v_WaitTimeAfterExpand))
                        {
                            command.v_WaitTimeAfterExpand = "1000";
                        }
                        var waitTime = cmd.ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeAfterExpand), engine);
                        expandPattern.Expand();
                        System.Threading.Thread.Sleep(waitTime);
                    }
                }
            }
        }
    }
}

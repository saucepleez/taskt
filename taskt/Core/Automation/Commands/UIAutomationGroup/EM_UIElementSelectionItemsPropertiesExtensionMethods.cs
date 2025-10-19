using System;
using System.Collections.Generic;
using System.Windows.Automation;
using taskt.Core.Automation.Engine;

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
                                ExpandCollapsePattern ecPtn = (ExpandCollapsePattern)selPtn;

                                ecPtn.Expand();
                                System.Threading.Thread.Sleep(500);
                                items = GetListItems(curElement);
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
                })
            );
        }
    }
}

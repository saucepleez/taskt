using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementTableUIElement
    {
        /// <summary>
        /// row search condition to DataGridView(.net)
        /// </summary>
        /// <returns></returns>
        public static PropertyCondition GetRowSearchConditionToDataGridView()
        {
            return new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom);
        }

        /// <summary>
        /// get row UIElements from DataGridView(.net)
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static AutomationElementCollection GetRowsFromDataGridView(AutomationElement targetElement)
        {
            return FindAllFromChildren(targetElement, GetRowSearchConditionToDataGridView());
        }

        /// <summary>
        /// column search condition to DataGridView(.net)
        /// </summary>
        /// <returns></returns>
        public static OrCondition GetColumnSearchConditionToDataGridView()
        {
            return new OrCondition(
                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Header),
                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit)
                        );
        }

        /// <summary>
        /// get column UIElements from DataGridView(.net)
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static AutomationElementCollection GetColumnsFromDataGridView(AutomationElement targetElement)
        {
            return FindAllFromChildren(targetElement, GetColumnSearchConditionToDataGridView());
        }

        /// <summary>
        /// get row search condition to GridPattern UIElement
        /// </summary>
        /// <returns></returns>
        public static OrCondition GetRowSearchConditionToGridPattern()
        {
            return new OrCondition(
                            new Condition[]
                            {
                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Header),
                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.DataItem),
                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
                            }
                        );
        }

        /// <summary>
        /// get rows UIElements from GridPattern
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static AutomationElementCollection GetRowsFromGridPattern(AutomationElement targetElement)
        {
            return FindAllFromChildren(targetElement, GetRowSearchConditionToGridPattern());
        }

        /// <summary>
        /// get column search condition to GridPattern UIElement
        /// </summary>
        /// <returns></returns>
        public static OrCondition GetColumnSearchConditionToGridPattern()
        {
            return new OrCondition(
                            new Condition[]
                            {
                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.HeaderItem),
                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text),
                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit),
                            }
                        );
        }

        /// <summary>
        /// get column UIElements from GridPattern
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static AutomationElementCollection GetColumnsFromGridPattern(AutomationElement targetElement)
        {
            return FindAllFromChildren(targetElement, GetColumnSearchConditionToGridPattern());
        }

        /// <summary>
        /// get row search condition to SelectionPattarn table
        /// </summary>
        /// <returns></returns>
        public static OrCondition GetRowSearchConditionToSelectionTable()
        {
            return new OrCondition(
                            new Condition[]
                            {
                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Header),
                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
                            }
                        );
        }

        /// <summary>
        /// get rows UIElement from SelectionPattern table
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static AutomationElementCollection GetRowsFromSelectionTable(AutomationElement targetElement)
        {
            return FindAllFromChildren(targetElement, GetRowSearchConditionToSelectionTable());
        }

        /// <summary>
        /// get column search condition to SelectionPattern table
        /// </summary>
        /// <returns></returns>
        public static OrCondition GetColumnSearchConditionToSelectionTable()
        {
            return new OrCondition(
                            new Condition[]
                            {
                                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.HeaderItem),
                                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text),
                                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit),
                            }
                        );
        }

        /// <summary>
        /// get column UIElements from SelectionPattern table
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static AutomationElementCollection GetColumnsFromSelectionTable(AutomationElement targetElement)
        {
            return FindAllFromChildren(targetElement, GetColumnSearchConditionToSelectionTable());
        }

        /// <summary>
        /// general find all
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static AutomationElementCollection FindAllFromChildren(AutomationElement targetElement, Condition condition)
        {
            return targetElement.FindAll(TreeScope.Children, condition);
        }
    }
}

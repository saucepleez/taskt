using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}

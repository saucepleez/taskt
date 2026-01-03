using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_CanHandleUIElementScrollBarExtensionMethods
    {
        public static ScrollPattern GetScrollPattern(AutomationElement targetElement, Action notScrollBarAction)
        {
            if (!targetElement.TryGetCurrentPattern(ScrollPattern.Pattern, out object scrollPtn))
            {
                if (targetElement.Current.ControlType == ControlType.ScrollBar)
                {
                    var parentElement = EM_CanHandleUIElementExtentionMethods.GetParentUIElement(targetElement);
                    if (!parentElement.TryGetCurrentPattern(ScrollPattern.Pattern, out scrollPtn))
                    {
                        notScrollBarAction();
                        return null;
                    }
                }
                else
                {
                    notScrollBarAction();
                    return null;
                }
            }
            return (ScrollPattern)scrollPtn;
        }
    }
}

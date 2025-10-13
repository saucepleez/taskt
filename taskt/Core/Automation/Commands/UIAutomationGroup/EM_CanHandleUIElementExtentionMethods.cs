using System;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_CanHandleUIElementExtentionMethods
    {
        /// <summary>
        /// check specified object is UIElement
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsUIElement(object obj, out AutomationElement e)
        {
            if (obj is AutomationElement te)
            {
                e = te;
                return true;
            }
            else
            {
                e = null;
                return false;
            }
        }

        /// <summary>
        /// get window name from UIElement
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static (string, IntPtr) GetWindowNameAndHandle(AutomationElement targetElement)
        {
            TreeWalker walker = TreeWalker.RawViewWalker;

            if (targetElement.Current.ControlType == ControlType.Window)
            {
                return (targetElement.Current.Name, (IntPtr)targetElement.Current.NativeWindowHandle);
            }

            try
            {
                var parent = walker.GetParent(targetElement);
                while (parent.Current.ControlType != ControlType.Window)
                {
                    parent = walker.GetParent(parent);
                }
                return (parent.Current.Name, (IntPtr)parent.Current.NativeWindowHandle);
            }
            catch
            {
                // try other method
                var windowNames = WindowControls.GetAllWindowTitles();
                if ((targetElement.Current.NativeWindowHandle != 0) && (windowNames.Contains(targetElement.Current.Name)))
                {
                    return (targetElement.Current.Name, (IntPtr)targetElement.Current.NativeWindowHandle);
                }

                try
                {
                    var parent = walker.GetParent(targetElement);
                    while ((parent.Current.NativeWindowHandle == 0) || (!windowNames.Contains(parent.Current.Name)))
                    {
                        parent = walker.GetParent(parent);
                    }
                    return (parent.Current.Name, (IntPtr)parent.Current.NativeWindowHandle);
                }
                catch
                {
                    throw new Exception("Fail Get Window Name and Window Handle from UIElement");
                }
            }
        }

        /// <summary>
        /// get ControlType text
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static string GetControlTypeText(AutomationElement elem)
        {
            var fullName = elem.Current.ControlType.ProgrammaticName;
            return fullName.Substring(fullName.LastIndexOf('.') + 1);
        }
    }
}

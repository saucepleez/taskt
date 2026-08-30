using System;
using System.Windows.Automation;
using taskt.Core.Automation.Commands.WindowGroup;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_CanHandleUIElementExtentionMethods
    {
        public enum AutomationElementPropertyValueTypes
        {
            String,
            Bool,
            Int,
        }

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
            // already specify window
            if (targetElement.Current.ControlType == ControlType.Window)
            {
                return (targetElement.Current.Name, (IntPtr)targetElement.Current.NativeWindowHandle);
            }

            var walker = TreeWalker.RawViewWalker;
            try
            {
                var desktopHandle = EM_CanHandleDesktopWindowHandleExtensionMethods.GetDesktopWindowHandle();

                var currentElement = targetElement;
                while (true)
                {
                    var tparent = walker.GetParent(currentElement);
                    if (tparent != null)
                    {
                        var myHandle = (IntPtr)tparent.Current.NativeWindowHandle;
                        if (myHandle == desktopHandle)
                        {
                            // tparent is Desktop, currentElement is window(?)
                            return (currentElement.Current.Name, (IntPtr)currentElement.Current.NativeWindowHandle);
                        }
                        else if (tparent.Current.ControlType == ControlType.Window)
                        {
                            // tparent is window
                            return (tparent.Current.Name, myHandle);
                        }
                        else
                        {
                            currentElement = tparent;
                        }
                    }
                    else
                    {
                        throw new Exception("Parent Element is null");
                    }
                }

                //var parent = walker.GetParent(targetElement);
                //while (parent.Current.ControlType != ControlType.Window)
                //{
                //    parent = walker.GetParent(parent);
                //}
                //return (parent.Current.Name, (IntPtr)parent.Current.NativeWindowHandle);
            }
            catch
            {
                // try other method
                var windowNames = EM_CanHandleWindowNameExtensionMethods.GetAllWindowNames();
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
            // MEMO: UIA_AppBarControlTypeId is null, why?
            var fullName = elem.Current.ControlType?.ProgrammaticName ?? ".UNKNOWN";
            return fullName.Substring(fullName.LastIndexOf('.') + 1);
        }

        /// <summary>
        /// get parent UIElement
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement GetParentUIElement(AutomationElement targetElement)
        {
            var walker = TreeWalker.RawViewWalker;

            var parent = walker.GetParent(targetElement);
            if (parent != null)
            {
                return parent;
            }
            else
            {
                throw new Exception("Parent UIElement does not exists");
            }
        }

        /// <summary>
        /// get property value
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="propName"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static string GetPropertyValueAsString(AutomationElement targetElement, AutomationProperty propName, AutomationElementPropertyValueTypes tp)
        {
            try
            {
                return targetElement.GetCurrentPropertyValue(propName).ToString();
            }
            catch
            {
                switch (tp)
                {
                    case AutomationElementPropertyValueTypes.String:
                        return string.Empty;
                        
                    case AutomationElementPropertyValueTypes.Bool:
                        return "false";
                        
                    case AutomationElementPropertyValueTypes.Int:
                        return "0";

                    default:
                        return string.Empty;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands
{
    internal class AutomationElementControls
    {
        public static AutomationElement GetFromWindowName(string windowName)
        {
            var windowElement = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, windowName));
            if (windowElement == null)
            {
                // more deep search
                windowElement = AutomationElement.RootElement.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, windowName));
            }

            if (windowElement == null)
            {
                throw new Exception("Window Name '" + windowName + "' not found AutomationElement");
            }
            else
            {
                return windowElement;
            }
        }

        public static void CreateEmptyParamters(DataTable table)
        {
            table.Rows.Clear();
            table.Rows.Add(false, "AcceleratorKey", "");
            table.Rows.Add(false, "AccessKey", "");
            table.Rows.Add(false, "AutomationId", "");
            table.Rows.Add(false, "ClassName", "");
            table.Rows.Add(false, "FrameworkId", "");
            table.Rows.Add(false, "HasKeyboardFocus", "");
            table.Rows.Add(false, "HelpText", "");
            table.Rows.Add(false, "IsContentElement", "");
            table.Rows.Add(false, "IsControlElement", "");
            table.Rows.Add(false, "IsEnabled", "");
            table.Rows.Add(false, "IsKeyboardFocusable", "");
            table.Rows.Add(false, "IsOffscreen", "");
            table.Rows.Add(false, "IsPassword", "");
            table.Rows.Add(false, "IsRequiredForForm", "");
            table.Rows.Add(false, "ItemStatus", "");
            table.Rows.Add(false, "ItemType", "");
            table.Rows.Add(false, "LocalizedControlType", "");
            table.Rows.Add(false, "Name", "");
            table.Rows.Add(false, "NativeWindowHandle", "");
            table.Rows.Add(false, "ProcessID", "");
        }

        public static void ParseInspectToolResult(string result, DataTable table, System.Windows.Forms.ComboBox windowNames = null)
        {
            string[] results = result.Split(new[] { "\r\n" }, StringSplitOptions.None);

            if ((results.Length >= 1) && (result != ""))
            {
                CreateEmptyParamters(table);

                List<string> ancestors = new List<string>();
                string currentParam = "";
                foreach (string res in results)
                {
                    string[] spt = res.Split('\t');
                    string value = (spt.Length >= 2) ? spt[1] : "";
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                    if (spt[0] != "")
                    {
                        string name = spt[0].Substring(0, spt[0].Length - 1);
                        currentParam = name;

                        switch (name)
                        {
                            case "AcceleratorKey":
                            case "AccessKey":
                            case "AutomationId":
                            case "ClassName":
                            case "FrameworkId":
                            case "HasKeyboardFocus":
                            case "HelpText":
                            case "IsContentElement":
                            case "IsControlElement":
                            case "IsEnabled":
                            case "IsKeyboardFocusable":
                            case "IsOffscreen":
                            case "IsPassword":
                            case "IsRequiredForForm":
                            case "ItemStatus":
                            case "ItemType":
                            case "LocalizedControlType":
                            case "Name":
                            case "NativeWindowHandle":
                            case "ProcessID":
                                DataTableControls.SetParameterValue(table, value, name, "Parameter Name", "Parameter Value");
                                break;

                            case "Ancestors":
                                ancestors.Add(value);
                                break;
                        }
                    }
                    else
                    {
                        if (currentParam == "Ancestors")
                        {
                            ancestors.Add(value);
                        }
                    }
                    if (windowNames != null)
                    {
                        setComboBoxWindowNameFromInspectAncestors(ancestors, windowNames);
                    }
                }
            }
            else
            {
                var f = new UI.Forms.Supplemental.frmDialog("No Inspect Tool Results", "Fail Parse", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0);
                f.ShowDialog();
            }
        }

        private static void setComboBoxWindowNameFromInspectAncestors(List<string> ancestors, System.Windows.Forms.ComboBox cmb)
        {
            if (ancestors.Count > 0)
            {
                string[] windows = new string[cmb.Items.Count];
                cmb.Items.CopyTo(windows, 0);

                bool isFound = false;
                foreach (string ancestor in ancestors)
                {
                    // get quoted " " text
                    int pos = ancestor.IndexOf("\"");
                    if (pos < 0)
                    {
                        continue;
                    }

                    string winName = ancestor.Substring(pos + 1);
                    pos = winName.IndexOf("\"");
                    if (pos < 0)
                    {
                        continue;
                    }
                    winName = winName.Substring(0, pos);

                    foreach (string win in windows)
                    {
                        if (winName == win)
                        {
                            cmb.Text = winName;
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound)
                    {
                        break;
                    }
                }
            }
        }

        private static PropertyCondition CreatePropertyCondition(string propertyName, object propertyValue)
        {
            string propName = propertyName + "Property";

            switch (propertyName)
            {
                case "AcceleratorKey":
                    return new PropertyCondition(AutomationElement.AcceleratorKeyProperty, propertyValue);
                case "AccessKey":
                    return new PropertyCondition(AutomationElement.AccessKeyProperty, propertyValue);
                case "AutomationId":
                    return new PropertyCondition(AutomationElement.AutomationIdProperty, propertyValue);
                case "ClassName":
                    return new PropertyCondition(AutomationElement.ClassNameProperty, propertyValue);
                case "FrameworkId":
                    return new PropertyCondition(AutomationElement.FrameworkIdProperty, propertyValue);
                case "HasKeyboardFocus":
                    return new PropertyCondition(AutomationElement.HasKeyboardFocusProperty, propertyValue);
                case "HelpText":
                    return new PropertyCondition(AutomationElement.HelpTextProperty, propertyValue);
                case "IsContentElement":
                    return new PropertyCondition(AutomationElement.IsContentElementProperty, propertyValue);
                case "IsControlElement":
                    return new PropertyCondition(AutomationElement.IsControlElementProperty, propertyValue);
                case "IsEnabled":
                    return new PropertyCondition(AutomationElement.IsEnabledProperty, propertyValue);
                case "IsKeyboardFocusable":
                    return new PropertyCondition(AutomationElement.IsKeyboardFocusableProperty, propertyValue);
                case "IsOffscreen":
                    return new PropertyCondition(AutomationElement.IsOffscreenProperty, propertyValue);
                case "IsPassword":
                    return new PropertyCondition(AutomationElement.IsPasswordProperty, propertyValue);
                case "IsRequiredForForm":
                    return new PropertyCondition(AutomationElement.IsRequiredForFormProperty, propertyValue);
                case "ItemStatus":
                    return new PropertyCondition(AutomationElement.ItemStatusProperty, propertyValue);
                case "ItemType":
                    return new PropertyCondition(AutomationElement.ItemTypeProperty, propertyValue);
                case "LocalizedControlType":
                    return new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, propertyValue);
                case "Name":
                    return new PropertyCondition(AutomationElement.NameProperty, propertyValue);
                case "NativeWindowHandle":
                    return new PropertyCondition(AutomationElement.NativeWindowHandleProperty, propertyValue);
                case "ProcessID":
                    return new PropertyCondition(AutomationElement.ProcessIdProperty, propertyValue);
                default:
                    throw new NotImplementedException("Property Type '" + propertyName + "' not implemented");
            }
        }

        private static Condition CreateSearchCondition(DataTable table, taskt.Core.Automation.Engine.AutomationEngineInstance engine)
        {  
            //create search params
            var searchParams = from rw in table.AsEnumerable()
                                where rw.Field<string>("Enabled") == "True"
                                select rw;

            if (searchParams.Count() == 0)
            {
                return null;
            }

            //create and populate condition list
            var conditionList = new List<Condition>();
            foreach (var param in searchParams)
            {
                var parameterName = (string)param["ParameterName"];
                var parameterValue = (string)param["ParameterValue"];

                parameterName = parameterName.ConvertToUserVariable(engine);
                parameterValue = parameterValue.ConvertToUserVariable(engine);

                PropertyCondition propCondition;
                if (bool.TryParse(parameterValue, out bool bValue))
                {
                    propCondition = CreatePropertyCondition(parameterName, bValue);
                }
                else
                {
                    propCondition = CreatePropertyCondition(parameterName, parameterValue);
                }
                conditionList.Add(propCondition);
            }

            //concatenate or take first condition
            Condition searchConditions;
            if (conditionList.Count > 1)
            {
                searchConditions = new AndCondition(conditionList.ToArray());

            }
            else
            {
                searchConditions = conditionList[0];
            }

            return searchConditions;
        }

        public static AutomationElement SearchGUIElement(AutomationElement rootElement, DataTable conditionTable, taskt.Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Condition searchConditions = CreateSearchCondition(conditionTable, engine);

            var element = rootElement.FindFirst(TreeScope.Descendants, searchConditions);
            if (element == null)
            {
                // more deep search
                element = rootElement.FindFirst(TreeScope.Subtree, searchConditions);
                if (element == null)
                {
                    element = DeepSearchGUIElement(rootElement, searchConditions);
                }
            }
            // if element not found, don't throw exception here
            return element;
        }

        private static AutomationElement DeepSearchGUIElement(AutomationElement rootElement, Condition searchCondition)
        {
            TreeWalker walker = TreeWalker.RawViewWalker;

            // format conditions
            PropertyCondition[] conditions;
            if (searchCondition is AndCondition)
            {
                Condition[] conds = ((AndCondition)searchCondition).GetConditions();
                conditions = new PropertyCondition[conds.Length];
                for (int i = 0; i < conds.Length; i++)
                {
                    conditions[i] = (PropertyCondition)conds[i];
                }
            }
            else
            {
                conditions = new PropertyCondition[1];
                conditions[0] = (PropertyCondition)searchCondition;
            }

            var ret = WalkerSearch(rootElement, conditions, walker);
            return ret;
        }

        private static AutomationElement WalkerSearch(AutomationElement rootElement, PropertyCondition[] searchConditions, TreeWalker walker)
        {
            AutomationElement node = walker.GetFirstChild(rootElement);
            AutomationElement ret = null;

            while (node != null)
            {
                bool result = true;
                foreach (PropertyCondition condition in searchConditions)
                {
                    if (node.GetCurrentPropertyValue(condition.Property) != condition.Value)
                    {
                        result = false;
                        break;
                    }
                }
                if (result)
                {
                    ret = node;
                    break;
                }

                // search child node
                if (walker.GetFirstChild(node) != null)
                {
                    ret = WalkerSearch(node, searchConditions, walker);
                    if (ret != null)
                    {
                        break;
                    }
                }

                // next sibling
                node = walker.GetNextSibling(node);
            }

            return ret;
        }

        public static List<AutomationElement> GetAllGUIElements(AutomationElement rootElement, DataTable conditionTable, taskt.Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Condition searchConditions = CreateSearchCondition(conditionTable, engine);

            if (searchConditions != null)
            {
                AutomationElementCollection elements = rootElement.FindAll(TreeScope.Children, searchConditions);

                // if element not found, don't throw exception here
                List<AutomationElement> ret = new List<AutomationElement>();
                foreach (AutomationElement element in elements)
                {
                    ret.Add(element);
                }
                return ret;
            }
            else
            {
                return GetAllChildElements(rootElement);
            }
        }

        private static List<AutomationElement> GetAllChildElements(AutomationElement rootElement)
        {
            TreeWalker walker = TreeWalker.RawViewWalker;

            var elems = new List<AutomationElement>();

            var node = walker.GetFirstChild(rootElement);
            while (node != null)
            {
                elems.Add(node);
                node = walker.GetNextSibling(node);
            }

            return elems;
        }
    }
}

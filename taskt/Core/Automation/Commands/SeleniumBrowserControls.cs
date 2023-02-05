using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Forms;
using OpenQA.Selenium;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Selenium WebBrowser methods
    /// </summary>
    internal static class SeleniumBrowserControls
    {
        /// <summary>
        /// instance property
        /// </summary>
        [PropertyDescription("WebBrowser Instance Name")]
        [InputSpecification("WebBrowser Instance Name", true)]
        [PropertyDetailSampleUsage("**RPABrowser**", PropertyDetailSampleUsage.ValueType.Value, "WebBrowser Instance")]
        [PropertyDetailSampleUsage("**{{{vInstance}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebBrowser Instance")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Broser** command will cause an error")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyFirstValue("%kwd_default_browser_instance%")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("WebBrowser Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public static string v_InputInstanceName { get; }

        /// <summary>
        /// search method property
        /// </summary>
        [PropertyDescription("Element Search Method")]
        [PropertyUISelectionOption("Find Element By XPath")]
        [PropertyUISelectionOption("Find Element By ID")]
        [PropertyUISelectionOption("Find Element By Name")]
        [PropertyUISelectionOption("Find Element By Tag Name")]
        [PropertyUISelectionOption("Find Element By Class Name")]
        [PropertyUISelectionOption("Find Element By CSS Selector")]
        [PropertyUISelectionOption("Find Element By Link Text")]
        [PropertyUISelectionOption("Find Elements By XPath")]
        [PropertyUISelectionOption("Find Elements By ID")]
        [PropertyUISelectionOption("Find Elements By Name")]
        [PropertyUISelectionOption("Find Elements By Tag Name")]
        [PropertyUISelectionOption("Find Elements By Class Name")]
        [PropertyUISelectionOption("Find Elements By CSS Selector")]
        [PropertyUISelectionOption("Find Elements By Link Text")]
        [InputSpecification("", true)]
        [PropertyShowSampleUsageInDescription(false)]
        //[SampleUsage("Select **Find Element By XPath**, **Find Element By ID**, **Find Element By Name**, **Find Element By Tag Name**, **Find Element By Class Name**, **Find Element By CSS Selector**, **Find Element By Link Text**")]
        [Remarks("Select the specific search type that you want to use to isolate the element in the web page.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Search Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Search Method")]
        public static string v_SearchMethod { get; }

        /// <summary>
        /// search parameter property
        /// </summary>
        [PropertyDescription("Element Search Parameter")]
        [InputSpecification("Element Search Parameter", true)]
        [SampleUsage("")]
        [PropertyShowSampleUsageInDescription(false)]
        [Remarks("Specifies the parameter text that matches to the element based on the previously selected search type.")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Search Parameter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Search Parameter")]
        public static string v_SearchParameter { get; }

        /// <summary>
        /// element index
        /// </summary>
        [PropertyDescription("Element Index")]
        [InputSpecification("Element Index", true)]
        //[SampleUsage("**0** or **1** or **{{{vIndex}}}**")]
        [PropertyDetailSampleUsage("**0**", "Specify the First Element Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Element Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Element Index")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        public static string v_ElementIndex { get; }

        /// <summary>
        /// Attributes Name
        /// </summary>
        [PropertyDescription("Attributes Name to Get")]
        [InputSpecification("Attributes Name", true)]
        [PropertyDetailSampleUsage("**id**", PropertyDetailSampleUsage.ValueType.Value, "Attribute")]
        [PropertyDetailSampleUsage("**title**", PropertyDetailSampleUsage.ValueType.Value, "Attribute")]
        [PropertyDetailSampleUsage("**textContent**", "Specify the Element **Text Content** Value")]
        [PropertyDetailSampleUsage("**{{{vAttribute}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Attribute")]
        [PropertyDetailSampleUsage("**Displayed**", "Get the Specified Element is Displayed or Not", false)]
        [PropertyDetailSampleUsage("**Enabled**", "Get the Specified Element is Enabled or Not", false)]
        [PropertyDetailSampleUsage("**Location**", "Get the Specified Element Location. like **X,Y**, comma separated.", false)]
        [PropertyDetailSampleUsage("**Selected**", "Get the Specified Element is Selected or Not", false)]
        [PropertyDetailSampleUsage("**Size**", "Get the Specified Element Size. like **W,H**, comma separated.", false)]
        [PropertyDetailSampleUsage("**TagName**", "Get the Specified Element Tag Name.", false)]
        [PropertyDetailSampleUsage("**Text**", "Get the Specified Element innerText.", false)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true)]
        [PropertyDataGridViewColumnSettings("AttributeName", "Attribute Name")]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public static string v_AttributesName { get; }

        #region methods

        /// <summary>
        /// get WebBrowser Instance
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IWebDriver GetSeleniumBrowserInstance(this string instanceName, Engine.AutomationEngineInstance engine)
        {
            var vInstance = instanceName.ConvertToUserVariable(engine);
            var browserObject = engine.GetAppInstance(vInstance);

            if (browserObject is IWebDriver wd)
            {
                return wd;
            }
            else
            {
                throw new Exception("Instance Name '" + instanceName + "' is not WebBrowser Instance. Parsed Value: '" + vInstance + "'");
            }
        }

        /// <summary>
        /// get WebBrowser Instance and Searched Elemnet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="instanceParameterName"></param>
        /// <param name="searchMethodName"></param>
        /// <param name="searchParameterName"></param>
        /// <param name="elementIndexName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (IWebDriver, IWebElement) GetSeleniumBrowserInstanceAndElement(ScriptCommand command, string instanceParameterName, string searchMethodName, string searchParameterName, string elementIndexName, Engine.AutomationEngineInstance engine)
        {
            var seleniumInstance = instanceParameterName.GetSeleniumBrowserInstance(engine);

            var searchParameter = command.ConvertToUserVariable(searchParameterName, "Search Parameter", engine);
            var searchMethod = command.ConvertToUserVariable(searchMethodName, "Search Method", engine);
            object e = FindElement(seleniumInstance, searchParameter, searchMethod, engine);

            if (e is IWebElement elem)
            {
                return (seleniumInstance, elem);
            }
            else if (e is ReadOnlyCollection<IWebElement> elems)
            {
                var index = command.ConvertToUserVariableAsInteger(elementIndexName, engine);
                if (index < 0)
                {
                    index += elems.Count;
                }
                if ((index >= 0) && (index < elems.Count))
                {
                    return (seleniumInstance, elems[index]);
                }
                else
                {
                    throw new Exception("Element Index is out of Range. Value: " + index);
                }
            }
            else
            {
                throw new Exception("Fail Get Element. Method: '" + searchMethod + "', Parameter: '" + searchParameter + "'");
            }
        }

        public static void GetElementAttributes(IWebElement elem, DataTable attributes, Engine.AutomationEngineInstance engine, Action<string, string> setValueFunc)
        {
            int rows = attributes.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                string attrName = (attributes.Rows[i][0]?.ToString() ?? "").ConvertToUserVariable(engine);
                if (attrName != "")
                {
                    setValueFunc(attrName, GetAttribute(elem, attrName, engine));
                }
            }
        }

        /// <summary>
        /// find selenium element from specified method and search parameter
        /// </summary>
        /// <param name="seleniumInstance"></param>
        /// <param name="searchParameter"></param>
        /// <param name="searchMethod"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object FindElement(IWebDriver seleniumInstance, string searchParameter, string searchMethod, Engine.AutomationEngineInstance engine)
        {
            searchParameter = searchParameter.ConvertToUserVariable(engine);
            var parsedSearchMethod = searchMethod.ConvertToUserVariable(engine);

            object element;
            switch (parsedSearchMethod.ToLower())
            {
                case "find element by xpath":
                    element = seleniumInstance.FindElement(By.XPath(searchParameter));
                    break;

                case "find element by id":
                    element = seleniumInstance.FindElement(By.Id(searchParameter));
                    break;

                case "find element by name":
                    element = seleniumInstance.FindElement(By.Name(searchParameter));
                    break;

                case "find element by tag name":
                    element = seleniumInstance.FindElement(By.TagName(searchParameter));
                    break;

                case "find element by class name":
                    element = seleniumInstance.FindElement(By.ClassName(searchParameter));
                    break;

                case "find element by css selector":
                    element = seleniumInstance.FindElement(By.CssSelector(searchParameter));
                    break;

                case "find element by link text":
                    element = seleniumInstance.FindElement(By.LinkText(searchParameter));
                    break;

                case "find elements by xpath":
                    element = seleniumInstance.FindElements(By.XPath(searchParameter));
                    break;

                case "find elements by id":
                    element = seleniumInstance.FindElements(By.Id(searchParameter));
                    break;

                case "find elements by name":
                    element = seleniumInstance.FindElements(By.Name(searchParameter));
                    break;

                case "find elements by tag name":
                    element = seleniumInstance.FindElements(By.TagName(searchParameter));
                    break;

                case "find elements by class name":
                    element = seleniumInstance.FindElements(By.ClassName(searchParameter));
                    break;

                case "find elements by css selector":
                    element = seleniumInstance.FindElements(By.CssSelector(searchParameter));
                    break;

                case "find elements by link text":
                    element = seleniumInstance.FindElements(By.LinkText(searchParameter));
                    break;

                default:
                    throw new Exception("Strange Search Method '" + searchMethod + "'. Parsed: '" + parsedSearchMethod + "'");
            }

            return element;
        }

        /// <summary>
        /// get element attribute
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetAttribute(IWebElement element, string attributeName, Engine.AutomationEngineInstance engine)
        {
            attributeName = attributeName.ConvertToUserVariable(engine);
            if (string.IsNullOrEmpty(attributeName))
            {
                throw new Exception("Attribute Name is empty.");
            }

            switch (attributeName.ToLower())
            {
                case "enabled":
                    return element.Enabled.ToString();

                case "displayed":
                    return element.Displayed.ToString();

                case "selected":
                    return element.Selected.ToString();

                case "text":
                    return element.Text;

                case "tag":
                case "tag name":
                case "tagname":
                    return element.TagName;

                case "location":
                    System.Drawing.Point lc = element.Location;
                    return lc.X.ToString() + "," + lc.Y.ToString();

                case "size":
                    System.Drawing.Size sz = element.Size;
                    return sz.Width.ToString() + "," + sz.Height.ToString();

                default:
                    return element.GetAttribute(attributeName);
            }
        }

        #endregion

        #region events

        public static void SearchMethodComboBox_SelectionChangeCommitted(Dictionary<string, Control> controlsList, ComboBox searchMethodComboBox, string indexParameterName)
        {
            string item = searchMethodComboBox.SelectedItem?.ToString().ToLower() ?? "";
            if (item.StartsWith("find elements"))
            {
                foreach (var ctrl in controlsList)
                {
                    if (ctrl.Key.Contains(indexParameterName))
                    {
                        ctrl.Value.Visible = true;
                    }
                }
            }
            else
            {
                foreach (var ctrl in controlsList)
                {
                    if (ctrl.Key.Contains(indexParameterName))
                    {
                        ctrl.Value.Visible = false;
                    }
                }
            }
        }

        #endregion
    }
}

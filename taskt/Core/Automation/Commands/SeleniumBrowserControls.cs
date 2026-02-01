using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
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
        #region Virtual Property

        /// <summary>
        /// webbrowser instance name
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_InputInstanceName))]
        [PropertyDescription("WebBrowser Instance Name")]
        [InputSpecification("WebBrowser Instance Name", true)]
        [PropertyDetailSampleUsage("**RPABrowser**", PropertyDetailSampleUsage.ValueType.Value, "WebBrowser Instance")]
        [PropertyDetailSampleUsage("**{{{vInstance}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebBrowser Instance")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Broser** command will cause an error")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        [PropertyValidationRule("WebBrowser Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        [PropertyFirstValue("%kwd_default_browser_instance%")]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterOrder(5000)]
        public static string v_InputInstanceName { get; }

        /// <summary>
        /// search method property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
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
        [Remarks("Select the specific search type that you want to use to isolate the element in the web page.")]
        [PropertyValidationRule("Search Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Search Method")]
        //[InputSpecification("", true)]
        //[PropertyShowSampleUsageInDescription(false)]
        //[SampleUsage("Select **Find Element By XPath**, **Find Element By ID**, **Find Element By Name**, **Find Element By Tag Name**, **Find Element By Class Name**, **Find Element By CSS Selector**, **Find Element By Link Text**")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyParameterOrder(5000)]
        public static string v_SearchMethod { get; }

        /// <summary>
        /// search parameter property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Element Search Parameter")]
        [InputSpecification("Element Search Parameter", true)]
        [PropertyValidationRule("Search Parameter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Search Parameter")]
        [Remarks("Specifies the parameter text that matches to the element based on the previously selected search type.")]
        //[SampleUsage("")]
        //[PropertyShowSampleUsageInDescription(false)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyParameterOrder(5000)]
        public static string v_SearchParameter { get; }

        /// <summary>
        /// element index
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Element Index")]
        [InputSpecification("Element Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specify the First Element Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Element Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Element Index")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyParameterOrder(5000)]
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
        [PropertyParameterOrder(5000)]
        public static string v_AttributesName { get; }

        /// <summary>
        /// attribute name
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
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
        [PropertyValidationRule("Attribute", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyParameterOrder(5000)]
        public static string v_AttributeName { get; }

        /// <summary>
        /// element wait time
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time for the WebElement to Exist (sec)")]
        [Remarks("Specify how long to Wait before an Error will occur because the WebElement is Not Found.")]
        [PropertyIsOptional(true, "120")]
        [PropertyFirstValue("120")]
        //[InputSpecification("Wait Time", true)]
        //[PropertyDetailSampleUsage("**120**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        //[PropertyDetailSampleUsage("**{{{vTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyParameterOrder(5000)]
        public static string v_WaitTime { get; }

        /// <summary>
        /// input WebElement property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_InputInstanceName))]
        [PropertyDescription("WebElement Variable Name")]
        [InputSpecification("WebElement Variable Name", true)]
        [PropertyDetailSampleUsage("**vElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WebElement, true)]
        [PropertyValidationRule("WebElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Element")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        //[PropertyParameterOrder(5000)]
        public static string v_InputWebElementName { get; }

        /// <summary>
        /// output WebElement property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store WebElement")]
        [InputSpecification("WebElement Variable Name", true)]
        [PropertyDetailSampleUsage("**vElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WebElement, true)]
        [PropertyValidationRule("WebElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "WebElement")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyParameterOrder(5000)]
        public static string v_OutputWebElementName { get; }

        /// <summary>
        /// scroll to element
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Scroll to WebElement")]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "Scroll")]
        //[InputSpecification("", true)]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]
        //[PropertyParameterOrder(5000)]
        public static string v_ScrollToElement { get; }
        #endregion

        #region methods

        #region convert store methods

        /// <summary>
        /// expand user variable as WebElement and WebDriver
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (IWebElement, IWebDriver) ExpandUserVariableAsWebElementAndWebDriver(this string str, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var v = str.GetRawVariable(engine);

            if (v.VariableValue is ValueTuple<IWebElement, IWebDriver> webs)
            {
                return webs;
            }
            else
            {
                throw new Exception($"{parameterName} '{str}' is not a WebElement-WebDriver tuple.");
            }
        }

        /// <summary>
        /// expand user variable as WebElement
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IWebElement ExpandUserVariableAsWebElement(this string str, string parameterName, Engine.AutomationEngineInstance engine)
        {
            //var v = str.GetRawVariable(engine);
            ////var tp = typeof((IWebElement, IWebDriver));
            ////var vtp = v.VariableValue.GetType();
            ////if (v.VariableValue is (IWebElement, IWebDriver))
            ////{
            ////    return webs.Item1;
            ////}
            ////else
            ////{
            ////    throw new Exception($"{parameterName} '{str}' is not a WebElement.");
            ////}
            ////try
            ////{
            ////    var webs = ((IWebElement, IWebDrier))v.VariableValue;
            ////}
            ////catch
            ////{
            ////    throw new Exception($"{parameterName} '{str}' is not a WebElement.");
            ////}
            //if (v.VariableValue is ValueTuple<IWebElement, IWebDriver> webs)
            //{
            //    return webs.Item1;
            //}
            //else
            //{
            //    throw new Exception($"{parameterName} '{str}' is not a WebElement.");
            //}

            (var e, _) = ExpandUserVariableAsWebElementAndWebDriver(str, parameterName, engine);
            return e;
        }

        /// <summary>
        /// expand user variable as webDriver from WebElement
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IWebDriver ExpandUserVariableAsWebDriverFromWebElement(this string str, string parameterName, Engine.AutomationEngineInstance engine)
        {
            //var v = str.GetRawVariable(engine);

            //if (v.VariableValue is ValueTuple<IWebElement, IWebDriver> webs)
            //{
            //    return webs.Item2;
            //}
            //else
            //{
            //    throw new Exception($"{parameterName} '{str}' is not a WebElement.");
            //}

            (_, var d) = ExpandUserVariableAsWebElementAndWebDriver(str, parameterName, engine);
            return d;
        }

        /// <summary>
        /// store WebElement (and WebDriver) to user variable
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <param name="targetVariable"></param>
        public static void StoreInUserVariable(this (IWebElement, IWebDriver) value, Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, engine, false);
        }

        #endregion

        #region instance methods
        /// <summary>
        /// get WebBrowser Instance
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IWebDriver ExpandValueOrUserVariableAsSeleniumBrowserInstance(this string instanceName, Engine.AutomationEngineInstance engine)
        {
            var vInstance = instanceName.ExpandValueOrUserVariable(engine);
            var browserObject = engine.GetAppInstance(vInstance);

            if (browserObject is ValueTuple<IWebDriver, string> pair)
            {
                return pair.Item1;
            }
            else
            {
                throw new Exception($"Instance Name '{instanceName}' is not WebBrowser Instance. Parsed Value: '{vInstance}'");
            }
        }
        #endregion

        #region search element(s) methods

        /// <summary>
        /// get web element search method by specified search method
        /// </summary>
        /// <param name="searchMethod"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Func<IWebDriver, string, object> GetWebElementSearchMethod(string searchMethod)
        {
            switch (searchMethod.ToLower())
            {
                case "find element by xpath":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.XPath(parameter));
                    });

                case "find element by id":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.Id(parameter));
                    });

                case "find element by name":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.Name(parameter));
                    });

                case "find element by tag name":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.TagName(parameter));
                    });

                case "find element by class name":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.ClassName(parameter));
                    });

                case "find element by css selector":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.CssSelector(parameter));
                    });

                case "find element by link text":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.LinkText(parameter));
                    });

                case "find elements by xpath":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.XPath(parameter));
                    });

                case "find elements by id":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.Id(parameter));
                    });

                case "find elements by name":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.Name(parameter));
                    });

                case "find elements by tag name":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.TagName(parameter));
                    });

                case "find elements by class name":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.ClassName(parameter));
                    });

                case "find elements by css selector":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.CssSelector(parameter));
                    });

                case "find elements by link text":
                    return new Func<IWebDriver, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.LinkText(parameter));
                    });

                default:
                    throw new Exception($"Strange Search Method '{searchMethod}'");
            }
        }

        /// <summary>
        /// expand value or user variable as instance and searched an webElement
        /// </summary>
        /// <param name="seleniumInstance"></param>
        /// <param name="searchMethod"></param>
        /// <param name="searchParameter"></param>
        /// <param name="index"></param>
        /// <param name="waitTime"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (IWebDriver, IWebElement) ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElement(IWebDriver seleniumInstance, string searchMethod, string searchParameter, int index, int waitTime, Engine.AutomationEngineInstance engine)
        {
            var searchFunc = GetWebElementSearchMethod(searchMethod);

            var ret = WaitControls.WaitProcess(waitTime, "WebElement", new Func<(bool, object)>(() =>
            {
                try
                {
                    var t = searchFunc(seleniumInstance, searchParameter);
                    if (t is IWebElement elem)
                    {
                        return (true, elem);
                    }
                    else if (t is ReadOnlyCollection<IWebElement> elems)
                    {
                        if (index < 0)
                        {
                            index += elems.Count;
                        }
                        if ((index >= 0) && (index < elems.Count))
                        {
                            return (true, elems[index]);
                        }
                        else
                        {
                            return (false, null);
                        }
                    }
                    else
                    {
                        return (false, null);
                    }
                }
                catch
                {
                    return (false, null);
                }
            }), engine);

            if (ret is IWebElement e)
            {
                return (seleniumInstance, e);
            }
            else
            {
                throw new Exception("WebElement Not Found.");
            }
        }

        /// <summary>
        /// expand value or user variable as instance and searched an webElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="instanceParameterName"></param>
        /// <param name="searchMethodName"></param>
        /// <param name="searchParameterName"></param>
        /// <param name="elementIndexName"></param>
        /// <param name="waitTimeName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (IWebDriver, IWebElement) ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElement(ScriptCommand command, string instanceParameterName, string searchMethodName, string searchParameterName, string elementIndexName, string waitTimeName, Engine.AutomationEngineInstance engine)
        {
            var instanceName = command.ExpandValueOrUserVariable(instanceParameterName, "WebBrowser Instance Name", engine);
            var seleniumInstance = instanceName.ExpandValueOrUserVariableAsSeleniumBrowserInstance(engine);

            var searchParameter = command.ExpandValueOrUserVariable(searchParameterName, "Search Parameter", engine);
            var searchMethod = command.ExpandValueOrUserVariable(searchMethodName, "Search Method", engine);

            var waitTime = command.ExpandValueOrUserVariableAsInteger(waitTimeName, engine);

            var indexString = command.GetRawPropertyValueAsString(elementIndexName, "Index");
            int index;
            if (string.IsNullOrEmpty(indexString))
            {
                index = 0;
            }
            else
            {
                index = command.ExpandValueOrUserVariableAsInteger(elementIndexName, engine);
            }

            return ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElement(seleniumInstance, searchMethod, searchParameter, index, waitTime, engine);
        }

        /// <summary>
        /// expand value or user variable as instance and searched an webElements
        /// </summary>
        /// <param name="seleniumInstance"></param>
        /// <param name="searchMethod"></param>
        /// <param name="searchParameter"></param>
        /// <param name="waitTime"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (IWebDriver, List<IWebElement>) ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElements(IWebDriver seleniumInstance, string searchMethod, string searchParameter, int waitTime, Engine.AutomationEngineInstance engine)
        {
            var searchFunc = GetWebElementSearchMethod(searchMethod);

            var ret = WaitControls.WaitProcess(waitTime, "WebElement", new Func<(bool, object)>(() =>
            {
                try
                {
                    var t = searchFunc(seleniumInstance, searchParameter);
                    if (t is IWebElement elem)
                    {
                        return (true, new List<IWebElement>() { elem });
                    }
                    else if (t is ReadOnlyCollection<IWebElement> elems)
                    {
                        if (elems.Count > 0)
                        {
                            return (true, elems.ToList());
                        }
                        else
                        {
                            return (false, null);
                        }
                    }
                    else
                    {
                        return (false, null);
                    }
                }
                catch
                {
                    return (false, null);
                }
            }), engine);

            if (ret is List<IWebElement> e)
            {
                return (seleniumInstance, e);
            }
            else
            {
                throw new Exception("WebElement(s) Not Found.");
            }
        }

        /// <summary>
        /// expand value or user variable as instance and searched an webElements
        /// </summary>
        /// <param name="command"></param>
        /// <param name="instanceParameterName"></param>
        /// <param name="searchMethodName"></param>
        /// <param name="searchParameterName"></param>
        /// <param name="waitTimeName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (IWebDriver, List<IWebElement>) ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElements(ScriptCommand command, string instanceParameterName, string searchMethodName, string searchParameterName, string waitTimeName, Engine.AutomationEngineInstance engine)
        {
            var instanceName = command.ExpandValueOrUserVariable(instanceParameterName, "WebBrowser Instance Name", engine);
            var seleniumInstance = instanceName.ExpandValueOrUserVariableAsSeleniumBrowserInstance(engine);

            var searchParameter = command.ExpandValueOrUserVariable(searchParameterName, "Search Parameter", engine);
            var searchMethod = command.ExpandValueOrUserVariable(searchMethodName, "Search Method", engine);

            var waitTime = command.ExpandValueOrUserVariableAsInteger(waitTimeName, engine);

            return ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElements(seleniumInstance, searchMethod, searchParameter, waitTime, engine);
        }

        #endregion

        #region Attribute methods

        /// <summary>
        /// get element attributes specified DataTable
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="attributes"></param>
        /// <param name="engine"></param>
        /// <param name="setValueFunc"></param>
        public static void GetElementAttributes(IWebElement elem, DataTable attributes, Engine.AutomationEngineInstance engine, Action<string, string> setValueFunc)
        {
            int rows = attributes.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                string attrName = (attributes.Rows[i][0]?.ToString() ?? "").ExpandValueOrUserVariable(engine);
                if (attrName != "")
                {
                    setValueFunc(attrName, GetAttribute(elem, attrName, engine));
                }
            }
        }

        /// <summary>
        /// get Elements attribute specified by argument
        /// </summary>
        /// <param name="elems"></param>
        /// <param name="attributeValue"></param>
        /// <param name="engine"></param>
        /// <param name="setValueFunc"></param>
        public static void GetElementsAttribute(List<IWebElement> elems, string attributeValue, Engine.AutomationEngineInstance engine, Action<int, string, string> setValueFunc)
        {
            var attr = attributeValue.ExpandValueOrUserVariable(engine);
            for (int i = 0; i < elems.Count; i++)
            {
                setValueFunc(i, attr, GetAttribute(elems[i], attr, engine));
            }
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
            attributeName = attributeName.ExpandValueOrUserVariable(engine);
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
                    return $"{lc.X},{lc.Y}";

                case "size":
                    System.Drawing.Size sz = element.Size;
                    return $"{sz.Width},{sz.Height}";

                default:
                    var attr = element.GetDomAttribute(attributeName) ?? element.GetDomProperty(attributeName);
                    if (attr != null)
                    {
                        return attr;
                    }
                    else
                    {
                        throw new Exception($"Attribute '{attributeName}' does not exists.");
                    }
            }
        }

        #endregion

        #region WebElement methods

        public static string CreateXPath(IWebElement elem)
        {
            // MEMO: it's probably works fine. :-)

            string path = string.Empty;

            var curElem = elem;
            var curElemId = curElem.ToString();

            // DBG
            //Debug.WriteLine($"curElem: {curElem.ToString()}, {curElem.GetHashCode()}");

            var pElem = curElem.FindElement(By.XPath("parent::*"));
            while (true)
            {
                var cElems = pElem.FindElements(By.XPath($"{curElem.TagName.ToLower()}"));
                if (cElems.Count > 1)
                {
                    int index = 1;
                    foreach (var e in cElems)
                    {
                        if (e.ToString() == curElemId)
                        {
                            break;
                        }
                        index++;
                    }
                    path = $"/{curElem.TagName.ToLower()}[{index}]{path}";
                }
                else
                {
                    path = $"/{curElem.TagName.ToLower()}{path}";
                }

                // DGB
                //Debug.WriteLine($"totyu: {path}");

                if (curElem.TagName.ToLower() == "body")
                {
                    path = $"/html{path}";
                    break;
                }
                else
                {
                    curElem = pElem;
                    curElemId = curElem.ToString();
                    pElem = curElem.FindElement(By.XPath("parent::*"));
                }
            }

            // DBG
            //Debug.WriteLine($"XPath: {path}");

            return path;
        }

        #endregion

        #region tag methods

        public static bool CheckTagName(this IWebElement elem, string tagName)
        {
            return (elem.TagName.ToLower() == tagName.ToLower());
        }

        #endregion

        #region JS execute

        public static object ExcecuteScript(IWebDriver seleniumInstance, string script)
        {
            var js = seleniumInstance as IJavaScriptExecutor;
            return js.ExecuteScript(script);
        }

        #endregion

        #endregion

        #region events

        public static void SearchMethodComboBox_SelectionChangeCommitted(Dictionary<string, Control> controlsList, ComboBox searchMethodComboBox, string indexParameterName)
        {
            string item = searchMethodComboBox.SelectedItem?.ToString().ToLower() ?? "";
            FormUIControls.SetVisibleParameterControlGroup(controlsList, indexParameterName, item.StartsWith("find elements"));
        }

        public static void ScrollToWebElement_SelectionChange(ComboBox scrollParameter, Dictionary<string, Control> controlsList, string instanceParameterName)
        {
            FormUIControls.SetVisibleParameterControlGroup(controlsList, instanceParameterName, ((scrollParameter.SelectedItem?.ToString().ToLower() ?? "") != "no"));
        }

        #endregion
    }
}

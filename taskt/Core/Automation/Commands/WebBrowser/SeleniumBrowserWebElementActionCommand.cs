using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("WebElement Action")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to manipulate, set, or get data on a webpage within the web browser.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class SeleniumBrowserWebElementActionCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("WebBrowser Instance Name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyShowSampleUsageInDescription(true)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Element Search Method")]
        //[PropertyUISelectionOption("Find Element By XPath")]
        //[PropertyUISelectionOption("Find Element By ID")]
        //[PropertyUISelectionOption("Find Element By Name")]
        //[PropertyUISelectionOption("Find Element By Tag Name")]
        //[PropertyUISelectionOption("Find Element By Class Name")]
        //[PropertyUISelectionOption("Find Element By CSS Selector")]
        //[PropertyUISelectionOption("Find Element By Link Text")]
        //[PropertyUISelectionOption("Find Elements By XPath")]
        //[PropertyUISelectionOption("Find Elements By ID")]
        //[PropertyUISelectionOption("Find Elements By Name")]
        //[PropertyUISelectionOption("Find Elements By Tag Name")]
        //[PropertyUISelectionOption("Find Elements By Class Name")]
        //[PropertyUISelectionOption("Find Elements By CSS Selector")]
        //[PropertyUISelectionOption("Find Elements By Link Text")]
        //[InputSpecification("Select the specific search type that you want to use to isolate the element in the web page.")]
        //[SampleUsage("Select **Find Element By XPath**, **Find Element By ID**, **Find Element By Name**, **Find Element By Tag Name**, **Find Element By Class Name**, **Find Element By CSS Selector**, **Find Element By Link Text**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        [PropertySelectionChangeEvent(nameof(cmbSearchType_SelectionChangeCommited))]
        public string v_SeleniumSearchType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Element Search Parameter")]
        //[InputSpecification("Specifies the parameter text that matches to the element based on the previously selected search type.")]
        //[SampleUsage("If search type **Find Element By ID** was specified, for example, given <div id='name'></div>, the value of this field would be **name**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        public string v_SeleniumSearchParameter { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Target Element Index (Only Use Fined Elements ***)")]
        //[InputSpecification("")]
        //[SampleUsage("**0** or **1** or **{{{vIndex}}}**")]
        //[Remarks("If parameter is $x('//div') and index is 5, it's means target is $x('//div')[5].")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ElementIndex))]
        public string v_SeleniumElementIndex { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Element Action")]
        //[PropertyUISelectionOption("Invoke Click")]
        //[PropertyUISelectionOption("Left Click")]
        //[PropertyUISelectionOption("Right Click")]
        //[PropertyUISelectionOption("Middle Click")]
        //[PropertyUISelectionOption("Double Left Click")]
        //[PropertyUISelectionOption("Clear Element")]
        //[PropertyUISelectionOption("Set Text")]
        //[PropertyUISelectionOption("Get Text")]
        //[PropertyUISelectionOption("Get Attribute")]
        //[PropertyUISelectionOption("Get Matching Elements")]
        //[PropertyUISelectionOption("Wait For Element To Exist")]
        //[PropertyUISelectionOption("Switch to frame")]
        //[PropertyUISelectionOption("Get Count")]
        //[PropertyUISelectionOption("Get Options")]
        //[PropertyUISelectionOption("Select Option")]
        //[InputSpecification("Select the appropriate corresponding action to take once the element has been located")]
        //[SampleUsage("Select from **Invoke Click**, **Left Click**, **Right Click**, **Middle Click**, **Double Left Click**, **Clear Element**, **Set Text**, **Get Text**, **Get Attribute**, **Wait For Element To Exist**, **Get Count**")]
        //[Remarks("Selecting this field changes the parameters that will be required in the next step")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertySelectionChangeEvent("seleniumAction_SelectionChangeCommitted")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Element Action")]
        [PropertyUISelectionOption("Click WebElement")]
        [PropertyUISelectionOption("Clear WebElement")]
        [PropertyUISelectionOption("Set Text")]
        [PropertyUISelectionOption("Get Text")]
        [PropertyUISelectionOption("Get Attribute")]
        [PropertyUISelectionOption("Get Matching WebElements")]
        [PropertyUISelectionOption("Wait For WebElement To Exist")]
        [PropertyUISelectionOption("Switch To Frame")]
        [PropertyUISelectionOption("Get WebElements Count")]
        [PropertyUISelectionOption("Get Options")]
        [PropertyUISelectionOption("Select Option")]
        [PropertySelectionChangeEvent(nameof(cmbSeleniumAction_SelectionChangeCommitted))]
        public string v_SeleniumElementAction { get; set; }

        [XmlElement]
        [PropertyDescription("Additional Parameters")]
        [InputSpecification("Additioal Parameters will be required based on the action settings selected.")]
        [SampleUsage("Additional Parameters range from adding offset coordinates to specifying a variable to apply element text to.")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true)]
        [PropertyDataGridViewColumnSettings("Parameter Name", "Parameter Name", true)]
        [PropertyDataGridViewColumnSettings("Parameter Value", "Parameter Value", false)]
        //[PropertyDataGridViewCellEditEvent(nameof(ElementsGridViewHelper_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        //[PropertyDataGridViewCellEditEvent(nameof(ElementsGridViewHelper_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        public DataTable v_WebActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        public string v_ScrollToElement { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView ElementsGridViewHelper;

        //[XmlIgnore]
        //[NonSerialized]
        //private ComboBox ElementActionDropdown;

        //[XmlIgnore]
        //[NonSerialized]
        //private List<Control> ElementParameterControls;

        public SeleniumBrowserWebElementActionCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var seleniumSearchParam = v_SeleniumSearchParameter.ConvertToUserVariable(sender);

            //var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            //dynamic element = null;

            //if (v_SeleniumElementAction == "Wait For Element To Exist")
            //{

            //    var timeoutText = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                       where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
            //                       select rw.Field<string>("Parameter Value")).FirstOrDefault();

            //    timeoutText = timeoutText.ConvertToUserVariable(sender);

            //    int timeOut = Convert.ToInt32(timeoutText);

            //    var timeToEnd = DateTime.Now.AddSeconds(timeOut);

            //    while (timeToEnd >= DateTime.Now)
            //    {
            //        try
            //        {
            //            element = FindElement(seleniumInstance, seleniumSearchParam);
            //            break;
            //        }
            //        catch (Exception)
            //        {
            //            engine.ReportProgress("Element Not Yet Found... " + (int)((timeToEnd - DateTime.Now).TotalSeconds) + "s remain");
            //            System.Threading.Thread.Sleep(1000);
            //        }
            //    }

            //    if (element == null)
            //    {
            //        throw new Exception("Element Not Found");
            //    }

            //    return;
            //}
            //else if (seleniumSearchParam != string.Empty)
            //{
            //    element = FindElement(seleniumInstance, seleniumSearchParam);
            //}

            //// set index
            //if ((v_SeleniumSearchType.ToLower().StartsWith("find elements ")) && !String.IsNullOrEmpty(v_SeleniumElementIndex))
            //{
            //    switch (v_SeleniumElementAction.ToLower())
            //    {
            //        case "invoke click":
            //        case "left click":
            //        case "right click":
            //        case "middle click":
            //        case "double left click":
            //        case "clear element":
            //        case "set text":
            //        case "get text":
            //        case "get attribute":
            //        case "select option":
            //            int idx = int.Parse(v_SeleniumElementIndex);
            //            element = ((ReadOnlyCollection<IWebElement>)element)[idx];
            //            break;
            //    }
            //}

            //switch (v_SeleniumElementAction)
            //{
            //    case "Invoke Click":
            //        int seleniumWindowHeightY = seleniumInstance.Manage().Window.Size.Height;
            //        int elementPositionY = element.Location.Y;
            //        if (elementPositionY > seleniumWindowHeightY)
            //        {
            //            String scroll = String.Format("window.scroll(0, {0})", elementPositionY);
            //            IJavaScriptExecutor js = seleniumInstance as IJavaScriptExecutor;
            //            js.ExecuteScript(scroll);
            //        }
            //        element.Click();
            //        break;

            //    case "Left Click":
            //    case "Right Click":
            //    case "Middle Click":
            //    case "Double Left Click":


            //        int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
            //                                           where rw.Field<string>("Parameter Name") == "X Adjustment"
            //                                           select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

            //        int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
            //                                           where rw.Field<string>("Parameter Name") == "Y Adjustment"
            //                                           select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

            //        var elementLocation = element.Location;
            //        MoveMouseCommand newMouseMove = new MoveMouseCommand();
            //        var seleniumWindowPosition = seleniumInstance.Manage().Window.Position;
            //        newMouseMove.v_XMousePosition = (seleniumWindowPosition.X + elementLocation.X + 30 + userXAdjust).ToString(); // added 30 for offset
            //        newMouseMove.v_YMousePosition = (seleniumWindowPosition.Y + elementLocation.Y + 130 + userYAdjust).ToString(); //added 130 for offset
            //        newMouseMove.v_MouseClick = v_SeleniumElementAction;
            //        newMouseMove.RunCommand(sender);
            //        break;

            //    case "Set Text":

            //        string textToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                            where rw.Field<string>("Parameter Name") == "Text To Set"
            //                            select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender); ;

            //        string clearElement = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                               where rw.Field<string>("Parameter Name") == "Clear Element Before Setting Text"
            //                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

            //        string encryptedData = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                               where rw.Field<string>("Parameter Name") == "Encrypted Text"
            //                                select rw.Field<string>("Parameter Value")).FirstOrDefault();



            //        if (clearElement == null)
            //        {
            //            clearElement = "No";
            //        }

            //        if (clearElement.ToLower() == "yes")
            //        {
            //            element.Clear();
            //        }


            //        if (encryptedData == "Encrypted")
            //        {
            //            textToSet = EncryptionServices.DecryptString(textToSet, "TASKT");
            //        }

            //        string[] potentialKeyPresses = textToSet.Split('{', '}');

            //        Type seleniumKeys = typeof(OpenQA.Selenium.Keys);
            //        System.Reflection.FieldInfo[] fields = seleniumKeys.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            //        //check if chunked string contains a key press command like {ENTER}
            //        foreach (string chunkedString in potentialKeyPresses)
            //        {
            //            if (chunkedString == "")
            //                continue;

            //            if (fields.Any(f => f.Name == chunkedString))
            //            {
            //                string keyPress = (string)fields.Where(f => f.Name == chunkedString).FirstOrDefault().GetValue(null);
            //                element.SendKeys(keyPress);


            //            }
            //            else
            //            {
            //                //convert to user variable - https://github.com/saucepleez/taskt/issues/22
            //                var convertedChunk = chunkedString.ConvertToUserVariable(sender);
            //                element.SendKeys(convertedChunk);
            //            }
            //        }

            //        break;
            //    case "Get Options":

            //        string applyToVarName = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                               where rw.Field<string>("Parameter Name") == "Variable Name"
            //                               select rw.Field<string>("Parameter Value")).FirstOrDefault();


            //        string attribName = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                                where rw.Field<string>("Parameter Name") == "Attribute Name"
            //                                select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);


            //        var optionsItems = new List<string>();
            //        var ele = (IWebElement)element;
            //        var select = new SelectElement(ele);
            //        var options = select.Options;

            //        foreach (var option in options)
            //        {
            //            var optionValue = option.GetAttribute(attribName);
            //            optionsItems.Add(optionValue);
            //        }

            //        var requiredVariable = engine.VariableList.Where(x => x.VariableName == applyToVarName).FirstOrDefault();

            //        if (requiredVariable == null)
            //        {
            //            engine.VariableList.Add(new Script.ScriptVariable() { VariableName = applyToVarName, CurrentPosition = 0 });
            //            requiredVariable = engine.VariableList.Where(x => x.VariableName == applyToVarName).FirstOrDefault();
            //        }

            //        requiredVariable.VariableValue = optionsItems;
            //        requiredVariable.CurrentPosition = 0;


            //        break;
            //    case "Select Option":

            //        string selectionType = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                                 where rw.Field<string>("Parameter Name") == "Selection Type"
            //                                 select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);

            //        string selectionParam = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                                where rw.Field<string>("Parameter Name") == "Selection Parameter"
            //                                select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);


            //        seleniumInstance.SwitchTo().ActiveElement();

            //        var el = (IWebElement)element;
            //        var selectionElement = new SelectElement(el);

            //        switch (selectionType)
            //        {
            //            case "Select By Index":
            //                selectionElement.SelectByIndex(int.Parse(selectionParam));
            //                break;
            //            case "Select By Text":
            //                selectionElement.SelectByText(selectionParam);
            //                break;
            //            case "Select By Value":
            //                selectionElement.SelectByValue(selectionParam);
            //                break;
            //            case "Deselect By Index":
            //                selectionElement.DeselectByIndex(int.Parse(selectionParam));
            //                break;
            //            case "Deselect By Text":
            //                selectionElement.DeselectByText(selectionParam);
            //                break;
            //            case "Deselect By Value":
            //                selectionElement.DeselectByValue(selectionParam);
            //                break;
            //            case "Deselect All":
            //                selectionElement.DeselectAll();
            //                break;
            //            default:
            //                throw new NotImplementedException();
            //        }

            //        break;
            //    case "Get Text":
            //    case "Get Attribute":
            //    case "Get Count":

            //        string VariableName = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                               where rw.Field<string>("Parameter Name") == "Variable Name"
            //                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

            //        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                                where rw.Field<string>("Parameter Name") == "Attribute Name"
            //                                select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);

            //        string elementValue;
            //        if (v_SeleniumElementAction == "Get Text")
            //        {
            //            elementValue = element.Text;
            //        }
            //        else if (v_SeleniumElementAction == "Get Count")
            //        {
            //            elementValue = "1";
            //            if (element is ReadOnlyCollection<IWebElement>)
            //                elementValue = ((ReadOnlyCollection<IWebElement>)element).Count().ToString(); 
            //        }
            //        else
            //        {
            //            elementValue = element.GetAttribute(attributeName);
            //        }

            //        elementValue.StoreInUserVariable(sender, VariableName);

            //        break;
            //    case "Get Matching Elements":
            //        var variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
            //                            where rw.Field<string>("Parameter Name") == "Variable Name"
            //                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

            //        var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == variableName).FirstOrDefault();

            //        if (requiredComplexVariable == null)
            //        {
            //            engine.VariableList.Add(new Script.ScriptVariable() { VariableName = variableName, CurrentPosition = 0 });
            //            requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == variableName).FirstOrDefault();
            //        }


            //        //set json settings
            //        JsonSerializerSettings settings = new JsonSerializerSettings();
            //        settings.Error = (serializer, err) => {
            //            err.ErrorContext.Handled = true;
            //        };

            //        settings.Formatting = Formatting.Indented;

            //        //create json list
            //        List<string> jsonList = new List<string>();
            //        foreach (IWebElement item in element)
            //        {
            //            var json = JsonConvert.SerializeObject(item, settings);
            //            jsonList.Add(json);
            //        }

            //        requiredComplexVariable.VariableValue = jsonList;
            //        requiredComplexVariable.CurrentPosition = 0;

            //        break;
            //    case "Clear Element":
            //        element.Clear();
            //        break;

            //    case "Switch to frame":
            //        if (seleniumSearchParam == "")
            //        {
            //            seleniumInstance.SwitchTo().DefaultContent();
            //        }
            //        else
            //        {
            //            seleniumInstance.SwitchTo().Frame(element);
            //        }
            //        break;

            //    default:
            //        throw new Exception("Element Action was not found");
            //}

            var actionType = this.GetUISelectionValue(nameof(v_SeleniumElementAction), engine);
            var parameters = DataTableControls.GetFieldValues(v_WebActionParameterTable, "Parameter Name", "Parameter Value", false, engine);
            switch (actionType)
            {
                case "wait for webelement to exist":
                    var waitCommand = new SeleniumBrowserWaitForWebElementExistCommand()
                    {
                        v_InstanceName = this.v_InstanceName,
                        v_SeleniumSearchType = this.v_SeleniumSearchType,
                        v_SeleniumSearchParameter = this.v_SeleniumSearchParameter,
                        v_ElementIndex = this.v_SeleniumElementIndex,
                        v_WaitTime = this.v_WaitTime,
                    };
                    waitCommand.RunCommand(engine);
                    break;
                case "get matching webelements":
                    var getMatching = new SeleniumBrowserGetMatchedWebElementsCommand()
                    {
                        v_InstanceName = this.v_InstanceName,
                        v_SeleniumSearchType = this.v_SeleniumSearchType,
                        v_SeleniumSearchParameter = this.v_SeleniumSearchParameter,
                        v_WaitTime = this.v_WaitTime,
                        v_Result = parameters["Variable Name"],
                    };
                    getMatching.RunCommand(engine);
                    break;
                case "get webelements count":
                    var getCount = new SeleniumBrowserGetWebElementsCountCommand()
                    {
                        v_InstanceName = this.v_InstanceName,
                        v_SeleniumSearchType = this.v_SeleniumSearchType,
                        v_SeleniumSearchParameter = this.v_SeleniumSearchParameter,
                        v_WaitTime = this.v_WaitTime,
                        v_Result = parameters["Variable Name"],
                    };
                    getCount.RunCommand(engine);
                    break;
                default:
                    var elemVariable = VariableNameControls.GetInnerVariableName(0, engine);
                    var searchElement = new SeleniumBrowserSearchWebElementCommand()
                    {
                        v_InstanceName = this.v_InstanceName,
                        v_SeleniumSearchType = this.v_SeleniumSearchType,
                        v_SeleniumSearchParameter = this.v_SeleniumSearchParameter,
                        v_ElementIndex = this.v_SeleniumElementIndex,
                        v_Result = elemVariable,
                        v_WaitTime = this.v_WaitTime,
                    };
                    searchElement.RunCommand(engine);

                    switch (actionType)
                    {
                        case "click webelement":
                            var clickElement = new SeleniumBrowserClickWebElementCommand()
                            {
                                v_WebElement = elemVariable,
                                v_ClickType = parameters["Click Type"],
                                v_XOffset = parameters["X Offset"],
                                v_YOffset = parameters["Y Offset"],
                                v_ScrollToElement = this.v_ScrollToElement,
                            };
                            clickElement.RunCommand(engine);
                            break;
                        case "clear webelement":
                            var clearElement = new SeleniumBrowserClearTextInWebElementCommand()
                            {
                                v_WebElement = elemVariable,
                            };
                            clearElement.RunCommand(engine);
                            break;
                        case "set text":
                            var setText = new SeleniumBrowserSetTextToWebElementCommand()
                            {
                                v_WebElement = elemVariable,
                                v_TextToSet = parameters["Text To Set"],
                                v_ClearTextBeforeSetting = parameters["Clear Element Before Setting Text"],
                                v_EncryptedText = parameters["Encrypted Text"],
                            };
                            setText.RunCommand(engine);
                            break;
                        case "get text":
                            var getText = new SeleniumBrowserGetTextFromWebElementCommand()
                            {
                                v_WebElement = elemVariable,
                                v_Result = parameters["Variable Name"],
                            };
                            getText.RunCommand(engine);
                            break;
                        case "get attribute":
                            var getAttribute = new SeleniumBrowserGetAttributeFromWebElementCommand()
                            {
                                v_WebElement = elemVariable,
                                v_AttributeName = parameters["Attribute Name"],
                                v_Result = parameters["Variable Name"],
                            };
                            getAttribute.RunCommand(engine);
                            break;
                        case "switch to frame":
                            var switchToFrame = new SeleniumBrowserSwitchFrameToWebElementCommand()
                            {
                                v_WebElement = elemVariable,
                            };
                            switchToFrame.RunCommand(engine);
                            break;
                        case "get options":
                            var getOptions = new SeleniumBrowserGetOptionsFromWebElementCommand()
                            {
                                v_WebElement = elemVariable,
                                v_AttributeName = parameters["Attribute Name"],
                                v_Result = parameters["Variable Name"],
                            };
                            getOptions.RunCommand(engine);
                            break;
                        case "select option":
                            var selectOption = new SeleniumBrowserSelectOptionForWebElementCommand()
                            {
                                v_WebElement = elemVariable,
                                v_SelectionType = parameters["Selection Type"],
                                v_SelectionValue = parameters["Selection Parameter"],
                            };
                            selectOption.RunCommand(engine);
                            break;
                    }
                    break;
            }

        }

        //private object FindElement(IWebDriver seleniumInstance, string searchParameter)
        //{
        //    object element = null;



        //    switch (v_SeleniumSearchType)
        //    {
        //        case "Find Element By XPath":
        //            element = seleniumInstance.FindElement(By.XPath(searchParameter));
        //            break;

        //        case "Find Element By ID":
        //            element = seleniumInstance.FindElement(By.Id(searchParameter));
        //            break;

        //        case "Find Element By Name":
        //            element = seleniumInstance.FindElement(By.Name(searchParameter));
        //            break;

        //        case "Find Element By Tag Name":
        //            element = seleniumInstance.FindElement(By.TagName(searchParameter));
        //            break;

        //        case "Find Element By Class Name":
        //            element = seleniumInstance.FindElement(By.ClassName(searchParameter));
        //            break;
        //        case "Find Element By CSS Selector":
        //            element = seleniumInstance.FindElement(By.CssSelector(searchParameter));
        //            break;

        //        case "Find Element By Link Text":
        //            element = seleniumInstance.FindElement(By.LinkText(searchParameter));
        //            break;

        //        case "Find Elements By XPath":
        //            element = seleniumInstance.FindElements(By.XPath(searchParameter));
        //            break;

        //        case "Find Elements By ID":
        //            element = seleniumInstance.FindElements(By.Id(searchParameter));
        //            break;

        //        case "Find Elements By Name":
        //            element = seleniumInstance.FindElements(By.Name(searchParameter));
        //            break;

        //        case "Find Elements By Tag Name":
        //            element = seleniumInstance.FindElements(By.TagName(searchParameter));
        //            break;

        //        case "Find Elements By Class Name":
        //            element = seleniumInstance.FindElements(By.ClassName(searchParameter));
        //            break;

        //        case "Find Elements By CSS Selector":
        //            element = seleniumInstance.FindElements(By.CssSelector(searchParameter));
        //            break;

        //        case "Find Elements By Link Text":
        //            element = seleniumInstance.FindElements(By.LinkText(searchParameter));
        //            break;

        //        default:
        //            throw new Exception("Element Search Type was not found: " + v_SeleniumSearchType);
        //    }

        //    return element;
        //}

        //public bool ElementExists(object sender, string searchType, string elementName)
        //{
        //    //get engine reference
        //    var engine = (Engine.AutomationEngineInstance)sender;
        //    var seleniumSearchParam = elementName.ConvertToUserVariable(sender);

        //    //get instance name
        //    var vInstance = v_InstanceName.ConvertToUserVariable(engine);

        //    //get stored app object
        //    var browserObject = engine.GetAppInstance(vInstance);

        //    //get selenium instance driver
        //    //var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
        //    var seleniumInstance = (IWebDriver)browserObject;

        //    try
        //    {
        //        //search for element
        //        var element = FindElement(seleniumInstance, seleniumSearchParam);

        //        //element exists
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        //element does not exist
        //        return false;
        //    }

        //}
        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //ElementsGridViewHelper = new DataGridView();
        //    //ElementsGridViewHelper.AllowUserToAddRows = true;
        //    //ElementsGridViewHelper.AllowUserToDeleteRows = true;
        //    //ElementsGridViewHelper.Size = new Size(400, 250);
        //    //ElementsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //    //ElementsGridViewHelper.DataBindings.Add("DataSource", this, "v_WebActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
        //    //ElementsGridViewHelper.AllowUserToAddRows = false;
        //    //ElementsGridViewHelper.AllowUserToDeleteRows = false;
        //    //ElementsGridViewHelper.AllowUserToResizeRows = false;
        //    //ElementsGridViewHelper = CommandControls.CreateDataGridView(this, "v_WebActionParameterTable", false, false);
        //    //ElementsGridViewHelper.CellBeginEdit += ElementsGridViewHelper_CellBeginEdit;
        //    //ElementsGridViewHelper.CellClick += ElementsGridViewHelper_CellClick;

        //    //var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    //UI.CustomControls.CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);
        //    //RenderedControls.AddRange(instanceCtrls);
        //    ////RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SeleniumSearchType", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SeleniumSearchParameter", this, editor));


        //    //ElementActionDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_SeleniumElementAction", this);
        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_SeleniumElementAction", this));
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_SeleniumElementAction", this, new Control[] { ElementActionDropdown }, editor));
        //    //ElementActionDropdown.SelectionChangeCommitted += seleniumAction_SelectionChangeCommitted;

        //    //RenderedControls.Add(ElementActionDropdown);

        //    //ElementParameterControls = new List<Control>();
        //    //ElementParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_WebActionParameterTable", this));
        //    //ElementParameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_WebActionParameterTable", this, new Control[] { ElementsGridViewHelper }, editor));
        //    //ElementParameterControls.Add(ElementsGridViewHelper);

        //    //RenderedControls.AddRange(ElementParameterControls);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    //ElementsGridViewHelper = (DataGridView)ctrls.Where(t => (t.Name == "v_WebActionParameterTable")).FirstOrDefault();
        //    //ElementsGridViewHelper.CellBeginEdit += ElementsGridViewHelper_CellBeginEdit;
        //    //ElementsGridViewHelper.CellClick += ElementsGridViewHelper_CellClick;
        //    //ElementsGridViewHelper = (DataGridView)ctrls.GetControlsByName("v_WebActionParameterTable")[0];
        //    foreach(var ctrl in ctrls)
        //    {
        //        if (ctrl is FlowLayoutPanel flp)
        //        {
        //            foreach(Control c in flp.Controls)
        //            {
        //                if (c.Name == nameof(v_WebActionParameterTable))
        //                {
        //                    ElementsGridViewHelper = (DataGridView)c;
        //                }
        //            }
        //        }
        //        else if (ctrl.Name == nameof(v_WebActionParameterTable))
        //        {
        //            ElementsGridViewHelper = (DataGridView)ctrl;
        //        }
        //    }


        //    //ElementActionDropdown = (ComboBox)ctrls.Where(t => (t.Name == "v_SeleniumElementAction")).FirstOrDefault();
        //    //ElementActionDropdown.SelectionChangeCommitted += seleniumAction_SelectionChangeCommitted;
        //    //ElementActionDropdown = (ComboBox)ctrls.GetControlsByName("v_SeleniumElementAction")[0];
        //    foreach (var ctrl in ctrls)
        //    {
        //        if (ctrl is FlowLayoutPanel flp)
        //        {
        //            foreach (Control c in flp.Controls)
        //            {
        //                if (c.Name == nameof(v_SeleniumElementAction))
        //                {
        //                    ElementActionDropdown = (ComboBox)c;
        //                }
        //            }
        //        }
        //        else if (ctrl.Name == nameof(v_SeleniumElementAction))
        //        {
        //            ElementActionDropdown = (ComboBox)ctrl;
        //        }
        //    }

        //    //seleniumAction_SelectionChangeCommitted(null, null);

        //    //int idx = ctrls.FindIndex(t => (t.Name == "lbl_v_WebActionParameterTable"));
        //    //ElementParameterControls = new List<Control>();
        //    //for (int i = idx; i < ctrls.Count; i++)
        //    //{
        //    //    ElementParameterControls.Add(ctrls[i]);
        //    //}
        //    ElementParameterControls = ctrls.GetControlGroup(nameof(v_WebActionParameterTable));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override void Refresh(UI.Forms.frmCommandEditor editor)
        //{
        //    //seleniumAction_SelectionChangeCommitted(null, null);
        //}

        private void cmbSearchType_SelectionChangeCommited(object sender, EventArgs e)
        {
            var searchType = ((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "";
            GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_SeleniumElementIndex), !searchType.StartsWith("find element "));
        }

        private void cmbSeleniumAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Core.Automation.Commands.SeleniumBrowserElementActionCommand cmd = (Core.Automation.Commands.SeleniumBrowserElementActionCommand)this;
            //DataTable actionParameters = cmd.v_WebActionParameterTable;

            //DataTable actionParameters = this.v_WebActionParameterTable;

            //if (sender != null)
            //{
            //    actionParameters.Rows.Clear();
            //}


            //switch (ElementActionDropdown.SelectedItem)
            //{
            //    case "Invoke Click":
            //    case "Clear Element":

            //        foreach (var ctrl in ElementParameterControls)
            //        {
            //            ctrl.Hide();
            //        }

            //        break;

            //    case "Left Click":
            //    case "Middle Click":
            //    case "Right Click":
            //    case "Double Left Click":
            //        foreach (var ctrl in ElementParameterControls)
            //        {
            //            ctrl.Show();
            //        }
            //        if (sender != null)
            //        {
            //            actionParameters.Rows.Add("X Adjustment", 0);
            //            actionParameters.Rows.Add("Y Adjustment", 0);
            //        }
            //        break;

            //    case "Set Text":
            //        foreach (var ctrl in ElementParameterControls)
            //        {
            //            ctrl.Show();
            //        }
            //        if (sender != null)
            //        {
            //            actionParameters.Rows.Add("Text To Set");
            //            actionParameters.Rows.Add("Clear Element Before Setting Text");
            //            actionParameters.Rows.Add("Encrypted Text");
            //            actionParameters.Rows.Add("Optional - Click to Encrypt 'Text To Set'");

            //            DataGridViewComboBoxCell encryptedBox = new DataGridViewComboBoxCell();
            //            encryptedBox.Items.Add("Not Encrypted");
            //            encryptedBox.Items.Add("Encrypted");
            //            ElementsGridViewHelper.Rows[2].Cells[1] = encryptedBox;
            //            ElementsGridViewHelper.Rows[2].Cells[1].Value = "Not Encrypted";

            //            var buttonCell = new DataGridViewButtonCell();
            //            ElementsGridViewHelper.Rows[3].Cells[1] = buttonCell;
            //            ElementsGridViewHelper.Rows[3].Cells[1].Value = "Encrypt Text";
            //            ElementsGridViewHelper.CellContentClick += ElementsGridViewHelper_CellContentClick;

            //        }

            //        DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
            //        comparisonComboBox.Items.Add("Yes");
            //        comparisonComboBox.Items.Add("No");

            //        //assign cell as a combobox
            //        if (sender != null)
            //        {
            //            ElementsGridViewHelper.Rows[1].Cells[1].Value = "No";
            //        }
            //        ElementsGridViewHelper.Rows[1].Cells[1] = comparisonComboBox;


            //        break;

            //    case "Get Text":
            //    case "Get Matching Elements":
            //    case "Get Count":
            //        foreach (var ctrl in ElementParameterControls)
            //        {
            //            ctrl.Show();
            //        }
            //        if (sender != null)
            //        {
            //            actionParameters.Rows.Add("Variable Name");
            //        }
            //        break;

            //    case "Get Attribute":
            //        foreach (var ctrl in ElementParameterControls)
            //        {
            //            ctrl.Show();
            //        }
            //        if (sender != null)
            //        {
            //            actionParameters.Rows.Add("Attribute Name");
            //            actionParameters.Rows.Add("Variable Name");
            //        }
            //        break;
            //    case "Get Options":
            //        actionParameters.Rows.Add("Attribute Name");
            //        actionParameters.Rows.Add("Variable Name");
            //        break;
            //    case "Select Option":
            //        actionParameters.Rows.Add("Selection Type");
            //        actionParameters.Rows.Add("Selection Parameter");


            //        DataGridViewComboBoxCell selectionTypeBox = new DataGridViewComboBoxCell();
            //        selectionTypeBox.Items.Add("Select By Index");
            //        selectionTypeBox.Items.Add("Select By Text");
            //        selectionTypeBox.Items.Add("Select By Value");
            //        selectionTypeBox.Items.Add("Deselect By Index");
            //        selectionTypeBox.Items.Add("Deselect By Text");
            //        selectionTypeBox.Items.Add("Deselect By Value");
            //        selectionTypeBox.Items.Add("Deselect All");

            //        //assign cell as a combobox
            //        if (sender != null)
            //        {
            //            ElementsGridViewHelper.Rows[0].Cells[1].Value = "Select By Text";
            //        }

            //        ElementsGridViewHelper.Rows[0].Cells[1] = selectionTypeBox;


            //        break;
            //    case "Wait For Element To Exist":
            //        foreach (var ctrl in ElementParameterControls)
            //        {
            //            ctrl.Show();
            //        }
            //        if (sender != null)
            //        {
            //            actionParameters.Rows.Add("Timeout (Seconds)");
            //        }
            //        break;

            //    case "Switch to frame":
            //        foreach (var ctrl in ElementParameterControls)
            //        {
            //            ctrl.Hide();
            //        }
            //        break;

            //    default:
            //        break;
            //}

            //ElementsGridViewHelper.DataSource = v_WebActionParameterTable;

            var dgv = PropertyControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_WebActionParameterTable));

            v_WebActionParameterTable.Clear();

            var actionType = ((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "";
            switch (actionType)
            {
                case "get text":
                case "get webelements count":
                case "get matching webelements":
                    // only variable name
                    v_WebActionParameterTable.Rows.Add("Variable Name");
                    break;

                case "get options":
                case "get attribute":
                    // attribute, variable name
                    v_WebActionParameterTable.Rows.Add("Attribute Name");
                    v_WebActionParameterTable.Rows.Add("Variable Name");
                    break;

                case "set text":
                    v_WebActionParameterTable.Rows.Add("Text To Set");
                    v_WebActionParameterTable.Rows.Add("Clear Element Before Setting Text");
                    v_WebActionParameterTable.Rows.Add("Encrypted Text");
                    break;

                case "select option":
                    v_WebActionParameterTable.Rows.Add("Selection Type");
                    v_WebActionParameterTable.Rows.Add("Selection Parameter");
                    break;

                case "click webelement":
                    v_WebActionParameterTable.Rows.Add("Click Type");
                    v_WebActionParameterTable.Rows.Add("X Offset");
                    v_WebActionParameterTable.Rows.Add("Y Offset");
                    break;

                case "clear webelement":
                case "switch to frame":
                case "wait for webelement to exist":
                default:
                    // no parameters
                    break;
            }
            dgv.DataSource = v_WebActionParameterTable;

            switch (actionType)
            {
                case "set text":
                    var clearBefore = new DataGridViewComboBoxCell();
                    clearBefore.Items.AddRange(new string[] { "", "Yes", "No" });
                    var encrypted = new DataGridViewComboBoxCell();
                    encrypted.Items.AddRange(new string[] { "", "Yes", "No" });
                    dgv.Rows[1].Cells[1].Value = "";
                    dgv.Rows[1].Cells[1] = clearBefore;
                    dgv.Rows[2].Cells[1].Value = "";
                    dgv.Rows[2].Cells[1] = encrypted;
                    break;
                case "select option":
                    var selectionType = new DataGridViewComboBoxCell();
                    selectionType.Items.AddRange(new string[] { 
                        "Select By Index",
                        "Select By Text",
                        "Select By Value",
                        "Deselect By Index",
                        "Deselect By Text",
                        "Deselect By Value",
                        "Deselect All",
                    });
                    dgv.Rows[0].Cells[1].Value = "Select By Value";
                    dgv.Rows[0].Cells[1] = selectionType;
                    break;
                case "click webelement":
                    var clickType = new DataGridViewComboBoxCell();
                    clickType.Items.AddRange(new string[]
                    {
                        "Left Click",
                        "Middle Click",
                        "Right Click",
                        "Left Down",
                        "Middle Down",
                        "Right Down",
                        "Left Up",
                        "Middle Up",
                        "Right Up",
                        "Double Left Click",
                        "None",
                        "Invoke Click",
                    });
                    dgv.Rows[0].Cells[1].Value = "Invoke Click";
                    dgv.Rows[0].Cells[1] = clickType;
                    break;
            }
        }

        //private void ElementsGridViewHelper_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    var targetCell = ElementsGridViewHelper.Rows[e.RowIndex].Cells[e.ColumnIndex];

        //    if (targetCell is DataGridViewButtonCell && targetCell.Value.ToString() == "Encrypt Text")
        //    {
        //        var targetElement = ElementsGridViewHelper.Rows[0].Cells[1];

        //        if (string.IsNullOrEmpty(targetElement.Value.ToString()))
        //            return;

        //        var warning = MessageBox.Show($"Warning! Text should only be encrypted one time and is not reversible in the builder.  Would you like to proceed and convert '{targetElement.Value.ToString()}' to an encrypted value?", "Encryption Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
               
        //        if (warning == DialogResult.Yes)
        //        {
        //            targetElement.Value = EncryptionServices.EncryptString(targetElement.Value.ToString(), "TASKT");
        //            ElementsGridViewHelper.Rows[2].Cells[1].Value = "Encrypted";
        //        }
               
        //    }
        //}

        //private void ElementsGridViewHelper_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        //{
        //    if (e.ColumnIndex == 0)
        //    {
        //        e.Cancel = true;
        //    }
        //}

        //private void ElementsGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if ((ElementsGridViewHelper.Rows.Count == 0) || (e.RowIndex == -1))
        //    {
        //        return;
        //    }
        //    if (e.ColumnIndex >= 0)
        //    {
        //        if (e.ColumnIndex == 1)
        //        {
        //            var targetCell = ElementsGridViewHelper.Rows[e.RowIndex].Cells[1];
        //            if (targetCell is DataGridViewTextBoxCell)
        //            {
        //                ElementsGridViewHelper.BeginEdit(false);
        //            }
        //            else if ((targetCell is DataGridViewComboBoxCell) && (targetCell.Value.ToString() == ""))
        //            {
        //                SendKeys.Send("%{DOWN}");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ElementsGridViewHelper.EndEdit();
        //    }
        //}

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_SeleniumSearchType + " and " + v_SeleniumElementAction + ", Instance Name: '" + v_InstanceName + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SeleniumSearchType))
        //    {
        //        this.validationResult += "Search Method is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SeleniumSearchParameter))
        //    {
        //        this.validationResult += "Search Parameter is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SeleniumElementAction))
        //    {
        //        this.validationResult += "Element Action is empty.\n";
        //        this.IsValid = false;
        //    }
        //    // TODO: DGV validate

        //    return this.IsValid;
        //}
    }
}
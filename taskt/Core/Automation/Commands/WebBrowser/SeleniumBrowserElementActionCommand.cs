using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to manipulate, set, or get data on a webpage within the web browser.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserElementActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name (ex. myInstance, {{{vInstance}}})")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Method")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By XPath")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By ID")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Tag Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Class Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By CSS Selector")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Link Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By XPath")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By ID")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By Tag Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By Class Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By CSS Selector")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By Link Text")]
        [Attributes.PropertyAttributes.InputSpecification("Select the specific search type that you want to use to isolate the element in the web page.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Find Element By XPath**, **Find Element By ID**, **Find Element By Name**, **Find Element By Tag Name**, **Find Element By Class Name**, **Find Element By CSS Selector**, **Find Element By Link Text**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SeleniumSearchType { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Parameter")]
        [Attributes.PropertyAttributes.InputSpecification("Specifies the parameter text that matches to the element based on the previously selected search type.")]
        [Attributes.PropertyAttributes.SampleUsage("If search type **Find Element By ID** was specified, for example, given <div id='name'></div>, the value of this field would be **name**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SeleniumSearchParameter { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Element Action")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Invoke Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Double Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Clear Element")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Set Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Attribute")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Matching Elements")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Wait For Element To Exist")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Switch to frame")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Count")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Options")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Select Option")]
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate corresponding action to take once the element has been located")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Invoke Click**, **Left Click**, **Right Click**, **Middle Click**, **Double Left Click**, **Clear Element**, **Set Text**, **Get Text**, **Get Attribute**, **Wait For Element To Exist**, **Get Count**")]
        [Attributes.PropertyAttributes.Remarks("Selecting this field changes the parameters that will be required in the next step")]
        public string v_SeleniumElementAction { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        [Attributes.PropertyAttributes.InputSpecification("Additioal Parameters will be required based on the action settings selected.")]
        [Attributes.PropertyAttributes.SampleUsage("Additional Parameters range from adding offset coordinates to specifying a variable to apply element text to.")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public System.Data.DataTable v_WebActionParameterTable { get; set; }


        [XmlIgnore]
        [NonSerialized]
        private DataGridView ElementsGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox ElementActionDropdown;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> ElementParameterControls;

        public SeleniumBrowserElementActionCommand()
        {
            this.CommandName = "SeleniumBrowserElementActionCommand";
            this.SelectionName = "Element Action";
            this.v_InstanceName = "";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_WebActionParameterTable = new System.Data.DataTable
            {
                TableName = "WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };
            this.v_WebActionParameterTable.Columns.Add("Parameter Name");
            this.v_WebActionParameterTable.Columns.Add("Parameter Value");
        }

        //private void ElementsGridViewHelper_MouseEnter(object sender, EventArgs e)
        //{
        //    seleniumAction_SelectionChangeCommitted(null, null);
        //}

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            //convert to user variable -- https://github.com/saucepleez/taskt/issues/66
            var seleniumSearchParam = v_SeleniumSearchParameter.ConvertToUserVariable(sender);


            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

            var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;

            dynamic element = null;

            if (v_SeleniumElementAction == "Wait For Element To Exist")
            {

                var timeoutText = (from rw in v_WebActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault();

                timeoutText = timeoutText.ConvertToUserVariable(sender);

                int timeOut = Convert.ToInt32(timeoutText);

                var timeToEnd = DateTime.Now.AddSeconds(timeOut);

                while (timeToEnd >= DateTime.Now)
                {
                    try
                    {
                        element = FindElement(seleniumInstance, seleniumSearchParam);
                        break;
                    }
                    catch (Exception)
                    {
                        engine.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
                        System.Threading.Thread.Sleep(1000);
                    }
                }

                if (element == null)
                {
                    throw new Exception("Element Not Found");
                }

                return;
            }
            else if (seleniumSearchParam != string.Empty)
            {
                element = FindElement(seleniumInstance, seleniumSearchParam);
            }




            switch (v_SeleniumElementAction)
            {
                case "Invoke Click":
                    int seleniumWindowHeightY = seleniumInstance.Manage().Window.Size.Height;
                    int elementPositionY = element.Location.Y;
                    if (elementPositionY > seleniumWindowHeightY)
                    {
                        String scroll = String.Format("window.scroll(0, {0})", elementPositionY);
                        IJavaScriptExecutor js = browserObject as IJavaScriptExecutor;
                        js.ExecuteScript(scroll);
                    }
                    element.Click();
                    break;

                case "Left Click":
                case "Right Click":
                case "Middle Click":
                case "Double Left Click":


                    int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                    int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                    var elementLocation = element.Location;
                    SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand();
                    var seleniumWindowPosition = seleniumInstance.Manage().Window.Position;
                    newMouseMove.v_XMousePosition = (seleniumWindowPosition.X + elementLocation.X + 30 + userXAdjust).ToString(); // added 30 for offset
                    newMouseMove.v_YMousePosition = (seleniumWindowPosition.Y + elementLocation.Y + 130 + userYAdjust).ToString(); //added 130 for offset
                    newMouseMove.v_MouseClick = v_SeleniumElementAction;
                    newMouseMove.RunCommand(sender);
                    break;

                case "Set Text":

                    string textToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Text To Set"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();


                    string clearElement = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Clear Element Before Setting Text"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string encryptedData = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Encrypted Text"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();



                    if (clearElement == null)
                    {
                        clearElement = "No";
                    }

                    if (clearElement.ToLower() == "yes")
                    {
                        element.Clear();
                    }


                    if (encryptedData == "Encrypted")
                    {
                        textToSet = Core.EncryptionServices.DecryptString(textToSet, "TASKT");
                    }
                   
                    string[] potentialKeyPresses = textToSet.Split('{', '}');

                    Type seleniumKeys = typeof(OpenQA.Selenium.Keys);
                    System.Reflection.FieldInfo[] fields = seleniumKeys.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                    //check if chunked string contains a key press command like {ENTER}
                    foreach (string chunkedString in potentialKeyPresses)
                    {
                        if (chunkedString == "")
                            continue;

                        if (fields.Any(f => f.Name == chunkedString))
                        {
                            string keyPress = (string)fields.Where(f => f.Name == chunkedString).FirstOrDefault().GetValue(null);
                            element.SendKeys(keyPress);


                        }
                        else
                        {
                            //convert to user variable - https://github.com/saucepleez/taskt/issues/22
                            var convertedChunk = chunkedString.ConvertToUserVariable(sender);
                            element.SendKeys(convertedChunk);
                        }
                    }

                    break;
                case "Get Options":

                    string applyToVarName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Variable Name"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();


                    string attribName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Attribute Name"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);


                    var optionsItems = new List<string>();
                    var ele = (IWebElement)element;
                    var select = new SelectElement(ele);
                    var options = select.Options;

                    foreach (var option in options)
                    {
                        var optionValue = option.GetAttribute(attribName);
                        optionsItems.Add(optionValue);
                    }
                 
                    var requiredVariable = engine.VariableList.Where(x => x.VariableName == applyToVarName).FirstOrDefault();

                    if (requiredVariable == null)
                    {
                        engine.VariableList.Add(new Script.ScriptVariable() { VariableName = applyToVarName, CurrentPosition = 0 });
                        requiredVariable = engine.VariableList.Where(x => x.VariableName == applyToVarName).FirstOrDefault();
                    }

                    requiredVariable.VariableValue = optionsItems;
                    requiredVariable.CurrentPosition = 0;


                    break;
                case "Select Option":

                    string selectionType = (from rw in v_WebActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Selection Type"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);

                    string selectionParam = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Selection Parameter"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);


                    seleniumInstance.SwitchTo().ActiveElement();

                    var el = (IWebElement)element;
                    var selectionElement = new SelectElement(el);
                
                    switch (selectionType)
                    {
                        case "Select By Index":
                            selectionElement.SelectByIndex(int.Parse(selectionParam));
                            break;
                        case "Select By Text":
                            selectionElement.SelectByText(selectionParam);
                            break;
                        case "Select By Value":
                            selectionElement.SelectByValue(selectionParam);
                            break;
                        case "Deselect By Index":
                            selectionElement.DeselectByIndex(int.Parse(selectionParam));
                            break;
                        case "Deselect By Text":
                            selectionElement.DeselectByText(selectionParam);
                            break;
                        case "Deselect By Value":
                            selectionElement.DeselectByValue(selectionParam);
                            break;
                        case "Deselect All":
                            selectionElement.DeselectAll();
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    break;
                case "Get Text":
                case "Get Attribute":
                case "Get Count":

                    string VariableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Variable Name"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Attribute Name"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);

                    string elementValue;
                    if (v_SeleniumElementAction == "Get Text")
                    {
                        elementValue = element.Text;
                    }
                    else if (v_SeleniumElementAction == "Get Count")
                    {
                        elementValue = "1";
                        if (element is ReadOnlyCollection<IWebElement>)
                            elementValue = ((ReadOnlyCollection<IWebElement>)element).Count().ToString(); 
                    }
                    else
                    {
                        elementValue = element.GetAttribute(attributeName);
                    }

                    elementValue.StoreInUserVariable(sender, VariableName);

                    break;
                case "Get Matching Elements":
                    var variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Variable Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == variableName).FirstOrDefault();

                    if (requiredComplexVariable == null)
                    {
                        engine.VariableList.Add(new Script.ScriptVariable() { VariableName = variableName, CurrentPosition = 0 });
                        requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == variableName).FirstOrDefault();
                    }


                    //set json settings
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Error = (serializer, err) => {
                        err.ErrorContext.Handled = true;
                    };

                    settings.Formatting = Formatting.Indented;

                    //create json list
                    List<string> jsonList = new List<string>();
                    foreach (OpenQA.Selenium.IWebElement item in element)
                    {
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(item, settings);
                        jsonList.Add(json);
                    }

                    requiredComplexVariable.VariableValue = jsonList;
                    requiredComplexVariable.CurrentPosition = 0;

                    break;
                case "Clear Element":
                    element.Clear();
                    break;

                case "Switch to frame":
                    if (seleniumSearchParam == "")
                    {
                        seleniumInstance.SwitchTo().DefaultContent();
                    }
                    else
                    {
                        seleniumInstance.SwitchTo().Frame(element);
                    }
                    break;

                default:
                    throw new Exception("Element Action was not found");
            }


        }

        private object FindElement(OpenQA.Selenium.IWebDriver seleniumInstance, string searchParameter)
        {
            object element = null;



            switch (v_SeleniumSearchType)
            {
                case "Find Element By XPath":
                    element = seleniumInstance.FindElement(By.XPath(searchParameter));
                    break;

                case "Find Element By ID":
                    element = seleniumInstance.FindElement(By.Id(searchParameter));
                    break;

                case "Find Element By Name":
                    element = seleniumInstance.FindElement(By.Name(searchParameter));
                    break;

                case "Find Element By Tag Name":
                    element = seleniumInstance.FindElement(By.TagName(searchParameter));
                    break;

                case "Find Element By Class Name":
                    element = seleniumInstance.FindElement(By.ClassName(searchParameter));
                    break;
                case "Find Element By CSS Selector":
                    element = seleniumInstance.FindElement(By.CssSelector(searchParameter));
                    break;

                case "Find Element By Link Text":
                    element = seleniumInstance.FindElement(By.LinkText(searchParameter));
                    break;

                case "Find Elements By XPath":
                    element = seleniumInstance.FindElements(By.XPath(searchParameter));
                    break;

                case "Find Elements By ID":
                    element = seleniumInstance.FindElements(By.Id(searchParameter));
                    break;

                case "Find Elements By Name":
                    element = seleniumInstance.FindElements(By.Name(searchParameter));
                    break;

                case "Find Elements By Tag Name":
                    element = seleniumInstance.FindElements(By.TagName(searchParameter));
                    break;

                case "Find Elements By Class Name":
                    element = seleniumInstance.FindElements(By.ClassName(searchParameter));
                    break;

                case "Find Elements By CSS Selector":
                    element = seleniumInstance.FindElements(By.CssSelector(searchParameter));
                    break;

                case "Find Elements By Link Text":
                    element = seleniumInstance.FindElements(By.LinkText(searchParameter));
                    break;

                default:
                    throw new Exception("Element Search Type was not found: " + v_SeleniumSearchType);
            }

            return element;
        }

        public bool ElementExists(object sender, string searchType, string elementName)
        {
            //get engine reference
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var seleniumSearchParam = elementName.ConvertToUserVariable(sender);

            //get instance name
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            //get stored app object
            var browserObject = engine.GetAppInstance(vInstance);

            //get selenium instance driver
            //var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
            var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;

            try
            {
                //search for element
                var element = FindElement(seleniumInstance, seleniumSearchParam);

                //element exists
                return true;
            }
            catch (Exception)
            {
                //element does not exist
                return false;
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //ElementsGridViewHelper = new DataGridView();
            //ElementsGridViewHelper.AllowUserToAddRows = true;
            //ElementsGridViewHelper.AllowUserToDeleteRows = true;
            //ElementsGridViewHelper.Size = new Size(400, 250);
            //ElementsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //ElementsGridViewHelper.DataBindings.Add("DataSource", this, "v_WebActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            //ElementsGridViewHelper.AllowUserToAddRows = false;
            //ElementsGridViewHelper.AllowUserToDeleteRows = false;
            //ElementsGridViewHelper.AllowUserToResizeRows = false;
            ElementsGridViewHelper = CommandControls.CreateDataGridView(this, "v_WebActionParameterTable", false, false);
            ElementsGridViewHelper.CellBeginEdit += ElementsGridViewHelper_CellBeginEdit;
            ElementsGridViewHelper.CellClick += ElementsGridViewHelper_CellClick;

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SeleniumSearchType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SeleniumSearchParameter", this, editor));


            ElementActionDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_SeleniumElementAction", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_SeleniumElementAction", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_SeleniumElementAction", this, new Control[] { ElementActionDropdown }, editor));
            ElementActionDropdown.SelectionChangeCommitted += seleniumAction_SelectionChangeCommitted;

            RenderedControls.Add(ElementActionDropdown);

            ElementParameterControls = new List<Control>();
            ElementParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_WebActionParameterTable", this));
            ElementParameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_WebActionParameterTable", this, new Control[] { ElementsGridViewHelper }, editor));
            ElementParameterControls.Add(ElementsGridViewHelper);

            RenderedControls.AddRange(ElementParameterControls);

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
            }

            seleniumAction_SelectionChangeCommitted(null, null);

            return RenderedControls;
        }



        //public override void Refresh(UI.Forms.frmCommandEditor editor)
        //{
        //    //seleniumAction_SelectionChangeCommitted(null, null);
        //}

        public void seleniumAction_SelectionChangeCommitted(object sender, EventArgs e)
        {

            Core.Automation.Commands.SeleniumBrowserElementActionCommand cmd = (Core.Automation.Commands.SeleniumBrowserElementActionCommand)this;
            DataTable actionParameters = cmd.v_WebActionParameterTable;

            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }


            switch (ElementActionDropdown.SelectedItem)
            {
                case "Invoke Click":
                case "Clear Element":

                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Hide();
                    }

                    break;

                case "Left Click":
                case "Middle Click":
                case "Right Click":
                case "Double Left Click":
                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("X Adjustment", 0);
                        actionParameters.Rows.Add("Y Adjustment", 0);
                    }
                    break;

                case "Set Text":
                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Text To Set");
                        actionParameters.Rows.Add("Clear Element Before Setting Text");
                        actionParameters.Rows.Add("Encrypted Text");
                        actionParameters.Rows.Add("Optional - Click to Encrypt 'Text To Set'");

                        DataGridViewComboBoxCell encryptedBox = new DataGridViewComboBoxCell();
                        encryptedBox.Items.Add("Not Encrypted");
                        encryptedBox.Items.Add("Encrypted");
                        ElementsGridViewHelper.Rows[2].Cells[1] = encryptedBox;
                        ElementsGridViewHelper.Rows[2].Cells[1].Value = "Not Encrypted";

                        var buttonCell = new DataGridViewButtonCell();
                        ElementsGridViewHelper.Rows[3].Cells[1] = buttonCell;
                        ElementsGridViewHelper.Rows[3].Cells[1].Value = "Encrypt Text";
                        ElementsGridViewHelper.CellContentClick += ElementsGridViewHelper_CellContentClick;

                    }

                    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("Yes");
                    comparisonComboBox.Items.Add("No");

                    //assign cell as a combobox
                    if (sender != null)
                    {
                        ElementsGridViewHelper.Rows[1].Cells[1].Value = "No";
                    }
                    ElementsGridViewHelper.Rows[1].Cells[1] = comparisonComboBox;


                    break;

                case "Get Text":
                case "Get Matching Elements":
                case "Get Count":
                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name");
                    }
                    break;

                case "Get Attribute":
                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Attribute Name");
                        actionParameters.Rows.Add("Variable Name");
                    }
                    break;
                case "Get Options":
                    actionParameters.Rows.Add("Attribute Name");
                    actionParameters.Rows.Add("Variable Name");
                    break;
                case "Select Option":
                    actionParameters.Rows.Add("Selection Type");
                    actionParameters.Rows.Add("Selection Parameter");


                    DataGridViewComboBoxCell selectionTypeBox = new DataGridViewComboBoxCell();
                    selectionTypeBox.Items.Add("Select By Index");
                    selectionTypeBox.Items.Add("Select By Text");
                    selectionTypeBox.Items.Add("Select By Value");
                    selectionTypeBox.Items.Add("Deselect By Index");
                    selectionTypeBox.Items.Add("Deselect By Text");
                    selectionTypeBox.Items.Add("Deselect By Value");
                    selectionTypeBox.Items.Add("Deselect All");

                    //assign cell as a combobox
                    if (sender != null)
                    {
                        ElementsGridViewHelper.Rows[0].Cells[1].Value = "Select By Text";
                    }

                    ElementsGridViewHelper.Rows[0].Cells[1] = selectionTypeBox;


                    break;
                case "Wait For Element To Exist":
                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Timeout (Seconds)");
                    }
                    break;

                case "Switch to frame":
                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Hide();
                    }
                    break;

                default:
                    break;
            }

            ElementsGridViewHelper.DataSource = v_WebActionParameterTable;
        }

        private void ElementsGridViewHelper_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var targetCell = ElementsGridViewHelper.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (targetCell is DataGridViewButtonCell && targetCell.Value.ToString() == "Encrypt Text")
            {
                var targetElement = ElementsGridViewHelper.Rows[0].Cells[1];

                if (string.IsNullOrEmpty(targetElement.Value.ToString()))
                    return;

                var warning = MessageBox.Show($"Warning! Text should only be encrypted one time and is not reversible in the builder.  Would you like to proceed and convert '{targetElement.Value.ToString()}' to an encrypted value?", "Encryption Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
               
                if (warning == DialogResult.Yes)
                {
                    targetElement.Value = EncryptionServices.EncryptString(targetElement.Value.ToString(), "TASKT");
                    ElementsGridViewHelper.Rows[2].Cells[1].Value = "Encrypted";
                }
               
            }
        }

        private void ElementsGridViewHelper_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Cancel = true;
            }
        }

        private void ElementsGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 1)
                {
                    var targetCell = ElementsGridViewHelper.Rows[e.RowIndex].Cells[1];
                    if (targetCell is DataGridViewTextBoxCell)
                    {
                        ElementsGridViewHelper.BeginEdit(false);
                    }
                    else if ((targetCell is DataGridViewComboBoxCell) && (targetCell.Value.ToString() == ""))
                    {
                        SendKeys.Send("%{DOWN}");
                    }
                }
            }
            else
            {
                ElementsGridViewHelper.EndEdit();
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_SeleniumSearchType + " and " + v_SeleniumElementAction + ", Instance Name: '" + v_InstanceName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SeleniumSearchType))
            {
                this.validationResult += "Search Method is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SeleniumSearchParameter))
            {
                this.validationResult += "Search Parameter is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SeleniumElementAction))
            {
                this.validationResult += "Element Action is empty.\n";
                this.IsValid = false;
            }
            // TODO: DGV validate

            return this.IsValid;
        }
    }
}
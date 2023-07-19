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
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        [PropertySelectionChangeEvent(nameof(cmbSearchType_SelectionChangeCommited))]
        public string v_SeleniumSearchType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        public string v_SeleniumSearchParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ElementIndex))]
        public string v_SeleniumElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Element Action")]
        [PropertyUISelectionOption("Click WebElement")]
        [PropertyUISelectionOption("Clear WebElement")]
        [PropertyUISelectionOption("Set Text")]
        [PropertyUISelectionOption("Get Text")]
        [PropertyUISelectionOption("Get Attribute")]
        [PropertyUISelectionOption("Get Matching WebElements")]
        [PropertyUISelectionOption("Wait For WebElement To Exists")]
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
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        public DataTable v_WebActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        public string v_ScrollToElement { get; set; }

        public SeleniumBrowserWebElementActionCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var actionType = this.GetUISelectionValue(nameof(v_SeleniumElementAction), engine);
            var parameters = DataTableControls.GetFieldValues(v_WebActionParameterTable, "Parameter Name", "Parameter Value", false, engine);
            switch (actionType)
            {
                case "wait for webelement to exists":
                    var waitCommand = new SeleniumBrowserWaitForWebElementToExistsCommand()
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

        public override void AfterShown()
        {
            var cmb = PropertyControls.GetPropertyControl<ComboBox>(ControlsList, nameof(v_SeleniumElementAction));
            var dgv = PropertyControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_WebActionParameterTable));
            actionParameterProcess(dgv, cmb.SelectedItem?.ToString() ?? "");
        }

        private void cmbSearchType_SelectionChangeCommited(object sender, EventArgs e)
        {
            var searchType = ((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "";
            GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_SeleniumElementIndex), !searchType.StartsWith("find element "));
        }

        private void cmbSeleniumAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var dgv = PropertyControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_WebActionParameterTable));

            //actionParameterProcess(dgv, v_WebActionParameterTable, (ComboBox)sender);

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
                    v_WebActionParameterTable.Rows.Add(new string[] { "Selection Type", "Select By Value" });
                    v_WebActionParameterTable.Rows.Add("Selection Parameter");
                    break;

                case "click webelement":
                    v_WebActionParameterTable.Rows.Add(new string[] { "Click Type", "Invoke Click" });
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

            actionParameterProcess(dgv, actionType);

            //switch (actionType)
            //{
            //    case "set text":
            //        var clearBefore = new DataGridViewComboBoxCell();
            //        clearBefore.Items.AddRange(new string[] { "", "Yes", "No" });
            //        var encrypted = new DataGridViewComboBoxCell();
            //        encrypted.Items.AddRange(new string[] { "", "Yes", "No" });
            //        //dgv.Rows[1].Cells[1].Value = "";
            //        dgv.Rows[1].Cells[1] = clearBefore;
            //        //dgv.Rows[2].Cells[1].Value = "";
            //        dgv.Rows[2].Cells[1] = encrypted;
            //        break;
            //    case "select option":
            //        var selectionType = new DataGridViewComboBoxCell();
            //        selectionType.Items.AddRange(new string[] { 
            //            "Select By Index",
            //            "Select By Text",
            //            "Select By Value",
            //            "Deselect By Index",
            //            "Deselect By Text",
            //            "Deselect By Value",
            //            "Deselect All",
            //        });
            //        //dgv.Rows[0].Cells[1].Value = "Select By Value";
            //        dgv.Rows[0].Cells[1] = selectionType;
            //        break;
            //    case "click webelement":
            //        var clickType = new DataGridViewComboBoxCell();
            //        clickType.Items.AddRange(new string[]
            //        {
            //            "Left Click",
            //            "Middle Click",
            //            "Right Click",
            //            "Left Down",
            //            "Middle Down",
            //            "Right Down",
            //            "Left Up",
            //            "Middle Up",
            //            "Right Up",
            //            "Double Left Click",
            //            "None",
            //            "Invoke Click",
            //        });
            //        //dgv.Rows[0].Cells[1].Value = "Invoke Click";
            //        dgv.Rows[0].Cells[1] = clickType;
            //        break;
            //}
        }

        private static void actionParameterProcess(DataGridView dgv, string actionType)
        {
            switch (actionType.ToLower())
            {
                case "set text":
                    var clearBefore = new DataGridViewComboBoxCell();
                    clearBefore.Items.AddRange(new string[] { "", "Yes", "No" });
                    var encrypted = new DataGridViewComboBoxCell();
                    encrypted.Items.AddRange(new string[] { "", "Yes", "No" });
                    dgv.Rows[1].Cells[1] = clearBefore;
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
                    dgv.Rows[0].Cells[1] = clickType;
                    break;
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_SeleniumSearchType + " and " + v_SeleniumElementAction + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
}
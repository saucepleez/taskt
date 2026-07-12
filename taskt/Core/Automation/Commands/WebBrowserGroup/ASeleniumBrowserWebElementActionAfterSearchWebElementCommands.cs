using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for WebElement Action After Search WebElement commands
    /// </summary>
    public abstract class ASeleniumBrowserWebElementActionAfterSearchWebElementCommands : ScriptCommand, ISeleniumSearchWebElementParametersProperties, IWhenFailActionBehaviorProperties, ISeleniumSendSpecialKeystrokesProperties, IHaveDataTableElements
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchMethod))]
        [PropertySelectionChangeEvent(nameof(cmbSearchType_SelectionChangeCommited))]
        [PropertyParameterOrder(6000)]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchParameter))]
        [PropertyParameterOrder(6100)]
        public string v_SearchParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SelectionMethod))]
        [PropertyParameterOrder(6200)]
        public string v_SelectionMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WebElementIndex))]
        [PropertyParameterOrder(6300)]
        public string v_WebElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("WebElement Action")]
        [PropertyUISelectionOption("Click WebElement")]
        [PropertyUISelectionOption("Clear Text")]
        [PropertyUISelectionOption("Set Text")]
        [PropertyUISelectionOption("Get Text")]
        [PropertyUISelectionOption("Get Options")]
        [PropertyUISelectionOption("Select Option")]
        [PropertyUISelectionOption("Get Attribute")]
        [PropertyUISelectionOption("Send Special Keystrokes")]
        [PropertyUISelectionOption("Remove WebElement")]
        [PropertyUISelectionOption("Wait For WebElement To Exists")]
        [PropertyUISelectionOption("Switch To Frame")]
        [PropertyUISelectionOption("Get Matching WebElements HTML As List")]
        [PropertyUISelectionOption("Get WebElements Count")]
        [PropertyUISelectionOption("Get WebElement Position")]
        [PropertyUISelectionOption("Get WebElement Size")]
        [PropertyUISelectionOption("Get CSS Selector")]
        [PropertyUISelectionOption("Get XPath")]
        [PropertyUISelectionOption("Get HTML")]
        [PropertyUISelectionOption("Get Special Value")]
        [PropertySelectionChangeEvent(nameof(cmbSeleniumAction_SelectionChangeCommitted))]
        [PropertyValidationRule("WebElement Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyParameterOrder(7000)]
        public string v_WebElementAction { get; set; }

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
        [PropertyParameterOrder(8000)]
        public DataTable v_WebActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WaitTimeForWebElement))]
        [PropertyParameterOrder(10000)]
        public string v_WaitTimeForWebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WhenFailAction))]
        [PropertyParameterOrder(11000)]
        public virtual string v_WhenFailAction { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_ScrollToWebElement))]
        [PropertyParameterOrder(12000)]
        public string v_ScrollToWebElement { get; set; }

        /// <summary>
        /// search and action process
        /// </summary>
        /// <param name="targetName">WebBrowser Instance name of WebElement variable name</param>
        /// <param name="instanceBase">true is WebBrower base</param>
        /// <param name="engine"></param>
        protected void SearchAndActionProcess(string targetName, bool instanceBase, Engine.AutomationEngineInstance engine)
        {
            var actionType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WebElementAction), engine);
            var parameters = DataTableControls.GetFieldValues(v_WebActionParameterTable, "Parameter Name", "Parameter Value", false, engine);
            switch (actionType)
            {
                case "wait for webelement to exists":
                    if (instanceBase)
                    {
                        var waitCommand = new SeleniumBrowserWaitForWebElementToExistsCommand()
                        {
                            v_InstanceName = targetName,
                            v_SearchMethod = this.v_SearchMethod,
                            v_SearchParameter = this.v_SearchParameter,
                            v_SelectionMethod = this.v_SelectionMethod,
                            v_WebElementIndex = this.v_WebElementIndex,
                            v_WaitTimeForWebElement = this.v_WaitTimeForWebElement,
                        };
                        waitCommand.RunCommand(engine);
                    }
                    else
                    {
                        var waitCommand = new SeleniumBrowserWaitForWebElementToExistsFromWebElementCommand()
                        {
                            v_WebElement = targetName,
                            v_SearchMethod = this.v_SearchMethod,
                            v_SearchParameter = this.v_SearchParameter,
                            v_SelectionMethod = this.v_SelectionMethod,
                            v_WebElementIndex = this.v_WebElementIndex,
                            v_WaitTimeForWebElement = this.v_WaitTimeForWebElement,
                        };
                        waitCommand.RunCommand(engine);
                    }
                    break;

                case "get matching webelements html as list":
                    if (instanceBase)
                    {
                        var getMatching = new SeleniumBrowserGetMatchedWebElementsHTMLAsListCommand()
                        {
                            v_InstanceName = targetName,
                            v_SearchMethod = this.v_SearchMethod,
                            v_SearchParameter = this.v_SearchParameter,
                            v_WaitTimeForWebElement = this.v_WaitTimeForWebElement,
                            v_Result = parameters["Variable Name"],
                        };
                        getMatching.RunCommand(engine);
                    }
                    else
                    {
                        var getMatching = new SeleniumBrowserGetMatchedWebElementsHTMLAsListFromWebElementCommand()
                        {
                            v_WebElement = targetName,
                            v_SearchMethod = this.v_SearchMethod,
                            v_SearchParameter = this.v_SearchParameter,
                            v_WaitTimeForWebElement = this.v_WaitTimeForWebElement,
                            v_Result = parameters["Variable Name"],
                        };
                        getMatching.RunCommand(engine);
                    }
                    break;

                case "get webelements count":
                    if (instanceBase)
                    {
                        var getCount = new SeleniumBrowserGetWebElementsCountCommand()
                        {
                            v_InstanceName = targetName,
                            v_SearchMethod = this.v_SearchMethod,
                            v_SearchParameter = this.v_SearchParameter,
                            v_WaitTimeForWebElement = this.v_WaitTimeForWebElement,
                            v_Result = parameters["Variable Name"],
                        };
                        getCount.RunCommand(engine);
                    }
                    else
                    {
                        var getCount = new SeleniumBrowserGetWebElementsCountFromWebElementCommand()
                        {
                            v_WebElement = targetName,
                            v_SearchMethod = this.v_SearchMethod,
                            v_SearchParameter = this.v_SearchParameter,
                            v_WaitTimeForWebElement = this.v_WaitTimeForWebElement,
                            v_Result = parameters["Variable Name"],
                        };
                        getCount.RunCommand(engine);
                    }
                    break;

                default:
                    using (var myWebElem = new InnerScriptVariable(engine))
                    {
                        if (instanceBase)
                        {
                            var searchElement = new SeleniumBrowserSearchWebElementCommand()
                            {
                                v_InstanceName = targetName,
                                v_SearchMethod = this.v_SearchMethod,
                                v_SearchParameter = this.v_SearchParameter,
                                v_SelectionMethod = this.v_SelectionMethod,
                                v_WebElementIndex = this.v_WebElementIndex,
                                v_Result = myWebElem.VariableName,
                                v_WaitTimeForWebElement = this.v_WaitTimeForWebElement,
                            };
                            searchElement.RunCommand(engine);
                        }
                        else
                        {
                            var searchElement = new SeleniumBrowserSearchWebElementFromWebElementCommand()
                            {
                                v_WebElement = targetName,
                                v_SearchMethod = this.v_SearchMethod,
                                v_SearchParameter = this.v_SearchParameter,
                                v_SelectionMethod = this.v_SelectionMethod,
                                v_WebElementIndex = this.v_WebElementIndex,
                                v_Result = myWebElem.VariableName,
                                v_WaitTimeForWebElement = this.v_WaitTimeForWebElement,
                            };
                            searchElement.RunCommand(engine);
                        }

                        switch (actionType)
                        {
                            case "click webelement":
                                var clickElement = new SeleniumBrowserClickWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_ClickType = parameters["Click Type"],
                                    v_XOffset = parameters["X Offset"],
                                    v_YOffset = parameters["Y Offset"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                clickElement.RunCommand(engine);
                                break;
                            case "clear text":
                                var clearElement = new SeleniumBrowserClearTextInWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                clearElement.RunCommand(engine);
                                break;
                            case "set text":
                                var setText = new SeleniumBrowserSetTextToWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_TextToSet = parameters["Text To Set"],
                                    v_ClearTextBeforeSetting = parameters["Clear Element Before Setting Text"],
                                    v_EncryptedText = parameters["Encrypted Text"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                setText.RunCommand(engine);
                                break;
                            case "get text":
                                var getText = new SeleniumBrowserGetTextFromWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_Result = parameters["Variable Name"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                getText.RunCommand(engine);
                                break;
                            case "get options":
                                var getOptions = new SeleniumBrowserGetOptionsFromWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_AttributeName = parameters["Attribute Name"],
                                    v_Result = parameters["Variable Name"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                getOptions.RunCommand(engine);
                                break;
                            case "select option":
                                var selectOption = new SeleniumBrowserSelectOptionForWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_SelectionType = parameters["Selection Type"],
                                    v_SelectionValue = parameters["Selection Parameter"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                selectOption.RunCommand(engine);
                                break;
                            case "get attribute":
                                var getAttribute = new SeleniumBrowserGetAttributeFromWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_AttributeName = parameters["Attribute Name"],
                                    v_Result = parameters["Variable Name"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                getAttribute.RunCommand(engine);
                                break;
                            case "send special keystrokes":
                                var sendKeys = new SeleniumBrowserSendSpecialKeystrokesToWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_SendKey = parameters["Send Key"],
                                    v_ControlKey = parameters["Use Control Key"],
                                    v_ShiftKey = parameters["Use Shift Key"],
                                    v_AltKey = parameters["Use Alt Key"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                sendKeys.RunCommand(engine);
                                break;
                            case "remove webelement":
                                var removeElem = new SeleniumBrowserRemoveWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                removeElem.RunCommand(engine);
                                break;
                            case "switch to frame":
                                var switchToFrame = new SeleniumBrowserSwitchToFrameWebElementCommand()
                                {
                                    //v_InstanceName = this.v_InstanceName,
                                    v_WebElement = myWebElem.VariableName,
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                switchToFrame.RunCommand(engine);
                                break;
                            case "get webelement position":
                                var getPos = new SeleniumBrowserGetWebElementPositionCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_XPosition = parameters["X Variable"],
                                    v_YPosition = parameters["Y Variable"],
                                    v_PositionBase = parameters["Base Position"],
                                    v_PositionType = parameters["Position Type"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                getPos.RunCommand(engine);
                                break;
                            case "get webelement size":
                                var getSize = new SeleniumBrowserGetWebElementSizeCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_Width = parameters["Width Variable"],
                                    v_Height = parameters["Height Variable"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                getSize.RunCommand(engine);
                                break;
                            case "get css selector":
                                var getCSSSel = new SeleniumBrowserGetCSSSelectorFromWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_Result = parameters["Variable Name"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                getCSSSel.RunCommand(engine);
                                break;
                            case "get xpath":
                                var getXPath = new SeleniumBrowserGetXPathFromWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_Result = parameters["Variable Name"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                getXPath.RunCommand(engine);
                                break;
                            case "get html":
                                var getHTML = new SeleniumBrowserGetHTMLFromWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_Result = parameters["Variable Name"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                getHTML.RunCommand(engine);
                                break;
                            case "get special value":
                                var getSpecial = new SeleniumBrowserGetSpecialValueFromWebElementCommand()
                                {
                                    v_WebElement = myWebElem.VariableName,
                                    v_ValueType = parameters["Value Type"],
                                    v_Result = parameters["Variable Name"],
                                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                                };
                                getSpecial.RunCommand(engine);
                                break;
                        }
                    }
                    break;
            }
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var cmb = FormUIControls.GetPropertyControl<ComboBox>(ControlsList, nameof(v_WebElementAction));
            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_WebActionParameterTable));
            ActionParameterProcess(dgv, cmb.SelectedItem?.ToString() ?? "");
        }

        protected void cmbSearchType_SelectionChangeCommited(object sender, EventArgs e)
        {
            var searchType = ((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "";
            FormUIControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_WebElementIndex), !searchType.StartsWith("find element "));
        }

        protected void cmbSeleniumAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_WebActionParameterTable));

            v_WebActionParameterTable.Clear();

            var actionType = ((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "";
            switch (actionType)
            {
                case "get text":
                case "get webelements count":
                case "get matching webelements html as list":
                case "get css selector":
                case "get xpath":
                case "get html":
                    // only variable name
                    AddDataTableRows(v_WebActionParameterTable, new string[] { "Variable Name" });
                    break;

                case "get options":
                case "get attribute":
                    // attribute, variable name
                    AddDataTableRows(v_WebActionParameterTable, new string[]
                    {
                        "Attribute Name",
                        "Variable Name",
                    });
                    break;

                case "set text":
                    AddDataTableRows(v_WebActionParameterTable, new string[]
                    {
                        "Text To Set",
                        "Clear Element Before Setting Text",
                        "Encrypted Text",
                    });
                    break;

                case "select option":
                    AddDataTableRows(v_WebActionParameterTable, new Dictionary<string, string>()
                    {
                        { "Selection Type", "Select By Value" },
                        { "Selection Parameter", string.Empty },
                    });
                    break;

                case "send special keystrokes":
                    AddDataTableRows(v_WebActionParameterTable, new string[]
                    {
                        "Send Key",
                        "Use Control Key",
                        "Use Shift Key",
                        "Use Alt Key",
                    });
                    break;

                case "click webelement":
                    AddDataTableRows(v_WebActionParameterTable, new Dictionary<string, string>()
                    {
                        { "Click Type", "Invoke Click" },
                        { "X Offset", string.Empty },
                        { "Y Offset", string.Empty },
                    });
                    break;

                case "get webelement position":
                    AddDataTableRows(v_WebActionParameterTable, new string[]
                    {
                        "X Variable",
                        "Y Variable",
                        "Base Position",
                        "Position Type",
                    });
                    break;

                case "get webelement size":
                    AddDataTableRows(v_WebActionParameterTable, new string[]
                    {
                        "Width Variable",
                        "Height Variable",
                    });
                    break;

                case "get special value":
                    AddDataTableRows(v_WebActionParameterTable, new string[]
                    {
                        "Value Type",
                        "Variable Name",
                    });
                    break;

                case "clear webelement":
                case "switch to frame":
                case "remove webelement":
                case "wait for webelement to exist":
                default:
                    // no parameters
                    break;
            }
            dgv.DataSource = v_WebActionParameterTable;

            ActionParameterProcess(dgv, actionType);
        }

        /// <summary>
        /// add rows in ActionParameters
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rows"></param>
        private static void AddDataTableRows(DataTable table, Dictionary<string, string> rows)
        {
            foreach (var kv in rows)
            {
                table.Rows.Add(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// add rows in ActionParameters
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rows"></param>
        private static void AddDataTableRows(DataTable table, string[] rows)
        {
            var dic = new Dictionary<string, string>();
            foreach (var item in rows)
            {
                dic.Add(item, string.Empty);
            }
            AddDataTableRows(table, dic);
        }

        /// <summary>
        /// add action parameters to DGV process
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="actionType"></param>
        private static void ActionParameterProcess(DataGridView dgv, string actionType)
        {
            switch (actionType.ToLower())
            {
                case "set text":
                    {
                        var yn = new string[]
                        {
                            "",
                            "Yes",
                            "No",
                        };
                        dgv.Rows[1].Cells[1] = CreateComboBox(yn);
                        dgv.Rows[2].Cells[1] = CreateComboBox(yn);
                    }
                    break;

                case "select option":
                    dgv.Rows[0].Cells[1] = CreateComboBox(new string[]
                    {
                        "Select By Index",
                        "Select By Text",
                        "Select By Value",
                        "Deselect By Index",
                        "Deselect By Text",
                        "Deselect By Value",
                        "Deselect All",
                    });
                    break;

                case "send special keystrokes":
                    {
                        var yn = new string[]
                        {
                            "",
                            "Yes",
                            "No",
                        };
                        dgv.Rows[0].Cells[1] = CreateComboBox(
                            EM_SeleniumSendSpecialKeystrokesPropetiesExtensionMethods.GetSpecialKeysList(
                                new SeleniumBrowserSendSpecialKeystrokesToWebElementCommand()).ToArray()
                        );
                        dgv.Rows[1].Cells[1] = CreateComboBox(yn);
                        dgv.Rows[2].Cells[1] = CreateComboBox(yn);
                        dgv.Rows[3].Cells[1] = CreateComboBox(yn);
                    }
                    break;

                case "click webelement":
                    dgv.Rows[0].Cells[1] = CreateComboBox(new string[]
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
                    break;

                case "get webelement position":
                    dgv.Rows[2].Cells[1] = CreateComboBox(new string[]
                    {
                        "Top Left",
                        "Bottom Right",
                        "Top Right",
                        "Bottom Left",
                        "Center",
                    });
                    dgv.Rows[3].Cells[1] = CreateComboBox(new string[]
                    {
                        "Screen",
                        "Viewport",
                    });
                    break;

                case "get special value":
                    dgv.Rows[0].Cells[1] = CreateComboBox(new string[]
                    {
                        "Enabled",
                        "Displayed",
                        "Selected",
                        "Text",
                        "Tag",
                        "X Position",
                        "Y Position",
                        "Width",
                        "Height",
                        "Location",
                        "Size",
                    });
                    break;
            }
        }

        /// <summary>
        /// create combobox cell for dgv
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private static DataGridViewComboBoxCell CreateComboBox(string[] items)
        {
            var cmb = new DataGridViewComboBoxCell();
            cmb.Items.AddRange(items);
            return cmb;
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_WebActionParameterTable)], v_WebActionParameterTable);
        }
    }
}

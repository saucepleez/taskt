using OpenQA.Selenium;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Scraping")]
    [Attributes.ClassAttributes.CommandSettings("Get Table Value As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to get a Table Values As DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Table Values As DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetTableValueAsDataTableCommand : ASeleniumSearchWebElementFromWebDriverCommands, IDataTableResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        //[PropertySelectionChangeEvent(nameof(SearchMethodComboBox_SelectionChangeCommitted))]
        //public string v_SearchMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        //public string v_SearchParameter { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ElementIndex))]
        //public string v_WebElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_AttributeName))]
        [PropertyIsOptional(true, "textContent")]
        [PropertyFirstValue("textContent")]
        [PropertyParameterOrder(7000)]
        public string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        [PropertyParameterOrder(8000)]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Method for the First Row")]
        [PropertyDetailSampleUsage("**First Row**", "The First Row is considered the First Row of Data.")]
        [PropertyDetailSampleUsage("**Column Name**", "The First Row is considered the Column Name.")]
        [PropertyDetailSampleUsage("**Ignore**", "Ignore First Row")]
        [PropertyUISelectionOption("First Row")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "First Row")]
        [PropertySelectionValueSensitive(false)]
        [PropertyDisplayText(false, "First Row")]
        [PropertyParameterOrder(9000)]
        public string v_FirstRowMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        //public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserGetTableValueAsDataTableCommand()
        {
            //this.CommandName = "SeleniumBrowserGetTableValueAsDataTableCommand";
            //this.SelectionName = "Get Table Value As DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////(var _, var trgElem) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElement(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), nameof(v_ElementIndex), engine);
            //(var _, var trgElem) = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElement(this, nameof(v_InstanceName), nameof(v_SearchMethod), nameof(v_SearchParameter), nameof(v_WebElementIndex), nameof(v_WaitTimeForWebElement), engine);

            //if (trgElem.TagName.ToLower() != "table")
            //{
            //    throw new Exception("Element is not Table");
            //}

            //var attrName = v_AttributeName.ExpandValueOrUserVariable(engine);

            //var firstRowMethod = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_FirstRowMethod), engine);

            //var newDT = new DataTable();

            //var trs = trgElem.FindElements(By.XPath("child::tr | child::thead/tr | child::tbody/tr | child::tfoot/tr"));
            //if (trs.Count > 0)
            //{
            //    var columns = trs[0].FindElements(By.XPath("child::th | child::td"));
            //    if (columns.Count > 0)
            //    {
            //        int columnSize = columns.Count;
            //        if (firstRowMethod == "column name")
            //        {
            //            for (int i = 0; i < columnSize; i++)
            //            {
            //                newDT.Columns.Add(columns[i].GetAttribute("textContent"));
            //            }
            //        }
            //        else
            //        {
            //            for (int i = 0; i < columnSize; i++)
            //            {
            //                newDT.Columns.Add("Column_" + i.ToString());
            //            }
            //        }

            //        int rowBias = 0;
            //        switch (firstRowMethod)
            //        {
            //            case "column name":
            //            case "ignore":
            //                rowBias = 1;
            //                break;
            //            default:
            //                break;
            //        }
                    
            //        for (int i = rowBias; i < trs.Count; i++)
            //        {
            //            newDT.Rows.Add();
            //            var row = trs[i].FindElements(By.XPath("child::th | child::td"));
            //            int cols = (row.Count > columnSize) ? columnSize : row.Count;
            //            for (int j = 0; j < cols; j++)
            //            {
            //                newDT.Rows[i - rowBias][j] = SeleniumBrowserControls.GetAttribute(row[j], attrName, engine);
            //            }
            //            for (int j = cols + 1; j < columnSize; j++)
            //            {
            //                newDT.Rows[i - rowBias][j] = "";
            //            }
            //        }
            //    }
            //}

            ////newDT.StoreInUserVariable(engine, v_DataTableVariableName);
            //this.StoreDataTableInUserVariable(newDT, nameof(v_Result), engine);

            this.SearchWebElementAction(new Action<IWebElement>(elem =>
            {
                if (elem.TagName.ToLower() != "table")
                {
                    throw new Exception("WebElement is not Table");
                }

                //var attrName = v_AttributeName.ExpandValueOrUserVariable(engine);

                var res = this.CreateEmptyDataTable();

                var trs = elem.FindElements(By.XPath("child::tr | child::thead/tr | child::tbody/tr | child::tfoot/tr"));
                if (trs.Count > 0)
                {
                    var columns = trs[0].FindElements(By.XPath("child::th | child::td"));
                    if (columns.Count > 0)
                    {
                        var firstRowMethod = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_FirstRowMethod), engine);

                        // set DataTable column headers
                        int columnSize = columns.Count;
                        if (firstRowMethod == "column name")
                        {
                            for (int i = 0; i < columnSize; i++)
                            {
                                res.Columns.Add(columns[i].GetAttribute("textContent"));
                            }
                        }
                        else
                        {
                            for (int i = 0; i < columnSize; i++)
                            {
                                res.Columns.Add($"Column_{i}");
                            }
                        }

                        int rowBias = 0;
                        switch (firstRowMethod)
                        {
                            case "column name":
                            case "ignore":
                                rowBias = 1;
                                break;
                            default:
                                break;
                        }

                        using (var resVar = new InnerScriptVariable(engine))
                        {
                            using (var elemVar = new InnerScriptVariable(engine))
                            {
                                var getAttr = new SeleniumBrowserGetAttributeFromWebElementCommand()
                                {
                                    v_WebElement = elemVar.VariableName,
                                    v_AttributeName = this.v_AttributeName,
                                    v_Result = resVar.VariableName,
                                };

                                for (int i = rowBias; i < trs.Count; i++)
                                {
                                    res.Rows.Add();
                                    var row = trs[i].FindElements(By.XPath("child::th | child::td"));
                                    int cols = (row.Count > columnSize) ? columnSize : row.Count;
                                    for (int j = 0; j < cols; j++)
                                    {
                                        elemVar.VariableValue = new ValueTuple<IWebElement, IWebDriver>(row[j], null);
                                        getAttr.RunCommand(engine);

                                        res.Rows[i - rowBias][j] = resVar.VariableValue.ToString();
                                    }

                                    // when columns merged
                                    for (int j = cols + 1; j < columnSize; j++)
                                    {
                                        res.Rows[i - rowBias][j] = string.Empty;
                                    }
                                }
                            }
                            
                        }
                    }
                }

                this.StoreDataTableInUserVariable(res, engine);
            }), engine);
        }

        //private void SearchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    SeleniumBrowserControls.SearchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_WebElementIndex));
        //}
    }
}
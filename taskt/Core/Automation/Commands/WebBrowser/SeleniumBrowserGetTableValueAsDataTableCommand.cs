using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenQA.Selenium;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Scraping")]
    [Attributes.ClassAttributes.CommandSettings("Get Table Value As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to get a Table Values As DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Table Values As DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserGetTableValueAsDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        [PropertySelectionChangeEvent(nameof(SearchMethodComboBox_SelectionChangeCommitted))]
        public string v_SeleniumSearchType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        public string v_SeleniumSearchParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ElementIndex))]
        public string v_ElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_AttributeName))]
        [PropertyIsOptional(true, "textContent")]
        [PropertyFirstValue("textContent")]
        public string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public string v_DataTableVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Method for the First Row")]
        [InputSpecification("", true)]
        [PropertyDetailSampleUsage("**First Row**", "The First Row is considered the First Row of Data.")]
        [PropertyDetailSampleUsage("**Column Name**", "The First Row is considered the Column Name.")]
        [PropertyDetailSampleUsage("**Ignore**", "Ignore First Row")]
        [Remarks("")]
        [PropertyUISelectionOption("First Row")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "First Row")]
        [PropertySelectionValueSensitive(false)]
        [PropertyDisplayText(false, "")]
        public string v_FirstRowMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public SeleniumBrowserGetTableValueAsDataTableCommand()
        {
            //this.CommandName = "SeleniumBrowserGetTableValueAsDataTableCommand";
            //this.SelectionName = "Get Table Value As DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //(var _, var trgElem) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElement(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), nameof(v_ElementIndex), engine);
            (var _, var trgElem) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElement(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), nameof(v_ElementIndex), nameof(v_WaitTime), engine);

            if (trgElem.TagName.ToLower() != "table")
            {
                throw new Exception("Element is not Table");
            }

            var attrName = v_AttributeName.ConvertToUserVariable(engine);

            var firstRowMethod = this.GetUISelectionValue(nameof(v_FirstRowMethod), engine);

            DataTable newDT = new DataTable();

            var trs = trgElem.FindElements(By.XPath("child::tr | child::thead/tr | child::tbody/tr | child::tfoot/tr"));
            if (trs.Count > 0)
            {
                var columns = trs[0].FindElements(By.XPath("child::th | child::td"));
                if (columns.Count > 0)
                {
                    int columnSize = columns.Count;
                    if (firstRowMethod == "column name")
                    {
                        for (int i = 0; i < columnSize; i++)
                        {
                            newDT.Columns.Add(columns[i].GetAttribute("textContent"));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < columnSize; i++)
                        {
                            newDT.Columns.Add("Column_" + i.ToString());
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
                    
                    for (int i = rowBias; i < trs.Count; i++)
                    {
                        newDT.Rows.Add();
                        var row = trs[i].FindElements(By.XPath("child::th | child::td"));
                        int cols = (row.Count > columnSize) ? columnSize : row.Count;
                        for (int j = 0; j < cols; j++)
                        {
                            newDT.Rows[i - rowBias][j] = SeleniumBrowserControls.GetAttribute(row[j], attrName, engine);
                        }
                        for (int j = cols + 1; j < columnSize; j++)
                        {
                            newDT.Rows[i - rowBias][j] = "";
                        }
                    }
                }
            }

            newDT.StoreInUserVariable(engine, v_DataTableVariableName);
        }

        private void SearchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SeleniumBrowserControls.SearchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_ElementIndex));
        }
    }
}
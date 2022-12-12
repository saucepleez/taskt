using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Scraping")]
    [Attributes.ClassAttributes.Description("This command allows you to get a Table Values As DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Table Values As DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SeleniumBrowserGetTableValueAsDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyFirstValue("%kwd_default_browser_instance%")]
        [PropertyValidationRule("Instance Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Element Search Method")]
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
        [InputSpecification("Select the specific search type that you want to use to isolate the element in the web page.")]
        [SampleUsage("Select **Find Element By XPath**, **Find Element By ID**, **Find Element By Name**, **Find Element By Tag Name**, **Find Element By Class Name**, **Find Element By CSS Selector**, **Find Element By Link Text**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertySelectionChangeEvent("SearchMethodComboBox_SelectionChangeCommitted")]
        [PropertyControlIntoCommandField("SearchMethodComboBox")]
        [PropertyValidationRule("Search Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertySelectionValueSensitive(false)]
        public string v_SeleniumSearchType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Element Search Parameter")]
        [InputSpecification("Specifies the parameter text that matches to the element based on the previously selected search type.")]
        [SampleUsage("If search type **Find Element By ID** was specified, for example, given <div id='name'></div>, the value of this field would be **name**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyValidationRule("Search Parameter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyTextBoxSetting(1, false)]
        public string v_SeleniumSearchParameter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Element Index")]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **{{{vIndex}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyTextBoxSetting(1, false)]
        public string v_ElementIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Attribute Name to Get")]
        [InputSpecification("")]
        [SampleUsage("**id** or **Text** or **textContent** or **{{{vAttribute}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "textContent")]
        [PropertyFirstValue("textContent")]
        [PropertyValidationRule("Attribute", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify DataTable Variable Name to store result")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DataTableVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify the method for the First Row")]
        [InputSpecification("")]
        [SampleUsage("**First Row** or **Column Name** or **Ignore**")]
        [Remarks("")]
        [PropertyUISelectionOption("First Row")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "First Row")]
        [PropertySelectionValueSensitive(false)]
        public string v_FirstRowMethod { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox SearchMethodComboBox;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> ElementIndexControls;

        public SeleniumBrowserGetTableValueAsDataTableCommand()
        {
            this.CommandName = "SeleniumBrowserGetTableValueAsDataTableCommand";
            this.SelectionName = "Get Table Value As DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var seleniumInstance = SeleniumControls.getWebBrowserInstance(engine, v_InstanceName.ConvertToUserVariable(engine));

            string searchMethod = v_SeleniumSearchType.GetUISelectionValue("v_SeleniumSearchType", this, engine);
            string seleniumSearchParam = v_SeleniumSearchParameter.ConvertToUserVariable(sender);
            var element = SeleniumControls.findElement(seleniumInstance, seleniumSearchParam, searchMethod);

            IWebElement trgElem = null;
            if (element is IWebElement)
            {
                trgElem = (IWebElement)element;
            }
            else if (element is ReadOnlyCollection<IWebElement>)
            {
                var elems = (ReadOnlyCollection<IWebElement>)element;
                int index = int.Parse(v_ElementIndex.ConvertToUserVariable(engine));
                if ((index >= 0) && (index < elems.Count))
                {
                    trgElem = elems[index];
                }
                else
                {
                    throw new Exception("Element index " + v_ElementIndex + " not exists");
                }
            }
            else
            {
                throw new Exception("not WebBrowser Element");
            }

            if (trgElem.TagName.ToLower() != "table")
            {
                throw new Exception("Element is not Table");
            }

            string attrName = "textContent";
            if (!String.IsNullOrEmpty(v_AttributeName))
            {
                attrName = v_AttributeName.ConvertToUserVariable(engine);
            }

            string firstRowMethod = v_FirstRowMethod.GetUISelectionValue("v_FirstRowMethod", this, engine);

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
                            newDT.Rows[i - rowBias][j] = SeleniumControls.getAttribute(row[j], attrName);
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
            string item = SearchMethodComboBox.SelectedItem.ToString();
            if (item.ToLower().StartsWith("find elements"))
            {
                foreach (Control ctl in ElementIndexControls)
                {
                    ctl.Visible = true;
                }
            }
            else
            {
                foreach (Control ctl in ElementIndexControls)
                {
                    ctl.Visible = false;
                }
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            ElementIndexControls = ctrls.Where(t => (t.Name.Contains("v_ElementIndex"))).ToList();

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get " + v_SeleniumSearchType + " Table Value " + v_AttributeName + " to store " + v_DataTableVariableName + ", Instance Name: '" + v_InstanceName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if ((!String.IsNullOrEmpty(this.v_SeleniumSearchType))
                    && (this.v_SeleniumSearchType.ToLower().StartsWith("find elements")))
            {

                if (String.IsNullOrEmpty(this.v_ElementIndex))
                {
                    this.IsValid = false;
                    this.validationResult += "Element Index is empty.\n";
                }
                else
                {
                    int idx = 0;
                    if (int.TryParse(this.v_ElementIndex, out idx))
                    {
                        if (idx < 0)
                        {
                            this.IsValid = false;
                            this.validationResult += "Element Index is less than 0";
                        }
                    }
                }
            }

            return this.IsValid;
        }
    }
}
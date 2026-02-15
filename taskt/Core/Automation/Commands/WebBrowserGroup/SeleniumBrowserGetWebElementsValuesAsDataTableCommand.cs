using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Scraping")]
    [Attributes.ClassAttributes.CommandSettings("Get WebElements Values As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to get Attributes value for WegElements As DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Attributes value for WegElements As DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWebElementsValuesAsDataTableCommand : ASeleniumSearchMultiWebElementsFromWebDriverCommands, IDataTableResultProperties, IHaveDataTableElements
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        //public string v_SearchMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        //public string v_SearchParameter { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_AttributeNames))]
        [PropertyParameterOrder(7000)]
        public DataTable v_AttributesName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        [PropertyParameterOrder(8000)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        //public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserGetWebElementsValuesAsDataTableCommand()
        {
            //this.CommandName = "SeleniumBrowserGetElementsValuesAsDataTableCommand";
            //this.SelectionName = "Get Elements Values As DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////(var _, var elems) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElements(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), engine);
            //(var _, var elems) = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElements(this, nameof(v_InstanceName), nameof(v_SearchMethod), nameof(v_SearchParameter), nameof(v_WaitTimeForWebElement), engine);

            //var newDT = new DataTable();

            //int attrs = v_AttributesName.Rows.Count;
            //for (int i = 0; i <attrs; i++)
            //{
            //    var attr = v_AttributesName.Rows[i][0]?.ToString().ExpandValueOrUserVariable(engine) ?? "";
            //    if (!newDT.Columns.Contains(attr))
            //    {
            //        newDT.Columns.Add(attr);
            //    }
            //}

            //int rows = elems.Count;
            //int cols = newDT.Columns.Count;
            //for (int i = 0; i < rows; i++)
            //{
            //    newDT.Rows.Add();
            //    for (int j = 0; j < cols; j++)
            //    {
            //        newDT.Rows[i][j] = SeleniumBrowserControls.GetAttribute(elems[i], newDT.Columns[j].ColumnName, engine);
            //    }
            //}

            ////newDT.StoreInUserVariable(engine, v_DataTableVariableName);
            //this.StoreDataTableInUserVariable(newDT, nameof(v_Result), engine);

            this.SearchMultiWebElementsAction(new Action<List<IWebElement>>(elems =>
            {
                using (var elemVar = new InnerScriptVariable(engine))
                {
                    using (var resVar = new InnerScriptVariable(engine))
                    {
                        // create attribute names list
                        var attrs = new List<string>();
                        var showAttrs = new List<string>();
                        foreach (DataRow row in v_AttributesName.Rows)
                        {
                            var expandAttr = (row[0]?.ToString() ?? string.Empty).ExpandValueOrUserVariable(engine);
                            attrs.Add(expandAttr);
                            showAttrs.Add((expandAttr.StartsWith("@") ? expandAttr.Substring(1) : expandAttr));
                        }

                        // set column names
                        var res = this.CreateEmptyDataTable();
                        foreach (var attr in showAttrs)
                        {
                            res.Columns.Add(attr);
                        }

                        foreach (var elem in elems)
                        {
                            elemVar.VariableValue = new ValueTuple<IWebElement, IWebDriver>(elem, null);

                            var getAttr = new SeleniumBrowserGetAttributeFromWebElementCommand()
                            {
                                v_WebElement = elemVar.VariableName,
                                v_Result = resVar.VariableName,
                            };

                            var row = res.NewRow();

                            int idx = 0;
                            foreach (var attr in attrs)
                            {
                                getAttr.v_AttributeName = attr;
                                getAttr.RunCommand(engine);

                                row[showAttrs[idx]] = resVar.VariableValue.ToString();

                                idx++;
                            }

                            res.Rows.Add(row);
                        }

                        this.StoreDataTableInUserVariable(res, engine);
                    }
                }
            }), engine);
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_AttributesName)], v_AttributesName);
        }
    }
}
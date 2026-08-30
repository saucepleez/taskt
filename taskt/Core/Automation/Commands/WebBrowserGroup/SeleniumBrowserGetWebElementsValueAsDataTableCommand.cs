using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Scraping")]
    [Attributes.ClassAttributes.CommandSettings("Get WebElements Value As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to get a Attribute value for WegElements As DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Attribute value for WegElements As DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWebElementsValueAsDataTableCommand : ASeleniumGetMultiWebElementsValueAsSomethingCommands, IDataTableResultProperties
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

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_AttributeName))]
        //public string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        //public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserGetWebElementsValueAsDataTableCommand()
        {
            //this.CommandName = "SeleniumBrowserGetElementsValueAsDataTableCommand";
            //this.SelectionName = "Get Elements Value As DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////(var _, var elems) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElements(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), engine);
            //(var _, var elems) = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElements(this, nameof(v_InstanceName), nameof(v_SearchMethod), nameof(v_SearchParameter), nameof(v_WaitTimeForWebElement), engine);

            //DataTable newDT = new DataTable();

            //SeleniumBrowserControls.GetElementsAttribute(elems, v_AttributeName, engine, new Action<int, string, string>((idx, name, value) =>
            //    {
            //        if (!newDT.Columns.Contains(name))
            //        {
            //            newDT.Columns.Add(name);
            //        }
            //        newDT.Rows.Add();
            //        newDT.Rows[idx][0] = value;
            //    })
            //);

            ////newDT.StoreInUserVariable(engine, v_DataTableVariableName);
            //this.StoreDataTableInUserVariable(newDT, nameof(v_Result), engine);

            var ret = this.CreateEmptyDataTable();
            this.GetMultiWebElementValueAction(new Action<string, string, int>((attrName, attrValue, idx) =>
            {
                if (!ret.Columns.Contains(attrName))
                {
                    ret.Columns.Add(attrName);
                }
                var newRow = ret.NewRow();
                newRow[0] = attrValue;
                ret.Rows.Add(newRow);
            }), engine);
            this.StoreDataTableInUserVariable(ret, engine);
        }
    }
}
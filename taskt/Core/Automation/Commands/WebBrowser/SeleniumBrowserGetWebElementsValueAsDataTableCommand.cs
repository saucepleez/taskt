using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Scraping")]
    [Attributes.ClassAttributes.CommandSettings("Get WebElements Value As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to get a Attribute value for WegElements As DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Attribute value for WegElements As DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserGetWebElementsValueAsDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        public string v_SeleniumSearchType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        public string v_SeleniumSearchParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_AttributeName))]
        public string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public string v_DataTableVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public SeleniumBrowserGetWebElementsValueAsDataTableCommand()
        {
            //this.CommandName = "SeleniumBrowserGetElementsValueAsDataTableCommand";
            //this.SelectionName = "Get Elements Value As DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //(var _, var elems) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElements(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), engine);
            (var _, var elems) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElements(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), nameof(v_WaitTime), engine);

            DataTable newDT = new DataTable();

            SeleniumBrowserControls.GetElementsAttribute(elems, v_AttributeName, engine, new Action<int, string, string>((idx, name, value) =>
                {
                    if (!newDT.Columns.Contains(name))
                    {
                        newDT.Columns.Add(name);
                    }
                    newDT.Rows.Add();
                    newDT.Rows[idx][0] = value;
                })
            );

            newDT.StoreInUserVariable(engine, v_DataTableVariableName);
        }
    }
}
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
    [Attributes.ClassAttributes.CommandSettings("Get One WebElement Values As List")]
    [Attributes.ClassAttributes.Description("This command allows you to get Attributes value for One WebElement As List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Attributes value for One WebElement As List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetOneWebElementValuesAsListCommand : ASeleniumGetOneWebElementValuesAsSomethingCommands, IListResultProperties
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

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_AttributesName))]
        //public DataTable v_AttributesName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        //public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserGetOneWebElementValuesAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////(var _, var trgElem) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElement(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), nameof(v_ElementIndex), engine);
            //(var _, var trgElem) = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElement(this, nameof(v_InstanceName), nameof(v_SearchMethod), nameof(v_SearchParameter), nameof(v_WebElementIndex), nameof(v_WaitTimeForWebElement), engine);

            //List<string> newList = new List<string>();

            //SeleniumBrowserControls.GetElementAttributes(trgElem, v_AttributesName, engine, new Action<string, string>((name, value) =>
            //    {
            //        newList.Add(value);
            //    })
            //);

            ////newList.StoreInUserVariable(engine, v_ListVariableName);
            //this.StoreListInUserVariable(newList, nameof(v_Result), engine);

            using (var dic = new InnerScriptVariable(engine))
            {
                var getAsDic = new SeleniumBrowserGetOneWebElementValuesAsDictionaryCommand()
                {
                    v_InstanceName = this.v_InstanceName,
                    v_SearchMethod = this.v_SearchMethod,
                    v_SearchParameter = this.v_SearchParameter,
                    v_WebElementIndex = this.v_WebElementIndex,
                    v_AttributesName = this.v_AttributesName,
                    v_Result = dic.VariableName,
                    v_WaitTimeForWebElement = this.v_WaitTimeForWebElement,
                };
                getAsDic.RunCommand(engine);

                var convToList = new ConvertDictionaryToListCommand()
                {
                    v_Dictionary = dic.VariableName,
                    v_Result = this.v_Result,
                };
                convToList.RunCommand(engine);
            }
        }


        //private void SearchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    SeleniumBrowserControls.SearchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_WebElementIndex));
        //}

        //public override void BeforeValidate()
        //{
        //    base.BeforeValidate();
        //    DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_AttributesName)], v_AttributesName);
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Scraping")]
    [Attributes.ClassAttributes.Description("This command allows you to get a Attribute value for Elements As List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Attribute value for Elements As List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserGetElementsValueAsListCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyFirstValue("%kwd_default_browser_instance%")]
        //[PropertyValidationRule("Instance Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify Element Search Method")]
        //[PropertyUISelectionOption("Find Element By XPath")]
        //[PropertyUISelectionOption("Find Element By ID")]
        //[PropertyUISelectionOption("Find Element By Name")]
        //[PropertyUISelectionOption("Find Element By Tag Name")]
        //[PropertyUISelectionOption("Find Element By Class Name")]
        //[PropertyUISelectionOption("Find Element By CSS Selector")]
        //[PropertyUISelectionOption("Find Element By Link Text")]
        //[PropertyUISelectionOption("Find Elements By XPath")]
        //[PropertyUISelectionOption("Find Elements By ID")]
        //[PropertyUISelectionOption("Find Elements By Name")]
        //[PropertyUISelectionOption("Find Elements By Tag Name")]
        //[PropertyUISelectionOption("Find Elements By Class Name")]
        //[PropertyUISelectionOption("Find Elements By CSS Selector")]
        //[PropertyUISelectionOption("Find Elements By Link Text")]
        //[InputSpecification("Select the specific search type that you want to use to isolate the element in the web page.")]
        //[SampleUsage("Select **Find Element By XPath**, **Find Element By ID**, **Find Element By Name**, **Find Element By Tag Name**, **Find Element By Class Name**, **Find Element By CSS Selector**, **Find Element By Link Text**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("Search Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        public string v_SeleniumSearchType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify Element Search Parameter")]
        //[InputSpecification("Specifies the parameter text that matches to the element based on the previously selected search type.")]
        //[SampleUsage("If search type **Find Element By ID** was specified, for example, given <div id='name'></div>, the value of this field would be **name**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyValidationRule("Search Parameter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyTextBoxSetting(1, false)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        public string v_SeleniumSearchParameter { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify Attribute Name to Get")]
        //[InputSpecification("")]
        //[SampleUsage("**id** or **Text** or **textContent** or **{{{vAttribute}}}**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Attribute", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_AttributeName))]
        public string v_AttributeName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify List Variable Name to store result")]
        //[InputSpecification("")]
        //[SampleUsage("")]
        //[Remarks("")]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        //[PropertyValidationRule("List Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_ListVariableName { get; set; }

        public SeleniumBrowserGetElementsValueAsListCommand()
        {
            this.CommandName = "SeleniumBrowserGetElementsValueAsListCommand";
            this.SelectionName = "Get Elements Value As List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var _, var elems) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElements(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), engine);

            List<string> newList = new List<string>();

            SeleniumBrowserControls.GetElementsAttribute(elems, v_AttributeName, engine, new Action<int, string, string>((idx, name, value) =>
                {
                    newList.Add(value);
                })
            );

            newList.StoreInUserVariable(engine, v_ListVariableName);
        }
    }
}
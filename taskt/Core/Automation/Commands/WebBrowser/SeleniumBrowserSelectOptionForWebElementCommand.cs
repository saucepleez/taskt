using OpenQA.Selenium.Support.UI;
using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Select Option For WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Select an Option for WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Select an Option for WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserSelectOptionForWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Selection Type")]
        [PropertyUISelectionOption("Select By Index")]
        [PropertyUISelectionOption("Select By Text")]
        [PropertyUISelectionOption("Select By Value")]
        [PropertyUISelectionOption("Deselect By Index")]
        [PropertyUISelectionOption("Deselect By Text")]
        [PropertyUISelectionOption("Deselect By Value")]
        [PropertyUISelectionOption("Deselect All")]
        [PropertySelectionChangeEvent(nameof(cmbSelectionType_SelectionChange))]
        [PropertyValidationRule("Select Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Select Type")]
        public string v_SelectionType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Option Value to Select")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Value")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Value")]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Value")]
        public string v_SelectionValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Fail Select Action")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        public string v_WhenFailSelectAction { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        [PropertySelectionChangeEvent(nameof(cmbScrollToElement_SelectionChange))]
        public string v_ScrollToElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        [PropertyIsOptional(true)]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserSelectOptionForWebElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var elem = v_WebElement.ConvertToUserVariableAsWebElement("WebElement", engine);

            if (this.GetYesNoSelectionValue(nameof(v_ScrollToElement), engine))
            {
                var scrollCommand = new SeleniumBrowserScrollToWebElementCommand()
                {
                    v_InstanceName = this.v_InstanceName,
                    v_WebElement = this.v_WebElement,
                    v_WhenFailScroll = "ignore"
                };
                scrollCommand.RunCommand(engine);
            }

            if (!elem.CheckTagName("select"))
            {
                throw new Exception("WebElement is not Select");
            }

            var sel = new SelectElement(elem);
            var value = v_SelectionValue.ConvertToUserVariable(engine);
            var selectType = this.GetUISelectionValue(nameof(v_SelectionType), engine);
            try
            {
                switch (selectType)
                {
                    case "select by index":
                    case "deselect by index":
                        if (int.TryParse(value, out int index))
                        {
                            switch (selectType)
                            {
                                case "select by index":
                                    sel.SelectByIndex(index);
                                    break;
                                default:
                                    sel.DeselectByIndex(index);
                                    break;
                            }
                        }
                        break;
                    case "select by text":
                        sel.SelectByText(value);
                        break;
                    case "select by value":
                        sel.SelectByValue(value);
                        break;
                    case "deselect by text":
                        sel.DeselectByText(value);
                        break;
                    case "deselect by value":
                        sel.DeselectByValue(value);
                        break;
                    case "deselect all":
                        sel.DeselectAll();
                        break;
                }
            }
            catch
            {
                if (this.GetUISelectionValue(nameof(v_WhenFailSelectAction), engine) == "error")
                {
                    throw new Exception($"Fail Select Option. Type:'{selectType}', Value:'{value}'");
                }
            }
        }

        private void cmbSelectionType_SelectionChange(object sender, EventArgs e)
        {
            var showFlag = (((ComboBox)sender).SelectedItem?.ToString().ToLower() != "deselect all");
            GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_SelectionValue), showFlag);
        }

        private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        {
            SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
        }
    }
}
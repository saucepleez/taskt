using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Get Attribute From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Attribute Value from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Attribute Value from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserGetAttributeFromWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Attribute Name")]
        [PropertyDetailSampleUsage("**class**", PropertyDetailSampleUsage.ValueType.Value, "Attribute")]
        [PropertyDetailSampleUsage("**{{{vAttribute}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Attribute")]
        [PropertyValidationRule("Attribute", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Attribute")]
        public string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When the WebElement does not have the Attribute")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        public string v_WhenNoAttribute { get; set; }

        public SeleniumBrowserGetAttributeFromWebElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var elem = v_WebElement.ConvertToUserVariableAsWebElement("WebElement", engine);

            var attributeName = v_AttributeName.ConvertToUserVariable(engine);

            var v = elem.GetAttribute(attributeName);

            if (v != null)
            {
                v.StoreInUserVariable(engine, v_Result);
            }
            else
            {
                if (this.GetUISelectionValue(nameof(v_WhenNoAttribute), engine) == "error")
                {
                    throw new Exception("Attribute '" + attributeName + "' does not exists.");
                }
                else
                {
                    "".StoreInUserVariable(engine, v_Result);
                }
            }
        }
    }
}
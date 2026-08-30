using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Special Value From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Special Value Value from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Special Value from WebElement. Enabled, Displayed, Selected, Tag, Size, etc.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetSpecialValueFromWebElementCommand : ASeleniumGetOneResultFromWebElementCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Value Type")]
        [PropertyUISelectionOption("Enabled")]
        [PropertyUISelectionOption("Displayed")]
        [PropertyUISelectionOption("Selected")]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Tag")]
        [PropertyUISelectionOption("X Position")]
        [PropertyUISelectionOption("Y Position")]
        [PropertyUISelectionOption("Width")]
        [PropertyUISelectionOption("Height")]
        [PropertyUISelectionOption("Location")]
        [PropertyUISelectionOption("Size")]
        [PropertyUISelectionOption("TagName")]
        [PropertyUISelectionOption("Tag Name")]
        [PropertyValidationRule("Value Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Value Type")]
        [PropertyParameterOrder(6000)]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public override string v_Result { get; set; }

        public SeleniumBrowserGetSpecialValueFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.GetFromWebElementAction(new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((elem, seleniumInstance) =>
            {
                string ret = string.Empty;
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), engine))
                {
                    case "enabled":
                        ret = elem.Enabled.ToString();
                        break;

                    case "displayed":
                        ret = elem.Displayed.ToString();
                        break;

                    case "selected":
                        ret = elem.Selected.ToString();
                        break;

                    case "text":
                        ret = elem.Text;
                        break;

                    case "tag":
                    case "tagname":
                    case "tag name":
                        ret = elem.TagName;
                        break;

                    case "x position":
                        ret = elem.Location.X.ToString();
                        break;

                    case "y position":
                        ret = elem.Location.Y.ToString();
                        break;

                    case "width":
                        ret = elem.Size.Width.ToString();
                        break;

                    case "height":
                        ret = elem.Size.Height.ToString();
                        break;

                    case "location":
                        var lc = elem.Location;
                        ret = $"{lc.X},{lc.Y}";
                        break;

                    case "size":
                        var sz = elem.Size;
                        ret = $"{sz.Width},{sz.Height}";
                        break;
                }
                ret.StoreInUserVariable(engine, v_Result);

            }), this.StoreEmptyValueToResult, engine);
        }
    }
}
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get WebElement Position")]
    [Attributes.ClassAttributes.Description("This command allows you to Get WebElement Position.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get WebElement Position.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWebElementPositionCommand : ASeleniumGetFromWebElementCommands, IPositionProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store X Position")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "X Position")]
        [PropertyParameterOrder(6000)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Y Position")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Y Position", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Y Position")]
        [PropertyParameterOrder(6100)]
        public string v_YPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Base position")]
        [PropertyUISelectionOption("Top Left")]
        [PropertyUISelectionOption("Bottom Right")]
        [PropertyUISelectionOption("Top Right")]
        [PropertyUISelectionOption("Bottom Left")]
        [PropertyUISelectionOption("Center")]
        [PropertyIsOptional(true, "Top Left")]
        [PropertyDisplayText(false, "Base Position")]
        [PropertyParameterOrder(7000)]
        public string v_PositionBase { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Position Type")]
        [PropertyUISelectionOption("Screen")]
        [PropertyUISelectionOption("Viewport")]
        [PropertyIsOptional(true, "Viewport")]
        [PropertyParameterOrder(7100)]
        public string v_PositionType { get; set; }

        public SeleniumBrowserGetWebElementPositionCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.GetFromWebElementAction(new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((elem, seleniumInstance) =>
            {
                var loc = elem.Location;
                var size = elem.Size;

                int x = 0, y = 0;
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionBase), engine))
                {
                    case "top left":
                        x = loc.X;
                        y = loc.Y;
                        break;
                    case "bottom right":
                        x = loc.X + size.Width;
                        y = loc.Y + size.Height;
                        break;
                    case "top right":
                        x = loc.X + size.Width;
                        y = loc.Y;
                        break;
                    case "bottom left":
                        x = loc.X;
                        y = loc.Y + size.Height;
                        break;
                    case "center":
                        x = (loc.X + size.Width) / 2;
                        y = (loc.Y + size.Height) / 2;
                        break;
                }

                switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionType), engine))
                {
                    case "screen":
                        // get Screen(Desktop) position
                        int baseX, baseY;
                        using (var vX = new InnerScriptVariable(engine))
                        {
                            using (var vY = new InnerScriptVariable(engine))
                            {
                                var insName = this.GetInstanceNameFromWebBrowserInstance(seleniumInstance, engine);

                                var getPos = new SeleniumBrowserGetWebBrowserPositionCommand()
                                {
                                    v_InstanceName = insName,
                                    v_PositionType = "Viewport",
                                    v_XPosition = vX.VariableName,
                                    v_YPosition = vY.VariableName,
                                };
                                getPos.RunCommand(engine);

                                baseX = int.Parse(vX.VariableValue.ToString());
                                baseY = int.Parse(vY.VariableValue.ToString());
                            }
                        }
                        x += baseX;
                        y += baseY;
                        break;
                }

                if (!string.IsNullOrEmpty(v_XPosition))
                {
                    x.StoreInUserVariable(engine, v_XPosition);
                }
                if (!string.IsNullOrEmpty(v_YPosition))
                {
                    y.StoreInUserVariable(engine, v_YPosition);
                }
            }), new Action<Engine.AutomationEngineInstance>(e =>
            {
                if (!string.IsNullOrEmpty(v_XPosition))
                {
                    string.Empty.StoreInUserVariable(engine, v_XPosition);
                }
                if (!string.IsNullOrEmpty(v_YPosition))
                {
                    string.Empty.StoreInUserVariable(engine, v_YPosition);
                }
            }), engine);
        }
    }
}
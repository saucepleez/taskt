using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From Web Browser")]
    [Attributes.ClassAttributes.CommandSettings("Get Web Browser Position")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Web Browser Position.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Web Browser Position.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWebBrowserPositionCommand : ASeleniumWebDriverActionCommands, IPositionProperties, ICanExecuteJavaScriptToWebDriver
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OptionalResult))]
        [PropertyDescription("Variable Name to Store X Position")]
        [PropertyValidationRule("X Postion", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "X Position")]
        [PropertyParameterOrder(6000)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OptionalResult))]
        [PropertyDescription("Variable Name to Store X Position")]
        [PropertyValidationRule("Y Postion", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Y Position")]
        [PropertyParameterOrder(6100)]
        public string v_YPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Position Type")]
        [PropertyUISelectionOption("Window")]
        [PropertyUISelectionOption("Viewport")]
        [PropertyIsOptional(true, "Window")]
        [PropertyDisplayText(true, "Type")]
        [PropertyParameterOrder(7000)]
        public string v_PositionType { get; set; }

        public SeleniumBrowserGetWebBrowserPositionCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WebDriverActionCore(new Action<OpenQA.Selenium.IWebDriver, string>((seleniumInstance, _) =>
            {
                var pt = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionType), engine);
                if ((pt == "window") || (seleniumInstance is OpenQA.Selenium.IE.InternetExplorerDriver))
                {
                    (var x, var y) = GetPosition(seleniumInstance);

                    this.StoreXYProcess(x, y, engine);
                }
                else
                {
                    int outerWidth, outerHeight;
                    int innerWidth, innerHeight;
                    using (var w = new InnerScriptVariable(engine))
                    {
                        using (var h = new InnerScriptVariable(engine))
                        {
                            var getSize = new SeleniumBrowserGetWebBrowserSizeCommand()
                            {
                                v_InstanceName = this.v_InstanceName,
                                v_Width = w.VariableName,
                                v_Height = h.VariableName,
                                v_SizeType = "Window",
                            };
                            getSize.RunCommand(engine);
                            outerWidth = int.Parse(w.VariableValue.ToString());
                            outerHeight = int.Parse(h.VariableValue.ToString());

                            getSize.v_SizeType = "Viewport";
                            getSize.RunCommand(engine);
                            innerWidth = int.Parse(w.VariableValue.ToString());
                            innerHeight = int.Parse(h.VariableValue.ToString());
                        }
                    }

                    (var px, var py) = GetPosition(seleniumInstance);
                    var xOffset = (outerWidth - innerWidth) / 2;

                    if (seleniumInstance is OpenQA.Selenium.Chrome.ChromeDriver)
                    {
                        var yOffset = outerHeight - innerHeight - 6;
                        this.StoreXYProcess(px + xOffset, py + yOffset, engine);
                    }
                    else if (seleniumInstance is OpenQA.Selenium.Edge.EdgeDriver)
                    {
                        // 4 is bottom window height, 8 is offset
                        var yOffset = outerHeight - innerHeight - 4 - 8;
                        this.StoreXYProcess(px + xOffset, py + yOffset, engine);
                    }
                    else if (seleniumInstance is OpenQA.Selenium.Firefox.FirefoxDriver)
                    {
                        var yOffset = outerHeight - innerHeight - 8;
                        this.StoreXYProcess(px + xOffset, py + yOffset, engine);
                    }
                }
            }), engine);
        }

        /// <summary>
        /// get WebBrowser position
        /// </summary>
        /// <param name="d"></param>
        /// <returns>(width, height)</returns>
        private (int, int) GetPosition(OpenQA.Selenium.IWebDriver d)
        {
            var script = "return window.screenX+','+window.screenY;";
            var r = this.ExecuteJavaScript(d, script).ToString();

            var spt = r.Split(',');
            return (int.Parse(spt[0]), int.Parse(spt[1]));
        }

        /// <summary>
        /// store variable to x,y result process
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="engine"></param>
        private void StoreXYProcess(int x, int y, Engine.AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(v_XPosition))
            {
                x.StoreInUserVariable(engine, v_XPosition);
            }
            if (!string.IsNullOrEmpty(v_YPosition))
            {
                y.StoreInUserVariable(engine, v_YPosition);
            }
        }
    }
}
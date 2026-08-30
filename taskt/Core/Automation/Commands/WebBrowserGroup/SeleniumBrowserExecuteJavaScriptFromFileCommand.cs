using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Web Browser Actions")]
    [Attributes.ClassAttributes.CommandSettings("Execute JavaScript From File")]
    [Attributes.ClassAttributes.Description("This command allows you to execute a JavaScript from File.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserExecuteJavaScriptFromFileCommand : ASeleniumExecuteJavaScriptCommands, ICanHandleFilePath
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyDescription("JavaScript File")]
        [PropertyDetailSampleUsage("**c:\\js\\mycode.js**", "Specify the JavaScript File Path")]
        [PropertyDetailSampleUsage("**{{{vFile}}}**", "Specify the Variable Value **vCode** for JavaScript Code or JavaScript File Path")]
        [PropertyParameterOrder(6000)]
        public string v_FilePath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Arguments")]
        //[InputSpecification("Argument", true)]
        //[PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Arguments")]
        //[Remarks("The value of the argument can be obtained with 'arguments[0]' in code.")]
        //[PropertyIsOptional(true)]
        //[PropertyDisplayText(false, "Arguments")]
        //[PropertyParameterOrder(7000)]
        //public string v_Arguments { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Recieve Result Value")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyParameterOrder(8000)]
        //public string v_Result { get; set; }

        public SeleniumBrowserExecuteJavaScriptFromFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WebDriverAction(new Action<OpenQA.Selenium.IWebDriver>(seleniumInstance =>
            {
                var scriptFile = this.ExpandValueOrUserVariableAsFilePath(nameof(v_FilePath), new PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "js"), engine);
                string script = System.IO.File.ReadAllText(scriptFile);

                //var args = v_Arguments.ExpandValueOrUserVariable(engine);

                //// run script
                //var js = (OpenQA.Selenium.IJavaScriptExecutor)seleniumInstance;

                //object result;
                //if (string.IsNullOrEmpty(args))
                //{
                //    result = js.ExecuteScript(script);
                //}
                //else
                //{
                //    result = js.ExecuteScript(script, args);
                //}

                //// apply result to variable
                //if (!string.IsNullOrEmpty(v_Result))
                //{
                //    (result?.ToString() ?? string.Empty).StoreInUserVariable(engine, v_Result);
                //}

                this.ExecuteJavaScriptProcess(seleniumInstance, script, engine);
            }), engine);
        }
    }
}
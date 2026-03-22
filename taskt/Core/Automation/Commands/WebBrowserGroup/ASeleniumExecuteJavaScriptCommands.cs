using OpenQA.Selenium;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public abstract class ASeleniumExecuteJavaScriptCommands : ASeleniumWebDriverActionCommands, ISeleniumExecuteJavaScriptProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Arguments")]
        [InputSpecification("Argument", true)]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Arguments")]
        [Remarks("The value of the argument can be obtained with 'arguments[0]' in code.")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Arguments")]
        [PropertyParameterOrder(7000)]
        public string v_Arguments { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve Result Value")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8000)]
        public string v_Result { get; set; }

        /// <summary>
        /// execute JavaScript process
        /// </summary>
        /// <param name="seleniumInstance"></param>
        /// <param name="script"></param>
        /// <param name="engine"></param>
        protected void ExecuteJavaScriptProcess(IWebDriver seleniumInstance, string script, Engine.AutomationEngineInstance engine)
        {
            var args = v_Arguments.ExpandValueOrUserVariable(engine);

            // run script
            //var js = (IJavaScriptExecutor)seleniumInstance;

            //object result;
            //if (string.IsNullOrEmpty(args))
            //{
            //    result = js.ExecuteScript(script);
            //}
            //else
            //{
            //    result = js.ExecuteScript(script, args);
            //}

            object result;
            if (string.IsNullOrEmpty(args))
            {
                result = this.ExecuteJavaScript(seleniumInstance, script);
            }
            else
            {
                result = this.ExecuteJavaScript(seleniumInstance, script, args);
            }

            // apply result to variable
            if (!string.IsNullOrEmpty(v_Result))
            {
                (result?.ToString() ?? string.Empty).StoreInUserVariable(engine, v_Result);
            }
        }
    }
}

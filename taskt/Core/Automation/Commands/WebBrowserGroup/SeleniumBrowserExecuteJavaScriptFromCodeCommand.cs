using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Web Browser Actions")]
    [Attributes.ClassAttributes.CommandSettings("Execute JavaScript From Code")]
    [Attributes.ClassAttributes.Description("This command allows you to execute a script in a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserExecuteJavaScriptFromCodeCommand : ASeleniumExecuteJavaScriptCommands, ICanExecuteJavaScriptToWebDriver
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("JavaScript Code Type")]
        ////[SampleUsage("**Code** or **File**")]
        //[PropertyDetailSampleUsage("**Code**", "Use Specfied JavaScript Code")]
        //[PropertyDetailSampleUsage("**File**", "Use Specfied JavaScript File")]
        //[Remarks("")]
        //[PropertyUISelectionOption("Code")]
        //[PropertyUISelectionOption("File")]
        //[PropertyIsOptional(true, "Code")]
        //[PropertyFirstValue("Code")]
        //[PropertyParameterOrder(6000)]
        //public string v_CodeType { get; set; }

        [XmlAttribute]
        [PropertyDescription("JavaScript Code")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("JavaScript", true)]
        [PropertyDetailSampleUsage("**return (2);**", "Specify the JavaScript Code")]
        //[PropertyDetailSampleUsage("**c:\\js\\mycode.js**", "Specify the JavaScript File Path")]
        [PropertyDetailSampleUsage("**{{{vCode}}}**", "Specify the Variable Value **vCode** for JavaScript Code or JavaScript File Path")]
        [PropertyShowSampleUsageInDescription(true)]
        //[Remarks("When Selected **Code**, plese Enter the JavaScript Code.\nWhen Selected **File**, please Enter the JavaScript File Path.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyValidationRule("JavaScript Code", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(6000)]
        public string v_ScriptCode { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Timeout in Seconds")]
        //[InputSpecification("Timeout in Seconds", true)]
        ////[SampleUsage("**0** or **10** or **{{{vWaitTime}}}**")]
        //[PropertyDetailSampleUsage("**0**", "Specify **0** for Timeout. This means Waiting until JavaScript is finished.")]
        //[PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value, "Timeout")]
        //[PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Timeout")]
        //[Remarks("When Value is Less Than or Equals to **0**, this means Waiting until JavaScript is finished.")]
        //[PropertyIsOptional(true, "0")]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(8000)]
        //public string v_TimeOut { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Arguments")]
        //[InputSpecification("Arguments", true)]
        //[PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Argument")]
        //[PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Argument")]
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

        public SeleniumBrowserExecuteJavaScriptFromCodeCommand()
        {
            //this.CommandName = "SeleniumBrowserExecuteScriptCommand";
            //this.SelectionName = "Execute Script";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            
            //this.v_InstanceName = "";
            //this.v_CodeType = "Code";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WebDriverAction(new Action<OpenQA.Selenium.IWebDriver>(seleniumInstance =>
            {
                //var codeType = SelectionItemsControls.ExpandValueOrUserVariableAsSelectionItem(this, nameof(v_CodeType), engine);

                //string script = string.Empty;
                //switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_CodeType), engine))
                //{
                //    case "code":
                //        script = v_ScriptCode.ExpandValueOrUserVariable(engine);
                //        break;
                //    case "file":
                //        var scriptFile = this.ExpandValueOrUserVariableAsFilePath(nameof(v_ScriptCode), new PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "js"), engine);
                //        script = System.IO.File.ReadAllText(scriptFile);
                //        break;
                //}

                var script = v_ScriptCode.ExpandValueOrUserVariable(engine);
                //var args = v_Arguments.ExpandValueOrUserVariable(engine);

                //// run script
                //OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)seleniumInstance;

                //object result;
                //if (string.IsNullOrEmpty(args))
                //{
                //    if (timeOut > 1)
                //    {
                //        result = js.ExecuteAsyncScript(script);
                //    }
                //    else
                //    {
                //        result = js.ExecuteScript(script);
                //    }
                //}
                //else
                //{
                //    if (timeOut > 1)
                //    {
                //        result = js.ExecuteAsyncScript(script, args);
                //    }
                //    else
                //    {
                //        result = js.ExecuteScript(script, args);
                //    }
                //}

                //// apply result to variable
                //if ((result != null) && (!string.IsNullOrEmpty(v_Result)))
                //{
                //    result.ToString().StoreInUserVariable(engine, v_Result);
                //}

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

                this.ExecuteJavaScript(seleniumInstance, script);
            }), engine);
        }
    }
}
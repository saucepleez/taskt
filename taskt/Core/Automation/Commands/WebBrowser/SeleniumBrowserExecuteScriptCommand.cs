using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to execute a script in a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserExecuteScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name (ex. myInstance, {{{{vInstance}}})")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("JavaScript Code Type")]
        //[SampleUsage("**Code** or **File**")]
        [PropertyDetailSampleUsage("**Code**", "Use Specfied JavaScript Code")]
        [PropertyDetailSampleUsage("**File**", "Use Specfied JavaScript File")]
        [Remarks("")]
        [PropertyUISelectionOption("Code")]
        [PropertyUISelectionOption("File")]
        [PropertyIsOptional(true, "Code")]
        [PropertyFirstValue("Code")]
        public string v_CodeType { get; set; }

        [XmlAttribute]
        [PropertyDescription("JavaScript Code")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("JavaScript", true)]
        [PropertyDetailSampleUsage("**return (2);**", "Specify the JavaScript Code")]
        [PropertyDetailSampleUsage("**c:\\js\\mycode.js**", "Specify the JavaScript File Path")]
        [PropertyDetailSampleUsage("**{{{vCode}}}**", "Specify the Variable Value **vCode** for JavaScript Code or JavaScript File Path")]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("When Selected **Code**, plese Enter the JavaScript Code.\nWhen Selected **File**, please Enter the JavaScript File Path.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyValidationRule("JavaScript Code", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(false, "")]
        public string v_ScriptCode { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Timeout in Seconds")]
        [InputSpecification("Timeout in Seconds", true)]
        //[SampleUsage("**0** or **10** or **{{{vWaitTime}}}**")]
        [PropertyDetailSampleUsage("**0**", "Specify **0** for Timeout. This means Waiting until JavaScript is finished.")]
        [PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value, "Timeout")]
        [PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Timeout")]
        [Remarks("When Value is Less Than or Equals to **0**, this means Waiting until JavaScript is finished.")]
        [PropertyIsOptional(true, "0")]
        [PropertyDisplayText(false, "")]
        public string v_TimeOut { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Argument")]
        [InputSpecification("Argument", true)]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Argument")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Argument")]
        [Remarks("The value of the argument can be obtained with 'arguments[0]' in code.")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "")]
        public string v_Args { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the data")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable** or **{{{vVariableName}}}**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyIsOptional(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve Result Value")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_userVariableName { get; set; }

        public SeleniumBrowserExecuteScriptCommand()
        {
            this.CommandName = "SeleniumBrowserExecuteScriptCommand";
            this.SelectionName = "Execute Script";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            
            //this.v_InstanceName = "";
            //this.v_CodeType = "Code";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //if (String.IsNullOrEmpty(v_CodeType))
            //{
            //    v_CodeType = "Code";
            //}
            var codeType = SelectionControls.GetUISelectionValue(this, nameof(v_CodeType), engine);

            string script = "";
            if (codeType == "code")
            {
                script = v_ScriptCode.ConvertToUserVariable(sender);
            }
            else if (codeType == "file")
            {
                string scriptFiile = FilePathControls.formatFilePath_NoFileCounter(v_ScriptCode, engine, "js", true);
                script = System.IO.File.ReadAllText(scriptFiile);
            }

            var args = v_Args.ConvertToUserVariable(sender);
            
            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var browserObject = engine.GetAppInstance(vInstance);
            //var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;
            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            //configure timeout
            //var inputTimeout = v_TimeOut.ConvertToUserVariable(sender);
            //int timeOut;
            //if (!int.TryParse(inputTimeout, out timeOut))
            //{
            //    timeOut = -1;
            //}
            var timeOut = this.ConvertToUserVariableAsInteger(nameof(v_TimeOut), engine);

            //set driver timeout
            if (timeOut > 0)
            {
                seleniumInstance.Manage().Timeouts().AsynchronousJavaScript = new TimeSpan(0, 0, timeOut);
            }
               
            //run script
            OpenQA.Selenium.IJavaScriptExecutor js = (OpenQA.Selenium.IJavaScriptExecutor)seleniumInstance;

            object result;
            if (String.IsNullOrEmpty(args))
            {
                if (timeOut > 1)
                {
                    result = js.ExecuteAsyncScript(script);
                }
                else
                {
                    result = js.ExecuteScript(script);
                }
            }
            else
            {
                if (timeOut > 1)
                {
                    result = js.ExecuteAsyncScript(script, args);
                }
                else
                {
                    result = js.ExecuteScript(script, args);
                }
            }

            //apply result to variable
            if ((result != null) && (!string.IsNullOrEmpty(v_userVariableName)))
            {   
                result.ToString().StoreInUserVariable(sender, v_userVariableName);
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    //UI.CustomControls.CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);
        //    //RenderedControls.AddRange(instanceCtrls);
        //    ////RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ScriptCode", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Args", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TimeOut", this, editor));

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance name is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
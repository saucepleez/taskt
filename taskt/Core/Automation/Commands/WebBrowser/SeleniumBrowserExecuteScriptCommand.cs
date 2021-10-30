using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to execute a script in a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserExecuteScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name (ex. myInstance, {{{{vInstance}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Specify script code type (Default is Code)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Code** or **File**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Code")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_CodeType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the script code")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("If selected Code plese enter script code.\nIf selected File, please enter script file path.")]
        public string v_ScriptCode { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the timeout in seconds (Default is 0)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("If less than or equal to 0, wait for the script to finish.")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **10** or **{{{vWaitTime}}}**")]
        [Attributes.PropertyAttributes.Remarks("time >= 1 is async, time <= 0 is sync")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_TimeOut { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply Argument")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("The value of the argument can be obtained with 'arguments[0]' in code.")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_Args { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the data")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable** or **{{{vVariableName}}}**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_userVariableName { get; set; }
        public SeleniumBrowserExecuteScriptCommand()
        {
            this.CommandName = "SeleniumBrowserExecuteScriptCommand";
            this.SelectionName = "Execute Script";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            
            this.v_InstanceName = "";
            this.v_CodeType = "Code";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

            if (String.IsNullOrEmpty(v_CodeType))
            {
                v_CodeType = "Code";
            }

            string script;
            if (v_CodeType.ToLower() == "code")
            {
                script = v_ScriptCode.ConvertToUserVariable(sender);
            }
            else if (v_CodeType.ToLower() == "file")
            {
                script = System.IO.File.ReadAllText(v_ScriptCode.ConvertToUserVariable(sender));
            }
            else
            {
                throw new Exception("Strange script code type " + v_CodeType);
            }
            var args = v_Args.ConvertToUserVariable(sender);
            var inputTimeout = v_TimeOut.ConvertToUserVariable(sender);

            var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;

            //configure timeout
            int timeOut;
            if (!int.TryParse(inputTimeout, out timeOut))
            {
                timeOut = -1;
            }

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
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
            //UI.CustomControls.CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);
            //RenderedControls.AddRange(instanceCtrls);
            ////RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ScriptCode", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Args", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TimeOut", this, editor));

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
            }

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance name is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
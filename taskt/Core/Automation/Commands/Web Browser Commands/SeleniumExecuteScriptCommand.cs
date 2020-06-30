using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Group("Web Browser Commands")]
    [Description("This command allows you to execute a script in a Selenium web browser session.")]

    public class SeleniumExecuteScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("MyBrowserInstance || {vBrowserInstance}")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; } 

        [XmlAttribute]
        [PropertyDescription("Script Code")]
        [InputSpecification("Enter the script code to execute.")]
        [SampleUsage("arguments[0].click(); || alert('Welcome to Taskt'); || {vScript}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ScriptCode { get; set; }

        [XmlAttribute]
        [PropertyDescription("Arguments")]
        [InputSpecification("Enter any necessary arguments.")]
        [SampleUsage("button || {vArguments}")]
        [Remarks("This input is optional.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Arguments { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Data Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public SeleniumExecuteScriptCommand()
        {
            CommandName = "SeleniumExecuteScriptCommand";
            SelectionName = "Execute Script";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultBrowser";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var browserObject = engine.GetAppInstance(vInstance);
            var script = v_ScriptCode.ConvertToUserVariable(sender);
            var args = v_Arguments.ConvertToUserVariable(sender);
            var seleniumInstance = (IWebDriver)browserObject;
            IJavaScriptExecutor js = (IJavaScriptExecutor)seleniumInstance;

            object result;
            if (string.IsNullOrEmpty(args))
                result = js.ExecuteScript(script);
            else
                result = js.ExecuteScript(script, args);

            //apply result to variable
            if ((result != null) && (!string.IsNullOrEmpty(v_OutputUserVariableName)))
                result.ToString().StoreInUserVariable(sender, v_OutputUserVariableName);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ScriptCode", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Arguments", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Data in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
        }
    }
}
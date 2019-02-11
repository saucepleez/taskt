using System;
using System.Xml.Serialization;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to execute a script in a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserExecuteScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the script code")]
        public string v_ScriptCode { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Supply Arguments")]
        public string v_Args { get; set; }
        public SeleniumBrowserExecuteScriptCommand()
        {
            this.CommandName = "SeleniumBrowserExecuteScriptCommand";
            this.SelectionName = "Execute Script";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);


            var script = v_ScriptCode.ConvertToUserVariable(sender);
            var args = v_Args.ConvertToUserVariable(sender);
            var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
            if (String.IsNullOrEmpty(args))
            {
                seleniumInstance.ExecuteScript(script);
            }
            else
            {
                seleniumInstance.ExecuteScript(script, args);
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{



    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Navigate")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session to a given URL or resource.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to navigate an existing Selenium instance to a known URL or web resource")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateURLCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name (ex. myInstacne, {{{vInstance}}})")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to (ex. https://mycompany.com/orders, {{{vURL}}})")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the destination URL that you want the selenium instance to navigate to")]
        [Attributes.PropertyAttributes.SampleUsage("**https://mycompany.com/orders** or **{{{vURL}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Specify HTTPS usage")]
        [Attributes.PropertyAttributes.InputSpecification("Choose if you want to use HTTP or HTTPS for navigation. If no protocol is specified in the URL above, taskt will resort to this choice.")]
        [Attributes.PropertyAttributes.SampleUsage("\"True\" to use HTTPS, \"False\" if you want to try HTTP instead")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("True")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("False")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public bool v_UseHttps { get; set; }
        private Dictionary<bool, string> v_HttpsChoice = new Dictionary<bool, string>();


        public SeleniumBrowserNavigateURLCommand()
        {
            this.CommandName = "SeleniumBrowserNavigateURLCommand";
            this.SelectionName = "Navigate to URL";
            this.v_InstanceName = "";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_UseHttps = true;
            this.v_HttpsChoice.Add(true, "https://");
            this.v_HttpsChoice.Add(false, "http://");

        }

        public override void RunCommand(object sender)
        {

            var caseType = v_UseHttps.ToString().ConvertToUserVariable(sender);

            this.v_UseHttps = bool.Parse(caseType.ToLower());

            var parsedURL = v_URL.ConvertToUserVariable(sender);
            if (!parsedURL.StartsWith("http"))
            {
                parsedURL = this.v_HttpsChoice[v_UseHttps] + parsedURL;
            }

            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

            var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;

            seleniumInstance.Navigate().GoToUrl(parsedURL);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_URL", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_UseHttps", this, editor));

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
            }

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            string url;
            if (String.IsNullOrEmpty(this.v_URL))
            {
                url = "";
            }
            else
            {
                url = v_URL;
            }
            if (!url.StartsWith("http"))
            {
                return base.GetDisplayValue() + " [URL: '" + v_HttpsChoice[v_UseHttps] + v_URL + "', Instance Name: '" + v_InstanceName + "']";
            }
            else
            {
                return base.GetDisplayValue() + " [URL: '" + v_URL + "', Instance Name: '" + v_InstanceName + "']";
            }
            
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_URL))
            {
                this.validationResult += "URL is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }

    }
}
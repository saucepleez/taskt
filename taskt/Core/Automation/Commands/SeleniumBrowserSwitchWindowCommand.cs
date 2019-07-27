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
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserSwitchWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select type of match to make")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Window URL")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Window Title")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Handle ID")]
        [Attributes.PropertyAttributes.InputSpecification("Select an option which best fits to the search type you would like to make.")]
        [Attributes.PropertyAttributes.SampleUsage("Select one of the provided options.")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowMatchType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define a match specification")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Exact Match")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Contains Match")]
        [Attributes.PropertyAttributes.InputSpecification("Select an option which best fits to the specification you would like to make.")]
        [Attributes.PropertyAttributes.SampleUsage("Select one of the provided options.")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MatchSpecification { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate if search is case-sensitive")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Select an option which best fits to the specification you would like to make.")]
        [Attributes.PropertyAttributes.SampleUsage("Select one of the provided options.")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_CaseSensitiveMatch { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please provide the parameter to match (ex. Window URL, Window Title, Handle ID)")]
        [Attributes.PropertyAttributes.SampleUsage("Enter in 'http://www.url.com' or 'Welcome to Homepage'")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MatchParameter { get; set; }
        public SeleniumBrowserSwitchWindowCommand()
        {
            this.CommandName = "SeleniumBrowserSwitchWindowCommand";
            this.SelectionName = "Switch Browser Window";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //set defaults
            this.v_WindowMatchType = "Window URL";
            this.v_MatchSpecification = "Exact Match";
            this.v_CaseSensitiveMatch = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(sender);

            var browserObject = engine.GetAppInstance(vInstance);
            var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;
            var matchParam = v_MatchParameter.ConvertToUserVariable(sender);

            var handles = seleniumInstance.WindowHandles;

            var currentHandle = seleniumInstance.CurrentWindowHandle;
            var matchFound = false;
            foreach (var hndl in handles)
            {

                var tempHandle = seleniumInstance.SwitchTo().Window(hndl);

                //array ordering is not guaranteed so skip if current window
                if (tempHandle.CurrentWindowHandle == currentHandle)
                    continue;

                var matchType = v_WindowMatchType.ConvertToUserVariable(sender);
                string matchData;

                if (matchType == "Window URL")
                {
                    matchData = tempHandle.Url;
                }
                else if (matchType == "Window Title")
                {
                    matchData = tempHandle.Title;
                }
                else if(matchType == "Handle ID")
                {
                    matchData = tempHandle.CurrentWindowHandle;
                }
                else
                {
                    throw new NotImplementedException($"Specified match type '{matchType}' is not supported for switching windows. Use either 'Window URL' or 'Window Title'");
                }

                bool exactMatchRequired = v_MatchSpecification.ConvertToUserVariable(sender) == "Exact Match Only";
                bool caseSensitive = v_CaseSensitiveMatch.ConvertToUserVariable(sender) == "Yes";

                if (!caseSensitive)
                {
                    matchData = matchData.ToLower();
                    matchParam = matchParam.ToLower();
                }

                if ((exactMatchRequired && matchData == matchParam) || (!exactMatchRequired && matchData.Contains(matchParam)))
                {
                    //match was made
                    matchFound = true;
                    break;
                }                         
            }

            if (!matchFound)
            {
                throw new Exception("Unable to find the specified window!");
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);


            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_WindowMatchType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MatchSpecification", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CaseSensitiveMatch", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MatchParameter", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return $"{base.GetDisplayValue()} - [To {v_WindowMatchType} '{v_MatchParameter}', Instance Name: '{v_InstanceName}']";
        }
    }
}
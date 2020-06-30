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
    [Description("This command switches between browser windows provided a valid search parameter.")]

    public class SeleniumSwitchBrowserWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("MyBrowserInstance || {vBrowserInstance}")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Broswer Search Type")]
        [PropertyUISelectionOption("Window URL")]
        [PropertyUISelectionOption("Window Title")]
        [PropertyUISelectionOption("Handle ID")]
        [InputSpecification("Select an option which best fits the search type you would like to use.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_WindowMatchType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Match Specification")]
        [PropertyUISelectionOption("Exact Match")]
        [PropertyUISelectionOption("Contains Match")]
        [InputSpecification("Select whether the search parameter should match the window type exactly or just contain it.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_MatchSpecification { get; set; }

        [XmlAttribute]
        [PropertyDescription("Case-Sensitive")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Select whether the search parameter is case-sensitive or not.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_CaseSensitiveMatch { get; set; }

        [XmlAttribute]
        [PropertyDescription("Browser Search Parameter")]
        [InputSpecification("Provide the parameter to match (ex. Window URL, Window Title, Handle ID).")]
        [SampleUsage("http://www.url.com || Welcome to Homepage || {vSearchData}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MatchParameter { get; set; }

        public SeleniumSwitchBrowserWindowCommand()
        {
            CommandName = "SeleniumSwitchBrowserWindowCommand";
            SelectionName = "Switch Browser Window";  
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultBrowser";
            v_WindowMatchType = "Window URL";
            v_MatchSpecification = "Exact Match";
            v_CaseSensitiveMatch = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(sender);
            var browserObject = engine.GetAppInstance(vInstance);
            var seleniumInstance = (IWebDriver)browserObject;
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

                string matchData;

                switch (v_WindowMatchType)
                {
                    case "Window URL":
                        matchData = tempHandle.Url;
                        break;
                    case "Window Title":
                        matchData = tempHandle.Title;
                        break;
                    case "Handle ID":
                        matchData = tempHandle.CurrentWindowHandle;
                        break;
                    default:
                        throw new NotImplementedException($"Specified match type '{v_WindowMatchType}' is not supported for switching windows. " + 
                            "Use either 'Window URL' or 'Window Title'");
                }

                bool exactMatchRequired = v_MatchSpecification == "Exact Match Only";
                bool caseSensitive = v_CaseSensitiveMatch == "Yes";

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
                throw new Exception("Unable to find the specified window!");
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
            return base.GetDisplayValue() + $" [To {v_WindowMatchType} '{v_MatchParameter}' - Instance Name '{v_InstanceName}']";
        }
    }
}
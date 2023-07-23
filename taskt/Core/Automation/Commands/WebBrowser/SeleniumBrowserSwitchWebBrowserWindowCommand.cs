using System;
using System.Xml.Serialization;
using OpenQA.Selenium;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Web Browser Actions")]
    [Attributes.ClassAttributes.CommandSettings("Switch Web Browser Window")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserSwitchWebBrowserWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Type of Match to Make")]
        [PropertyUISelectionOption("Window URL")]
        [PropertyUISelectionOption("Window Title")]
        [PropertyUISelectionOption("Handle ID")]
        [InputSpecification("", true)]
        [PropertyFirstValue("Window URL")]
        [PropertyValidationRule("Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_WindowMatchType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Match Specification")]
        [PropertyUISelectionOption("Exact Match")]
        [PropertyUISelectionOption("Contains Match")]
        [InputSpecification("", true)]
        [PropertyIsOptional(true, "Exact Match")]
        [PropertyDisplayText(false, "")]
        public string v_MatchSpecification { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Search is Case-Sensitive")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("", true)]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "")]
        public string v_CaseSensitiveMatch { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Parameter to Match")]
        [PropertyDetailSampleUsage("**http://www.mysite.com**", PropertyDetailSampleUsage.ValueType.Value, "Parameter")]
        [PropertyDetailSampleUsage("**Welcome to Homepage**", PropertyDetailSampleUsage.ValueType.Value, "Parameter")]
        [PropertyDetailSampleUsage("**{{{vTitle}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Parameter")]
        [PropertyDetailSampleUsage("**{{{vURL}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Parameter", false)]
        [InputSpecification("Parameter to Match", true)]
        [Remarks("")]
        [PropertyValidationRule("Parameter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Parameter")]
        public string v_MatchParameter { get; set; }

        public SeleniumBrowserSwitchWebBrowserWindowCommand()
        {
            //this.CommandName = "SeleniumBrowserSwitchWindowCommand";
            //this.SelectionName = "Switch Browser Window";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //this.v_InstanceName = "";

            //set defaults
            //this.v_WindowMatchType = "Window URL";
            //this.v_MatchSpecification = "Exact Match";
            //this.v_CaseSensitiveMatch = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            var matchType = this.GetUISelectionValue(nameof(v_WindowMatchType), engine);

            var exactMatchRequired = this.GetUISelectionValue(nameof(v_MatchSpecification), engine);
            var caseSensitive = this.GetUISelectionValue(nameof(v_CaseSensitiveMatch), engine);

            Func<IWebDriver, string, bool> matchFunc = getMatchFunc(matchType, exactMatchRequired, caseSensitive);

            var matchParam = v_MatchParameter.ConvertToUserVariable(sender);
            var handles = seleniumInstance.WindowHandles;
            var currentHandle = seleniumInstance.CurrentWindowHandle;
            var matchFound = false;
            foreach (var hndl in handles)
            {
                var tempHandle = seleniumInstance.SwitchTo().Window(hndl);

                //array ordering is not guaranteed so skip if current window
                if (tempHandle.CurrentWindowHandle == currentHandle)
                {
                    continue;
                }

                matchFound = matchFunc(tempHandle, matchParam);
                if (matchFound)
                {
                    break;
                }
            }

            if (!matchFound)
            {
                throw new Exception("Unable to find the specified window!");
            }
        }

        private static Func<IWebDriver, string, bool> getMatchFunc(string targetType, string searchType, string caseSensitive)
        {
            Func<string, string> caseFunc;
            if (caseSensitive == "yes")
            {
                caseFunc = new Func<string, string>((str) =>
                {
                    return str;
                });
            }
            else
            {
                caseFunc = new Func<string, string>((str) =>
                {
                    return str.ToLower();
                });
            }

            Func<string, string, bool> compFunc = null;
            switch (searchType)
            {
                case "exact match":
                    compFunc = new Func<string, string, bool>( (a, b) => {
                        return (caseFunc(a) == caseFunc(b));
                    });
                    break;
                case "contains match":
                    compFunc = new Func<string, string, bool>((a, b) =>
                    {
                        return caseFunc(a).Contains(caseFunc(b));
                    });
                    break;
            }

            Func<IWebDriver, string, bool> retFunc = null;
            switch(targetType)
            {
                case "window url":
                    retFunc = new Func<IWebDriver, string, bool>((iw, str) =>
                    {
                        return compFunc(iw.Url, str);
                    });
                    break;
                case "window title":
                    retFunc = new Func<IWebDriver, string, bool>((iw, str) =>
                    {
                        return compFunc(iw.Title, str);
                    });
                    break;
                case "handle id":
                    retFunc = new Func<IWebDriver, string, bool>((iw, str) =>
                    {
                        return compFunc(iw.CurrentWindowHandle, str);
                    });
                    break;
            }
            return retFunc;
        }
    }
}
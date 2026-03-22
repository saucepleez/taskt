using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using OpenQA.Selenium;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.TextGroup;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Web Browser Actions")]
    [Attributes.ClassAttributes.CommandSettings("Switch Web Browser Window And Tab")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserSwitchWebBrowserWindowAndTabCommand : ASeleniumWebDriverActionCommands, ITextCheckProperties, ISelectionMethodProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Type of Match to Make")]
        [PropertyUISelectionOption("URL")]
        [PropertyUISelectionOption("Page Title")]
        [PropertyUISelectionOption("Handle")]
        [PropertyFirstValue("URL")]
        [PropertyValidationRule("Target", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Target")]
        [PropertyParameterOrder(6000)]
        public string v_CheckTarget { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("Match Specification")]
        //[PropertyUISelectionOption("Exact Match")]
        //[PropertyUISelectionOption("Contains")]
        //[PropertyIsOptional(true, "Exact Match")]
        //[PropertyDisplayText(false, "")]
        [PropertyVirtualProperty(nameof(VP_TextCheckMethodControls), nameof(VP_TextCheckMethodControls.v_CheckMethod))]
        [PropertyParameterOrder(7000)]
        public string v_CheckMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Text to Check")]
        [PropertyDetailSampleUsage("**http://www.mysite.com**", PropertyDetailSampleUsage.ValueType.Value, "Parameter")]
        [PropertyDetailSampleUsage("**Welcome to Homepage**", PropertyDetailSampleUsage.ValueType.Value, "Parameter")]
        [PropertyDetailSampleUsage("**{{{vTitle}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Parameter")]
        [PropertyDetailSampleUsage("**{{{vURL}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Parameter", false)]
        [InputSpecification("Parameter to Match", true)]
        [Remarks("")]
        [PropertyValidationRule("Parameter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Parameter")]
        [PropertyParameterOrder(8000)]
        public string v_CheckText { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("Search is Case-Sensitive")]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]
        //[PropertyIsOptional(true, "No")]
        //[PropertyDisplayText(false, "")]
        [PropertyVirtualProperty(nameof(VP_TextCheckMethodControls), nameof(VP_TextCheckMethodControls.v_CaseSensitiveNo))]
        [PropertyParameterOrder(8000)]
        public string v_CaseSensitive { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_TextCheckMethodControls), nameof(VP_TextCheckMethodControls.v_TrimBeforeCheck))]
        [PropertyParameterOrder(8100)]
        public string v_TrimBeforeCheck { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Selection Method for Window or Tab")]
        [PropertyUISelectionOption("First")]
        [PropertyUISelectionOption("Last")]
        [PropertyUISelectionOption("Index")]
        [PropertyDetailSampleUsage("**First**", "Specify the First Window or Tab")]
        [PropertyDetailSampleUsage("**Last**", "Specify the Last Window or Tab")]
        [PropertyDetailSampleUsage("**Index**", "the Window specifed by Index. **0** means First Window or Tab")]
        [PropertyIsOptional(true, "First")]
        [PropertyValidationRule("Selection Method", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Select")]
        [PropertyParameterOrder(8200)]
        public string v_SelectionMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Window or Tab Index")]
        [PropertyDetailSampleUsage("**0**", "Specify the First")]
        [PropertyDetailSampleUsage("**-1**", PropertyDetailSampleUsage.ValueType.Value, "Specify the Last")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Window or Tab Index")]
        [PropertyIsOptional(true, "0")]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Index")]
        [PropertyParameterOrder(8300)]
        public string v_SelectionIndex { get; set; }


        public SeleniumBrowserSwitchWebBrowserWindowAndTabCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WebDriverAction(new Action<IWebDriver>(seleniumInstance =>
            {
                //var matchType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_CheckTarget), engine);

                //var exactMatchRequired = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_CheckMethod), engine);
                //var caseSensitive = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_CaseSensitive), engine);

                //var matchFunc = GetMatchFunc(matchType, exactMatchRequired, caseSensitive);

                //var matchParam = v_CheckText.ExpandValueOrUserVariable(engine);
                //var handles = seleniumInstance.WindowHandles;
                //var currentHandle = seleniumInstance.CurrentWindowHandle;
                //var matchFound = false;
                //foreach (var hndl in handles)
                //{
                //    var tempHandle = seleniumInstance.SwitchTo().Window(hndl);

                //    // array ordering is not guaranteed so skip if current window
                //    if (tempHandle.CurrentWindowHandle == currentHandle)
                //    {
                //        continue;
                //    }

                //    matchFound = matchFunc(tempHandle, matchParam);
                //    if (matchFound)
                //    {
                //        break;
                //    }
                //}

                //if (!matchFound)
                //{
                //    throw new Exception("Unable to find the specified window!");
                //}

                var conditionText = this.ExpandValueOrUserVariable(nameof(v_CheckText), "Check Text", engine);

                // text check method
                var checkFunc = this.GetTextCheckFunction(engine);

                Func<string> valueFunc = null;
                switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_CheckTarget), engine))
                {
                    case "url":
                        valueFunc = new Func<string>(() => seleniumInstance.Url);
                        break;
                    case "page title":
                        valueFunc = new Func<string>(() => seleniumInstance.Title);
                        break;
                    case "handle":
                        valueFunc = new Func<string>(() => seleniumInstance.CurrentWindowHandle);
                        break;
                }

                var whnds = new List<string>(seleniumInstance.WindowHandles);
                //var currentHandle = seleniumInstance.CurrentWindowHandle;
                var handles = new List<string>();
                foreach (var h in whnds)
                {
                    seleniumInstance.SwitchTo().Window(h);
                    if (checkFunc(valueFunc(), conditionText))
                    {
                        handles.Add(h);
                    }
                }

                if (handles.Count == 0)
                {
                    throw new Exception($"Window or Tab does not Found. Name: '{v_CheckText}', Expand Value: '{conditionText}'");
                }

                var targetHandle = string.Empty;
                switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_SelectionMethod), engine))
                {
                    case "first":
                        targetHandle = handles[0];
                        break;
                    case "last":
                        targetHandle = handles[handles.Count - 1];
                        break;
                    case "index":
                        var index = this.ExpandValueOrUserVariableAsInteger(nameof(v_SelectionIndex), engine);
                        if (index < 0)
                        {
                            index += handles.Count;
                        }

                        if (index >= 0 && index < handles.Count)
                        {
                            targetHandle = handles[index];
                        }
                        else
                        {
                            throw new Exception($"Index is out of Range. Index: '{v_SelectionIndex}', Expand Value: '{index}'");
                        }
                        break;
                }
                seleniumInstance.SwitchTo().Window(targetHandle);
            }), engine);
        }

        //private static Func<IWebDriver, string, bool> GetMatchFunc(string targetType, string searchType, string caseSensitive)
        //{
        //    Func<string, string> caseFunc;
        //    if (caseSensitive == "yes")
        //    {
        //        caseFunc = new Func<string, string>((str) =>
        //        {
        //            return str;
        //        });
        //    }
        //    else
        //    {
        //        caseFunc = new Func<string, string>((str) =>
        //        {
        //            return str.ToLower();
        //        });
        //    }

        //    Func<string, string, bool> compFunc = null;
        //    switch (searchType)
        //    {
        //        case "exact match":
        //            compFunc = new Func<string, string, bool>( (a, b) => {
        //                return (caseFunc(a) == caseFunc(b));
        //            });
        //            break;
        //        case "contains":
        //            compFunc = new Func<string, string, bool>((a, b) =>
        //            {
        //                return caseFunc(a).Contains(caseFunc(b));
        //            });
        //            break;
        //    }

        //    Func<IWebDriver, string, bool> retFunc = null;
        //    switch(targetType)
        //    {
        //        case "url":
        //            retFunc = new Func<IWebDriver, string, bool>((iw, str) =>
        //            {
        //                return compFunc(iw.Url, str);
        //            });
        //            break;
        //        case "page title":
        //            retFunc = new Func<IWebDriver, string, bool>((iw, str) =>
        //            {
        //                return compFunc(iw.Title, str);
        //            });
        //            break;
        //        case "handle":
        //            retFunc = new Func<IWebDriver, string, bool>((iw, str) =>
        //            {
        //                return compFunc(iw.CurrentWindowHandle, str);
        //            });
        //            break;
        //    }
        //    return retFunc;
        //}
    }
}
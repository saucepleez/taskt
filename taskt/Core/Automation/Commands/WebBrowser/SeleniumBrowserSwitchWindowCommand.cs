using System;
using System.Xml.Serialization;
using OpenQA.Selenium;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserSwitchWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name (ex. myInstance, {{{vInstance}}})")]
        //[InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
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

        public SeleniumBrowserSwitchWindowCommand()
        {
            this.CommandName = "SeleniumBrowserSwitchWindowCommand";
            this.SelectionName = "Switch Browser Window";
            //this.v_InstanceName = "";
            this.CommandEnabled = true;
            this.CustomRendering = true;

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

                //string matchData = "";
                //switch(matchType)
                //{
                //    case "Window URL":
                //        matchData = tempHandle.Url;
                //        break;

                //    case "Window Title":
                //        matchData = tempHandle.Title;
                //        break;

                //    case "Handle ID":
                //        matchData = tempHandle.CurrentWindowHandle;
                //        break;
                //}

                //if (!caseSensitive)
                //{
                //    matchData = matchData.ToLower();
                //    matchParam = matchParam.ToLower();
                //}

                //if ((exactMatchRequired && matchData == matchParam) || (!exactMatchRequired && matchData.Contains(matchParam)))
                //{
                //    //match was made
                //    matchFound = true;
                //    break;
                //}

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


        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.WebBrowser);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_WindowMatchType", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MatchSpecification", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CaseSensitiveMatch", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MatchParameter", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return $"{base.GetDisplayValue()} - [To {v_WindowMatchType} '{v_MatchParameter}', Instance Name: '{v_InstanceName}']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);


        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_WindowMatchType))
        //    {
        //        this.validationResult += "Type of match is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
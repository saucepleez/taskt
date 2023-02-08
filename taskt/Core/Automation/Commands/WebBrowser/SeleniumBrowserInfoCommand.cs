using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session to a given URL or resource.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to navigate an existing Selenium instance to a known URL or web resource")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name (ex. myInstance, {{{vInstance}}})")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Information Type")]
        [PropertyUISelectionOption("Window Title")]
        [PropertyUISelectionOption("Window URL")]
        [PropertyUISelectionOption("Current Handle ID")]
        [PropertyUISelectionOption("HTML Page Source")]
        [PropertyUISelectionOption("Handle ID List")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [PropertyValidationRule("Information Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Information Type")]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the result")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public SeleniumBrowserInfoCommand()
        {
            this.CommandName = "SeleniumBrowserInfoCommand";
            this.SelectionName = "Get Browser Info";
            this.v_InstanceName = "";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var browserObject = engine.GetAppInstance(vInstance);

            //var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;
            //var requestedInfo = v_InfoType.ConvertToUserVariable(sender);
            //string info;

            var seleniumInstance = SeleniumBrowserControls.GetSeleniumBrowserInstance(v_InstanceName, engine);

            var requestedInfo = this.GetUISelectionValue(nameof(v_InfoType), engine);
            string info = "";
            switch (requestedInfo)
            {
                case "window title":
                    info = seleniumInstance.Title;
                    break;
                case "window url":
                    info = seleniumInstance.Url;
                    break;
                case "current handle id":
                    info = seleniumInstance.CurrentWindowHandle;
                    break;
                case "html page source":
                    info = seleniumInstance.PageSource;
                    break;
                case "handle id list":
                    info = Newtonsoft.Json.JsonConvert.SerializeObject(seleniumInstance.WindowHandles);
                    break;
            }

            //store data
            info.StoreInUserVariable(sender, v_applyToVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.WebBrowser);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_InfoType", this, editor));

        //    //apply to variable name
        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    var applyToVariableControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_applyToVariableName", this, applyToVariableControl, editor));
        //    RenderedControls.Add(applyToVariableControl);

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return $"{base.GetDisplayValue()} [Get '{v_InfoType}' and apply to '{v_applyToVariableName}', Instance Name: '{v_InstanceName}']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_InfoType))
        //    {
        //        this.validationResult += "Info property name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
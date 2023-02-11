using System;
using System.Xml.Serialization;
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
    public class SeleniumBrowserSwitchFrameCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name (ex. myInstance , {{{vInstance}}})")]
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
        [PropertyDescription("Frame Type")]
        [PropertyUISelectionOption("Index")]
        [PropertyUISelectionOption("Name or ID")]
        [PropertyUISelectionOption("Parent Frame")]
        [PropertyUISelectionOption("Default Content")]
        [PropertyUISelectionOption("Alert")]
        [InputSpecification("", true)]
        //[SampleUsage("")]
        [PropertyDetailSampleUsage("**Index**", "Specify Frame Index to Frame Search Parameter")]
        [PropertyDetailSampleUsage("**Name or ID**", "Specify Frame Name or ID to Frame Search Parameter")]
        [PropertyDetailSampleUsage("**Parent Frame**", "Switch to Parent Frame")]
        [PropertyDetailSampleUsage("**Default Content**", "Switch to Default Content")]
        [PropertyDetailSampleUsage("**Alert**", "Switch to Alert")]
        [Remarks("")]
        [PropertyFirstValue("Index")]
        [PropertyValidationRule("Frame Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Frame Type")]
        public string v_SelectionType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Frame Search Parameter")]
        [SampleUsage("Index: **0** or **{{{vIndex}}}**, Name/ID: **top** or **{{{vName}}}**")]
        [Remarks("If Frame Type is **Index** or **Name of ID**, please enter. If Frame Type is **Index**, default index is **0**.")]
        [PropertyIsOptional(true)]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "Frame")]
        public string v_FrameParameter { get; set; }

        public SeleniumBrowserSwitchFrameCommand()
        {
            this.CommandName = "SeleniumBrowserSwitchFrameCommand";
            this.SelectionName = "Switch Browser Frame";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var vInstance = v_InstanceName.ConvertToUserVariable(sender);
            //var browserObject = engine.GetAppInstance(vInstance);
            //var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;
            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            //var selectionType = v_SelectionType.ConvertToUserVariable(sender);
            var selectionType = this.GetUISelectionValue(nameof(v_SelectionType), engine);
            switch (selectionType)
            {
                case "index":
                    //var frameIndex = v_FrameParameter.ConvertToUserVariable(sender);
                    //int intFrameIndex;
                    //if (!int.TryParse(frameIndex, out intFrameIndex))
                    //{
                    //    intFrameIndex = 0;
                    //}
                    if (string.IsNullOrEmpty(v_FrameParameter))
                    {
                        v_FrameParameter = "0";
                    }
                    var frameIndex = this.ConvertToUserVariableAsInteger(nameof(v_FrameParameter), engine);
                    seleniumInstance.SwitchTo().Frame(frameIndex);
                    break;

                case "name or id":
                    var frameName = v_FrameParameter.ConvertToUserVariable(engine);
                    seleniumInstance.SwitchTo().Frame(frameName);
                    break;

                case "parent frame":
                    seleniumInstance.SwitchTo().ParentFrame();
                    break;

                case "default content":
                    seleniumInstance.SwitchTo().DefaultContent();
                    break;

                case "alert":
                    seleniumInstance.SwitchTo().Alert();
                    break;
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.WebBrowser);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SelectionType", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FrameParameter", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return $"{base.GetDisplayValue()} - [Find {v_SelectionType}, Instance Name: '{v_InstanceName}']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SelectionType))
        //    {
        //        this.validationResult += "Selection Type is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        switch (this.v_SelectionType)
        //        {
        //            case "Index":
        //                break;

        //            case "Name or ID":
        //                NameOrIDValidate();
        //                break;

        //            default:
        //                break;
        //        }
        //    }

        //    return this.IsValid;
        //}

        //private void NameOrIDValidate()
        //{
        //    if (String.IsNullOrEmpty(this.v_FrameParameter))
        //    {
        //        this.validationResult += "Frame Search Parameter is empty.\n";
        //        this.IsValid = false;
        //    }
        //}
    }
}
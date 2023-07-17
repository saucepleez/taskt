using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Web Browser Actions")]
    [Attributes.ClassAttributes.CommandSettings("Switch Web Browser Frame")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserSwitchWebBrowserFrameCommand : ScriptCommand
    {
        [XmlAttribute]
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

        public SeleniumBrowserSwitchWebBrowserFrameCommand()
        {
            //this.CommandName = "SeleniumBrowserSwitchFrameCommand";
            //this.SelectionName = "Switch Browser Frame";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            var selectionType = this.GetUISelectionValue(nameof(v_SelectionType), engine);
            switch (selectionType)
            {
                case "index":
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
    }
}
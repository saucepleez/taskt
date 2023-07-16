using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Text From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Text Value from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Text Value from UIElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetTextFromUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_TextVariable { get; set; }

        public UIAutomationGetTextFromUIElementCommand()
        {
            //this.CommandName = "UIAutomationGetTextFromElementCommand";
            //this.SelectionName = "Get Text From Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetUIElementVariable(engine);

            string res = UIElementControls.GetTextValue(targetElement);
            res.StoreInUserVariable(engine, v_TextVariable);
        }
    }
}
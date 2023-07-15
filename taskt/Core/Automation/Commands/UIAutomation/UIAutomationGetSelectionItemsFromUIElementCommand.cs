using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Selection Items From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Selection Items Name from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Selection Items Name from UIElement. Search for only Child Elements.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetSelectionItemsFromUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_ListVariable { get; set; }

        public UIAutomationGetSelectionItemsFromUIElementCommand()
        {
            //this.CommandName = "UIAutomationGetSelectionItemsFromElementCommand";
            //this.SelectionName = "Get Selection Items From Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetUIElementVariable(engine);

            var items = UIElementControls.GetSelectionItems(targetElement);

            List<string> res = new List<string>();
            foreach(var item in items)
            {
                res.Add(item.Current.Name);
            }
            res.StoreInUserVariable(engine, v_ListVariable);
        }
    }
}
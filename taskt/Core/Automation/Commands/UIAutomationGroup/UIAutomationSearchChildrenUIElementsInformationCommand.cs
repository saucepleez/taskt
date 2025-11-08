using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Search Children UIElements Information")]
    [Attributes.ClassAttributes.Description("This command allows you to get Children UIElements Information from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Children UIElements Information from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchChildrenUIElementsInformationCommand : ACoreSearchUIElementsFromUIElementByTreeWalkerCommands, IGetUIElementsInformationProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_SearchParameters))]
        //[PropertyParameterOrder(6000)]
        //public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6100)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeForUIElement))]
        //[PropertyParameterOrder(7100)]
        //public string v_WaitTimeForUIElement { get; set; }

        public UIAutomationSearchChildrenUIElementsInformationCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.UIElementAction(engine, new Action<AutomationElement>((targetElement) =>
            {
                var elems = this.SearchChildrenUIElements(targetElement, engine);

                //string result = "";

                //int counts = elems.Count;
                //for (int i = 0; i < counts; i++)
                //{
                //    var elem = elems[i];
                //    result += $"Index: {i}, Name: {elem.Current.Name}, LocalizedControlType: {elem.Current.LocalizedControlType}, ControlType: {EM_CanHandleUIElementExtentionMethods.GetControlTypeText(elem)}\n";
                //}
                //result.Trim().StoreInUserVariable(engine, v_Result);

                this.StoreUIElementsInformationInUserVariable(elems, engine);
            }));
        }
    }
}
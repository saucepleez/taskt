using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement By XPath")]
    [Attributes.ClassAttributes.CommandSettings("Search UIElements Tree XML From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Search UIElements Tree XML from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Search UIElements Tree XML from UIElement. XML content is based on WinAppDriver UI Recorder.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementsTreeXMLFromUIElementCommand : ADoSomethingUIElementCommands, IUIElementChildrenSearchSomewayProperties, IUIElementDescendantsSearchSomewayProperties, IResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to store XML")]
        [PropertyDetailSampleUsage("**vXML**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vXML}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("XML content is based on WinAppDriver UI Recorder.")]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeForUIElement))]
        [PropertyParameterOrder(7990)]
        public string v_WaitTimeForUIElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxSiblings))]
        [PropertyParameterOrder(7991)]
        public string v_MaxSiblings { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxDepth))]
        [PropertyParameterOrder(7992)]
        public string v_MaxDepth { get; set; }

        public UIAutomationSearchUIElementsTreeXMLFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);
            ////var xml = AutomationElementControls.GetElementXml(targetElement, out _);
            //(var xml, _) = UIElementControls.GetElementXml(targetElement);
            //using(System.IO.StringWriter sw = new System.IO.StringWriter())
            //{
            //    xml.Save(sw);
            //    sw.ToString().StoreInUserVariable(engine, v_Result);
            //}

            var maxTime = this.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);
            var finishTime = (maxTime > 0) ? DateTime.Now.AddSeconds(maxTime) : DateTime.Now;
            Func<bool> timeFunc =  (maxTime <= 0) ? 
                            new Func<bool>(() => false) :
                            new Func<bool>(() =>
                            {
                                return (DateTime.Now >= finishTime);
                            });

            var targetElement = this.ExpandUserVariableAsUIElement(engine);
            var cmd = new UIAutomationSearchUIElementFromUIElementByXPathCommand()
            {
                v_MaxDepth = this.v_MaxDepth,
                v_MaxSiblings = this.v_MaxSiblings,
                v_WaitTimeForUIElement = this.v_WaitTimeForUIElement,
                v_WindowNameResult = this.v_WindowNameResult,
                v_WindowHandleResult = this.v_WindowHandleResult,
            };
            (var xml, _) = cmd.DeepCreateUIElementXMLCore(targetElement, timeFunc, engine);

            using(var sw = new System.IO.StringWriter())
            {
                xml.Save(sw);
                sw.ToString().StoreInUserVariable(engine, v_Result);
            }

            cmd.StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(targetElement, engine);
        }
    }
}
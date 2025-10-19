using System;
using System.Collections.Generic;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Selection Items Value From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Selection Items Name from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Selection Items Name from UIElement. Search for only Child Elements.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetSelectionItemsValueFromUIElementCommand : AGetFromUIElementCommands, IUIElementSelectionItemsProperties, ICanHandleList
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        public UIAutomationGetSelectionItemsValueFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            //var items = UIElementControls.GetSelectionItems(targetElement);

            //var res = new List<string>();
            //foreach(var item in items)
            //{
            //    res.Add(item.Current.Name);
            //}
            //this.StoreListInUserVariable(res, nameof(v_Result), engine);

            this.SelectionItemsAction(engine,
                new Action<List<AutomationElement>>((items) =>
                {
                    var res = new List<string>();
                    foreach (var item in items)
                    {
                        res.Add(item.Current.Name);
                    }
                    this.StoreListInUserVariable(res, nameof(v_Result), engine);
                }),
                new Action(()=>
                {
                    this.ValueCanNotRetrievedProcess("Selection Items", new Action(() =>
                    {
                        (new List<string>()).StoreInUserVariable(engine, v_Result);
                    }), engine);
                })
            );
        }
    }
}
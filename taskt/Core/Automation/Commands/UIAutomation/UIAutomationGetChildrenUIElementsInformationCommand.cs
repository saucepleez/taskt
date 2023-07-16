using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Windows.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Children Elements Information")]
    [Attributes.ClassAttributes.Description("This command allows you to get Children UIElements Information from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Children UIElements Information from UIElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetChildrenUIElementsInformationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_RootElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_ResultVariable { get; set; }

        public UIAutomationGetChildrenUIElementsInformationCommand()
        {
            //this.CommandName = "UIAutomationGetChildrenElementsInformationCommand";
            //this.SelectionName = "Get Children Elements Information";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_RootElement.GetUIElementVariable(engine);

            var elems = UIElementControls.GetChildrenUIElements(targetElement, v_SearchParameters, engine);

            string result = "";

            int counts = elems.Count;
            for (int i = 0; i < counts; i++)
            {
                var elem = elems[i];
                result += "Index: " + i + ", Name: " + elem.Current.Name + ", LocalizedControlType: " + elem.Current.LocalizedControlType + ", ControlType: " + UIElementControls.GetControlTypeText(elem.Current.ControlType) + "\n";
            }
            result.Trim().StoreInUserVariable(engine, v_ResultVariable);
        }

        public override void AfterShown()
        {
            //AutomationElementControls.RenderSearchParameterDataGridView((DataGridView)ControlsList[nameof(v_SearchParameters)]);
            UIElementControls.RenderSearchParameterDataGridView(ControlsList.GetPropertyControl<DataGridView>(nameof(v_SearchParameters)));
        }
    }
}
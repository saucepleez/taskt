using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Property Value From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Property Value from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Property Value from UIElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetPropertyValueFromUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Property Name")]
        [PropertyUISelectionOption("Name")]
        [PropertyUISelectionOption("ControlType")]
        [PropertyUISelectionOption("LocalizedControlType")]
        [PropertyUISelectionOption("IsEnabled")]
        [PropertyUISelectionOption("IsOffscreen")]
        [PropertyUISelectionOption("IsKeyboardFocusable")]
        [PropertyUISelectionOption("HasKeyboardFocusable")]
        [PropertyUISelectionOption("AccessKey")]
        [PropertyUISelectionOption("ProcessId")]
        [PropertyUISelectionOption("AutomationId")]
        [PropertyUISelectionOption("FrameworkId")]
        [PropertyUISelectionOption("ClassName")]
        [PropertyUISelectionOption("IsContentElement")]
        [PropertyUISelectionOption("IsPassword")]
        [PropertyUISelectionOption("AcceleratorKey")]
        [PropertyUISelectionOption("HelpText")]
        [PropertyUISelectionOption("IsControlElement")]
        [PropertyUISelectionOption("IsRequiredForForm")]
        [PropertyUISelectionOption("ItemStatus")]
        [PropertyUISelectionOption("ItemType")]
        [PropertyUISelectionOption("NativeWindowHandle")]
        [PropertyValidationRule("Property", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Property")]
        public string v_PropertyName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public UIAutomationGetPropertyValueFromUIElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetUIElementVariable(engine);

            var propName = this.GetUISelectionValue(nameof(v_PropertyName), engine);
            switch (propName)
            {
                case "name":
                    propName = "Name";
                    break;
                case "controltype":
                    propName = "ControlType";
                    break;
                case "localizedcontroltype":
                    propName = "LocalizedControlType";
                    break;
                case "isenabled":
                    propName = "IsEnabled";
                    break;
                case "isoffscreen":
                    propName = "IsOffscreen";
                    break;
                case "iskeyboardfocusable":
                    propName = "IsKeyboardFocusable";
                    break;
                case "haskeyboardfocusable":
                    propName = "HasKeyboardFocusable";
                    break;
                case "accesskey":
                    propName = "AccessKey";
                    break;
                case "processid":
                    propName = "ProcessId";
                    break;
                case "automationid":
                    propName = "AutomationId";
                    break;
                case "frameworkid":
                    propName = "FrameworkId";
                    break;
                case "classname":
                    propName = "ClassName";
                    break;
                case "iscontentelement":
                    propName = "IsContentElement";
                    break;
                case "ispassword":
                    propName = "IsPassword";
                    break;
                case "acceleratorkey":
                    propName = "AcceleratorKey";
                    break;
                case "helptext":
                    propName = "HelpText";
                    break;
                case "iscontrolelement":
                    propName = "IsControlElement";
                    break;
                case "isrequiredforform":
                    propName = "IsRequiredForForm";
                    break;
                case "itemstatus":
                    propName = "ItemStatus";
                    break;
                case "itemtype":
                    propName = "ItemType";
                    break;
                case "nativewindowhandle":
                    propName = "NativeWindowHandle";
                    break;
            }

            string v;
            if (propName == "ControlType")
            {
                var c = (ControlType)targetElement.Current.GetType().GetProperty(propName).GetValue(targetElement.Current);
                v = UIElementControls.GetControlTypeText(c);
            }
            else
            {
                v = targetElement.Current.GetType().GetProperty(propName)?.GetValue(targetElement.Current)?.ToString() ?? "";
            }

            v.StoreInUserVariable(engine, v_Result);
        }
    }
}
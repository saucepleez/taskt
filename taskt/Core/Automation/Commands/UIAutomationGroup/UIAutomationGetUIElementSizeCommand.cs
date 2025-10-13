using System;
using System.Windows;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get UIElement Size")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement Size.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement Size.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetUIElementSizeCommand : AGetFromUIElementCommands, ISizeProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Width")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Width")]
        [PropertyParameterOrder(6000)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Height")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Height")]
        [PropertyParameterOrder(6100)]
        public string v_Height { get; set; }

        public UIAutomationGetUIElementSizeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            //var rct = targetElement.Current.BoundingRectangle;
            //if (!string.IsNullOrEmpty(v_Width))
            //{
            //    rct.Width.StoreInUserVariable(engine, v_Width);
            //}
            //if (!string.IsNullOrEmpty(v_Height))
            //{
            //    rct.Height.StoreInUserVariable(engine, v_Height);
            //}

            this.UIElementAction(engine,
                new Action<AutomationElement>((targetElement) =>
                {
                    Rect rct;
                    try
                    {
                        rct = targetElement.Current.BoundingRectangle;
                    }
                    catch
                    {
                        this.ValueCanNotRetrievedProcess("Size", new Action(() =>
                        {
                            if (!string.IsNullOrEmpty(v_Width))
                            {
                                "".StoreInUserVariable(engine, v_Width);
                            }
                            if (!string.IsNullOrEmpty(v_Height))
                            {
                                "".StoreInUserVariable(engine, v_Height);
                            }
                        }) , engine);
                        return;
                    }

                    if (!string.IsNullOrEmpty(v_Width))
                    {
                        rct.Width.StoreInUserVariable(engine, v_Width);
                    }
                    if (!string.IsNullOrEmpty(v_Height))
                    {
                        rct.Height.StoreInUserVariable(engine, v_Height);
                    }
                })
            );
        }
    }
}
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
    [Attributes.ClassAttributes.CommandSettings("Get UIElement Position")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement Position.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement Position.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetUIElementPositionCommand : AGetFromUIElementCommands, IPositionProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store X Position")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "X")]
        [PropertyParameterOrder(6000)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Y Position")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Y Position", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Y Position")]
        [PropertyParameterOrder(6100)]
        public string v_YPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Base position")]
        [PropertyUISelectionOption("Top Left")]
        [PropertyUISelectionOption("Bottom Right")]
        [PropertyUISelectionOption("Top Right")]
        [PropertyUISelectionOption("Bottom Left")]
        [PropertyUISelectionOption("Center")]
        [PropertyIsOptional(true, "Top Left")]
        [PropertyParameterOrder(6300)]
        public string v_PositionBase { get; set; }

        public UIAutomationGetUIElementPositionCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            //var rct = targetElement.Current.BoundingRectangle;

            //double x = 0.0, y = 0.0;
            //switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionBase), engine))
            //{
            //    case "top left":
            //        x = rct.Left;
            //        y = rct.Top;
            //        break;
            //    case "bottom right":
            //        x = rct.Right;
            //        y = rct.Bottom;
            //        break;
            //    case "top right":
            //        x = rct.Right;
            //        y = rct.Top;
            //        break;
            //    case "bottom left":
            //        x = rct.Left;
            //        y = rct.Bottom;
            //        break;
            //    case "center":
            //        x = (rct.Right - rct.Left) / 2.0;
            //        y = (rct.Bottom - rct.Top) / 2.0;
            //        break;
            //}

            //if (!string.IsNullOrEmpty(v_XPosition))
            //{
            //    x.StoreInUserVariable(engine, v_XPosition);
            //}
            //if (!string.IsNullOrEmpty(v_YPosition))
            //{
            //    y.StoreInUserVariable(engine, v_YPosition);
            //}

            this.UIElementAction(engine, 
                new Action<AutomationElement>((targetElement) => {
                    Rect rct;
                    try
                    {
                        rct = targetElement.Current.BoundingRectangle;
                    }
                    catch
                    {
                        this.ValueCanNotRetrievedProcess("Position", new Action(() =>
                        {
                            if (!string.IsNullOrEmpty(v_XPosition))
                            {
                                "".StoreInUserVariable(engine, v_XPosition);
                            }
                            if (!string.IsNullOrEmpty(v_YPosition))
                            {
                                "".StoreInUserVariable(engine, v_YPosition);
                            }
                        }), engine);
                        return;
                    }

                    double x = 0.0, y = 0.0;
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionBase), engine))
                    {
                        case "top left":
                            x = rct.Left;
                            y = rct.Top;
                            break;
                        case "bottom right":
                            x = rct.Right;
                            y = rct.Bottom;
                            break;
                        case "top right":
                            x = rct.Right;
                            y = rct.Top;
                            break;
                        case "bottom left":
                            x = rct.Left;
                            y = rct.Bottom;
                            break;
                        case "center":
                            x = (rct.Right - rct.Left) / 2.0;
                            y = (rct.Bottom - rct.Top) / 2.0;
                            break;
                    }

                    if (!string.IsNullOrEmpty(v_XPosition))
                    {
                        x.StoreInUserVariable(engine, v_XPosition);
                    }
                    if (!string.IsNullOrEmpty(v_YPosition))
                    {
                        y.StoreInUserVariable(engine, v_YPosition);
                    }
                })
            );
        }
    }
}
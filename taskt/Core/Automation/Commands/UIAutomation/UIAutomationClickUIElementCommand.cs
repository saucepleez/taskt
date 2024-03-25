﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("UIElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Click UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Click UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Click UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationClickUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_MouseClickType))]
        public string v_ClickType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_XOffsetAdjustment))]
        public string v_XOffset { get; set; }
        
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_YOffsetAdjustment))]
        public string v_YOffset { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Activate Window before Click")]
        [PropertyIsOptional(true, "Yes")]
        public string v_ActivateWindow { get; set; }

        public UIAutomationClickUIElementCommand()
        {
            //this.CommandName = "UIAutomationClickElementCommand";
            //this.SelectionName = "Click Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            string windowName = UIElementControls.GetWindowName(targetElement);
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActivateWindow), engine))
            {
                var activateWindow = new ActivateWindowCommand()
                {
                    v_WindowName = windowName
                };
                activateWindow.RunCommand(engine);
            }

            System.Windows.Point point;
            try
            {
                if (!targetElement.TryGetClickablePoint(out point))
                {
                    var moveWindow = new MoveWindowCommand()
                    {
                        v_WindowName = windowName,
                        v_XPosition = "0",
                        v_YPosition = "0"
                    };
                    moveWindow.RunCommand(engine);
                    targetElement.TryGetClickablePoint(out point);
                }
                if ((point.X < 0.0) || (point.Y < 0.0))
                {
                    var moveWindow = new MoveWindowCommand()
                    {
                        v_WindowName = windowName,
                        v_XPosition = "0",
                        v_YPosition = "0"
                    };
                    moveWindow.RunCommand(engine);

                    if (!targetElement.TryGetClickablePoint(out point))
                    {
                        throw new Exception("No Clickable Point in UIElement '" + v_TargetElement + "'");
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("No Clickable Point in UIElement '" + v_TargetElement + "'");
            }

            var click = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ClickType), engine);
            var xAd = this.ExpandValueOrUserVariableAsInteger(nameof(v_XOffset), engine);
            var yAd = this.ExpandValueOrUserVariableAsInteger(nameof(v_YOffset), engine);

            var mouseClick = new MoveMouseCommand()
            {
                v_MouseClick = click,
                v_XMousePosition = (point.X + xAd).ToString(),
                v_YMousePosition = (point.Y + yAd).ToString()
            };
            mouseClick.RunCommand(engine);
        }
    }
}
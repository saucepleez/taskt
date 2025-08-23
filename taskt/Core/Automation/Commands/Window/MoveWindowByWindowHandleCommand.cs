using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Window Handle Actions")]
    [Attributes.ClassAttributes.CommandSettings("Move Window By Window Handle")]
    [Attributes.ClassAttributes.Description("This command moves a window to a specified location on screen.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move an existing window by name to a certain point on the screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class MoveWindowByWindowHandleCommand : AWindowHandleActionBaseCommands, IWindowPositionProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("X horizontal coordinate (pixel) for the Window's Location")]
        //[InputSpecification("X Window Location", true)]
        //[PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        //[PropertyDetailSampleUsage("**0**", "Specify X Top Position")]
        //[PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "X Position")]
        //[PropertyDetailSampleUsage("**{{{vXPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "X Position")]
        //[PropertyDetailSampleUsage("**%kwd_current_position%**", "Specify Current Position for X Position")]
        //[PropertyDetailSampleUsage("**%kwd_current_xposition%**", "Specify Current X Position for X Position", false)]
        //[PropertyDetailSampleUsage("**%kwd_current_yposition%**", "Specify Current Y Position for X Position", false)]
        //[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        //[PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "X Position")]
        //[PropertyIntermediateConvert(nameof(ApplicationSettings.EngineSettings.convertToIntermediateWindowPosition), nameof(ApplicationSettings.EngineSettings.convertToRawWindowPosition))]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputXPosition))]
        [PropertyParameterOrder(5500)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Y vertical coordinate (pixel) for the Window's Location")]
        //[InputSpecification("Y Window Location", true)]
        //[PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        //[PropertyDetailSampleUsage("**0**", "Specify Y Left Position")]
        //[PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Y Position")]
        //[PropertyDetailSampleUsage("**{{{vYPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Y Position")]
        //[PropertyDetailSampleUsage("**%kwd_current_position%**", "Specify Current Position for Y Position")]
        //[PropertyDetailSampleUsage("**%kwd_current_xposition%**", "Specify Current X Position for Y Position", false)]
        //[PropertyDetailSampleUsage("**%kwd_current_yposition%**", "Specify Current Y Position for Y Position", false)]
        //[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        //[PropertyValidationRule("Y Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Y Position")]
        //[PropertyIntermediateConvert(nameof(ApplicationSettings.EngineSettings.convertToIntermediateWindowPosition), nameof(ApplicationSettings.EngineSettings.convertToRawWindowPosition))]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputYPosition))]
        [PropertyParameterOrder(5500)]
        public string v_YPosition { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForSet))]
        [PropertyParameterOrder(9000)]
        public string v_WhenWindowIsMinimized { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMaximizedForSet))]
        [PropertyParameterOrder(9001)]
        public string v_WhenWindowIsMaximized { get; set; }

        //public string v_WaintTimeFindAndAction {get; set;}

        public MoveWindowByWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowControls.WindowHandleAction(this, engine,
                new Action<IntPtr>(whnd =>
                {
                    var xPos = this.ExpandValueOrVariableAsWindowXPosition(whnd, engine);
                    var yPos = this.ExpandValueOrVariableAsWindowYPosition(whnd, engine);

                    WindowControls.SetWindowPosition(whnd, xPos, yPos);
                })
            );

            void MoveWindowProcess(IntPtr wh)
            {
                var xPos = this.ExpandValueOrVariableAsWindowXPosition(wh, engine);
                var yPos = this.ExpandValueOrVariableAsWindowYPosition(wh, engine);

                EM_WindowPositionPropertiesExtensionMethods.MoveWindow(wh, xPos, yPos);
            }

            void RestoreWindowProcess(IntPtr wh)
            {
                var restoreCommand = new SetWindowStateByWindowHandleCommand()
                {
                    v_WindowHandle = wh.ToString(),
                    v_WindowState = "Restore",
                };
                restoreCommand.RunCommand(engine);
            }

            this.WindowHandleActionBeforeWait(engine, new Action<IntPtr>((whnd) =>
            {
                if (EM_CanHandleWindowHandleExtentionMethods.IsWindowMinimized(whnd))
                {
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenWindowIsMinimized), engine))
                    {
                        case "execute":
                            MoveWindowProcess(whnd);
                            return;
                        case "ignore":
                            return;
                        case "error":
                            throw new Exception($"Error. Target Window is Minimized. Handle: '{v_WindowHandle}', Expand Value: '{whnd}'");

                        case "restore":
                            RestoreWindowProcess(whnd);
                            break;
                    }
                }

                if (EM_CanHandleWindowHandleExtentionMethods.IsWindowMaximized(whnd))
                {
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenWindowIsMinimized), engine))
                    {
                        case "execute":
                            MoveWindowProcess(whnd);
                            return;
                        case "ignore":
                            return;
                        case "error":
                            throw new Exception($"Error. Target Window is Maximized. Handle: '{v_WindowHandle}', Expand Value: '{whnd}'");

                        case "restore":
                            RestoreWindowProcess(whnd);
                            break;
                    }
                }

                MoveWindowProcess(whnd);
            }));
        }
    }
}
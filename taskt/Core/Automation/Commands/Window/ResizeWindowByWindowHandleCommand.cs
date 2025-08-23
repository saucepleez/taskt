using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Window Handle Actions")]
    [Attributes.ClassAttributes.CommandSettings("Resize Window By Window Handle")]
    [Attributes.ClassAttributes.Description("This command resizes a window to a specified size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reize a window by name to a specific size on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ResizeWindowByWindowHandleCommand : AWindowHandleActionBaseCommands, IWindowResizeProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Window Width (Pixcel)")]
        //[InputSpecification("Window Width", true)]
        //[PropertyDetailSampleUsage("**640**", PropertyDetailSampleUsage.ValueType.Value, "Width")]
        //[PropertyDetailSampleUsage("**{{{vWidth}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Width")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        //[PropertyDisplayText(true, "Width")]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputWidth))]
        [PropertyParameterOrder(5500)]
        public string v_Width { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Window Height (Pixcel)")]
        //[InputSpecification("Window Height", true)]
        //[PropertyDetailSampleUsage("**480**", PropertyDetailSampleUsage.ValueType.Value, "Height")]
        //[PropertyDetailSampleUsage("**{{{vHeight}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Height")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        //[PropertyDisplayText(true, "Height")]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputHeight))]
        [PropertyParameterOrder(5500)]
        public string v_Height { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForSet))]
        [PropertyParameterOrder(9000)]
        public string v_WhenWindowIsMinimized { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForSet))]
        [PropertyParameterOrder(9001)]
        public string v_WhenWindowIsMaximized { get; set; }

        public ResizeWindowByWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //WindowControls.WindowHandleAction(this, engine,
            //    new Action<IntPtr>(whnd =>
            //    {
            //        var width = this.ExpandValueOrVariableAsWindowWidth(whnd, engine);
            //        var height = this.ExpandValueOrVariableAsWindowHeight(whnd, engine);

            //        WindowControls.SetWindowSize(whnd, width, height);
            //    })
            //);

            void ResizeWindowProcess(IntPtr wh)
            {
                var width = this.ExpandValueOrVariableAsWindowWidth(wh, engine);
                var height = this.ExpandValueOrVariableAsWindowHeight(wh, engine);

                EM_WindowSizePropertiesExtensionMethods.ResizeWindow(wh, width, height);
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
                            ResizeWindowProcess(whnd);
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
                            ResizeWindowProcess(whnd);
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

                ResizeWindowProcess(whnd);
            }));
        }
    }
}
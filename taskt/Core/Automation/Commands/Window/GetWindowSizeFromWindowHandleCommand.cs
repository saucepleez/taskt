using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Handle")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Size From Window Handle")]
    [Attributes.ClassAttributes.Description("This command returns window size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want window size.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowSizeFromWindowHandleCommand : AWindowHandleCommands, IWindowSizeProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Width")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Width")]
        [PropertyParameterOrder(5500)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Height")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Height")]
        [PropertyParameterOrder(5501)]
        public string v_Height { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForGet))]
        [PropertyParameterOrder(9000)]
        public string v_WhenWindowIsMinimized {  get; set; }

        public GetWindowSizeFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            void GetWindowSizeProcess(IntPtr h)
            {
                (var width, var height) = EM_WindowSizePropertiesExtensionMethods.GetWindowSize(h);
                if (!string.IsNullOrEmpty(v_Width))
                {
                    width.StoreInUserVariable(engine, v_Width);
                }
                if (!string.IsNullOrEmpty(v_Height))
                {
                    height.StoreInUserVariable(engine, v_Height);
                }
            }

            this.GetWindowHandle(engine, new Action<IntPtr>((whnd) =>
            {
                if (EM_CanHandleWindowHandleExtentionMethods.IsWindowMinimized(whnd))
                {
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenWindowIsMinimized), engine))
                    {
                        case "execute":
                            GetWindowSizeProcess(whnd);
                            break;
                        case "restore":
                            var setRestore = new SetWindowStateByWindowHandleCommand
                            {
                                v_WindowHandle = whnd.ToString(),
                                v_WindowState = "Restore",
                            };
                            setRestore.RunCommand(engine);
                            GetWindowSizeProcess(whnd);
                            break;
                        case "set zero":
                            if (!string.IsNullOrEmpty(v_Width))
                            {
                                0.StoreInUserVariable(engine, v_Width);
                            }
                            if (!string.IsNullOrEmpty(v_Height))
                            {
                                0.StoreInUserVariable(engine, v_Height);
                            }
                            break;
                        case "ignore":
                            break;
                        case "error":
                            throw new Exception($"Error. Target Window is Minimized. Handle: '{v_WindowHandle}', Expand Value: '{whnd}'");
                    }
                }
                else
                {
                    GetWindowSizeProcess(whnd);
                }
            }));
        }
    }
}
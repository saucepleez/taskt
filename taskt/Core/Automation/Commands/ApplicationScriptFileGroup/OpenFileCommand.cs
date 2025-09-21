using System;
using System.Diagnostics;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("File/Folder")]
    [Attributes.ClassAttributes.CommandSettings("Open File")]
    [Attributes.ClassAttributes.Description("This command opens the specified file")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to open the specified file")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class OpenFileCommand : AFileExistsFilePathPathResultCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time until Folder Opens")]
        [PropertyIsOptional(true, "2")]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyFirstValue("2")]
        [PropertyDisplayText(true, "Wait Time for Open")]
        [PropertyParameterOrder(7000)]
        public string v_WaitTimeForOpen { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowNameResult))]
        [PropertyParameterOrder(20100)]
        public string v_WindowNameResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        [PropertyParameterOrder(20200)]
        public string v_WindowHandleResult { get; set; }

        [XmlAttribute]
        [PropertyDisplayText(true, "Wait Time for File")]
        public override string v_WaitTimeForFile { get; set; }

        public OpenFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.FileAction(engine,
                new Func<string, string>(path =>
                {
                    using (var p = new Process())
                    {
                        p.StartInfo = new ProcessStartInfo()
                        {
                            UseShellExecute = true,
                            FileName= path,
                        };
                        p.Start();

                        // wait time to open
                        var wait = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeForOpen), engine);
                        System.Threading.Thread.Sleep(wait * 1000);

                        string newWindowName;
                        IntPtr newWindowHandle;
                        if (p.MainWindowHandle == IntPtr.Zero)
                        {
                            newWindowHandle = EM_CanHandleWindowHandleExtentionMethods.GetActiveWindowHandle();
                            using (var n = new InnerScriptVariable(engine))
                            {
                                var getName = new GetWindowNameFromWindowHandleCommand()
                                {
                                    v_WindowHandle = newWindowHandle.ToString(),
                                    v_Result = n.VariableName,
                                };
                                getName.RunCommand(engine);
                                newWindowName = n.VariableValue.ToString();
                            }
                        }
                        else
                        {
                            newWindowName = p.MainWindowTitle;
                            newWindowHandle = p.MainWindowHandle;
                        }

                        if (!string.IsNullOrEmpty(v_WindowNameResult))
                        {
                            newWindowName.StoreInUserVariable(engine, v_WindowNameResult);
                        }
                        if (!string.IsNullOrEmpty(v_WindowHandleResult))
                        {
                            newWindowHandle.StoreInUserVariable(engine, v_WindowHandleResult);
                        }
                    }
                    return path;
                })
            );
        }
    }
}

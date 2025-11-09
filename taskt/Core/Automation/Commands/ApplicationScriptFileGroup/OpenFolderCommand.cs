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
    [Attributes.ClassAttributes.CommandSettings("Open Folder")]
    [Attributes.ClassAttributes.Description("This command opens the specified folder")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to open the specified folder")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class OpenFolderCommand : AFolderExistsFolderPathPathResultCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time until Folder Opens")]
        [PropertyIsOptional(true, "1")]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyFirstValue("1")]
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
        [PropertyDisplayText(true, "Wait Time for Folder")]
        public override string v_WaitTimeForFolder { get; set; }

        /// <summary>
        /// explorer process name
        /// </summary>
        private const string ExplorerProcessName = "explorer";

        public OpenFolderCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.FolderAction(engine,
                new Func<string, string>(path =>
                {
                    var procInfo = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        Verb = "Open",
                        FileName = path,
                    };
                    Process.Start(procInfo);

                    // wait time to open
                    var wait = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeForOpen), engine);
                    System.Threading.Thread.Sleep(wait * 1000);

                    var currentHandle = EM_CanHandleWindowHandleExtentionMethods.GetActiveWindowHandle();

                    if (!string.IsNullOrEmpty(v_WindowNameResult))
                    {
                        using (var name = new InnerScriptVariable(engine))
                        {
                            var getName = new GetWindowNameFromWindowHandleCommand()
                            {
                                v_WindowHandle = currentHandle.ToString(),
                                v_WindowNameResult = name.VariableName,
                            };
                            getName.RunCommand(engine);
                            name.VariableValue.ToString().StoreInUserVariable(engine, v_WindowNameResult);
                        }
                    }
                    if (!string.IsNullOrEmpty(v_WindowHandleResult))
                    {
                        currentHandle.StoreInUserVariable(engine, v_WindowHandleResult);
                    }

                    return path;
                })
            );
        }
    }
}

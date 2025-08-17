using System;
using System.Diagnostics;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("Application")]
    [Attributes.ClassAttributes.CommandSettings("Start Application")]
    [Attributes.ClassAttributes.Description("This command allows you to start a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to start applications by entering their name such as 'chrome.exe' or a fully qualified path to a file 'c:/some.exe'")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start'.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_script))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class StartApplicationCommand : ScriptCommand, ICanHandleFilePath
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Application Path or Application Name")]
        [InputSpecification("Application Path or Name", true)]
        [PropertyDetailSampleUsage("**notepad**", "Run Notepad")]
        [PropertyDetailSampleUsage("**C:\\Apps\\myapp.exe**", PropertyDetailSampleUsage.ValueType.Value, "Path")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path")]
        [Remarks("Provide a valid program name or enter a full path to the script/executable including the extension.\nIf file does not contain folder path, this command do not supplement folder path.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyValidationRule("Application", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Application")]
        public string v_ApplicationPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Arguments")]
        [InputSpecification("Arguments", true)]
        [PropertyDetailSampleUsage("**-a**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        [PropertyDetailSampleUsage("**-verswion**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        [PropertyDetailSampleUsage("**{{{vArgs}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Arguments")]
        [Remarks("You will need to consult documentation to determine if your executable supports arguments or flags on startup.")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Arguments", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Arguments")]
        public string v_Arguments { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Wait for the Application to Complete")]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "")]
        public string v_WaitForExit { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Application Process Name")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Process Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_StartedProcessName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window Name")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Wait Time until Application Starts (ms)")]
        [PropertyIsOptional(true, "2000")]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyFirstValue("2000")]
        [PropertyDisplayText(false, "")]
        public string v_WaitTimeForExecute { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Wait Time before Executing Next Command (ms)")]
        [PropertyIsOptional(true, "2000")]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyFirstValue("2000")]
        [PropertyDisplayText(false, "")]
        public string v_WaitTimeBeforeNext { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        [PropertyDescription("Wait Time for the Appliaction to Exist (sec)")]
        public string v_WaitTimeForApplication { get; set; }

        public StartApplicationCommand()
        {
            //this.CommandName = "StartProcessCommand";
            //this.SelectionName = "Start Process";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            // local start process func
            Process StartProcess(string name, string arguments, bool shell)
            {
                //System.Diagnostics.Process proc;
                //if (string.IsNullOrEmpty(arguments))
                //{
                //    proc = System.Diagnostics.Process.Start(name);
                //}
                //else
                //{
                //    proc = System.Diagnostics.Process.Start(name, arguments);
                //}

                var proc = new Process();
                proc.StartInfo.FileName = name;
                if (!string.IsNullOrEmpty(arguments)) 
                {
                    proc.StartInfo.Arguments = arguments;
                }
                proc.StartInfo.UseShellExecute = shell;
                proc.Start();

                return proc;
            }

            var args = v_Arguments.ExpandValueOrUserVariable(engine);

            Process p;
            try
            {
                // consider the application name is specified
                var appName = v_ApplicationPath.ExpandValueOrUserVariable(engine);
                
                p = StartProcess(appName, args, false);
            }
            catch
            {
                // consider the application path is specified
                var filePath = this.ExpandValueOrUserVariableAsFilePath(nameof(v_ApplicationPath),
                                new PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "exe"), engine);
                using (var v = new InnerScriptVariable(engine))
                {
                    var fileExists = new WaitForFileToExistCommand()
                    {
                        v_TargetFilePath = filePath,
                        v_WaitTimeForFile = this.v_WaitTimeForApplication,
                        v_ResultPath = v.VariableName,
                    };
                    fileExists.RunCommand(engine);
                }
                p = StartProcess(filePath, args, false);
            }

            var waitTimeUntil = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeForExecute), engine);
            System.Threading.Thread.Sleep(waitTimeUntil);

            // process name
            if (!string.IsNullOrEmpty(v_StartedProcessName))
            {
                try
                {
                    p.ProcessName.StoreInUserVariable(engine, v_StartedProcessName);
                }
                catch
                {
                    throw new Exception("Error. Fail get Process Name.");
                }
            }
            // window name
            if (!string.IsNullOrEmpty(v_WindowName))
            {
                try
                {
                    p.MainWindowTitle.StoreInUserVariable(engine, v_WindowName);
                }
                catch
                {
                    throw new Exception("Error. Fail get Window Name.");
                }
            }
            // window handle
            if (!string.IsNullOrEmpty(v_WindowHandle))
            {
                try
                {
                    p.MainWindowHandle.StoreInUserVariable(engine, v_WindowHandle);
                }
                catch
                {
                    throw new Exception("Error. Fail get WindowHandle.");
                }
            }

            //var waitForExit = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WaitForExit), engine);
            //if (waitForExit == "yes")
            //{
            //    p.WaitForExit();
            //}

            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_WaitForExit), engine))
            {
                p.WaitForExit();
            }

            //System.Threading.Thread.Sleep(2000);
            var waitTimeBeforeNext = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeBeforeNext), engine);
            System.Threading.Thread.Sleep(waitTimeBeforeNext);
        }
    }
}
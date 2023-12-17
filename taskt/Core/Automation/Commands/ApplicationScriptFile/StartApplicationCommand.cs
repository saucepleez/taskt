using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script Commands")]
    [Attributes.ClassAttributes.SubGruop("Application")]
    [Attributes.ClassAttributes.CommandSettings("Start Application")]
    [Attributes.ClassAttributes.Description("This command allows you to start a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to start applications by entering their name such as 'chrome.exe' or a fully qualified path to a file 'c:/some.exe'")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start'.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_script))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class StartApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Path to the Application or Application Name")]
        [InputSpecification("Path", true)]
        [PropertyDetailSampleUsage("**notepad**", "Run Notepad")]
        [PropertyDetailSampleUsage("**C:\\Apps\\myapp.exe**", PropertyDetailSampleUsage.ValueType.Value, "Path")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path")]
        [Remarks("Provide a valid program name or enter a full path to the script/executable including the extension.\nIf file does not contain folder path, this command do not supplement folder path.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Path")]
        public string v_ProgramName { get; set; }

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
        public string v_ProgramArgs { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Wait for the Application to Complete")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
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
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_OutputWindowHandle))]
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

        public StartApplicationCommand()
        {
            //this.CommandName = "StartProcessCommand";
            //this.SelectionName = "Start Process";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var vProgramName = v_ProgramName.ExpandValueOrUserVariable(engine);
            var vProgramArgs = v_ProgramArgs.ExpandValueOrUserVariable(engine);

            System.Diagnostics.Process p;
            if (string.IsNullOrEmpty(vProgramArgs))
            {
                p = System.Diagnostics.Process.Start(vProgramName);
            }
            else
            {
                p = System.Diagnostics.Process.Start(vProgramName, vProgramArgs);
            }

            var waitTimeUntil = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeForExecute), engine);
            System.Threading.Thread.Sleep(waitTimeUntil);

            // process name
            if (!string.IsNullOrEmpty(v_StartedProcessName))
            {
                p.ProcessName.StoreInUserVariable(engine, v_StartedProcessName);
            }
            // window name
            if (!string.IsNullOrEmpty(v_WindowName))
            {
                p.MainWindowTitle.StoreInUserVariable(engine, v_WindowName);
            }
            // window handle
            if (!string.IsNullOrEmpty(v_WindowHandle))
            {
                p.MainWindowHandle.StoreInUserVariable(engine, v_WindowHandle);
            }

            var waitForExit = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WaitForExit), engine);

            if (waitForExit == "yes")
            {
                p.WaitForExit();
            }

            //System.Threading.Thread.Sleep(2000);
            var waitTimeBeforeNext = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeBeforeNext), engine);
            System.Threading.Thread.Sleep(waitTimeBeforeNext);
        }
    }
}
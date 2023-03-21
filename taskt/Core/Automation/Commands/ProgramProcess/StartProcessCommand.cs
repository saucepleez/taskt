using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.CommandSettings("Start Process")]
    [Attributes.ClassAttributes.Description("This command allows you to start a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to start applications by entering their name such as 'chrome.exe' or a fully qualified path to a file 'c:/some.exe'")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start'.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class StartProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Path to the Program")]
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
        [PropertyDisplayText(true, "Arguments")]
        public string v_ProgramArgs { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Wait for the Process to Complete")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyIsOptional(true, "No")]
        public string v_WaitForExit { get; set; }

        public StartProcessCommand()
        {
            //this.CommandName = "StartProcessCommand";
            //this.SelectionName = "Start Process";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            
            var vProgramName = v_ProgramName.ConvertToUserVariable(engine);
            var vProgramArgs = v_ProgramArgs.ConvertToUserVariable(engine);

            System.Diagnostics.Process p;
            if (String.IsNullOrEmpty(vProgramArgs))
            {
                p = System.Diagnostics.Process.Start(vProgramName);
            }
            else
            {
                p = System.Diagnostics.Process.Start(vProgramName, vProgramArgs);
            }

            var waitForExit = this.GetUISelectionValue(nameof(v_WaitForExit), engine);

            if (waitForExit == "Yes")
            {
                p.WaitForExit();
            }

            System.Threading.Thread.Sleep(2000);
        }
    }
}
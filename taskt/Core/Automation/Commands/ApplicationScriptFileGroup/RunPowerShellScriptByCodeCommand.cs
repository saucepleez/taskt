using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run PowerShell Script By Code")]
    [Attributes.ClassAttributes.Description("This command allows you to run a PowerShell Script by code.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a PowerShell Script by code.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_script))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RunPowerShellScriptByCodeCommand : ARunScriptByCodeCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        //[PropertyDescription("JavaScript Code")]
        [PropertyDetailSampleUsage("**New-Item -Type File sample.txt**", PropertyDetailSampleUsage.ValueType.Value, "PowerShell Script")]
        [PropertyDetailSampleUsage("**{{{vCode}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "PowerShell Script")]
        //[PropertyParameterOrder(5000)]
        public override string v_ScriptCode { get; set; }

        [XmlAttribute]
        [Remarks("Arguments are sent to the Script")]
        public override string v_Arguments { get; set; }

        //[XmlAttribute]
        //public string v_Result { get; set; }

        //[XmlAttribute]
        //public string v_DeleteScriptFile { get; set; }

        public RunPowerShellScriptByCodeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.RunScriptAction(
                new Func<string>(() => { return "ps1"; }), 
                new RunPowerShellScriptFileCommand()
                {
                    v_Arguments = this.v_Arguments,
                    v_Result = this.v_Result,
                    v_ExecutionMethod = "ExecutionPolicy Bypass",
                },
                engine);
        }
    }
}

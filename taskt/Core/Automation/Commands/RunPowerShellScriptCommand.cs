using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to run a script or program and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a script (such as vbScript, javascript, or executable) but wait for it to close before taskt continues executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    public class RunPowerShellScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the path to the PowerShell script")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a fully qualified path to the script, including the script extension.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myscript.ps1**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ScriptPath { get; set; }

        public RunPowerShellScriptCommand()
        {
            this.CommandName = "RunPowerShellScriptCommand";
            this.SelectionName = "Run PowerShell Script";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            {
                var scriptPath = v_ScriptPath.ConvertToUserVariable(sender);

                RunScript(scriptPath);
                
            }
        }

        public static ICollection<PSObject> RunScript(string scriptFullPath, ICollection<CommandParameter> parameters = null)
        {
            var runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            var pipeline = runspace.CreatePipeline();
            var cmd = new Command(scriptFullPath);
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
            }
            pipeline.Commands.Add(cmd);
            var results = pipeline.Invoke();
            pipeline.Dispose();
            runspace.Dispose();
            return results;
            
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ScriptPath", this, editor));



            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Script Path: " + v_ScriptPath + "]";
        }
    }
}
using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script Commands")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run CSharp Code")]
    [Attributes.ClassAttributes.Description("This command allows you to run C# code from the input")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run custom C# code commands.  The code in this command is compiled and run at runtime when this command is invoked.  This command only supports the standard framework classes.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RunCSharpCodeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("C# Code")]
        [InputSpecification("C# Code", true)]
        [SampleUsage("")]
        [Remarks("Enter the code to be executed or use the builder to create your custom C# code.  The builder contains a Hello World template that you can use to build from.")]
        [PropertyCustomUIHelper("Show Code Builder", nameof(lnkShowCodeBuilderLink_Clicked))]
        [PropertyValidationRule("C# Code", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(false, "")]
        [PropertyIntermediateConvert(nameof(ApplicationSettings.EngineSettings.convertToIntermediate), "")]
        public string v_Code { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Arguments")]
        [InputSpecification("Arguments", true)]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        [PropertyDetailSampleUsage("**1 2 3**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        [PropertyDetailSampleUsage("**{{{vArgs}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Arguments")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Arguments", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_Args { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Receive the Output")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Result")]
        public string v_applyToVariableName { get; set; }

        public RunCSharpCodeCommand()
        {
            //this.CommandName = "RunCustomCodeCommand";
            //this.SelectionName = "Run Custom Code";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var customCode = v_Code.ConvertToUserVariable(engine);

            //create compiler service
            var compilerSvc = new CompilerServices();

            //compile custom code
            var result = compilerSvc.CompileInput(customCode);

            //check for errors
            if (result.Errors.HasErrors)
            {
                //throw exception
                var errors = string.Join(", ", result.Errors);
                throw new Exception("Errors Occured: " + errors);
            }
            else
            {
                //run code, taskt will wait for the app to exit before resuming
                System.Diagnostics.Process scriptProc = new System.Diagnostics.Process();
                scriptProc.StartInfo.FileName = result.PathToAssembly;

                var arguments = v_Args.ConvertToUserVariable(sender);
                scriptProc.StartInfo.Arguments = arguments;

                if (!String.IsNullOrEmpty(v_applyToVariableName))
                {
                    //redirect output
                    scriptProc.StartInfo.RedirectStandardOutput = true;
                    scriptProc.StartInfo.UseShellExecute = false;
                }
                
                scriptProc.Start();

                scriptProc.WaitForExit();

                if (!String.IsNullOrEmpty(v_applyToVariableName))
                {
                    var output = scriptProc.StandardOutput.ReadToEnd();
                    output.StoreInUserVariable(sender, v_applyToVariableName);
                }

                scriptProc.Close();
            }
        }

        private void lnkShowCodeBuilderLink_Clicked(object sender, EventArgs e)
        {
            var targetTextbox = (TextBox)((CommandItemControl)sender).Tag;
            using (UI.Forms.Supplemental.frmCodeBuilder codeBuilder = new UI.Forms.Supplemental.frmCodeBuilder(targetTextbox.Text))
            {
                if (codeBuilder.ShowDialog() == DialogResult.OK)
                {
                    targetTextbox.Text = codeBuilder.rtbCode.Text;
                }
            }
        }
    }
}
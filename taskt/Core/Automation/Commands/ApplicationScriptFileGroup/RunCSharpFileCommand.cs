using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run CSharp File")]
    [Attributes.ClassAttributes.Description("This command allows you to run C# File")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_script))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RunCSharpFileCommand : ARunCSharpSomethingCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyFilePathSetting(true, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "cs")]
        [PropertyDescription("C# File Path")]
        [PropertyDetailSampleUsage("**C:\\temp\\mycode.cs**", PropertyDetailSampleUsage.ValueType.Value, "C# File")]
        [PropertyDetailSampleUsage("**{{{vSourcePath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "C# File")]
        [InputSpecification("C# File", true)]
        [PropertyValidationRule("C# File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "C# File")]
        [PropertyParameterOrder(5000)]
        public string v_TargetFilePath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ApplicationScriptControls), nameof(ApplicationScriptControls.v_Arguments))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Arguments")]
        //[InputSpecification("Arguments", true)]
        //[PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**1 2 3**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**{{{vArgs}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Arguments")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Arguments", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(6000)]
        //public string v_Arguments { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ApplicationScriptControls), nameof(ApplicationScriptControls.v_Result))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Receive the Output")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "Result")]
        //[PropertyParameterOrder(7000)]
        //public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(CSharpCodeCompilerControls), nameof(CSharpCodeCompilerControls.v_ExecutableFileName))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Compiled Executable File Name")]
        //[PropertyIsOptional(true, "tasktOnTheFly")]
        //[PropertyFirstValue("tasktOnTheFly")]
        //[PropertyValidationRule("File Name", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "File Name")]
        //[PropertyParameterOrder(8000)]
        //public string v_ExecutableFileName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(CSharpCodeCompilerControls), nameof(CSharpCodeCompilerControls.v_ExecutableFilePath))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Store Executable File Path")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Executable File Path", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "Executable File Path")]
        //[PropertyParameterOrder(9000)]
        //public string v_ExecutableFilePath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(CSharpCodeCompilerControls), nameof(CSharpCodeCompilerControls.v_CSharpLanguageVersion))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("C# Language Version")]
        //[PropertyUISelectionOption("default")]
        //[PropertyUISelectionOption("latest")]
        //[PropertyUISelectionOption("preview")]
        //[PropertyUISelectionOption("14.0")]
        //[PropertyUISelectionOption("13.0")]
        //[PropertyUISelectionOption("12.0")]
        //[PropertyUISelectionOption("11.0")]
        //[PropertyUISelectionOption("10.0")]
        //[PropertyUISelectionOption("9.0")]
        //[PropertyUISelectionOption("8.0")]
        //[PropertyUISelectionOption("7.3")]
        //[PropertyUISelectionOption("7.2")]
        //[PropertyUISelectionOption("7.1")]
        //[PropertyUISelectionOption("7")]
        //[PropertyUISelectionOption("6")]
        //[PropertyUISelectionOption("5")]
        //[PropertyUISelectionOption("4")]
        //[PropertyUISelectionOption("3")]
        //[PropertyUISelectionOption("2")]
        //[PropertyUISelectionOption("1")]
        //[PropertyIsOptional(true, "default")]
        //[PropertyFirstValue("default")]
        //[PropertyValidationRule("C# Language Version", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "C# Language Version")]
        //[Remarks("More Information: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version?WT.mc_id=AI-MVP-123445")]
        //[PropertyParameterOrder(10000)]
        //public string v_CSharpLanguageVersion { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(CSharpCodeCompilerControls), nameof(CSharpCodeCompilerControls.v_DeleteExecutableFile))]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        //[PropertyDescription("Delete Executable File After Execute")]
        //[PropertyIsOptional(true, "Yes")]
        //[PropertyFirstValue("Yes")]
        //[PropertyValidationRule("Delete Executable File", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(11000)]
        //public string v_DeleteExecutableFile { get; set; }

        public RunCSharpFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            string exePath;
            using (var exePathVar = new InnerScriptVariable(engine))
            {
                var compileCS = new CompileCSharpFileCommand()
                {
                    v_TargetFilePath = this.v_TargetFilePath,
                    v_ExecutableFileName = this.v_ExecutableFileName,
                    v_CSharpLanguageVersion = this.v_CSharpLanguageVersion,
                    v_ExecutableFilePath = exePathVar.VariableName,
                };
                compileCS.RunCommand(engine);

                exePath = exePathVar.VariableValue.ToString();
            }

            // run code, taskt will wait for the app to exit before resuming
            var scriptProc = new System.Diagnostics.Process();
            scriptProc.StartInfo.FileName = exePath;

            var arguments = v_Arguments.ExpandValueOrUserVariable(engine);
            scriptProc.StartInfo.Arguments = arguments;

            if (!string.IsNullOrEmpty(v_Result))
            {
                // redirect output
                scriptProc.StartInfo.RedirectStandardOutput = true;
                scriptProc.StartInfo.UseShellExecute = false;
            }

            scriptProc.Start();

            scriptProc.WaitForExit();

            if (!string.IsNullOrEmpty(v_Result))
            {
                var output = scriptProc.StandardOutput.ReadToEnd();
                output.StoreInUserVariable(engine, v_Result);
            }

            scriptProc.Close();

            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_DeleteExecutableFile), engine))
            {
                var deleteFile = new DeleteFileCommand()
                {
                    v_TargetFilePath = exePath,
                };
                deleteFile.RunCommand(engine);
            }

            if (!string.IsNullOrEmpty(v_ExecutableFilePath))
            {
                exePath.StoreInUserVariable(engine, v_ExecutableFilePath);
            }
        }
    }
}

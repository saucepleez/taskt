using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Compile CSharp File")]
    [Attributes.ClassAttributes.Description("This command allows you to compile C# file (*.cs) from the input")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to compile custom C# file (*.cs) commands.  The code in this command is compiled and run at runtime when this command is invoked.  This command only supports the standard framework classes.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_script))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CompileCSharpFileCommand : ACompileCSharpSomethingCommands, ICanHandleFileName, ICanHandleFilePath
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
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Compiled Executable File Name")]
        //[PropertyIsOptional(true, "tasktOnTheFly")]
        //[PropertyFirstValue("tasktOnTheFly")]
        //[PropertyValidationRule("File Name", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "File Name")]
        //public string v_ExecutableFileName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Store Executable File Path")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Executable File Path", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "Executable File Path")]
        //public string v_ExecutableFilePath { get; set; }

        //[XmlAttribute]
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
        //public string v_CSharpLanguageVersion { get; set; }

        public CompileCSharpFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            string filePath = this.ExpandValueOrUserVariableAsFilePath(nameof(v_TargetFilePath), engine);

            var fileName = this.ExpandValueOrUserVariableAsFileName(nameof(v_ExecutableFileName), engine);
            var langVer = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_CSharpLanguageVersion), engine);

            // compile custom code
            var result = CSharpCodeCompilerControls.CompileCSFile(filePath, langVer, fileName);

            // check for errors
            if (result.Errors.HasErrors)
            {
                string errors = "";
                foreach(CompilerError er in result.Errors)
                {
                    errors += $"{er.ErrorText}, ";
                }
                errors = errors.Trim().Substring(0, errors.Length - 2);

                throw new Exception($"Compile Error. Errors Occured: {errors}");
            }
            else
            {
                if (!string.IsNullOrEmpty(v_ExecutableFilePath))
                {
                    result.PathToAssembly.StoreInUserVariable(engine, v_ExecutableFilePath);
                }
            }
        }
    }
}

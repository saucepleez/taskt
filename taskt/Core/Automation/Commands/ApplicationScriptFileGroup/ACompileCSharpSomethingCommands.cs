using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for compile CSharp file or code commands
    /// </summary>
    public abstract class ACompileCSharpSomethingCommands : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(CSharpCodeCompilerControls), nameof(CSharpCodeCompilerControls.v_ExecutableFileName))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Compiled Executable File Name")]
        //[PropertyIsOptional(true, "tasktOnTheFly")]
        //[PropertyFirstValue("tasktOnTheFly")]
        //[PropertyValidationRule("File Name", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "File Name")]
        [PropertyParameterOrder(8000)]
        public virtual string v_ExecutableFileName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(CSharpCodeCompilerControls), nameof(CSharpCodeCompilerControls.v_ExecutableFilePath))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Store Executable File Path")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Executable File Path", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "Executable File Path")]
        [PropertyParameterOrder(9000)]
        public virtual string v_ExecutableFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(CSharpCodeCompilerControls), nameof(CSharpCodeCompilerControls.v_CSharpLanguageVersion))]
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
        [PropertyParameterOrder(10000)]
        public virtual string v_CSharpLanguageVersion { get; set; }
    }
}

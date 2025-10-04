using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for run script file ocmmands
    /// </summary>
    public abstract class ARunScriptFileCommands : AScriptFileCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        //public override string v_TargetFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ApplicationScriptControls), nameof(ApplicationScriptControls.v_Arguments))]
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
        [PropertyParameterOrder(6000)]
        public virtual string v_Arguments { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ApplicationScriptControls), nameof(ApplicationScriptControls.v_Result))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Receive the Output")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "Result")]
        [PropertyParameterOrder(8000)]
        public virtual string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }
    }
}
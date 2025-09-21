using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation")]
    [Attributes.ClassAttributes.CommandSettings("Get Last Existent Numbered File Path")]
    [Attributes.ClassAttributes.Description("This command allows you to get a last Numbered File Path that Exists.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a last Numbered File Path that Exists.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetLastExistentNumberedFilePathCommand : AFileNumberedFileCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //public string v_TargetFolderPath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("File Name Before Counter")]
        //[Remarks("")]
        //[PropertyIsOptional(true)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDetailSampleUsage("**log_**", PropertyDetailSampleUsage.ValueType.Value, "File Name Before Counter")]
        //[PropertyDetailSampleUsage("**{{{vPreFix}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name Before Counter")]
        //[PropertyValidationRule("Before", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "Before")]
        //public string v_BeforeFileCounter { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("File Counter Digits")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "3")]
        //[PropertyFirstValue("3")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDetailSampleUsage("**3**", PropertyDetailSampleUsage.ValueType.Value, "File Counter")]
        //[PropertyDetailSampleUsage("**{{{vDigits}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Counter")]
        //[PropertyValidationRule("Digits", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        //[PropertyDisplayText(true, "Digits")]
        //public string v_Digits { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("File Name After Counter")]
        //[Remarks("")]
        //[PropertyIsOptional(true)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDetailSampleUsage("**_log**", PropertyDetailSampleUsage.ValueType.Value, "File Name After Counter")]
        //[PropertyDetailSampleUsage("**{{{vPostFix}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name After Counter")]
        //[PropertyValidationRule("After", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "After")]
        //public string v_AfterFileCounter { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("File Extension")]
        //[Remarks("")]
        //[PropertyIsOptional(true)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDetailSampleUsage("**txt**", "Specify Text File")]
        //[PropertyDetailSampleUsage("**{{{vExtension}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name After Counter")]
        //[PropertyValidationRule("Extension", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "Extension")]
        //public string v_Extension { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Counter Start Value")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "1")]
        //[PropertyFirstValue("1")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Start Value")]
        //[PropertyDetailSampleUsage("**{{{vStart}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Start Value")]
        //[PropertyValidationRule("Start", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "Start Value")]
        //public string v_StartValue { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        //[PropertyFirstValue("0")]
        //public string v_WaitTimeForFolder { get; set; }

        public GetLastExistentNumberedFilePathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var attr = new PropertyFilePathSetting(true, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.LastExists);
            this.SearchNonExistentFileAction(attr, engine);
        }
    }
}
﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Run Macro")]
    [Attributes.ClassAttributes.Description("This command runs a macro.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a run a specific macro in the Excel workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelRunMacroCommand : AExcelInstanceCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Macro Name")]
        [InputSpecification("Macro Name", true)]
        //[SampleUsage("**Macro1** or **Module1.Macro1** or **{{{vMacro}}}**")]
        [PropertyDetailSampleUsage("**Macro1**", PropertyDetailSampleUsage.ValueType.Value, "Macro")]
        [PropertyDetailSampleUsage("**Module1.Macro1**", PropertyDetailSampleUsage.ValueType.Value, "Macro")]
        [PropertyDetailSampleUsage("**{{{vMacro}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Macro")]
        [PropertyValidationRule("Macro", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Macro")]
        [PropertyParameterOrder(6000)]
        public string v_MacroName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the macro argument1")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the value of the macro argument")]
        [SampleUsage("**1** or **Hello** or **{{{vArgument}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyParameterOrder(6001)]
        public string v_Argument1 { get; set; }

        public ExcelRunMacroCommand()
        {
            //this.CommandName = "ExcelRunMacroCommand";
            //this.SelectionName = "Run Macro";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var excelInstance = v_InstanceName.ExpandValueOrUserVariableAsExcelInstance(engine);
            var excelInstance = this.ExpandValueOrVariableAsExcelInstance(engine);

            var vMacroName = v_MacroName.ExpandValueOrUserVariable(engine);

            var vArg1 = v_Argument1.ExpandValueOrUserVariable(engine);

            if (string.IsNullOrEmpty(vArg1))
            {
                excelInstance.Run(vMacroName);
            }
            else
            {
                excelInstance.Run(vMacroName, vArg1);
            }
        }
    }
}
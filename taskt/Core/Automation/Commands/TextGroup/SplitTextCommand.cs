using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Split Text")]
    [Attributes.ClassAttributes.Description("This command allows you to split a Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to split a single Text or variable into multiple items")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Split method to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SplitTextCommand : ScriptCommand, ICanHandleList
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delimiter")]
        [InputSpecification("Delimiter", true)]
        [PropertyDetailSampleUsage("**,**", PropertyDetailSampleUsage.ValueType.Value, "Delimiter")]
        [PropertyDetailSampleUsage("**{{{Char.NewLine}}}**", "Specify **Line Break** for Delimiter")]
        [PropertyDetailSampleUsage("**{{{TextSplit.Charactor}}}**", "Split by one character")]
        [PropertyDetailSampleUsage("**{{{vChar}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Delimiter")]
        [PropertyDetailSampleUsage("**{{{Char.Space}}}**", "Split by **Space**. Please specify **Disable Auto Calculation** before this command.", false)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyAvailableSystemVariable(Engine.SystemVariables.LimitedSystemVariableNames.Text_Split)]
        [PropertyValidationRule("Delimiter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Delimiter")]
        public string v_splitCharacter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_applyConvertToUserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Remove Empty Item")]
        [PropertyIsOptional(true, "No")]
        [PropertyValidationRule("Remove Empty Item", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_RemoveEmptyItem { get; set; }

        public SplitTextCommand()
        {
            //this.CommandName = "SplitTextCommand";
            //this.SelectionName = "Split Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var stringVariable = v_userVariableName.ExpandValueOrUserVariable(engine);

            List<string> splitString;
            if (v_splitCharacter == VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Text_Split_Charactor.VariableName, engine))
            {
                splitString = stringVariable.ToCharArray().Select(c => c.ToString()).ToList();
            }
            else
            {
                var split = v_splitCharacter.ExpandValueOrUserVariable(engine);
                //switch (split)
                //{
                //    case "[crLF]":
                //        splitString = stringVariable.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                //        break;
                //    case "[chars]":
                //        splitString = stringVariable.ToCharArray().Select(c => c.ToString()).ToList();
                //        break;
                //    default:
                //        splitString = stringVariable.Split(new string[] { split }, StringSplitOptions.None).ToList();
                //        break;
                //}
                splitString = stringVariable.Split(new string[] { split }, StringSplitOptions.None).ToList();
            }
            
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_RemoveEmptyItem), engine))
            {
                for (int i = splitString.Count - 1; i >= 0; i--) 
                {
                    if (string.IsNullOrEmpty(splitString[i]))
                    {
                        splitString.RemoveAt(i);
                    }
                }
            }

            //splitString.StoreInUserVariable(engine, v_applyConvertToUserVariableName);
            this.StoreListInUserVariable(splitString, nameof(v_applyConvertToUserVariableName), engine);
        }
    }
}
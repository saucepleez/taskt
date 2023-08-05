using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Split Text")]
    [Attributes.ClassAttributes.Description("This command allows you to split a Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to split a single Text or variable into multiple items")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Split method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SplitTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delimiter")]
        [InputSpecification("Delimiter", true)]
        [PropertyDetailSampleUsage("**,**", PropertyDetailSampleUsage.ValueType.Value, "Delimiter")]
        [PropertyDetailSampleUsage("**[crLF]**", "Specify **Line Break** for Delimiter")]
        [PropertyDetailSampleUsage("**[chars]**", "Split by one character")]
        [PropertyDetailSampleUsage("**{{{vChar}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Delimiter")]
        [PropertyDetailSampleUsage("**{{{Char.Space}}}**", "Split by **Space**. Please specify **Disable Auto Calculation** before this command.", false)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Delimiter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Delimiter")]
        public string v_splitCharacter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_applyConvertToUserVariableName { get; set; }

        public SplitTextCommand()
        {
            //this.CommandName = "SplitTextCommand";
            //this.SelectionName = "Split Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var stringVariable = v_userVariableName.ConvertToUserVariable(engine);

            var split = v_splitCharacter.ConvertToUserVariable(engine);
            List<string> splitString;

            switch (split)
            {
                case "[crLF]":
                    splitString = stringVariable.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                    break;
                case "[chars]":
                    splitString = stringVariable.ToCharArray().Select(c => c.ToString()).ToList();
                    break;
                default:
                    splitString = stringVariable.Split(new string[] { split }, StringSplitOptions.None).ToList();
                    break;
            }

            splitString.StoreInUserVariable(engine, v_applyConvertToUserVariableName);
        }
    }
}
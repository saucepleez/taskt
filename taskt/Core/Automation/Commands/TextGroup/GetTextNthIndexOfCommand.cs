using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text")]
    [Attributes.ClassAttributes.SubGruop("Check/Get")]
    [Attributes.ClassAttributes.CommandSettings("Get Text Nth Index Of")]
    [Attributes.ClassAttributes.Description("This command allows you to the nth position of the Specified Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to the nth Position of the Specified Text")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetTextNthIndexOfCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        public string v_Text { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Search Text")]
        [InputSpecification("Search Text", true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Search Text")]
        [PropertyDetailSampleUsage("**{{{vSearch}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Search Text")]
        [PropertyValidationRule("Search Text", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Search Text")]
        public string v_SearchText { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("nth Value")]
        [InputSpecification("nth Value", true)]
        [PropertyDetailSampleUsage("**0**", "Specify First Position")]
        [PropertyDetailSampleUsage("**1**", "Specify Second Position")]
        [PropertyDetailSampleUsage("**-1**", "Specify Last Position")]
        [PropertyDetailSampleUsage("**{{{vPosition}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "nth Value")]
        [PropertyValidationRule("nth Value", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "nth Value")]
        public string v_NthValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextCompareSelectMethodControls), nameof(TextCompareSelectMethodControls.v_CaseSensitiveYes))]
        public string v_CaseSensitive { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When nth Does Not Exists")]
        [PropertyIsOptional(true, "Get -1")]
        [PropertyUISelectionOption("Get -1")]
        [PropertyUISelectionOption("Rise a Error")]
        [PropertyDetailSampleUsage("Get -1", "Get **-1**")]
        [PropertyDetailSampleUsage("Rise a Error", "Rise a Error")]
        [PropertyValidationRule("When nth Does Not Exists", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_WhenNthNotExists { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Greedly Match")]
        [PropertyIsOptional(true, "No")]
        [PropertyValidationRule("Greedly Match", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_GreedlyMatch { get; set; }

        public GetTextNthIndexOfCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine) 
        {
            var targetText = this.ExpandValueOrUserVariable(nameof(v_Text), "Text", engine);

            var searchText = this.ExpandValueOrUserVariable(nameof(v_SearchText), "Search Text", engine);

            // case sensitive
            if (!this.ExpandValueOrUserVariableAsYesNo(nameof(v_CaseSensitive), engine))
            {
                targetText = targetText.ToLower();
                searchText = searchText.ToLower();
            }

            // get positions
            var pos = TextControls.SearchTextPositions(targetText, searchText, this.ExpandValueOrUserVariableAsYesNo(nameof(v_GreedlyMatch), engine));

            var nth = this.ExpandValueOrUserVariableAsInteger(nameof(v_NthValue), engine);
            if (nth < 0)
            {
                nth += pos.Count;
            }

            if (nth >= 0 && nth < pos.Count)
            {
                pos[nth].StoreInUserVariable(engine, v_Result);
            }
            else
            {
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenNthNotExists), engine))
                {
                    case "get -1":
                        "-1".StoreInUserVariable(engine, v_Result);
                        break;
                    default:
                        throw new Exception($"Index Not Found. Position: '{v_NthValue}', Expand Value: '{nth}'");
                }
            }
        }
    }
}

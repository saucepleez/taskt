using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text")]
    [Attributes.ClassAttributes.SubGruop("Check/Get")]
    [Attributes.ClassAttributes.CommandSettings("Count Word In Text")]
    [Attributes.ClassAttributes.Description("This command allows you to Count the Number of Words In Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Count the Number of Words In Text")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CountWordInTextCommand : ScriptCommand
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
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextCompareSelectMethodControls), nameof(TextCompareSelectMethodControls.v_CaseSensitiveYes))]
        public string v_CaseSensitive { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Greedly Match")]
        [PropertyIsOptional(true, "No")]
        [PropertyValidationRule("Greedly Match", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_GreedlyMatch { get; set; }

        public CountWordInTextCommand()
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

            pos.Count.StoreInUserVariable(engine, v_Result);
        }
    }
}

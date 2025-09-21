using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text")]
    [Attributes.ClassAttributes.SubGruop("Check/Get")]
    [Attributes.ClassAttributes.CommandSettings("Get Text Index Of")]
    [Attributes.ClassAttributes.Description("This command allows you to the First Position of the Specified Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to the First Position of the Specified Text")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetTextIndexOfCommand : ScriptCommand
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
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Search Start Position")]
        [InputSpecification("Search Start Position")]
        [PropertyIsOptional(true, "0")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyFirstValue("0")]
        [PropertyDetailSampleUsage("**0**", "Specify First Charactor Position")]
        [PropertyDetailSampleUsage("**-1**", "Specify Last Charactor Position")]
        [PropertyDetailSampleUsage("**{{{vPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Search Start Position")]
        [PropertyDisplayText(false, "Search Start Position")]
        public string v_SearchStartPosition { get; set; }

        public GetTextIndexOfCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine) 
        {
            var targetText = this.ExpandValueOrUserVariable(nameof(v_Text), "Text", engine);

            var searchText = this.ExpandValueOrUserVariable(nameof(v_SearchText), "Search Text", engine);

            if (string.IsNullOrEmpty(v_SearchStartPosition))
            {
                v_SearchStartPosition = "0";
            }
            var searchStartPosition = this.ExpandValueOrUserVariableAsInteger(nameof(v_SearchStartPosition), engine);
            if (searchStartPosition < 0)
            {
                searchStartPosition += targetText.Length;
            }

            if (searchStartPosition < 0)
            {
                searchStartPosition = 0;
            }
            else if (searchStartPosition >= targetText.Length)
            {
                searchStartPosition = targetText.Length;
            }

            if (!this.ExpandValueOrUserVariableAsYesNo(nameof(v_CaseSensitive), engine))
            {
                targetText = targetText.ToLower();
                searchText = searchText.ToLower();
            }

            int idx;
            if (searchStartPosition == 0)
            {
                idx = targetText.IndexOf(searchText);
            }
            else
            {   // (searchStartPosition > 0 && searchStartPosition < targetText.Length)
                idx = targetText.IndexOf(searchText, searchStartPosition);
            }
            idx.StoreInUserVariable(engine, v_Result);
        }
    }
}

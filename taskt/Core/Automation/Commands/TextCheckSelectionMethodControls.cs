using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public static class TextCheckSelectionMethodControls
    {
        /// <summary>
        /// text check method
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Check Method")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Exact match")]
        [PropertyUISelectionOption("Not Contains")]
        [PropertyUISelectionOption("Not Starts with")]
        [PropertyUISelectionOption("Not Ends with")]
        [PropertyUISelectionOption("Not Match")]
        [PropertyUISelectionOption("Wildcard")]
        [PropertyUISelectionOption("Not Wildcard")]
        [PropertyUISelectionOption("Not Empty")]
        [PropertyUISelectionOption("Is Number")]
        [PropertyUISelectionOption("Is Boolean")]
        [PropertyUISelectionOption("Is Boolean Loose")]
        [PropertyUISelectionOption("Is Empty")]
        [PropertyUISelectionOption("Is Not Number")]
        [PropertyUISelectionOption("Is Not Boolean")]
        [PropertyUISelectionOption("Is Not Boolean Loose")]
        [PropertyIsOptional(true, "Contains")]
        [PropertyDetailSampleUsage("**Contains**", "It's like Comparing whether to Contains **hello**.")]
        [PropertyDetailSampleUsage("**Starts with**", "It's like Comparing whether to Starts With **hello**.")]
        [PropertyDetailSampleUsage("**Ends with**", "It's like Comparing whether to Ends With **hello**.")]
        [PropertyDetailSampleUsage("**Exact match**", "It's like Comparing whether an Exact match to **hello**.")]
        [PropertyDetailSampleUsage("**Not Contains**", "It's like Comparing whether to Not Contains **hello**.")]
        [PropertyDetailSampleUsage("**Not Starts with**", "It's like Comparing whether to Not Starts With **hello**.")]
        [PropertyDetailSampleUsage("**Not Ends with**", "It's like Comparing whether to Not Ends With **hello**.")]
        [PropertyDetailSampleUsage("**Not Match**", "It's like Comparing whether an Not Matche to **hello**.")]
        [PropertyDetailSampleUsage("**Not Empty**", "This determines not empty text.")]
        [PropertyDetailSampleUsage("**Wildcard**", "Check for Wildcard match.")]
        [PropertyDetailSampleUsage("**Not Wildcard**", "Check for Wildcard Not match.")]
        [PropertyDetailSampleUsage("**Is Number**", "This determines whether a number.")]
        [PropertyDetailSampleUsage("**Is Boolean**", "This determines whether a boolean, such as **True** or **False**.")]
        [PropertyDetailSampleUsage("**Is Boolean Loose**", "This determines whether it is a loose boolean, such as **True**, **False**, **Yes**, **No**, **1**, or **0**.")]
        [PropertyDetailSampleUsage("**Is Empty**", "This determines empty text.")]
        [PropertyDetailSampleUsage("**Is Not Number**", "This determines whether it is **Not** a number.")]
        [PropertyDetailSampleUsage("**Is Not Boolean**", "This determines whether it is **Not** a boolean, such as **True** or **False**.")]
        [PropertyDetailSampleUsage("**Is Not Boolean Loose**", "This determines whether it is **Not** a loose boolean, such as **True**, **False**, **Yes**, **No**, **1**, or **0**.")]
        [PropertyDisplayText(true, "Check Method")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[InputSpecification("", true)]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(false)]
        //[PropertyParameterOrder(5000)]
        public static string v_CheckMethod { get; }

        /// <summary>
        /// check method is case sensitive or not
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Case Sensitive")]
        [PropertyIsOptional(true, "No")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**Yes**", "Check Method is Case Sensitive")]
        [PropertyDetailSampleUsage("**No**", "Check Method is NOT Case Sensitive")]
        [PropertyDisplayText(false, "Case Sensitive")]
        //[InputSpecification("", true)]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]
        //[PropertyParameterOrder(5000)]
        public static string v_CaseSensitiveNo { get; }

        /// <summary>
        /// check method is case sensitive or not
        /// </summary>
        [PropertyVirtualProperty(nameof(TextCheckSelectionMethodControls), nameof(TextCheckSelectionMethodControls.v_CaseSensitiveNo))]
        [PropertyIsOptional(true, "Yes")]
        //[PropertyDescription("Case Sensitive")]
        //[InputSpecification("", true)]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]
        //[PropertyDetailSampleUsage("**Yes**", "Comparison Method is Case Sensitive")]
        //[PropertyDetailSampleUsage("**No**", "Comparison Method is NOT Case Sensitive")]
        //[PropertyDisplayText(false, "Case Sensitive")]
        //[PropertyParameterOrder(5000)]
        public static string v_CaseSensitiveYes { get; }

        /// <summary>
        /// trim before check
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Trim Before Check")]
        [PropertyUISelectionOption("Trim")]
        [PropertyUISelectionOption("Trim Start")]
        [PropertyUISelectionOption("Trim End")]
        [PropertyUISelectionOption("No")]
        [PropertyIsOptional(true, "No")]
        [PropertyDetailSampleUsage("**Trim**", "Remove White Space at the Start and End of Text")]
        [PropertyDetailSampleUsage("**Trim Start**", "Remove White Space at the Start of Text")]
        [PropertyDetailSampleUsage("**Trim End**", "Remove White Space at the End of Text")]
        [PropertyDetailSampleUsage("**No**", "Not Trim")]
        [PropertyDisplayText(false, "Trim Before Check")]
        //[InputSpecification("", true)]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyParameterOrder(5000)]
        public static string v_TrimBeforeCheck { get; }

        /// <summary>
        /// selection method, select one item
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Selection Method")]
        [PropertyUISelectionOption("First")]
        [PropertyUISelectionOption("Last")]
        [PropertyUISelectionOption("Index")]
        [PropertyDetailSampleUsage("**First**", "Specify the First Item")]
        [PropertyDetailSampleUsage("**Last**", "Specify the Last Item")]
        [PropertyDetailSampleUsage("**Index**", "the Item specifed by Index. **0** means First Item")]
        [PropertyIsOptional(true, "First")]
        [PropertyDisplayText(false, "Selection Method")]
        //[InputSpecification("", true)]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyParameterOrder(5000)]
        public static string v_SelectionMethod { get; }

        /// <summary>
        /// selection item index
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Selection Item Index")]
        [InputSpecification("Selection Item Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specify the First Item")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Item Index")]
        //[PropertyIsOptional(true, "0")]
        //[PropertyFirstValue("0")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyParameterOrder(5000)]
        public static string v_SelectionItemIndex { get; }
    }
}

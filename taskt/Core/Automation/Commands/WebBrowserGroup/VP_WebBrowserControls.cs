using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class VP_WebBrowserControls
    {
        /// <summary>
        /// WebBrowser instance name
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_InputInstanceName))]
        [PropertyDescription("WebBrowser Instance Name")]
        [InputSpecification("WebBrowser Instance Name", true)]
        [PropertyDetailSampleUsage("**RPABrowser**", PropertyDetailSampleUsage.ValueType.Value, "WebBrowser Instance")]
        [PropertyDetailSampleUsage("**{{{vInstance}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebBrowser Instance")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Broser** command will cause an error")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        [PropertyValidationRule("WebBrowser Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        [PropertyFirstValue("%kwd_default_browser_instance%")]
        public static string v_InputInstanceName { get; }

        /// <summary>
        /// WebBrowser Instance name
        /// </summary>
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_InputInstanceName))]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        public static string v_OutputInstanceName { get; }

        /// <summary>
        /// input WebElement variable name
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_InputInstanceName))]
        [PropertyDescription("WebElement Variable Name")]
        [InputSpecification("WebElement Variable Name", true)]
        [PropertyDetailSampleUsage("**vElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WebElement, true)]
        [PropertyValidationRule("WebElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "WebElement")]
        public static string v_InputWebElementName { get; }

        /// <summary>
        /// output WebElement property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store WebElement")]
        [InputSpecification("WebElement Variable Name", true)]
        [PropertyDetailSampleUsage("**vElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WebElement, true)]
        [PropertyValidationRule("WebElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "WebElement")]
        public static string v_OutputWebElementName { get; }

        /// <summary>
        /// behavior when fail action
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnore))]
        [PropertyDescription("When Fail Action")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "When Fail Action")]
        public static string v_WhenFailAction { get; }

        /// <summary>
        /// scroll to WebElement before Action
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Scroll to WebElement")]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "Scroll")]
        public static string v_ScrollToWebElement { get; }

        /// <summary>
        /// WebElements search method property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("WebElement Search Method")]
        [PropertyUISelectionOption("Find Elements By XPath")]
        [PropertyUISelectionOption("Find Elements By ID")]
        [PropertyUISelectionOption("Find Elements By Name")]
        [PropertyUISelectionOption("Find Elements By Tag Name")]
        [PropertyUISelectionOption("Find Elements By Class Name")]
        [PropertyUISelectionOption("Find Elements By CSS Selector")]
        [PropertyUISelectionOption("Find Elements By Link Text")]
        [PropertyUISelectionOption("Find Element By XPath")]
        [PropertyUISelectionOption("Find Element By ID")]
        [PropertyUISelectionOption("Find Element By Name")]
        [PropertyUISelectionOption("Find Element By Tag Name")]
        [PropertyUISelectionOption("Find Element By Class Name")]
        [PropertyUISelectionOption("Find Element By CSS Selector")]
        [PropertyUISelectionOption("Find Element By Link Text")]
        [Remarks("Select the specific search type that you want to use to isolate the WebElement in the web page.")]
        [PropertyValidationRule("Search Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Search Method")]
        public static string v_SearchMethod { get; }

        /// <summary>
        /// WebElements search parameter property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("WebElement Search Parameter")]
        [InputSpecification("WebElement Search Parameter", true)]
        [PropertyValidationRule("Search Parameter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Search Parameter")]
        [Remarks("Specifies the parameter text that matches to the element based on the previously selected search type.")]
        public static string v_SearchParameter { get; }

        /// <summary>
        /// WebElement selection method
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Selection Method for the WebElement")]
        [PropertyUISelectionOption("First")]
        [PropertyUISelectionOption("Last")]
        [PropertyUISelectionOption("Index")]
        [PropertyDetailSampleUsage("**First**", "Specify the First WebElement")]
        [PropertyDetailSampleUsage("**Last**", "Specify the Last WebElement")]
        [PropertyDetailSampleUsage("**Index**", "the Window specifed by Index. **0** means WebElement")]
        [PropertyIsOptional(true, "First")]
        [PropertyDisplayText(true, "Select")]
        public static string v_SelectionMethod { get; }

        /// <summary>
        /// WebElement index
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("WebElement Index")]
        [InputSpecification("Number", true)]
        [PropertyDetailSampleUsage("**0**", "Specify the First WebElement Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "WebElement Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebElement Index")]
        [PropertyDisplayText(true, "WebElement Index")]
        public static string v_WebElementIndex { get; }

        /// <summary>
        /// WebElement wait time
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time for the WebElement to Exist (sec)")]
        [Remarks("Specify how long to Wait before an Error will occur because the WebElement is Not Found.")]
        [PropertyIsOptional(true, "120")]
        [PropertyFirstValue("120")]
        [PropertyDisplayText(false, "Wait Time")]
        public static string v_WaitTimeForWebElement { get; }

        /// <summary>
        /// when Value(s) can not retrieved
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnoreSetEmpty))]
        [PropertyDescription("When the Value(s) can not Retrieved")]
        [PropertyIsOptional(true, "Error")]
        [PropertyValidationRule("When Value can not Retrieved", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "When can not Retrieved")]
        public static string v_WhenValueCanNotRetrieved { get; }

        /// <summary>
        /// attribute name
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Attribute Name")]
        [PropertyDetailSampleUsage("**textContent**", PropertyDetailSampleUsage.ValueType.Value, "Attribute")]
        [PropertyDetailSampleUsage("**value**", PropertyDetailSampleUsage.ValueType.Value, "Attribute")]
        [PropertyDetailSampleUsage("**{{{vAttribute}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Attribute")]
        [PropertyDetailSampleUsage("**@tag**", "Get Tab name from WebElement. Use Get Special Value From WebElement command.")]
        [PropertyValidationRule("Attribute", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Attribute")]
        public static string v_AttributeName { get; }

        /// <summary>
        /// Attribute Names
        /// </summary>
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_AttributeName))]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDescription("Attribute Names")]
        [InputSpecification("Attribute Names", true)]
        [PropertyDataGridViewSetting(true, true, true)]
        [PropertyDataGridViewColumnSettings("AttributeName", "Attribute Name")]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public static string v_AttributeNames { get; }
    }
}

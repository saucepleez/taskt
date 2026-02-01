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
    }
}

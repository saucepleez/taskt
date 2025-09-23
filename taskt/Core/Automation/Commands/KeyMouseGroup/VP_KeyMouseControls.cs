using System;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.CustomControls;
using taskt.UI.Forms.ScriptBuilder.CommandEditor;

namespace taskt.Core.Automation.Commands.KeyMouseGroup
{
    /// <summary>
    /// virtual propertyes for key mouse commands
    /// </summary>
    public static class VP_KeyMouseControls
    {
        /// <summary>
        /// text to send
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OneLineTextBox))]
        [PropertyDescription("Text or Keys to Send")]
        [PropertyCustomUIHelper("Keys Builder", nameof(VP_KeyMouseControls) + "+" + nameof(lnkKeysBulider_Click))]
        [PropertyCustomUIHelper("Encrypt Text", nameof(VP_KeyMouseControls) + "+" + nameof(lnkEncryptText_Click))]
        [InputSpecification("Text", true)]
        [PropertyDetailSampleUsage("**Hello, World!**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        [PropertyDetailSampleUsage("**^s**", "Specify **Ctrl+S** for Enter Keys")]
        [PropertyDetailSampleUsage("**{WIN_KEY}**", "Specify **Windows Key** for Enter Keys")]
        [PropertyDetailSampleUsage("**{WIN_KEY+R}**", "Specify **Windows Key** and **R** for Enter Keys")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIntermediateConvert(nameof(IntermediateControls.ConvertToIntermediate_CheckedVariableMarker), "")]
        [PropertyDisplayText(true, "Text")]
        public static string v_TextToSend { get; }

        /// <summary>
        /// send text ins encrypted or not
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Text is Encrypted")]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "Encrypted")]
        public static string v_EncryptionOption { get; }

        /// <summary>
        /// use clipbard to send text
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use Paste from Clipboard")]
        [PropertyIsOptional(true, "No")]
        [Remarks("When entering keys in combination with the Ctrl key, etc., It will NOT work correctly.")]
        [PropertyDisplayText(false, "Use Clipboard")]
        public static string v_UseClipBoard { get; }

        /// <summary>
        /// wait time after keys enter
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time for After Keys Enter")]
        [Remarks("When the Wait Time is less than **100** is specified, it will be **100**")]
        [PropertyIsOptional(true, "500")]
        [PropertyFirstValue("500")]
        [PropertyDisplayText(false, "Wait Time after Keys Enter")]
        public static string v_WaitTimeAfterKeyEnter { get; }

        /// <summary>
        /// activate window when specified Current Window Variable
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Try Activate Window, when Specified Current Window Variable")]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "Try Activate when Specified Current Window")]
        public static string v_ActivateCurrentWindow { get; }

        /// <summary>
        /// clear clipboard after paste
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Clear Clipboard After Paste")]
        [PropertyIsOptional(true, "No")]
        [PropertyValidationRule("Clear Clipboard", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Clear Clipboard after Paste")]
        public static string v_ClearClipboardAfterPaste { get; }

        /// <summary>
        /// shortcut keys
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Shortcut Key to Enter")]
        [PropertyUISelectionOption("New")]
        [PropertyUISelectionOption("New Window")]
        [PropertyUISelectionOption("Open")]
        [PropertyUISelectionOption("Print")]
        [PropertyUISelectionOption("Save")]
        [PropertyUISelectionOption("Save As")]
        [PropertyUISelectionOption("Undo")]
        [PropertyUISelectionOption("Cut")]
        [PropertyUISelectionOption("Copy")]
        [PropertyUISelectionOption("Paste")]
        [PropertyUISelectionOption("Delete")]
        [PropertyUISelectionOption("Search")]
        [PropertyUISelectionOption("Find")]
        [PropertyUISelectionOption("Find Next")]
        [PropertyUISelectionOption("Find Previous")]
        [PropertyUISelectionOption("Replace")]
        [PropertyUISelectionOption("Go To")]
        [PropertyUISelectionOption("Select All")]
        [PropertySecondaryLabel(true)]
        [PropertyAddtionalParameterInfo("New", "Send Ctrl + N")]
        [PropertyAddtionalParameterInfo("New Window", "Send Ctrl + Shift + N")]
        [PropertyAddtionalParameterInfo("Open", "Send Ctrl + O")]
        [PropertyAddtionalParameterInfo("Print", "Send Ctrl + P")]
        [PropertyAddtionalParameterInfo("Save", "Send Ctrl + S")]
        [PropertyAddtionalParameterInfo("Save As", "Send Ctrl + Shift + S")]
        [PropertyAddtionalParameterInfo("Undo", "Send Ctrl + Z")]
        [PropertyAddtionalParameterInfo("Cut", "Send Ctrl + X")]
        [PropertyAddtionalParameterInfo("Copy", "Send Ctrl + C")]
        [PropertyAddtionalParameterInfo("Paste", "Send Ctrl + V")]
        [PropertyAddtionalParameterInfo("Delete", "Send Delete")]
        [PropertyAddtionalParameterInfo("Search", "Send Ctrl + E")]
        [PropertyAddtionalParameterInfo("Find", "Send Ctrl + F")]
        [PropertyAddtionalParameterInfo("Find Next", "Send F3")]
        [PropertyAddtionalParameterInfo("Find Previous", "Send Shift + F3")]
        [PropertyAddtionalParameterInfo("Replace", "Send Ctrl + H")]
        [PropertyAddtionalParameterInfo("Go To", "Send Ctrl + G")]
        [PropertyAddtionalParameterInfo("Select All", "Send Ctrl + A")]
        [PropertySelectionChangeEvent(nameof(VP_KeyMouseControls) + "+" + nameof(cmbHotkey_SelectedIndexChanged))]
        [PropertyValidationRule("Shortcut Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Shortcut")]
        [PropertyParameterOrder(5100)]
        public static string v_Hotkey { get; }

        /// <summary>
        /// encrypt text to send keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkEncryptText_Click(object sender, EventArgs e)
        {
            //var inputText = ControlsList.GetPropertyControl<TextBox>(nameof(v_TextToSend));
            //var inputText = (TextBox)((CommandItemControl)sender).Tag;
            var inputText = FormUIControls.GetLinkTargetControl<TextBox>(sender);

            if (string.IsNullOrEmpty(inputText.Text))
            {
                MessageBox.Show("Text to send is empty.", "Notice");
                return;
            }

            var encrypted = EncryptionServices.EncryptString(inputText.Text, "TASKT");
            inputText.Text = encrypted;

            //this.v_EncryptionOption = "Encrypted";
            //var cmd = (IEnterKeysProperties)((frmCommandEditor)((Control)sender).FindForm()).editingCommand;
            var fm = FormUIControls.GetCommandEditorFromControl((Control)sender);
            var cmd = (IEnterKeysProperties)fm.editingCommand;

            cmd.v_EncryptionOption = "Yes";
        }

        /// <summary>
        /// key builder clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkKeysBulider_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmKeysBuilder())
            {
                if (fm.ShowDialog(((Control)sender).FindForm()) == DialogResult.OK)
                {
                    //var inputText = ControlsList.GetPropertyControl<TextBox>(nameof(v_TextToSend));
                    //var inputText = (TextBox)((CommandItemControl)sender).Tag;
                    var inputText = FormUIControls.GetLinkTargetControl<TextBox>(sender);

                    inputText.Text = fm.Result;
                }
            }
        }

        private static void cmbHotkey_SelectedIndexChanged(object sender, EventArgs e)
        {
            var searchedKey = ((ComboBox)sender).SelectedItem?.ToString() ?? "";

            var fm = FormUIControls.GetCommandEditorFromControl((Control)sender);

            ControlsList.SecondLabelProcess(nameof(v_Hotkey), nameof(v_Hotkey), searchedKey);
        }
    }
}

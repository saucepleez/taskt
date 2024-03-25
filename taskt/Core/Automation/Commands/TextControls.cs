﻿using System.Text;
using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Text commands
    /// </summary>
    internal static class TextControls
    {
        #region Virtual Property
        /// <summary>
        /// text
        /// </summary>
        [PropertyDescription("Text Value")]
        [InputSpecification("Text Value", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Text Value")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text Value")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text Value")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyDisplayText(true, "Text")]
        [PropertyParameterOrder(5000)]
        public static string v_Text { get; }

        /// <summary>
        /// text multiline
        /// </summary>
        [PropertyDescription("Text Value")]
        [InputSpecification("Text Value", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Text Value")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text Value")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text Value")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyDisplayText(true, "Text")]
        [PropertyParameterOrder(5000)]
        public static string v_Text_MultiLine { get; }

        /// <summary>
        /// text variable name
        /// </summary>
        [PropertyDescription("Text Variable Name")]
        [InputSpecification("Text Variable Name", true)]
        [PropertyDetailSampleUsage("**vText**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        [PropertyParameterOrder(5000)]

        public static string v_OutputTextVariableName { get; set; }

        /// <summary>
        /// for text file path
        /// </summary>
        [PropertyDescription("Path to the File")]
        [InputSpecification("Path of the File", true)]
        [Remarks("If file does not contain extensin, supplement txt automatically.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyDetailSampleUsage("**C:\\temp\\myfile.txt**", PropertyDetailSampleUsage.ValueType.Value, "Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path")]
        [PropertyDetailSampleUsage("**http://exmample.com/mytext.txt**", PropertyDetailSampleUsage.ValueType.Value, "Path")]
        [PropertyDetailSampleUsage("**{{{vURL}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Path")]
        [PropertyParameterOrder(5000)]
        public static string v_FilePath { get; }
        #endregion

        public static string ConvertToBase64(this string text)
        {
            return ConvertToBase64(text, Encoding.UTF8);
        }

        public static string ConvertToBase64(this string text, Encoding encoding)
        {
            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        public static bool TryParseBase64(this string text, out string decodedText)
        {
            return TryParseBase64(text, Encoding.UTF8, out decodedText);
        }

        public static bool TryParseBase64(this string text, Encoding encoding, out string decodedText)
        {
            if (string.IsNullOrEmpty(text))
            {
                decodedText = text;
                return false;
            }

            try
            {
                byte[] textAsBytes = Convert.FromBase64String(text);
                decodedText = encoding.GetString(textAsBytes);
                return true;
            }
            catch (Exception)
            {
                decodedText = null;
                return false;
            }
        }
    }
}

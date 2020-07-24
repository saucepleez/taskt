using System;

namespace taskt.Core.Automation.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertyUIHelper : Attribute
    {
        public UIAdditionalHelperType AdditionalHelper { get; private set; }
        public PropertyUIHelper(UIAdditionalHelperType helperType)
        {
            AdditionalHelper = helperType;
        }
        public enum UIAdditionalHelperType
        {
            ShowVariableHelper,
            ShowElementHelper,
            ShowFileSelectionHelper,
            ShowFolderSelectionHelper,
            ShowImageRecogitionHelper,
            ShowCodeBuilder,
            ShowMouseCaptureHelper,
            ShowElementRecorder,
            GenerateDLLParameters,
            ShowDLLExplorer,
            AddInputParameter,
            ShowHTMLBuilder,
            ShowIfBuilder,
            ShowLoopBuilder
        }
    }
}

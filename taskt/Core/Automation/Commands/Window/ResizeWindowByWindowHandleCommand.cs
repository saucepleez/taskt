using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Handle Actions")]
    [Attributes.ClassAttributes.CommandSettings("Resize Window By Window Handle")]
    [Attributes.ClassAttributes.Description("This command resizes a window to a specified size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reize a window by name to a specific size on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ResizeWindowByWindowHandleCommand : AWindowHandleCommands, IWindowSizeProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Window Width (Pixcel)")]
        //[InputSpecification("Window Width", true)]
        //[PropertyDetailSampleUsage("**640**", PropertyDetailSampleUsage.ValueType.Value, "Width")]
        //[PropertyDetailSampleUsage("**{{{vWidth}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Width")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        //[PropertyDisplayText(true, "Width")]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputWidth))]
        [PropertyParameterOrder(5500)]
        public string v_Width { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Window Height (Pixcel)")]
        //[InputSpecification("Window Height", true)]
        //[PropertyDetailSampleUsage("**480**", PropertyDetailSampleUsage.ValueType.Value, "Height")]
        //[PropertyDetailSampleUsage("**{{{vHeight}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Height")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        //[PropertyDisplayText(true, "Height")]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputHeight))]
        [PropertyParameterOrder(5500)]
        public string v_Height { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        public ResizeWindowByWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowControls.WindowHandleAction(this, engine,
                new Action<IntPtr>(whnd =>
                {
                    //var width = this.ExpandValueOrUserVariableAsInteger(nameof(v_Width), engine);
                    //var height = this.ExpandValueOrUserVariableAsInteger(nameof(v_Height), engine);
                    var width = this.ExpandValueOrVariableAsWindowWidth(whnd, engine);
                    var height = this.ExpandValueOrVariableAsWindowHeight(whnd, engine);

                    WindowControls.SetWindowSize(whnd, width, height);
                })
            );
        }
    }
}
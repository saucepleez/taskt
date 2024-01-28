using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Handle Actions")]
    [Attributes.ClassAttributes.CommandSettings("Move Window By Window Handle")]
    [Attributes.ClassAttributes.Description("This command moves a window to a specified location on screen.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move an existing window by name to a certain point on the screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MoveWindowByWindowHandleCommand : AWindowHandleCommand, IWindowPositionProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("X horizontal coordinate (pixel) for the Window's Location")]
        [InputSpecification("X Window Location", true)]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**0**", "Specify X Top Position")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "X Position")]
        [PropertyDetailSampleUsage("**{{{vXPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "X Position")]
        [PropertyDetailSampleUsage("**%kwd_current_position%**", "Specify Current Position for X Position")]
        [PropertyDetailSampleUsage("**%kwd_current_xposition%**", "Specify Current X Position for X Position", false)]
        [PropertyDetailSampleUsage("**%kwd_current_yposition%**", "Specify Current Y Position for X Position", false)]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        [PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "X Position")]
        [PropertyIntermediateConvert(nameof(ApplicationSettings.EngineSettings.convertToIntermediateWindowPosition), nameof(ApplicationSettings.EngineSettings.convertToRawWindowPosition))]
        [PropertyParameterOrder(5500)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Y vertical coordinate (pixel) for the Window's Location")]
        [InputSpecification("Y Window Location", true)]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**0**", "Specify Y Left Position")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Y Position")]
        [PropertyDetailSampleUsage("**{{{vYPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Y Position")]
        [PropertyDetailSampleUsage("**%kwd_current_position%**", "Specify Current Position for Y Position")]
        [PropertyDetailSampleUsage("**%kwd_current_xposition%**", "Specify Current X Position for Y Position", false)]
        [PropertyDetailSampleUsage("**%kwd_current_yposition%**", "Specify Current Y Position for Y Position", false)]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        [PropertyValidationRule("Y Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Y Position")]
        [PropertyIntermediateConvert(nameof(ApplicationSettings.EngineSettings.convertToIntermediateWindowPosition), nameof(ApplicationSettings.EngineSettings.convertToRawWindowPosition))]
        [PropertyParameterOrder(5500)]
        public string v_YPosition { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        public MoveWindowByWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowControls.WindowHandleAction(this, engine,
                new Action<IntPtr>(whnd =>
                {
                    var pos = WindowControls.GetWindowRect(whnd);

                    var variableXPosition = v_XPosition.ExpandValueOrUserVariable(engine);
                    int xPos;
                    if ((variableXPosition == engine.engineSettings.CurrentWindowPositionKeyword) || (variableXPosition == engine.engineSettings.CurrentWindowXPositionKeyword))
                    {
                        xPos = pos.left;
                    }
                    else if (variableXPosition == engine.engineSettings.CurrentWindowYPositionKeyword)
                    {
                        xPos = pos.top;
                    }
                    else
                    {
                        xPos = v_XPosition.ExpandValueOrUserVariableAsInteger("X Position", engine);
                    }

                    var variableYPosition = v_YPosition.ExpandValueOrUserVariable(engine);
                    int yPos;
                    if ((variableYPosition == engine.engineSettings.CurrentWindowPositionKeyword) || (variableYPosition == engine.engineSettings.CurrentWindowYPositionKeyword))
                    {
                        yPos = pos.top;
                    }
                    else if (variableYPosition == engine.engineSettings.CurrentWindowXPositionKeyword)
                    {
                        yPos = pos.left;
                    }
                    else
                    {
                        yPos = v_YPosition.ExpandValueOrUserVariableAsInteger("Y Position", engine);
                    }

                    WindowControls.SetWindowPosition(whnd, xPos, yPos);
                })
            );
        }
    }
}
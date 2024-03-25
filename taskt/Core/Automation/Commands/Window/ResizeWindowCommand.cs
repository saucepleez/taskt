﻿using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.CommandSettings("Resize Window")]
    [Attributes.ClassAttributes.Description("This command resizes a window to a specified size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reize a window by name to a specific size on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ResizeWindowCommand : AWindowNameCommands, IWindowSizeProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        //public string v_WindowName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        //public string v_SearchMethod { get; set; }

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
        //[PropertyAvailableSystemVariable(Engine.SystemVariables.LimitedSystemVariableNames.Window_Size)]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputWidth))]
        [PropertyParameterOrder(6500)]
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
        [PropertyParameterOrder(6500)]
        public string v_Height { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_MatchMethod))]
        //[PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        //public string v_MatchMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_TargetWindowIndex))]
        //public string v_TargetWindowIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowNameResult))]
        //public string v_NameResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_OutputWindowHandle))]
        //public string v_HandleResult { get; set; }

        public ResizeWindowCommand()
        {
            //this.CommandName = "ResizeWindowCommand";
            //this.SelectionName = "Resize Window";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowControls.WindowAction(this, engine,
                new Action<List<(IntPtr, string)>>(wins =>
                {
                    //var width = this.ExpandValueOrUserVariableAsInteger(nameof(v_Width), engine);
                    //var height = this.ExpandValueOrUserVariableAsInteger(nameof(v_Height), engine);
                    
                    foreach (var win in wins)
                    {
                        var whnd = win.Item1;

                        var width = this.ExpandValueOrVariableAsWindowWidth(whnd, engine);
                        var height = this.ExpandValueOrVariableAsWindowHeight(whnd, engine);

                        WindowControls.SetWindowSize(whnd, width, height);
                    }
                })
            );
        }

        //private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        //}

        //public override void Refresh(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        //}
    }
}
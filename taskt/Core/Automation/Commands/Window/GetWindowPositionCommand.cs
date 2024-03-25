﻿using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window State")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Position")]
    [Attributes.ClassAttributes.Description("This command returns window position.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want window position.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetWindowPositionCommand : AWindowNameCommands, IWindowPositionProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        //public string v_WindowName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        //public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Position X")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(6500)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Position Y")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(6500)]
        public string v_YPosition { get; set; }

        [XmlAttribute]
        [PropertyDescription("Base position")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUISelectionOption("Top Left")]
        [PropertyUISelectionOption("Bottom Right")]
        [PropertyUISelectionOption("Top Right")]
        [PropertyUISelectionOption("Bottom Left")]
        [PropertyUISelectionOption("Center")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Top Left")]
        [PropertyParameterOrder(6500)]
        public string v_PositionBase { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_MatchMethod_Single))]
        public override string v_MatchMethod { get; set; }

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

        public GetWindowPositionCommand()
        {
            //this.CommandName = "GetWindowPositionCommand";
            //this.SelectionName = "Get Window Position";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }
        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowControls.WindowAction(this, engine,
                new Action<List<(IntPtr, string)>>(wins =>
                {
                    var whnd = wins[0].Item1;

                    var pos = WindowControls.GetWindowRect(whnd);

                    int x = 0, y = 0;
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionBase), engine))
                    {
                        case "top left":
                            x = pos.left;
                            y = pos.top;
                            break;
                        case "bottom right":
                            x = pos.right;
                            y = pos.bottom;
                            break;
                        case "top right":
                            x = pos.right;
                            y = pos.top;
                            break;
                        case "bottom left":
                            x = pos.left;
                            y = pos.bottom;
                            break;
                        case "center":
                            x = (pos.right + pos.left) / 2;
                            y = (pos.top + pos.bottom) / 2;
                            break;
                    }
                    if (!string.IsNullOrEmpty(v_XPosition))
                    {
                        x.ToString().StoreInUserVariable(engine, v_XPosition);
                    }
                    if (!string.IsNullOrEmpty(v_YPosition))
                    {
                        y.ToString().StoreInUserVariable(engine, v_YPosition);
                    }
                })
            );
        }

        //public override void Refresh(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        //}

        //private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        //}
    }
}
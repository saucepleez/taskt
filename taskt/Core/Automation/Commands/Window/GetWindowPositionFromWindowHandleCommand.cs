using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Handle")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Position From Window Handle")]
    [Attributes.ClassAttributes.Description("This command returns window position.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want window position.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowPositionFromWindowHandleCommand : AWindowHandleCommands, IWindowPositionProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Position X")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Position X")]
        [PropertyParameterOrder(5001)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Position Y")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Position Y")]
        [PropertyParameterOrder(5002)]
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
        [PropertyParameterOrder(5003)]
        public string v_PositionBase { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        public GetWindowPositionFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //WindowControls.WindowHandleAction(this, engine,
            //    new Action<IntPtr>(whnd =>
            //    {
            //        var pos = WindowControls.GetWindowRect(whnd);

            //        int x = 0, y = 0;
            //        switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionBase), engine))
            //        {
            //            case "top left":
            //                x = pos.left;
            //                y = pos.top;
            //                break;
            //            case "bottom right":
            //                x = pos.right;
            //                y = pos.bottom;
            //                break;
            //            case "top right":
            //                x = pos.right;
            //                y = pos.top;
            //                break;
            //            case "bottom left":
            //                x = pos.left;
            //                y = pos.bottom;
            //                break;
            //            case "center":
            //                x = (pos.right + pos.left) / 2;
            //                y = (pos.top + pos.bottom) / 2;
            //                break;
            //        }
            //        if (!string.IsNullOrEmpty(v_XPosition))
            //        {
            //            x.ToString().StoreInUserVariable(engine, v_XPosition);
            //        }
            //        if (!string.IsNullOrEmpty(v_YPosition))
            //        {
            //            y.ToString().StoreInUserVariable(engine, v_YPosition);
            //        }
            //    })
            //);

            //var whnd = this.GetWindowHandle(engine);
            //var r = EM_WindowRECTPropertiesExtentionMethods.GetWindowRect(whnd);
            //int x = 0, y = 0;
            //switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionBase), engine))
            //{
            //    case "top left":
            //        x = r.left;
            //        y = r.top;
            //        break;
            //    case "bottom right":
            //        x = r.right;
            //        y = r.bottom;
            //        break;
            //    case "top right":
            //        x = r.right;
            //        y = r.top;
            //        break;
            //    case "bottom left":
            //        x = r.left;
            //        y = r.bottom;
            //        break;
            //    case "center":
            //        x = (r.right + r.left) / 2;
            //        y = (r.top + r.bottom) / 2;
            //        break;
            //}
            //if (!string.IsNullOrEmpty(v_XPosition))
            //{
            //    x.StoreInUserVariable(engine, v_XPosition);
            //}
            //if (!string.IsNullOrEmpty(v_YPosition))
            //{
            //    y.StoreInUserVariable(engine, v_YPosition);
            //}

            this.GetWindowHandle(engine, new Action<IntPtr>((whnd) =>
            {
                var r = EM_WindowRECTPropertiesExtentionMethods.GetWindowRect(whnd);
                int x = 0, y = 0;
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_PositionBase), engine))
                {
                    case "top left":
                        x = r.left;
                        y = r.top;
                        break;
                    case "bottom right":
                        x = r.right;
                        y = r.bottom;
                        break;
                    case "top right":
                        x = r.right;
                        y = r.top;
                        break;
                    case "bottom left":
                        x = r.left;
                        y = r.bottom;
                        break;
                    case "center":
                        x = (r.right + r.left) / 2;
                        y = (r.top + r.bottom) / 2;
                        break;
                }
                if (!string.IsNullOrEmpty(v_XPosition))
                {
                    x.StoreInUserVariable(engine, v_XPosition);
                }
                if (!string.IsNullOrEmpty(v_YPosition))
                {
                    y.StoreInUserVariable(engine, v_YPosition);
                }
            }));
        }
    }
}
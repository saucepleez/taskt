using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Positions From Window Names As DataTable")]
    [Attributes.ClassAttributes.Description("This command returns window process names.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window positions.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowPositionsFromWindowNamesAsDataTableCommand : GetFromWindowNamesAsDataTableCommands
    {
        [XmlAttribute]
        [Remarks("Column Names are **WindowName**, **WindowHandle**, **X**, and **Y**")]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_PositionBase))]
        [PropertyParameterOrder(7000)]
        public string v_PositionBase { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForGet))]
        [PropertyParameterOrder(9000)]
        public string v_WhenWindowIsMinimized { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMaximizedForGet))]
        [PropertyIsOptional(true, "Execute")]
        [PropertyParameterOrder(9001)]
        public string v_WhenWindowIsMaximized { get; set; }

        public GetWindowPositionsFromWindowNamesAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<System.Collections.Generic.List<(IntPtr, string)>>(wins =>
            {
                var ret = new DataTable();
                ret.Columns.Add("WindowName");
                ret.Columns.Add("WindowHandle");
                ret.Columns.Add("X");
                ret.Columns.Add("Y");
                foreach ((var whnd, var name) in wins)
                {
                    using(var x = new InnerScriptVariable(engine))
                    {
                        using (var y = new InnerScriptVariable(engine))
                        {
                            var getPos = new GetWindowPositionFromWindowHandleCommand()
                            {
                                v_WindowHandle = whnd.ToString(),
                                v_XPosition = x.VariableName,
                                v_YPosition = y.VariableName,
                                v_PositionBase = this.v_PositionBase,
                                v_WhenWindowIsMaximized = this.v_WhenWindowIsMaximized,
                                v_WhenWindowIsMinimized = this.v_WhenWindowIsMinimized,
                            };
                            getPos.RunCommand(engine);
                            ret.Rows.Add(new string[] { name, whnd.ToString(), x.VariableValue.ToString(), y.VariableValue.ToString() });
                        }
                    }
                }
                this.StoreDataTableInUserVariable(ret, engine);
            }));
        }
    }
}

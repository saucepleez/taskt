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
    [Attributes.ClassAttributes.CommandSettings("Get Window Sizes From Window Names As DataTable")]
    [Attributes.ClassAttributes.Description("This command returns window process names.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window sizes.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowSizesFromWindowNamesAsDataTableCommand : GetFromWindowNamesAsDataTableCommands
    {
        [XmlAttribute]
        [Remarks("Column Names are **WindowName**, **WindowHandle**, **Width**, and **Height**")]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForGet))]
        [PropertyParameterOrder(9000)]
        public string v_WhenWindowIsMinimized { get; set; }

        public GetWindowSizesFromWindowNamesAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<System.Collections.Generic.List<(IntPtr, string)>>(wins =>
            {
                var ret = new DataTable();
                ret.Columns.Add("WindowName");
                ret.Columns.Add("WindowHandle");
                ret.Columns.Add("Width");
                ret.Columns.Add("Height");

                foreach ((var whnd, var name) in wins) 
                {
                    using (var w = new InnerScriptVariable(engine))
                    {
                        using (var h = new InnerScriptVariable(engine))
                        {
                            var getSize = new GetWindowSizeFromWindowHandleCommand()
                            {
                                v_WindowHandle = whnd.ToString(),
                                v_Width = w.VariableName,
                                v_Height = h.VariableName,
                                v_WhenWindowIsMinimized = this.v_WhenWindowIsMinimized,
                            };
                            getSize.RunCommand(engine);

                            ret.Rows.Add(new string[] { name, whnd.ToString(), w.VariableValue.ToString(), h.VariableValue.ToString() });
                        }
                    }
                }

                this.StoreDataTableInUserVariable(ret, engine);
            }));
        }
    }
}

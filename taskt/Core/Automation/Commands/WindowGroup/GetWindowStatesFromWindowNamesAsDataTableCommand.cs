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
    [Attributes.ClassAttributes.CommandSettings("Get Window States From Window Names As DataTable")]
    [Attributes.ClassAttributes.Description("This command returns window process states.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window states.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowStatesFromWindowNamesAsDataTableCommand : GetFromWindowNamesAsDataTableCommands
    {
        [XmlAttribute]
        [Remarks("Column Names are **WindowName**, **WindowHandle**, **State**, and **StateText**")]
        public override string v_Result { get; set; }

        public GetWindowStatesFromWindowNamesAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<System.Collections.Generic.List<(IntPtr, string)>>(wins =>
            {
                var ret = new DataTable();
                ret.Columns.Add("WindowName");
                ret.Columns.Add("WindowHandle");
                ret.Columns.Add("State");
                ret.Columns.Add("StateText");

                foreach ((var whnd, var name) in wins) 
                {
                    using (var s = new InnerScriptVariable(engine))
                    {
                        using (var st = new InnerScriptVariable(engine))
                        {
                            var getState = new GetWindowStateFromWindowHandleCommand()
                            {
                                v_WindowHandle = whnd.ToString(),
                                v_WindowState = s.VariableName,
                                v_WindowStateText = st.VariableName,
                            };
                            getState.RunCommand(engine);

                            ret.Rows.Add(new string[] { name, whnd.ToString(), s.VariableValue.ToString(), st.VariableValue.ToString() });
                        }
                    }
                }

                this.StoreDataTableInUserVariable(ret, engine);
            }));
        }
    }
}

using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Handles From Window Names As DataTable")]
    [Attributes.ClassAttributes.Description("This command returns window handles.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window handles.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowHandlesFromWindowNamesAsDataTableCommand : GetFromWindowNamesAsDataTableCommands
    {
        [XmlAttribute]
        [Remarks("Column Names are **WindowName**, **WindowHandle**")]
        public override string v_Result { get; set; }

        public GetWindowHandlesFromWindowNamesAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<System.Collections.Generic.List<(IntPtr, string)>>(wins =>
            {
                var ret = new DataTable();
                ret.Columns.Add("WindowName");
                ret.Columns.Add("WindowHandle");
                foreach ((var whnd, var name) in wins)
                {
                    ret.Rows.Add(new string[] { name, whnd.ToString(), });
                }
                this.StoreDataTableInUserVariable(ret, engine);
            }));
        }
    }
}

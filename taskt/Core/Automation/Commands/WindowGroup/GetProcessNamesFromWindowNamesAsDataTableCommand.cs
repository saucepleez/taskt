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
    [Attributes.ClassAttributes.CommandSettings("Get Process Names From Window Names As DataTable")]
    [Attributes.ClassAttributes.Description("This command returns window process names.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window process names.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetProcessNamesFromWindowNamesAsDataTableCommand : GetFromWindowNamesAsDataTableCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [Remarks("Column Names are **WindowName**, **WindowHandle**, and **ProcessName**")]
        //[PropertyParameterOrder(6500)]
        public override string v_Result { get; set; }

        public GetProcessNamesFromWindowNamesAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<System.Collections.Generic.List<(IntPtr, string)>>(wins =>
            {
                var ret = new DataTable();
                ret.Columns.Add("WindowName");
                ret.Columns.Add("WindowHandle");
                ret.Columns.Add("ProcessName");
                foreach ((var whnd, var name) in wins)
                {
                    using(var p = new InnerScriptVariable(engine))
                    {
                        var getProcess = new GetProcessNameFromWindowHandleCommand()
                        {
                            v_WindowHandle = whnd.ToString(),
                            v_Result = p.VariableName,
                        };
                        getProcess.RunCommand(engine);
                        ret.Rows.Add(new string[] { name, whnd.ToString(), p.VariableValue.ToString(), });
                    }
                }
                this.StoreDataTableInUserVariable(ret, engine);
            }));
        }
    }
}

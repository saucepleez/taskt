using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Positions From Window Names As List")]
    [Attributes.ClassAttributes.Description("This command returns window handles.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window positions.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowPositionsFromWindowNamesAsListCommand : GetFromWindowNamesAsListCommands, IWindowPositionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Position X")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Position X")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterOrder(5001)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Position Y")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Position Y")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterOrder(5002)]
        public string v_YPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_PositionBase))]
        [PropertyParameterOrder(5003)]
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

        public GetWindowPositionsFromWindowNamesAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var table = new InnerScriptVariable(engine))
            {
                var getPos = new GetWindowPositionsFromWindowNamesAsDataTableCommand()
                {
                    v_WindowName = this.v_WindowName,
                    v_CheckMethod = this.v_CheckMethod,
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                    v_PositionBase = this.v_PositionBase,
                    v_WhenWindowIsMaximized = this.v_WhenWindowIsMaximized,
                    v_WhenWindowIsMinimized = this.v_WhenWindowIsMinimized,
                    v_Result = table.VariableName,
                };
                getPos.RunCommand(engine);

                if (!string.IsNullOrEmpty(v_XPosition))
                {
                    this.StoreListInUserVariable(GetColumnValues(table, 2), nameof(v_XPosition), engine);
                }
                if (!string.IsNullOrEmpty(v_YPosition))
                {
                    this.StoreListInUserVariable(GetColumnValues(table, 3), nameof(v_YPosition), engine);
                }
            }
        }
    }
}

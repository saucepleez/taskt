using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Sizes From Window Names As List")]
    [Attributes.ClassAttributes.Description("This command returns window handles.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window sizes.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowSizesFromWindowNamesAsListCommand : GetFromWindowNamesAsListCommands, IWindowSizeProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Width")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Width")]
        [PropertyParameterOrder(5500)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Height")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Height")]
        [PropertyParameterOrder(5501)]
        public string v_Height { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForGet))]
        [PropertyParameterOrder(9000)]
        public string v_WhenWindowIsMinimized { get; set; }

        public GetWindowSizesFromWindowNamesAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var table = new InnerScriptVariable(engine))
            {
                var getSize = new GetWindowSizesFromWindowNamesAsDataTableCommand()
                {
                    v_WindowName = this.v_WindowName,
                    v_CompareMethod = this.v_CompareMethod,
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                    v_WhenWindowIsMinimized = this.v_WhenWindowIsMinimized,
                    v_Result = table.VariableName,
                };
                getSize.RunCommand(engine);

                if (!string.IsNullOrEmpty(v_Width))
                {
                    this.StoreListInUserVariable(GetColumnValues(table, 2), nameof(v_Width), engine);
                }
                if (!string.IsNullOrEmpty(v_Height))
                {
                    this.StoreListInUserVariable(GetColumnValues(table, 3), nameof(v_Height), engine);
                }
            }
        }
    }
}

using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get Window States From Window States As List")]
    [Attributes.ClassAttributes.Description("This command returns window handles.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window states.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowStatesFromWindowNamesAsListCommand : GetFromWindowNamesAsListCommands, IWindowStateProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window State Text")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window State", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "State")]
        [Remarks("Restore is **1**, Minimize is **2**, Maximize is **3**")]
        [PropertyParameterOrder(5500)]
        public string v_WindowState { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyDescription("Variable Name to Store Window State Text")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window State Text", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "State Text")]
        [PropertyParameterOrder(5501)]
        public string v_WindowStateText { get; set; }

        public GetWindowStatesFromWindowNamesAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var table = new InnerScriptVariable(engine))
            {
                var getState = new GetWindowStatesFromWindowNamesAsDataTableCommand()
                {
                    v_WindowName = this.v_WindowName,
                    v_CompareMethod = this.v_CompareMethod,
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                    v_Result = table.VariableName,
                };
                getState.RunCommand(engine);

                if (!string.IsNullOrEmpty(v_WindowState))
                {
                    this.StoreListInUserVariable(GetColumnValues(table, 2), nameof(v_WindowState), engine);
                }
                if (!string.IsNullOrEmpty(v_WindowStateText))
                {
                    this.StoreListInUserVariable(GetColumnValues(table, 3), nameof(v_WindowStateText), engine);
                }
            }
        }
    }
}

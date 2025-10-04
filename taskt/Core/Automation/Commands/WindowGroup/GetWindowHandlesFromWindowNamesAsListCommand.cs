using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Handles From Window Names As List")]
    [Attributes.ClassAttributes.Description("This command returns window handles.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window handles.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowHandlesFromWindowNamesAsListCommand : GetFromWindowNamesAsListCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterOrder(6500)]
        public string v_Result { get; set; }

        public GetWindowHandlesFromWindowNamesAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<List<(IntPtr, string)>>(wins =>
            {
                var res = new List<string>();
                foreach((var whnd, _) in wins)
                {
                    res.Add(whnd.ToString());
                }
                this.StoreListInUserVariable(res, nameof(v_Result), engine);
            }));
        }
    }
}

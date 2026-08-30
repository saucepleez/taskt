using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("UIElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Set Selected State To UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to set Selected State from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to set Selected State from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSetSelectedStateToUIElementCommand : AUIElementActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Selected State")]
        [PropertyUISelectionOption("Selected")]
        [PropertyUISelectionOption("Unselected")]
        [PropertyDisplayText(true, "State")]
        [PropertyParameterOrder(6000)]
        public string v_State { get; set; }

        public UIAutomationSetSelectedStateToUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.UIElementActionAndWait(engine,
                new Action<AutomationElement, IntPtr>((targetElement, whnd) =>
                {
                    bool newState = false;
                    switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_State), engine))
                    {
                        case "selected":
                            newState = true;
                            break;
                    }

                    using(var state = new InnerScriptVariable(engine))
                    {
                        var getSelect = new UIAutomationGetSelectedStateFromUIElementCommand()
                        {
                            v_TargetElement = this.v_TargetElement,
                            v_Result = state.VariableName,
                        };
                        getSelect.RunCommand(engine);
                        if (bool.Parse(state.VariableValue.ToString()) != newState)
                        {
                            if (targetElement.TryGetCurrentPattern(TogglePattern.Pattern, out object tgl))
                            {
                                var tglPtn = (TogglePattern)tgl;
                                tglPtn.Toggle();
                            }
                            else if (targetElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out object sel))
                            {
                                var selPtn = (SelectionItemPattern)sel;
                                selPtn.Select();
                            }
                            else
                            {
                                this.ActionNotSupportedProcess("Set Selected State", engine);
                            }
                        }
                    }
                })
            );
        }
    }
}
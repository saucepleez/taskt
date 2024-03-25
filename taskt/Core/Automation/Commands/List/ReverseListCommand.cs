using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Reverse List")]
    [Attributes.ClassAttributes.Description("This command allows you to reverse list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reverse list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReverseListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List Variable Name to Reverse")]
        [PropertyValidationRule("List to Reverse", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List to Reverse")]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        public string v_OutputList { get; set; }

        public ReverseListCommand()
        {
            //this.CommandName = "ReverseListCommand";
            //this.SelectionName = "Reverse List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            List<string> targetList = v_InputList.ExpandUserVariableAsList(engine);

            List<string> newList = new List<string>(targetList);
            newList.Reverse();
            newList.StoreInUserVariable(engine, v_OutputList);
        }
    }
}
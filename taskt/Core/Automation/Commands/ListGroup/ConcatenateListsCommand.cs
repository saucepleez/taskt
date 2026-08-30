using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Concatenate Lists")]
    [Attributes.ClassAttributes.Description("This command allows you to concatenate 2 lists.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to concatenate 2 lists.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConcatenateListsCommand : ScriptCommand, ICanHandleList
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List1 Variable Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**vList1**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vList1}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDisplayText(true, "List1")]
        public string v_ListA { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List2 Variable Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**vList2**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vList2}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDisplayText(true, "List2")]
        public string v_ListB { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        [Remarks("Concatenate List1, List2 in that order")]
        public string v_NewList { get; set; }

        public ConcatenateListsCommand()
        {
            //this.CommandName = "ConcatenateListsCommand";
            //this.SelectionName = "Concatenate Lists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var listA = this.ExpandUserVariableAsList(nameof(v_ListA), engine);
            var listB = this.ExpandUserVariableAsList(nameof(v_ListB), engine);

            var newList = new List<string>();
            newList.AddRange(listA);
            newList.AddRange(listB);

            this.StoreListInUserVariable(newList, nameof(v_NewList), engine);
        }
    }
}

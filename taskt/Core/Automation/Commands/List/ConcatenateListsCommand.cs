using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Concatenate Lists")]
    [Attributes.ClassAttributes.Description("This command allows you to concatenate 2 lists.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to concatenate 2 lists.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConcatenateListsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List1 Variable Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**vList1**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vList1}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDisplayText(true, "List1")]
        public string v_InputListA { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List2 Variable Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**vList2**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vList2}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDisplayText(true, "List2")]
        public string v_InputListB { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        [Remarks("Concatenate List1, List2 in that order")]
        public string v_OutputList { get; set; }

        public ConcatenateListsCommand()
        {
            //this.CommandName = "ConcatenateListsCommand";
            //this.SelectionName = "Concatenate Lists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> listA = v_InputListA.GetListVariable(engine);
            List<string> listB = v_InputListB.GetListVariable(engine);

            List<string> newList = new List<string>();
            newList.AddRange(listA);
            newList.AddRange(listB);
            newList.StoreInUserVariable(engine, v_OutputList);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Get List With Duplicate Items Removed")]
    [Attributes.ClassAttributes.Description("This command allows you to Get List with Duplicate Items Removed.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get List with Duplicate Items Removed.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetListWithDuplicateItemsRemoved : AListCreateFromListCommands
    {
        //[XmlAttribute]
        ////[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //[PropertyDescription("List Variable Name to Reverse")]
        //[PropertyValidationRule("List to Reverse", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "List to Reverse")]
        //public string v_TargetList { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        //public string v_NewList { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextCompareSelectMethodControls), nameof(TextCompareSelectMethodControls.v_CaseSensitiveNo))]
        [PropertyParameterOrder(11000)]
        [PropertyIsOptional(true, "Yes")]
        public string v_CaseSenstive { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        [PropertyDescription("Removed Items List")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(12000)]
        public string v_RemovedItemsList { get; set; }

        public GetListWithDuplicateItemsRemoved()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var list = this.ExpandUserVariableAsList(engine);

            // remove check flag
            var removeFlag = new List<bool>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                removeFlag.Add(false);
            }

            Func<string, string> cnvFunc;
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_CaseSenstive), engine))
            {
                cnvFunc = new Func<string, string>(s => s);
            }
            else
            {
                cnvFunc = new Func<string, string>(s => s.ToLower());
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (removeFlag[i])
                {
                    continue;
                }

                var targetValue = cnvFunc(list[i]);
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (targetValue == cnvFunc(list[j]))
                    {
                        removeFlag[j] = true;
                    }
                }
            }

            // remove dup items process
            var newList = new List<string>();
            var removedList = new List<string>();
            for (int i = 0; i < removeFlag.Count; i++)
            {
                if (removeFlag[i])
                {
                    removedList.Add(list[i]);
                }
                else
                {
                    newList.Add(list[i]);
                }
            }

            this.StoreListInUserVariable(newList, engine);

            if (!string.IsNullOrEmpty(v_RemovedItemsList))
            {
                removedList.StoreInUserVariable(engine, v_RemovedItemsList);
            }
        }
    }
}

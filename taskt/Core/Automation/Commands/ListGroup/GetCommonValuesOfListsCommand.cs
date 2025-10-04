using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Get Common Values Of Lists")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Common Valus of 2 Lists.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Common Valus of 2 Lists.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetCommonValuesOfListsCommand : ScriptCommand, ICanHandleList
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
        [PropertyDescription("Common Values List")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Common Values", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Common Values")]
        public string v_CommonValues { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextCompareSelectMethodControls), nameof(TextCompareSelectMethodControls.v_CaseSensitiveNo))]
        [PropertyIsOptional(true, "Yes")]
        public string v_CaseSenstive { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Duplicate Items are Considered One")]
        [PropertyIsOptional(true, "No")]
        public string v_DuplicatesAreOne { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        [PropertyDescription("Values of List1 Only")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("List1 Only", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "List1 Only")]
        public string v_ListAOnly { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        [PropertyDescription("Values of List2 Only")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("List2 Only", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "List2 Only")]
        public string v_ListBOnly { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        [PropertyDescription("Not Common Values")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Not Common Values", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Not Common Values")]
        public string v_NotCommonValues { get; set; }

        public GetCommonValuesOfListsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            List<string> ta;
            List<string> tb;
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_DuplicatesAreOne), engine))
            {
                List<string> RemoveDupFunc(string listVariableName)
                {
                    using(var v = new InnerScriptVariable(engine))
                    {
                        var dup = new GetListWithDuplicateItemsRemoved()
                        {
                            v_TargetList = listVariableName,
                            v_CaseSenstive = this.v_CaseSenstive,
                            v_NewList = v.VariableName,
                        };
                        dup.RunCommand(engine);
                        return (List<string>)v.VariableValue;
                    }
                }

                //using(var l1 = new InnerScriptVariable(engine))
                //{
                //    var dup = new GetListWithDuplicateItemsRemoved()
                //    {
                //        v_TargetList = this.v_ListA,
                //        v_CaseSenstive = this.v_CaseSenstive,
                //        v_NewList = l1.VariableName,
                //    };
                //    dup.RunCommand(engine);
                //    ta = (List<string>)l1.VariableValue;
                //}
                //using (var l2 = new InnerScriptVariable(engine))
                //{
                //    var dup = new GetListWithDuplicateItemsRemoved()
                //    {
                //        v_TargetList = this.v_ListB,
                //        v_CaseSenstive = this.v_CaseSenstive,
                //        v_NewList = l2.VariableName,
                //    };
                //    dup.RunCommand(engine);
                //    tb = (List<string>)l2.VariableValue;
                //}
                ta = RemoveDupFunc(v_ListA);
                tb = RemoveDupFunc(v_ListB);
            }
            else
            {
                ta = this.ExpandUserVariableAsList(nameof(v_ListA), engine);
                tb = this.ExpandUserVariableAsList(nameof(v_ListB), engine);
            }
            
            // copy
            var tempA = new List<string>(ta);
            var tempB = new List<string>(tb);

            // unlink
            ta = null;
            tb = null;

            Func<string, string> cnvFunc;
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_CaseSenstive), engine))
            {
                cnvFunc = new Func<string, string>(s => s);
            }
            else
            {
                cnvFunc = new Func<string, string>(s => s.ToLower());
            }

            var commonValues = new List<string>();
            var onlyA = new List<string>();
            var onlyB = new List<string>();
            var notCommon = new List<string>();

            for (int i = 0; i < tempA.Count; i++)
            {
                var targetValue = cnvFunc(tempA[i]);

                if (tempB.Count > 0)
                {
                    // search tempB
                    int j = 0;
                    for (; j < tempB.Count; j++)
                    {
                        if (targetValue == cnvFunc(tempB[j]))
                        {
                            break;
                        }
                    }
                    if (j < tempB.Count)
                    {
                        // exists -> common-value
                        commonValues.Add(tempA[i]);
                        tempB.RemoveAt(j);
                    }
                    else
                    {
                        // not exists -> onlyA, not-common
                        notCommon.Add(tempA[i]);
                        onlyA.Add(tempA[i]);
                    }
                }
                else
                {
                    // no tempB items
                    onlyA.Add(tempA[i]);
                    notCommon.Add(tempA[i]);
                }
            }

            // onlyB
            if (tempB.Count > 0)
            {
                onlyB.AddRange(tempB);
                notCommon.AddRange(tempB);
            }

            if (!string.IsNullOrEmpty(v_CommonValues))
            {
                commonValues.StoreInUserVariable(engine, v_CommonValues);
            }
            if (!string.IsNullOrEmpty(v_ListAOnly))
            {
                onlyA.StoreInUserVariable(engine, v_ListAOnly);
            }
            if (!string.IsNullOrEmpty(v_ListBOnly))
            {
                onlyB.StoreInUserVariable(engine, v_ListBOnly);
            }
            if (!string.IsNullOrEmpty(v_NotCommonValues))
            {
                notCommon.StoreInUserVariable(engine, v_NotCommonValues);
            }
        }
    }
}

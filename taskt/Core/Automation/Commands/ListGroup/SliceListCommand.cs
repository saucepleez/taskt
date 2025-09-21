using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Slice List")]
    [Attributes.ClassAttributes.Description("This command allows you to Get a Part of the List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get a Part of the List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SliceListCommand : AListCreateFromListCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //[PropertyValidationRule("List to Reverse", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "List to Reverse")]
        //public string v_TargetList { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Slice Method")]
        [PropertyUISelectionOption("Range")]
        [PropertyUISelectionOption("From First To Nth")]
        [PropertyUISelectionOption("After Nth From First")]
        [PropertyUISelectionOption("From Last To Nth")]
        [PropertyUISelectionOption("Before Nth From Last")]
        [PropertyUISelectionOption("Odd Numbered Items")]
        [PropertyUISelectionOption("Even Numbered Items")]
        [PropertyUISelectionOption("Custom Print Like Specifications")]
        [PropertyValidationRule("Slice Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyFirstValue("Range")]
        [PropertyDisplayText(true, "Method")]
        [PropertySelectionChangeEvent(nameof(cmbSliceMethod_SelectionChangeCommitted))]
        [PropertyParameterOrder(6000)]
        public string v_SliceMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        [PropertyDescription("List Index 1")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Index 1", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Index1")]
        [PropertyParameterOrder(6100)]
        public string v_Index1 { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        [PropertyDescription("List Index 2")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Index 2", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Index2")]
        [PropertyParameterOrder(6200)]
        public string v_Index2 { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        //public string v_NewList { get; set; }

        public SliceListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var list = this.ExpandUserVariableAsList(engine);

            int GetListIndex(string parameterValue)
            {
                using (var r = new InnerScriptVariable(engine))
                {
                    using (var index = new InnerScriptVariable(engine))
                    {
                        var checkIndex = new CheckValidListIndex()
                        {
                            v_List = this.v_TargetList,
                            v_Index = parameterValue,
                            v_Result = r.VariableName,
                            v_IndexResult = index.VariableName,
                        };
                        checkIndex.RunCommand(engine);
                        if (bool.Parse(r.VariableValue.ToString()))
                        {
                            return int.Parse(index.VariableValue.ToString());
                        }
                        else
                        {
                            throw new Exception($"Strange List Index Value. Value: '{parameterValue}'");
                        }
                    }
                }
            }

            void AddListRange(List<string> srcList, List<string> dstList, int startIndex, int endIndex)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    dstList.Add(srcList[i]);
                }
            }

            // TODO: when index is out of range

            // use for range
            int index1, index2;

            var res = new List<string>();
            switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_SliceMethod), engine))
            {
                case "range":
                    index1 = GetListIndex(v_Index1);
                    index2 = GetListIndex(v_Index2);

                    bool isReverse = false;
                    if (index1 > index2)
                    {
                        (index1, index2) = (index2, index1);    // swap
                        isReverse = true;
                    }
                    AddListRange(list, res, index1, index2);
                    if (isReverse)
                    {
                        res.Reverse();
                    }
                    break;
                case "from first to nth":
                    index2 = this.ExpandValueOrUserVariableAsInteger(nameof(v_Index1), engine);
                    index2--; // fix value
                    index2 = GetListIndex(index2.ToString());
                    AddListRange(list, res, 0, index2);
                    break;
                case "after nth from first":
                    index1 = this.ExpandValueOrUserVariableAsInteger(nameof(v_Index1), engine);
                    index1--;   // fix value
                    AddListRange(list, res, index1, list.Count - 1);
                    break;
                case "from last to nth":
                    index1 = this.ExpandValueOrUserVariableAsInteger(nameof(v_Index1), engine);
                    index1 = GetListIndex((-index1).ToString());
                    AddListRange(list, res, index1, list.Count - 1);
                    break;
                case "before nth from last":
                    index2 = this.ExpandValueOrUserVariableAsInteger(nameof(v_Index1), engine);
                    index2 = GetListIndex((-index2).ToString());
                    AddListRange(list, res, 0, index2);
                    break;
                case "odd numbered items":
                    for (int i = 1; i < list.Count; i+= 2)
                    {
                        res.Add(list[i]);
                    }
                    break;
                case "even numbered items":
                    for (int i = 0; i < list.Count; i += 2)
                    {
                        res.Add(list[i]);
                    }
                    break;
                case "custom print like specifications":
                    var pages = this.ExpandValueOrUserVariable(nameof(v_Index1), "Custom Print", engine);
                    var spt = pages.Split(',').ToList();
                    foreach(var p in spt)
                    {
                        var pp = p.Trim();
                        if (pp.Contains("-"))
                        {
                            var sptPP = pp.Split('-');
                            if (sptPP.Length == 2)
                            {
                                index1 = GetListIndex(sptPP[0]);
                                index2 = GetListIndex(sptPP[1]);

                                if (index1 <= index2)
                                {
                                    AddListRange(list, res, index1, index2);
                                }
                                else
                                {
                                    (index1, index2) = (index2, index1);    // swap
                                    var tList = new List<string>();
                                    AddListRange(list, tList, index1, index2);
                                    tList.Reverse();
                                    res.AddRange(tList);
                                }
                            }
                            else
                            {
                                throw new Exception($"Strange Index Specification: Range: '{pp}'");
                            }
                        }
                        else
                        {
                            var index = GetListIndex(pp);
                            res.Add(list[index]);
                        }
                    }
                    break;
            }

            this.StoreListInUserVariable(res, engine);
        }

        private void cmbSliceMethod_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var cmb = (ComboBox)sender;
            bool visible1 = true;
            bool visible2 = false;
            switch (cmb.SelectedItem.ToString().ToLower())
            {
                case "range":
                    visible2 = true;
                    break;
                case "odd numbered items":
                case "even numbered items":
                    visible1 = false;
                    break;
            }
            FormUIControls.SetVisibleParameterControlGroup(this.ControlsList, nameof(v_Index1), visible1);
            FormUIControls.SetVisibleParameterControlGroup(this.ControlsList, nameof(v_Index2), visible2);
        }
    }
}

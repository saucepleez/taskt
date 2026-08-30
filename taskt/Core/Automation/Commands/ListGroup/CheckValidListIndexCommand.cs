using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Index")]
    [Attributes.ClassAttributes.CommandSettings("Check Valid List Index")]
    [Attributes.ClassAttributes.Description("This command allows you to Check Valid List Index.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Check Valid List Index.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CheckValidListIndex : AListIndexCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //public string v_List { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        //public string v_Index { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store List Index Value")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("List Index Value", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8000)]
        public string v_IndexResult { get; set; }

        public CheckValidListIndex()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            bool res;
            int resIndex;
            try
            {
                (_, var index, _) = this.ExpandValueOrUserVariableAsListAndIndexAndValue(engine);
                res = true;
                resIndex = index;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Strange List Index Value. Value:"))
                {
                    res = false;
                    resIndex = -1;
                }
                else
                {
                    throw ex;
                }
            }

            res.StoreInUserVariable(engine, v_Result);
            if (!string.IsNullOrEmpty(v_IndexResult))
            {
                resIndex.StoreInUserVariable(engine, v_IndexResult);
            }
        }
    }
}

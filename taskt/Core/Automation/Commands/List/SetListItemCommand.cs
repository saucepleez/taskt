using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.CommandSettings("Set List Item")]
    [Attributes.ClassAttributes.Description("This command allows you want to set an item in a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set an item in a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetListItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_BothListName))]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        public string v_ItemIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OneLineTextBox))]
        [PropertyDescription("Value to Set")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Value to Set")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Value to Set")]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Value")]
        public string v_NewValue { get; set; }

        public SetListItemCommand()
        {
            //this.CommandName = "SetListItemCommand";
            //this.SelectionName = "Set List Item";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var listVariable = v_ListName.GetRawVariable(engine);
            //if (listVariable == null)
            //{
            //    throw new Exception("Complex Variable '" + v_ListName + "' or '" + v_ListName.ApplyVariableFormatting(engine) + "' not found. Ensure the variable exists before attempting to modify it.");
            //}

            //List<string> targetList;
            //if (listVariable.VariableValue is List<string>)
            //{
            //    targetList = (List<string>)listVariable.VariableValue;
            //}
            //else
            //{
            //    throw new Exception(v_ListName + " is not List");
            //}

            //int index = 0;
            //if (String.IsNullOrEmpty(v_ItemIndex))
            //{
            //    index = listVariable.CurrentPosition;
            //}
            //else
            //{
            //    string itemIndex = v_ItemIndex.ConvertToUserVariable(sender);
            //    index = int.Parse(itemIndex);
            //}
            //if (index < 0)
            //{
            //    index = targetList.Count + index;
            //}

            (var list, var index) = this.GetListVariableAndIndex(nameof(v_ListName), nameof(v_ItemIndex), engine);

            if ((index >= 0) && (index < list.Count))
            {
                list[index] = v_NewValue.ConvertToUserVariable(engine);
            }
            else
            {
                throw new Exception("Strange index " + v_ItemIndex + ", parsed " + index);
            }
        }
    }
}
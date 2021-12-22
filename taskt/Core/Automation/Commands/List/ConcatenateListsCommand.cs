using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to concatenate 2 lists.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to concatenate 2 lists.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConcatenateListsCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a List Variable Name to concatenate")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vList1** or **{{{vList1}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_InputListA { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a List Variable Name to concatenate")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vList2** or **{{{vList2}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_InputListB { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a List Variable Name of the New List")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vNewList** or **{{{vNewList}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        public string v_OutputList { get; set; }

        public ConcatenateListsCommand()
        {
            this.CommandName = "ConcatenateListsCommand";
            this.SelectionName = "Concatenate Lists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            ////get variable by regular name
            //Script.ScriptVariable listVariable1 = v_InputListA.GetRawVariable(engine);

            ////if still null then throw exception
            //if (listVariable1 == null)
            //{
            //    throw new System.Exception("Complex Variable '" + v_InputListA + "' or '" + v_InputListA.ApplyVariableFormatting(engine) + "' not found. Ensure the variable exists before attempting to modify it.");
            //}

            ////get variable by regular name
            //Script.ScriptVariable listVariable2 = v_InputListB.GetRawVariable(engine);

            ////if still null then throw exception
            //if (listVariable2 == null)
            //{
            //    throw new System.Exception("Complex Variable '" + v_InputListB + "' or '" + v_InputListB.ApplyVariableFormatting(engine) + "' not found. Ensure the variable exists before attempting to modify it.");
            //}

            //if ((listVariable1.VariableValue is List<string>) && (listVariable2.VariableValue is List<string>))
            //{
            //    List<string> newList = new List<string>();
            //    newList.AddRange((List<string>)listVariable1.VariableValue);
            //    newList.AddRange((List<string>)listVariable2.VariableValue);
            //    newList.StoreInUserVariable(engine, v_OutputList);
            //}
            //else
            //{
            //    throw new Exception(v_InputListA + " or " + v_InputListB + " is not List or not-supported List.");
            //}

            List<string> listA = v_InputListA.GetListVariable(engine);
            List<string> listB = v_InputListB.GetListVariable(engine);

            List<string> newList = new List<string>();
            newList.AddRange(listA);
            newList.AddRange(listB);
            newList.StoreInUserVariable(engine, v_OutputList);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Concatenate Lists " + this.v_InputListA + " and " + this.v_InputListB + ", To: " + this.v_OutputList + "]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InputListA))
            {
                this.validationResult += "List1 is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_InputListB))
            {
                this.validationResult += "List2 is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_OutputList))
            {
                this.validationResult += "New list is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
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
    [Attributes.ClassAttributes.Description("This command allows you to sort list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to sort list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SortListCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a List Variable Name to sort")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vList** or **{{{vList}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select sort order (Default is Ascending)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Ascending** or **Descending**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ascending")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Descending")]
        public string v_SortOrder { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a List Variable Name of the Sorted List")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vNewList** or **{{{vNewList}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        public string v_OutputList { get; set; }

        public SortListCommand()
        {
            this.CommandName = "SortListCommand";
            this.SelectionName = "Sort List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //get variable by regular name
            Script.ScriptVariable listVariable = v_InputList.GetRawVariable(engine);

            //if still null then throw exception
            if (listVariable == null)
            {
                throw new System.Exception("Complex Variable '" + v_InputList + "' or '" + v_InputList.ApplyVariableFormatting(engine) + "' not found. Ensure the variable exists before attempting to modify it.");
            }

            string sortOrder = v_SortOrder.ConvertToUserVariable(engine);
            if (String.IsNullOrEmpty(v_SortOrder))
            {
                sortOrder = "Ascending";
            }

            if (listVariable.VariableValue is List<string>)
            {
                List<string> newList = new List<string>();
                newList.AddRange((List<string>)listVariable.VariableValue);

                newList.Sort();
                if (sortOrder.ToLower() == "descending")
                {
                    newList.Reverse();
                }
                newList.StoreInUserVariable(engine, v_OutputList);
            }
            else
            {
                throw new Exception(v_InputList + " is not List or not-supported List.");
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Sort List " + this.v_InputList + ", To: " + this.v_OutputList + ", Order: " + this.v_SortOrder + "]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InputList))
            {
                this.validationResult += "List is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_OutputList))
            {
                this.validationResult += "Sorted list is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
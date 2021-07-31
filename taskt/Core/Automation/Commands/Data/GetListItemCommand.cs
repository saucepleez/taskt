using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using Microsoft.Office.Interop.Outlook;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.SubGruop("List")]
    [Attributes.ClassAttributes.Description("This command allows you to get an item from a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get an item from a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetListItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the List Name.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing List.")]
        [Attributes.PropertyAttributes.SampleUsage("**{{{myData}}}** or **[1,2,3]**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the index of the List item.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a valid List index value")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **{{{vIndex}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_ItemIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_UserVariableName { get; set; }

        public GetListItemCommand()
        {
            this.CommandName = "GetListItemCommand";
            this.SelectionName = "Get List Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;

        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var itemIndex = v_ItemIndex.ConvertToUserVariable(sender);
            int index = int.Parse(itemIndex);
            //get variable by regular name
            Script.ScriptVariable listVariable = engine.VariableList.Where(x => x.VariableName == v_ListName).FirstOrDefault();

            //user may potentially include brackets []
            if (listVariable == null)
            {
                listVariable = engine.VariableList.Where(x => x.VariableName.ApplyVariableFormatting() == v_ListName).FirstOrDefault();
            }

            //if still null then throw exception
            if (listVariable == null)
            {
                throw new System.Exception("Complex Variable '" + v_ListName + "' or '" + v_ListName.ApplyVariableFormatting() + "' not found. Ensure the variable exists before attempting to modify it.");
            }

            dynamic listToIndex;
            if (listVariable.VariableValue is List<string>)
            {
                listToIndex = (List<string>)listVariable.VariableValue;
            }
            else if (listVariable.VariableValue is List<MailItem>)
            {
                listToIndex = (List<MailItem>)listVariable.VariableValue;
            }
            else if (listVariable.VariableValue is List<OpenQA.Selenium.IWebElement>)
            {
                listToIndex = (List<OpenQA.Selenium.IWebElement>)listVariable.VariableValue;
            }
            else if ((listVariable.VariableValue.ToString().StartsWith("[")) && (listVariable.VariableValue.ToString().EndsWith("]")) && (listVariable.VariableValue.ToString().Contains(",")))
            {
                //automatically handle if user has given a json array
                Newtonsoft.Json.Linq.JArray jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(listVariable.VariableValue.ToString()) as Newtonsoft.Json.Linq.JArray;

                var itemList = new List<string>();
                foreach (var jsonItem in jsonArray)
                {
                    var value = (Newtonsoft.Json.Linq.JValue)jsonItem;
                    itemList.Add(value.ToString());
                }

                listVariable.VariableValue = itemList;
                listToIndex = itemList;
            }
            else
            {
                throw new System.Exception("Complex Variable List Type<T> Not Supported");
            }

            var item = listToIndex[index];

            Script.ScriptVariable newListItem = new Script.ScriptVariable
            {
                VariableName = v_UserVariableName,
                VariableValue = item
            };

            //Overwrites variable if it already exists
            if (engine.VariableList.Exists(x => x.VariableName == newListItem.VariableName))
            {
                Script.ScriptVariable temp = engine.VariableList.Where(x => x.VariableName == newListItem.VariableName).FirstOrDefault();
                engine.VariableList.Remove(temp);
            }
            engine.VariableList.Add(newListItem);

        }
        
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ItemIndex", this, editor));
            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UserVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_UserVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_UserVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);
            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_ListName}', Store In: '{v_UserVariableName}']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_ListName))
            {
                this.validationResult += "List Name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_ItemIndex))
            {
                this.validationResult += "Index of List item is empty.\n";
                this.IsValid = false;
            }
            else
            {
                int vIndex;
                if (int.TryParse(this.v_ItemIndex, out vIndex))
                {
                    if (vIndex < 0)
                    {
                        this.validationResult += "Specify a value of 0 or more for index of List item.\n";
                        this.IsValid = false;
                    }
                }
            }
            if (String.IsNullOrEmpty(this.v_UserVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
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
    [Attributes.ClassAttributes.Description("This command allows you to get the item count of a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get the item count of a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetListCountCommand : ScriptCommand
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
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_UserVariableName { get; set; }

        public GetListCountCommand()
        {
            this.CommandName = "GetListCountCommand";
            this.SelectionName = "Get List Count";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
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

            dynamic listToCount; 
            if (listVariable.VariableValue is List<string>)
            {
                listToCount = (List<string>)listVariable.VariableValue;
            }
            else if (listVariable.VariableValue is List<MailItem>)
            {
                listToCount = (List<MailItem>)listVariable.VariableValue;
            }
            else if (listVariable.VariableValue is List<OpenQA.Selenium.IWebElement>)
            {
                listToCount = (List<OpenQA.Selenium.IWebElement>)listVariable.VariableValue;
            }
            else if ((listVariable.VariableValue.ToString().StartsWith("[")) && (listVariable.VariableValue.ToString().EndsWith("]")) && (listVariable.VariableValue.ToString().Contains(",")))
            {
                //automatically handle if user has given a json array
                Newtonsoft.Json.Linq.JArray jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(listVariable.VariableValue.ToString()) as Newtonsoft.Json.Linq.JArray;

                var itemList = new List<string>();
                foreach (var item in jsonArray)
                {
                    var value = (Newtonsoft.Json.Linq.JValue)item;
                    itemList.Add(value.ToString());
                }

                listVariable.VariableValue = itemList;
                listToCount = itemList;
            }
            else
            {
                throw new System.Exception("Complex Variable List Type<T> Not Supported");
            }

            string count = listToCount.Count.ToString();

            count.StoreInUserVariable(sender, v_UserVariableName);

        }
        
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
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
            if (String.IsNullOrEmpty(this.v_UserVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}
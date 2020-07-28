using Microsoft.Office.Interop.Outlook;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command adds an item to an existing list variable.")]
    public class AddListItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("List")]
        [InputSpecification("Provide a list variable.")]
        [SampleUsage("{vList}")]
        [Remarks("Any type of variable other than list will cause error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("List Item")]
        [InputSpecification("Enter the list item for the variable.")]
        [SampleUsage("Hello || {vText}")]
        [Remarks("List item can only be a String, DataTable, MailItem or IWebElement.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListItem { get; set; }

        public AddListItemCommand()
        {
            CommandName = "AddListItemCommand";
            SelectionName = "Add List Item";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (AutomationEngineInstance)sender;

            var listVariable = VariableMethods.LookupVariable(engine, v_ListName);

            if (listVariable != null)
            {
                if (listVariable.VariableValue is List<string>)
                {
                    ((List<string>)listVariable.VariableValue).Add(v_ListItem.ConvertToUserVariable(engine));
                }
                else if (listVariable.VariableValue is List<DataTable>)
                {
                    var dataTable = VariableMethods.LookupVariable(engine, v_ListItem).VariableValue;
                    if (!(dataTable is DataTable))
                        throw new System.Exception("Invalid List Item type, please provide valid List Item type.");
                    ((List<DataTable>)listVariable.VariableValue).Add((DataTable)dataTable);
                }
                else if (listVariable.VariableValue is List<MailItem>)
                {
                    var mailItem = VariableMethods.LookupVariable(engine, v_ListItem).VariableValue;
                    if (!(mailItem is MailItem))
                        throw new System.Exception("Invalid List Item type, please provide valid List Item type."); 
                    ((List<MailItem>)listVariable.VariableValue).Add((MailItem)mailItem);
                }
                else if (listVariable.VariableValue is List<IWebElement>)
                {
                    var webElement = VariableMethods.LookupVariable(engine, v_ListItem).VariableValue;
                    if (!(webElement is IWebElement))
                        throw new System.Exception("Invalid List Item type, please provide valid List Item type.");
                    ((List<IWebElement>)listVariable.VariableValue).Add((IWebElement)webElement);
                }
                else
                {
                    throw new System.Exception("Complex Variable List Type<T> Not Supported");
                }
            }
            else
            {
                throw new System.Exception("Attempted to add data to a variable, but the variable was not found. Enclose variables within braces, ex. {vVariable}");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            //custom rendering
            base.Render(editor);

            //create control for variable name
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListItem", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Add Item '{v_ListItem}' to List '{v_ListName}']";
        }
    }
}
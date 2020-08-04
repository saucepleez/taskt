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
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using Exception = System.Exception;

namespace taskt.Commands
{
    [Serializable]
    [Group("List Commands")]
    [Description("This command adds an item to an existing List variable.")]
    public class AddListItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("List")]
        [InputSpecification("Provide a List variable.")]
        [SampleUsage("{vList}")]
        [Remarks("Any type of variable other than List will cause error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("List Item")]
        [InputSpecification("Enter the item to add to the List.")]
        [SampleUsage("Hello || {vItem}")]
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

            var vListVariable = VariableMethods.LookupVariable(engine, v_ListName);

            if (vListVariable != null)
            {
                if (vListVariable.VariableValue is List<string>)
                {
                    ((List<string>)vListVariable.VariableValue).Add(v_ListItem.Trim().ConvertToUserVariable(engine));
                }
                else if (vListVariable.VariableValue is List<DataTable>)
                {
                    DataTable dataTable;
                    ScriptVariable dataTableVariable = VariableMethods.LookupVariable(engine, v_ListItem.Trim());
                    if (dataTableVariable != null && dataTableVariable.VariableValue is DataTable)
                        dataTable = (DataTable)dataTableVariable.VariableValue;
                    else
                        throw new Exception("Invalid List Item type, please provide valid List Item type.");
                    ((List<DataTable>)vListVariable.VariableValue).Add(dataTable);
                }
                else if (vListVariable.VariableValue is List<MailItem>)
                {
                    MailItem mailItem;
                    ScriptVariable mailItemVariable = VariableMethods.LookupVariable(engine, v_ListItem.Trim());
                    if (mailItemVariable != null && mailItemVariable.VariableValue is MailItem)
                        mailItem = (MailItem)mailItemVariable.VariableValue;
                    else
                        throw new Exception("Invalid List Item type, please provide valid List Item type.");
                    ((List<MailItem>)vListVariable.VariableValue).Add(mailItem);
                }
                else if (vListVariable.VariableValue is List<IWebElement>)
                {
                    IWebElement webElement;
                    ScriptVariable webElementVariable = VariableMethods.LookupVariable(engine, v_ListItem.Trim());
                    if (webElementVariable != null && webElementVariable.VariableValue is IWebElement)
                        webElement = (IWebElement)webElementVariable.VariableValue;
                    else
                        throw new Exception("Invalid List Item type, please provide valid List Item type.");
                    ((List<IWebElement>)vListVariable.VariableValue).Add(webElement);
                }
                else
                {
                    throw new Exception("Complex Variable List Type<T> Not Supported");
                }
            }
            else
            {
                throw new Exception("Attempted to add data to a variable, but the variable was not found. Enclose variables within braces, ex. {vVariable}");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

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

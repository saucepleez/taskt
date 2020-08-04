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
    [Description("This command creates a new List variable.")]
    public class CreateListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("List Type")]
        [PropertyUISelectionOption("String")]
        [PropertyUISelectionOption("DataTable")]
        [PropertyUISelectionOption("MailItem")]
        [PropertyUISelectionOption("IWebElement")]
        [InputSpecification("Specify the data type of the List to be created.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ListType { get; set; }

        [XmlAttribute]
        [PropertyDescription("List Item(s)")]
        [InputSpecification("Enter the item(s) to write to the List.")]
        [SampleUsage("Hello || {vItem} || Hello,World || {vItem1},{vItem2}")]
        [Remarks("List item can only be a String, DataTable, MailItem or IWebElement.\n" + 
                 "Multiple items should be delimited by a comma(,). This input is optional.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListItems { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output List Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public CreateListCommand()
        {
            CommandName = "CreateListCommand";
            SelectionName = "Create List";
            CommandEnabled = true;
            CustomRendering = true;
            v_ListType = "String";
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (AutomationEngineInstance)sender;
            dynamic vNewList = null;
            string[] splitListItems = null;

            if (!string.IsNullOrEmpty(v_ListItems.Trim()))
            {
                splitListItems = v_ListItems.Split(',');
            }

            switch (v_ListType)
            {
                case "String":
                    vNewList = new List<string>();
                    if (splitListItems != null)
                    {
                        foreach (string item in splitListItems)
                            ((List<string>)vNewList).Add(item.Trim().ConvertToUserVariable(engine));
                    }                   
                    break;
                case "DataTable":
                    vNewList = new List<DataTable>();
                    if (splitListItems != null)
                    {                       
                        foreach (string item in splitListItems)
                        {
                            DataTable dataTable;
                            ScriptVariable dataTableVariable = VariableMethods.LookupVariable(engine, item.Trim());
                            if (dataTableVariable != null && dataTableVariable.VariableValue is DataTable)
                                dataTable = (DataTable)dataTableVariable.VariableValue;
                            else
                                throw new Exception("Invalid List Item type, please provide valid List Item type.");
                            ((List<DataTable>)vNewList).Add(dataTable);
                        }                           
                    }
                    break;
                case "MailItem":
                    vNewList = new List<MailItem>();
                    if (splitListItems != null)
                    {
                        foreach (string item in splitListItems)
                        {
                            MailItem mailItem;
                            ScriptVariable mailItemVariable = VariableMethods.LookupVariable(engine, item.Trim());
                            if (mailItemVariable != null && mailItemVariable.VariableValue is MailItem)
                                mailItem = (MailItem)mailItemVariable.VariableValue;
                            else
                                throw new Exception("Invalid List Item type, please provide valid List Item type.");
                            ((List<MailItem>)vNewList).Add(mailItem);
                        }
                    }
                    break;
                case "IWebElement":
                    vNewList = new List<IWebElement>();
                    if (splitListItems != null)
                    {
                        foreach (string item in splitListItems)
                        {
                            IWebElement webElement;
                            ScriptVariable webElementVariable = VariableMethods.LookupVariable(engine, item.Trim());
                            if (webElementVariable != null && webElementVariable.VariableValue is IWebElement)
                                webElement = (IWebElement)webElementVariable.VariableValue;
                            else
                                throw new Exception("Invalid List Item type, please provide valid List Item type.");
                            ((List<IWebElement>)vNewList).Add(webElement);
                        }
                    }
                    break;
            }

            engine.AddVariable(v_OutputUserVariableName, vNewList);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ListType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListItems", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Create New List<{v_ListType}> With Item(s) '{v_ListItems}' - Store List<{v_ListType}> in '{v_OutputUserVariableName}']";
        }
    }
}
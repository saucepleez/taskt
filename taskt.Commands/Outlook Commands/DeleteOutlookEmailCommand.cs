using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
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
    [Group("Outlook Commands")]
    [Description("This command deletes selected emails in Outlook.")]

    public class DeleteOutlookEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("MailItem")]
        [InputSpecification("Enter the MailItem to delete.")]
        [SampleUsage("{vMailItem}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MailItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delete Read Emails Only")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to delete read email messages only.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_DeleteReadOnly { get; set; }

        public DeleteOutlookEmailCommand()
        {
            CommandName = "DeleteOutlookEmailCommand";
            SelectionName = "Delete Outlook Email";
            CommandEnabled = true;
            CustomRendering = true;
            v_DeleteReadOnly = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            MailItem vMailItem = (MailItem)VariableMethods.LookupVariable(engine, v_MailItem).VariableValue;

            if (v_DeleteReadOnly == "Yes")
            {
                if (vMailItem.UnRead == false)
                    vMailItem.Delete();
            }
            else
                vMailItem.Delete();
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DeleteReadOnly", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [MailItem '{v_MailItem}']";
        }
    }
}
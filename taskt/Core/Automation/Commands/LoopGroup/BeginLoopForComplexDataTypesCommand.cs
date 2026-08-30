using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop")]
    [Attributes.ClassAttributes.CommandSettings("Loop Complex Data Types")]
    [Attributes.ClassAttributes.Description("This command allows you to Repeat actions on the values held by List, Dictioanry, and other data. This command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to iterate over each item in a list, or a series of items.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class BeginLoopForComplexDataTypesCommand : ScriptCommand, IHaveLoopAdditionalCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Variable Name or Value to Loop")]
        [InputSpecification("Enter a variable which contains a list of items")]
        //[SampleUsage("**{{{vMyList}}}** or **[1,2,3]**")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**{{{vList}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**[1,2,3]**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyValidationRule("Loop Target", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [Remarks("Use this command to iterate over the results of the Split command.")]
        public string v_LoopParameter { get; set; }

        public BeginLoopForComplexDataTypesCommand()
        {
            //this.CommandName = "BeginListLoopCommand";
            //this.SelectionName = "Loop List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            var loopCommand = (BeginLoopForComplexDataTypesCommand)parentCommand.ScriptCommand;

            //if (!engine.VariableList.Any(f => f.VariableName == "Loop.CurrentIndex"))
            //{
            //    engine.VariableList.Add(new Script.ScriptVariable() { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
            //}

            //Script.ScriptVariable complexVariable = null;

            ////get variable by regular name
            //complexVariable = engine.VariableList.Where(x => x.VariableName == v_LoopParameter).FirstOrDefault();

            ////user may potentially include brackets []
            //if (complexVariable == null)
            //{
            //    complexVariable = engine.VariableList.Where(x => x.VariableName.ApplyVariableFormatting(engine) == v_LoopParameter).FirstOrDefault();
            //}

            var complexVariable = v_LoopParameter.GetRawVariable(engine);

            //if still null then throw exception
            //if (complexVariable == null)
            //{
            //    throw new Exception("Complex Variable '" + v_LoopParameter + "' or '" + v_LoopParameter.ApplyVariableFormatting(engine) + "' not found. Ensure the variable exists before attempting to modify it.");
            //}

            dynamic listToLoop;
            if (complexVariable.VariableValue is List<string> lst)
            {
                listToLoop = lst;
            }
            else if (complexVariable.VariableValue is List<OpenQA.Selenium.IWebElement> webElems)
            {
                listToLoop = webElems;
            }
            else if (complexVariable.VariableValue is DataTable tbl)
            {
                listToLoop = tbl.Rows;
            }
            else if (complexVariable.VariableValue is List<Microsoft.Office.Interop.Outlook.MailItem> outlookMails)
            {
                listToLoop = outlookMails;
            }
            else if (complexVariable.VariableValue is Dictionary<string, string> dic)
            {
                listToLoop = dic.Values.ToList();
            }
            else if (complexVariable.VariableValue is List<MimeKit.MimeMessage> mailkitMails)
            {
                listToLoop = mailkitMails;
            }
            else if ((complexVariable.VariableValue.ToString().StartsWith("[")) && (complexVariable.VariableValue.ToString().EndsWith("]")) && (complexVariable.VariableValue.ToString().Contains(",")))
            {
                //automatically handle if user has given a json array
                var jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(complexVariable.VariableValue.ToString()) as Newtonsoft.Json.Linq.JArray;

                var itemList = new List<string>();
                foreach (var item in jsonArray)
                {
                    var value = (Newtonsoft.Json.Linq.JValue)item;
                    itemList.Add(value.ToString());
                }

                complexVariable.VariableValue = itemList;
                listToLoop = itemList;
            }
            else
            {
                throw new Exception("Complex Variable List Type<T> Not Supported");
            }

            int loopTimes = listToLoop.Count;

            for (int i = 0; i < loopTimes; i++)
            {
                if (complexVariable != null)
                {
                    complexVariable.CurrentPosition = i;
                }
                
                engine.ReportProgress($"Starting Loop Number {(i + 1)}/{loopTimes} From Line {loopCommand.LineNumber}");

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                    {
                        return;
                    }

                    //(i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");
                    SystemVariables.Update_LoopCurrentIndex(i + 1);

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress($"Exiting Loop From Line {loopCommand.LineNumber}");
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress($"Continuing Next Loop From Line {loopCommand.LineNumber}");
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }

                engine.ReportProgress($"Finished Loop From Line {loopCommand.LineNumber}");
            }
        }

        //public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LoopParameter", this, editor));
           
        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return "Loop List Variable '" + v_LoopParameter + "'";
        //}

        //public override bool IsValidate(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (string.IsNullOrEmpty(this.v_LoopParameter))
        //    {
        //        this.validationResult += "List variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
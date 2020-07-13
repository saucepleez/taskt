using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using Microsoft.Office.Interop.Outlook;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions several times (loop).  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to iterate over each item in a list, or a series of items.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    public class BeginListLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please input the list variable to be looped")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a variable which contains a list of items")]
        [Attributes.PropertyAttributes.SampleUsage("[vMyList]")]
        [Attributes.PropertyAttributes.Remarks("Use this command to iterate over the results of the Split command.")]
        public string v_LoopParameter { get; set; }

        public BeginListLoopCommand()
        {
            this.CommandName = "BeginListLoopCommand";
            this.SelectionName = "Loop List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            Core.Automation.Commands.BeginListLoopCommand loopCommand = (Core.Automation.Commands.BeginListLoopCommand)parentCommand.ScriptCommand;
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            int loopTimes;
            Script.ScriptVariable complexVariable = null;


            //get variable by regular name
            complexVariable = engine.VariableList.Where(x => x.VariableName == v_LoopParameter).FirstOrDefault();


            if (!engine.VariableList.Any(f => f.VariableName == "Loop.CurrentIndex"))
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
            }

            //user may potentially include brackets []
            if (complexVariable == null)
            {
                complexVariable = engine.VariableList.Where(x => x.VariableName.ApplyVariableFormatting() == v_LoopParameter).FirstOrDefault();
            }

            //if still null then throw exception
            if (complexVariable == null)
            {
                throw new System.Exception("Complex Variable '" + v_LoopParameter + "' or '" + v_LoopParameter.ApplyVariableFormatting() + "' not found. Ensure the variable exists before attempting to modify it.");
            }

            dynamic listToLoop;
            if (complexVariable.VariableValue is List<string>)
            {
             listToLoop = (List<string>)complexVariable.VariableValue;
            }
            else if (complexVariable.VariableValue is List<OpenQA.Selenium.IWebElement>)
            {
              listToLoop = (List<OpenQA.Selenium.IWebElement>)complexVariable.VariableValue;
            }
            else if (complexVariable.VariableValue is DataTable)
            {
                listToLoop = ((DataTable)complexVariable.VariableValue).Rows;
            }
            else if (complexVariable.VariableValue is List<MailItem>)
            {
                listToLoop = (List<MailItem>)complexVariable.VariableValue;
            }
            else if ((complexVariable.VariableValue.ToString().StartsWith("[")) && (complexVariable.VariableValue.ToString().EndsWith("]")) && (complexVariable.VariableValue.ToString().Contains(",")))
            {
                //automatically handle if user has given a json array
                Newtonsoft.Json.Linq.JArray jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(complexVariable.VariableValue.ToString()) as Newtonsoft.Json.Linq.JArray;

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
                throw new System.Exception("Complex Variable List Type<T> Not Supported");
            }


            loopTimes = listToLoop.Count;


            for (int i = 0; i < loopTimes; i++)
            {
                if (complexVariable != null)
                    complexVariable.CurrentPosition = i;

             

                engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    (i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");

                    engine.ExecuteCommand(cmd);


                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress("Continuing Next Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }

                engine.ReportProgress("Finished Loop From Line " + loopCommand.LineNumber);
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LoopParameter", this, editor));
           
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return "Loop List Variable '" + v_LoopParameter + "'";
        }
    }
}
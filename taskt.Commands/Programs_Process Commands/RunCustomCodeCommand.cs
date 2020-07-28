using SimpleNLG.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using Group = taskt.Core.Attributes.ClassAttributes.Group;

namespace taskt.Commands
{
    [Serializable]
    [Group("Programs/Process Commands")]
    [Description("This command runs custom C# code. The code in this command is compiled and run at runtime when this command is invoked.")]

    public class RunCustomCodeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("C# Code")]
        [InputSpecification("Enter the code to be executed or use the builder to create your custom C# code. "+
                            "The builder contains a Hello World template that you can use to build from.")]
        [SampleUsage("{vString}.Remove() || {vCode}")]
        [Remarks("This command only supports the standard framework classes.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowCodeBuilder)]
        public string v_Code { get; set; }

        [XmlAttribute]
        [PropertyDescription("Arguments")]
        [InputSpecification("Enter arguments that the custom code will receive during execution, split them using commas.")]
        [SampleUsage("hello || {vArg} || hello,world || {vArg1},{vArg2}")]
        [Remarks("This input is optional.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Args { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Data Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public RunCustomCodeCommand()
        {
            CommandName = "RunCustomCodeCommand";
            SelectionName = "Run Custom Code";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var customCode = v_Code.ConvertToUserVariable(engine);

            if (customCode.Contains("static void Main"))
            {
                //User entered a Main so compile and execute the code
                CompileAndExecute(customCode, sender);
            } else
            {
                // TODO: This branch of the functionality should be split into its own command, say Execute Method.
                // User entered a line of code to be evaluated at runtime
                dynamic result;

                // Using Regex, retrieve all the entries in v_Code that match {myVar}, then trim the curly braces
                var userInputtedVariables = Regex.Matches(v_Code, @"{\w+}").Cast<Match>().Select(match => match.Value).ToList();
                userInputtedVariables = userInputtedVariables.Select(var => var.Trim('{', '}')).ToList();

                var memberToInvoke = Regex.Match(v_Code, @"\.(\w+)").Groups[1].ToString();

                List<ScriptVariable> variableList = engine.VariableList;
                variableList.AddRange(Common.GenerateSystemVariables());

                object invokingVar = (from vars in variableList
                                      where vars.VariableName == userInputtedVariables[0]
                                      select vars.VariableValue).FirstOrDefault();
                userInputtedVariables.RemoveAt(0);

                try
                {
                    if (v_Code.contains("(") && v_Code.contains(")"))
                    {
                        List<ScriptVariable> argList = new List<ScriptVariable>();

                        foreach(string var in userInputtedVariables)
                        {
                            ScriptVariable arg = (from vars in variableList
                                                  where vars.VariableName == var
                                                  select vars).FirstOrDefault();

                            argList.Add(arg);
                        }

                        result = InvokeTypeMember(invokingVar, memberToInvoke, argList);
                    }
                    else
                    {
                        // User wants to access a property
                        PropertyInfo propInfo = invokingVar.GetType().GetProperty(memberToInvoke);
                        result = propInfo.GetValue(invokingVar, null);
                    }
                } catch
                {
                    throw new Exception("Failed to invoke member " + memberToInvoke + " for class: " + invokingVar.GetType().ToString());
                }

                if(v_OutputUserVariableName.Length != 0)
                {
                    engine.AddVariable(v_OutputUserVariableName, result);
                }
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Code", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Args", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Data in '{v_OutputUserVariableName}']";
        }

        private void CompileAndExecute(string customCode, object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //compile custom code
            var compilerSvc = new CompilerServices();
            var result = compilerSvc.CompileInput(customCode);

            //check for errors
            if (result.Errors.HasErrors)
            {
                //throw exception
                var errors = string.Join(", ", result.Errors);
                throw new Exception("Errors Occured: " + errors);
            }
            else
            {
                var arguments = v_Args.ConvertToUserVariable(engine);

                //run code, taskt will wait for the app to exit before resuming
                using (Process scriptProc = new Process())
                {
                    scriptProc.StartInfo.FileName = result.PathToAssembly;
                    scriptProc.StartInfo.Arguments = arguments;

                    if (v_OutputUserVariableName != "")
                    {
                        //redirect output
                        scriptProc.StartInfo.RedirectStandardOutput = true;
                        scriptProc.StartInfo.UseShellExecute = false;
                    }

                    scriptProc.Start();
                    scriptProc.WaitForExit();

                    if (v_OutputUserVariableName != "")
                    {
                        var output = scriptProc.StandardOutput.ReadToEnd();
                        output.StoreInUserVariable(engine, v_OutputUserVariableName);
                    }
                }
            }
        }

        private dynamic InvokeTypeMember(object invokingVar, string memberToInvoke, List<ScriptVariable> methodArgs)
        {
            dynamic result;
            MethodInfo methodInfo;
            Type type = invokingVar.GetType();

            if (methodArgs.isEmpty())
            {
                // Retrieving the method info with restrictions to limit our search to method definitions
                // without arguments to avoid ambigious matches.
                methodInfo = type.GetMethods()
                    .Single(method =>
                        method.Name == memberToInvoke &&
                        method.GetGenericArguments().Length == 0 &&
                        method.GetParameters().Length == 0
                        );
                result = methodInfo.Invoke(invokingVar, null);
            }
            else
            {
                // Converts methodArgs, a list of ScriptVariables, into a list of objects
                // which we can use to determine which method to invoke
                object[] methodArgsValueArray = methodArgs.Select(arg => arg.VariableValue).ToArray();

                // Converting our array of objects into an array of types to pass into GetMethod.
                // This lets us select only the method who's argument types match our argument types.
                Type[] argumentTypeArray = methodArgsValueArray.Select(variable => variable.GetType()).ToArray();
                methodInfo = type.GetMethod(memberToInvoke, argumentTypeArray);

                result = methodInfo.Invoke(invokingVar, methodArgsValueArray);
            }
            return result;
        }
    }
}


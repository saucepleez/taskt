﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.Description("This command waits for a window to exist.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to explicitly wait for a window to exist before continuing script execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class WaitForWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter or select the window name that you are waiting for to exist.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Input or Type the name of the window that you want to wait to exist.")]
        [SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindow}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsWindowNamesList(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [PropertyDescription("Window title search method")]
        [InputSpecification("")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Exact match")]
        [SampleUsage("**Contains** or **Starts with** or **Ends with** or **Exact match**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Contains")]
        public string v_SearchMethod { get; set; }
        [XmlAttribute]
        [PropertyDescription("Indicate how many seconds to wait before an error should be raised.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify how many seconds to wait before an error should be invoked")]
        [SampleUsage("**5** or **{{{vWaitTime}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_LengthToWait { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;

        public WaitForWindowCommand()
        {
            this.CommandName = "WaitForWindowCommand";
            this.SelectionName = "Wait For Window To Exist";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //string windowName = v_WindowName.ConvertToUserVariable(sender);
            //bool targetIsCurrentWindow = ((Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword == windowName;

            //if (targetIsCurrentWindow)
            //{
            //    return; // always exists
            //}

            //string searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(searchMethod))
            //{
            //    searchMethod = "Contains";
            //}

            //var lengthToWait = v_LengthToWait.ConvertToUserVariable(sender);
            //var waitUntil = int.Parse(lengthToWait);
            //var endDateTime = DateTime.Now.AddSeconds(waitUntil);

            //if (searchMethod == "Contains")
            //{
            //    IntPtr hWnd = IntPtr.Zero;
            //    while (DateTime.Now < endDateTime)
            //    {
            //        hWnd = User32Functions.FindWindow(windowName);

            //        if (hWnd != IntPtr.Zero) //If found
            //            break;

            //        System.Threading.Thread.Sleep(1000);
            //    }
            //    if (hWnd == IntPtr.Zero)
            //    {
            //        throw new Exception("Window was not found in the allowed time!");
            //    }
            //}
            //else
            //{
            //    Func<string, bool> searchFunc;
            //    switch (searchMethod)
            //    {
            //        case "Starts with":
            //            searchFunc = (s) => s.StartsWith(windowName);
            //            break;

            //        case "Ends with":
            //            searchFunc = (s) => s.EndsWith(windowName);
            //            break;

            //        case "Exact match":
            //            searchFunc = (s) => (s == windowName);
            //            break;

            //        default:
            //            throw new Exception("Search method " + searchMethod + " is not support.");
            //            break;
            //    }

            //    bool isFind = false;
            //    while (DateTime.Now < endDateTime)
            //    {
            //        List<IntPtr> hWnds = User32Functions.FindWindowsGreedy(windowName);
            //        foreach(var hWnd in hWnds)
            //        {
            //            if (searchFunc(User32Functions.GetWindowTitle(hWnd)))
            //            {
            //                isFind = true;
            //                break;
            //            }
            //        }

            //        System.Threading.Thread.Sleep(1000);
            //    }

            //    if (!isFind)
            //    {
            //        throw new Exception("Window was not found in the allowed time!");
            //    }
            //}

            var engine = (Engine.AutomationEngineInstance)sender;

            string windowName = v_WindowName.ConvertToUserVariable(sender);
            string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

            if (windowName == engine.engineSettings.CurrentWindowKeyword)
            {
                windowName = WindowNameControls.GetCurrentWindowName();
            }

            var waitUntil = v_LengthToWait.ConvertToUserVariableAsInteger("Length to Wait", sender);
            var endDateTime = DateTime.Now.AddSeconds(waitUntil);

            bool isFind = false;
            while (DateTime.Now < endDateTime)
            {
                try
                {
                    IntPtr wHnd = WindowNameControls.FindWindow(windowName, searchMethod, engine);
                    isFind = true;
                    break;
                }
                catch
                {
                    
                }
                System.Threading.Thread.Sleep(1000);
            }

            if (!isFind)
            {
                throw new Exception("Window was not found in the allowed time!");
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            //WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            //RenderedControls.Add(WindowNameControl);

            //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

            ////create standard group controls
            //var lengthToWaitControlSet = CommandControls.CreateDefaultInputGroupFor("v_LengthToWait", this, editor);
            //RenderedControls.AddRange(lengthToWaitControlSet);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }
        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            WindowNameControl.AddWindowNames();
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_WindowName + "', Wait Up To " + v_LengthToWait + " seconds]";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_WindowName))
        //    {
        //        this.validationResult += "Window is empty.\n";
        //        this.IsValid = false;
        //    }

        //    if (String.IsNullOrEmpty(this.v_LengthToWait))
        //    {
        //        this.validationResult += "Wait time is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        int wait;
        //        if (int.TryParse(this.v_LengthToWait, out wait))
        //        {
        //            if (wait <= 0)
        //            {
        //                this.validationResult += "Specify a value of 1 or more for wait time.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }

        //    return this.IsValid;
        //}

        public override void convertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToIntermediateWindowName");
            convertToIntermediate(settings, cnv, variables);
        }

        public override void convertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToRawWindowName");
            convertToRaw(settings, cnv);
        }
    }
}
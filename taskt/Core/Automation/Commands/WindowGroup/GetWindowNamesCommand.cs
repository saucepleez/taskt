using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Names")]
    [Attributes.ClassAttributes.Description("This command returns window names.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want window names.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowNamesCommand : AWindowNamesCommands, ICanHandleList
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        [PropertyAvailableSystemVariable(SystemVariables.LimitedSystemVariableNames.Window_AllWindows)]
        [PropertyIsWindowNamesList(true, true, true, false)]
        public override string v_WindowName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        //public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyIsOptional(false)]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyParameterOrder(6500)]
        public override string v_WindowNameResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnore))]
        [PropertyDescription("When Window Not Found")]
        [PropertyUISelectionOption("Set Empty")]
        [PropertyDetailSampleUsage("**Set Empty**", "Window Names Result Is Empty LIST")]
        [PropertyIsOptional(true, "Set Empty")]
        [PropertyParameterOrder(6600)]
        public string v_WhenWindowNotFound { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //[PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        public override string v_WaitTimeForWindow { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowNameResult))]
        //public string v_NameResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_OutputWindowHandle))]
        //public string v_HandleResult { get; set; }

        public GetWindowNamesCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine,
                new Action<List<(IntPtr, string)>>((wins) =>
                {
                    //this.StoreListInUserVariable(wins.Select(w => w.Item2).ToList(), nameof(v_WindowNameResult), engine);
                }),
                new Action<Exception>((ex) =>
                {
                    switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenWindowNotFound), engine))
                    {
                        case "set empty":
                            this.StoreListInUserVariable(new List<string>(), nameof(v_WindowNameResult), engine);
                            break;
                        case "ignore":
                            break;
                        case "error":
                            throw ex;
                    }
                })
            );
        }

        //public override void Refresh(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        //}
    }
}
using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;
using taskt.Core.Script;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for descendants search UIElements from Window Name by TreeWalker commands
    /// </summary>
    public abstract class ADescendantsSearchUIElementsFromWindowNameByTreeWalkerCommands : ADescendantsSearchUIElementsFromSomethingByTreeWalkerCommands, IOneWindowNameProperties, IUIElementSearchUIElementFromWindowSomethingByAnywayProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_CheckMethod))]
        [PropertyParameterOrder(8100)]
        public virtual string v_CheckMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_SelectionMethod_Single))]
        [PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        [PropertyParameterOrder(8200)]
        public virtual string v_SelectionMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_TargetWindowIndex))]
        [PropertyParameterOrder(8300)]
        public virtual string v_TargetWindowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        [PropertyParameterOrder(8400)]
        public virtual string v_WaitTimeForWindow { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WindowUIElementName))]
        [PropertyParameterOrder(10200)]
        public virtual string v_WindowUIElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_CaseSensitive))]
        [PropertyParameterOrder(11000)]
        public virtual string v_CaseSensitive { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_TrimBeforeCheck))]
        [PropertyParameterOrder(11100)]
        public virtual string v_TrimBeforeCheck { get; set; }

        protected void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        public override void Refresh(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        }

        /// <summary>
        /// search window UIElement after action
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="actionFunc">action process, arg1 is Window UIElement</param>
        protected void SearchWindowAfterAction(Engine.AutomationEngineInstance engine, Func<InnerScriptVariable, ScriptCommand> actionFunc)
        {
            this.SearchWindowAfterActionCore(engine,
                new Func<InnerScriptVariable, ScriptCommand>(winElem =>
                {
                    return new UIAutomationGetWindowUIElementCommand()
                    {
                        v_WindowName = this.v_WindowName,
                        v_CheckMethod = this.v_CheckMethod,
                        v_SelectionMethod = this.v_SelectionMethod,
                        v_TargetWindowIndex = this.v_TargetWindowIndex,
                        v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                        v_Result = winElem.VariableName,
                        v_WindowNameResult = this.v_WindowNameResult,
                        v_WindowHandleResult = this.v_WindowHandleResult,
                        v_CaseSensitive = this.v_CaseSensitive,
                        v_TrimBeforeCheck = this.v_TrimBeforeCheck,
                    };
                }),
                actionFunc);
        }
    }
}
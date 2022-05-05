using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.Description("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class UIAutomationClickElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify AutomationElement Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindowName}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyDescription("Set Search Parameters")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Use the Element Recorder to generate a listing of potential search parameters.")]
        [SampleUsage("n/a")]
        [Remarks("Once you have clicked on a valid window the search parameters will be populated.  Enable only the ones required to be a match at runtime.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true)]
        [PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        public DataTable v_ActionParameters { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView SearchParametersGridViewHelper;

        public UIAutomationClickElementCommand()
        {
            this.CommandName = "UIAutomationClickElementCommand";
            this.SelectionName = "Click Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);

            System.Windows.Point point;
            if (!targetElement.TryGetClickablePoint(out point))
            {
                throw new Exception("No Clickable Point in AutomationElement '" + v_TargetElement + "'");
            }

            var actionParams = DataTableControls.GetFieldValues(v_ActionParameters, "ParameterName", "ParameterValue");
            var click = actionParams["Click Type"].ConvertToUserVariable(engine);
            var xAd = actionParams["X Adjustment"].ConvertToUserVariableAsInteger("X Adjustment", engine);
            var yAd = actionParams["Y Adjustment"].ConvertToUserVariableAsInteger("Y Adjustment", engine);

            string windowName = AutomationElementControls.GetWindowName(targetElement);
            var activateWindow = new ActivateWindowCommand()
            {
                v_WindowName = windowName
            };
            activateWindow.RunCommand(sender);

            var mouseClick = new SendMouseMoveCommand()
            {
                v_MouseClick = click,
                v_XMousePosition = (point.X + xAd).ToString(),
                v_YMousePosition = (point.Y + yAd).ToString()
            };
            mouseClick.RunCommand(sender);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            DataGridView dgv = (DataGridView)CommandControls.GetControlsByName(ctrls, "v_ActionParameters", CommandControls.CommandControlType.Body)[0];
            v_ActionParameters.Rows.Add("Click Type", "");
            v_ActionParameters.Rows.Add("X Adjustment", 0);
            v_ActionParameters.Rows.Add("Y Adjustment", 0);

            var mouseClickBox = new DataGridViewComboBoxCell();
            mouseClickBox.Items.Add("Left Click");
            mouseClickBox.Items.Add("Middle Click");
            mouseClickBox.Items.Add("Right Click");
            mouseClickBox.Items.Add("Left Down");
            mouseClickBox.Items.Add("Middle Down");
            mouseClickBox.Items.Add("Right Down");
            mouseClickBox.Items.Add("Left Up");
            mouseClickBox.Items.Add("Middle Up");
            mouseClickBox.Items.Add("Right Up");
            mouseClickBox.Items.Add("Double Left Click");
            dgv.Rows[0].Cells[1] = mouseClickBox;

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Root Element: '" + v_TargetElement + "']";
        }

    }
}
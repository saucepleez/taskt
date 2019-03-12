using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Server;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to upload data to a local tasktServer bot store")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to upload or share data across bots.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class UploadDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate a name of the key to create")]
        [Attributes.PropertyAttributes.InputSpecification("Select a variable or provide an input value")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_KeyName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a target variable or input value to upload")]
        [Attributes.PropertyAttributes.InputSpecification("Select a variable or provide an input value")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        public UploadDataCommand()
        {
            this.CommandName = "UploadDataCommand";
            this.SelectionName = "Upload BotStore Data";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var keyName = v_KeyName.ConvertToUserVariable(sender);
            var keyValue = v_InputValue.ConvertToUserVariable(sender);

            try
            {
                var result = HttpServerClient.UploadData(keyName, keyValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_KeyName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Upload Data to Key '" + v_KeyName + "' in tasktServer BotStore]";
        }
    }
}
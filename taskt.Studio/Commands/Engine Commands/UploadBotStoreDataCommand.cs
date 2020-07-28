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
using taskt.Server;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command allows you to upload data to a local tasktServer bot store")]
    [UsesDescription("Use this command when you want to upload or share data across bots.")]
    [ImplementationDescription("")]
    public class UploadBotStoreDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate a name of the key to create")]
        [InputSpecification("Select a variable or provide an input value")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_KeyName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a target variable or input value to upload")]
        [InputSpecification("Select a variable or provide an input value")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        public UploadBotStoreDataCommand()
        {
            CommandName = "UploadBotStoreDataCommand";
            SelectionName = "Upload BotStore Data";
            CommandEnabled = true;
            CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var keyName = v_KeyName.ConvertToUserVariable(engine);
            var keyValue = v_InputValue.ConvertToUserVariable(engine);
            
            try
            {
                var result = HttpServerClient.UploadData(keyName, keyValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        public override List<Control> Render(IfrmCommandEditor editor)
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
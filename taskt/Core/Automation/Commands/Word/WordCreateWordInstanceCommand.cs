using System;
using System.Diagnostics;
using System.Windows.Automation.Provider;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command creates a Word Instance.")]
    [Attributes.ClassAttributes.CommandSettings("Create Word Instance")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Word.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordCreateWordInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        public string v_WindowHandle { get; set; }

        public WordCreateWordInstanceCommand()
        {
            //this.CommandName = "WordCreateApplicationCommand";
            //this.SelectionName = "Create Word Application";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var vInstance = v_InstanceName.ExpandValueOrUserVariable(engine);
            var newWordSession = new Microsoft.Office.Interop.Word.Application
            {
                Visible = true
            };

            engine.AddAppInstance(vInstance, newWordSession);

            if (!string.IsNullOrEmpty(v_WindowHandle))
            {
                newWordSession.Activate();
                var currentCaption = newWordSession.Application.Caption;

                var rnd = new Random();

                var newCaption = "rpa_word_" + rnd.Next() + "_" + rnd.Next();
                newWordSession.Application.Caption = newCaption;

                bool isFound = false;
                foreach(var p in Process.GetProcessesByName("winword"))
                {
                    if (p.MainWindowTitle == newCaption)
                    {
                        p.MainWindowHandle.StoreInUserVariable(engine, v_WindowHandle);
                        newWordSession.Application.Caption = currentCaption;
                        isFound = true;
                        break;
                    }
                }
                if (!isFound)
                {
                    throw new Exception("Fail to Get Word Instance Window Handle");
                }
            }

        }
    }
}
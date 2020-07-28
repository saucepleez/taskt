using SHDocVw;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.App;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("IE Browser Commands")]
    [Description("This command creates a new IE Web Browser Session.")]
    public class IECreateBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("IE Instance Name")]
        [InputSpecification("Enter a unique name that will represent the IE Browser instance.")]
        [SampleUsage("MyIEInstance")]
        [Remarks("This unique name allows you to refer to the instance by name in future commands, " +
                 "ensuring that the commands you specify run against the correct IE Browser.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Instance Tracking (after task ends)")]
        [PropertyUISelectionOption("Forget Instance")]
        [PropertyUISelectionOption("Keep Instance Alive")]
        [InputSpecification("Specify if taskt should remember this instance name after the script has finished executing.")]
        [SampleUsage("")]
        [Remarks("Calling the **Close Browser** command or closing the application will end the instance.")]
        public string v_InstanceTracking { get; set; }

        [XmlAttribute]
        [PropertyDescription("Navigate to URL")]
        [InputSpecification("Enter a Web URL to navigate to.")]
        [SampleUsage("https://example.com/ || {vURL}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public IECreateBrowserCommand()
        {
            CommandName = "IECreateBrowserCommand";
            SelectionName = "Create Browser";
            v_InstanceName = "default";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var webURL = v_URL.ConvertToUserVariable(engine);

            InternetExplorer newBrowserSession = new InternetExplorer();
            try
            {
                newBrowserSession.Navigate(webURL);
                WaitForReadyState(newBrowserSession);
                newBrowserSession.Visible = true;
            }
            catch (Exception ex) 
            {
                throw ex;
            }

            //add app instance
            engine.AddAppInstance(v_InstanceName, newBrowserSession);

            //handle app instance tracking
            if (v_InstanceTracking == "Keep Instance Alive")
            {
                GlobalAppInstances.AddInstance(v_InstanceName, newBrowserSession);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_InstanceTracking", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_URL", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
        }

        public static void WaitForReadyState(InternetExplorer ieInstance)
        {
            try
            {
                DateTime waitExpires = DateTime.Now.AddSeconds(15);

                do
                {
                    Thread.Sleep(500);
                }

                while ((ieInstance.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
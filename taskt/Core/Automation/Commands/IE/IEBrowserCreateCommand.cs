using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new IE web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserCreateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Instance Tracking (after task ends)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Forget Instance")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Keep Instance Alive")]
        [Attributes.PropertyAttributes.InputSpecification("Specify if taskt should remember this instance name after the script has finished executing.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Forget Instance** to forget the instance or **Keep Instance Alive** to allow subsequent tasks to call the instance by name.")]
        [Attributes.PropertyAttributes.Remarks("Calling the **Close Browser** command or ending the browser session will end the instance.  This command only works during the lifetime of the application.  If the application is closed, the references will be forgetten automatically.")]
        public string v_InstanceTracking { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public IEBrowserCreateCommand()
        {
            this.CommandName = "IEBrowserCreateCommand";
            this.SelectionName = "Create Browser";
            this.v_InstanceName = "";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var instanceName = v_InstanceName.ConvertToUserVariable(sender);

            SHDocVw.InternetExplorer newBrowserSession = new SHDocVw.InternetExplorer();
            try
            {
                newBrowserSession.Navigate(v_URL.ConvertToUserVariable(sender));
                WaitForReadyState(newBrowserSession);
                newBrowserSession.Visible = true;
            }
            catch (Exception ex) { }

            //add app instance
            engine.AddAppInstance(instanceName, newBrowserSession);

            //handle app instance tracking
            if (v_InstanceTracking == "Keep Instance Alive")
            {
                GlobalAppInstances.AddInstance(instanceName, newBrowserSession);
            }

        }

        public static void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {
            try
            {
                DateTime waitExpires = DateTime.Now.AddSeconds(15);

                do
                {
                    System.Threading.Thread.Sleep(500);
                }

                while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
            }
            catch (Exception ex) { }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_URL", this, editor));

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
            }

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Selenium WebBrowser methods
    /// </summary>
    internal static class SeleniumBrowserControls
    {
        /// <summary>
        /// instance property
        /// </summary>
        [PropertyDescription("WebBrowser Instance Name")]
        [InputSpecification("WebBrowser Instance Name", true)]
        [PropertyDetailSampleUsage("**RPABrowser**", PropertyDetailSampleUsage.ValueType.Value, "WebBrowser Instance")]
        [PropertyDetailSampleUsage("**{{{vInstance}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebBrowser Instance")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Broser** command will cause an error")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyFirstValue("%kwd_default_browser_instance%")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("WebBrowser Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public static string v_InputInstanceName { get; }

        /// <summary>
        /// get WebBrowser Instance
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IWebDriver GetSeleniumBrowserInstance(string instanceName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var vInstance = instanceName.ConvertToUserVariable(engine);
            var browserObject = engine.GetAppInstance(vInstance);

            if (browserObject is IWebDriver wd)
            {
                return wd;
            }
            else
            {
                throw new Exception("Instance Name '" + instanceName + "' is not WebBrowser Instance. Parsed Value: '" + vInstance + "'");
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumSendSpecialKeystrokesPropetiesExtensionMethods
    {
        /// <summary>
        /// Get special keys list of OpenQA
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSpecialKeysList(this ISeleniumSendSpecialKeystrokesProperties command)
        {
            var fields = typeof(OpenQA.Selenium.Keys).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            return fields.Select(f => f.Name).ToList();
        }
    }
}

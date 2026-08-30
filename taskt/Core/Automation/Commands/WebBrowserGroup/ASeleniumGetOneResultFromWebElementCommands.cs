using System.Xml.Serialization;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for get one result from WebElement commands
    /// </summary>
    public abstract class ASeleniumGetOneResultFromWebElementCommands : ASeleniumGetFromWebElementCommands, IResultProperties
    {
        /// <summary>
        /// variable name to store result
        /// </summary>
        [XmlAttribute]
        public abstract string v_Result { get; set; }

        /// <summary>
        /// store empty value to Result varialbe
        /// </summary>
        /// <param name="engine"></param>
        protected void StoreEmptyValueToResult(AutomationEngineInstance engine)
        {
            string.Empty.StoreInUserVariable(engine, v_Result);
        }
    }
}

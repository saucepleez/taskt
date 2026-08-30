using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for remove invalid file/folder name commands
    /// </summary>
    public abstract class AFileRemoveInvalidFileNameCharactorsCommands : ScriptCommand, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("File Name")]
        [PropertyParameterOrder(5000)]
        public virtual string v_TargetName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6000)]
        public virtual string v_Result { get; set; }

        /// <summary>
        /// remove invalid file/folder name charactors
        /// </summary>
        /// <param name="engine"></param>
        protected void RemoveInvalidCharactors(Engine.AutomationEngineInstance engine)
        {
            var name = this.ExpandValueOrUserVariable(nameof(v_TargetName), "Target Name", engine);

            var chars = Path.GetInvalidFileNameChars();

            var isNextLoop = true;
            do
            {
                var idx = name.IndexOfAny(chars);
                if (idx >= 0)
                {
                    var bef = name.Substring(0, idx);
                    var aft = name.Substring(idx + 1);
                    name = bef + aft;
                }
                else
                {
                    isNextLoop = false;
                }
            } while (isNextLoop);

            name.StoreInUserVariable(engine, v_Result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Get Special Folder Path")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Special Folder Path, like Documents, etc.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Special Folder Path, like Documents, etc.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetSpecialFolderPathCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Special Folder Type")]
        [PropertyValidationRule("Folder Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyComboBoxItemMethod(nameof(CreateSpecialFoldersList))]
        [PropertyDisplayText(true, "FolderType")]
        public string v_FolderType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public GetSpecialFolderPathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var t = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_FolderType), engine);

            if (Enum.TryParse<Environment.SpecialFolder>(t, true, out var specialFolderValue))
            {
                var p = Environment.GetFolderPath(specialFolderValue);
                p.StoreInUserVariable(engine, v_Result);
            }
            else
            {
                string p = "";
                switch (t.ToLower())
                {
                    case "temporary":
                        p = Path.GetTempPath();
                        break;
                    case "downloads":
                        p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                        break;
                    default:
                        throw new Exception($"Strange Special Folder Type. Value: '{v_FolderType}', Expand Value: '{t}'");
                }
                p.StoreInUserVariable(engine, v_Result);
            }
        }

        /// <summary>
        /// get special folders list
        /// </summary>
        /// <returns></returns>
        private List<string> CreateSpecialFoldersList()
        {
            var lst = Enum.GetNames(typeof(Environment.SpecialFolder)).ToList();
            lst.Add("Temporary");
            lst.Add("Downloads");

            return lst;
        }
    }
}
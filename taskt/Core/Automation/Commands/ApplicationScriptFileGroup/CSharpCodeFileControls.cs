using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public static class CSharpCodeFileControls
    {
        /// <summary>
        /// executable file name
        /// </summary>
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Compiled Executable File Name")]
        [PropertyIsOptional(true, "tasktOnTheFly")]
        [PropertyFirstValue("tasktOnTheFly")]
        [PropertyValidationRule("File Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "File Name")]
        public static string v_ExecutableFileName { get;}
    }
}

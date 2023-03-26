using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Comment")]
    [Attributes.ClassAttributes.Description("This command allows you to add an in-line comment to the script.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add code comments or document code.  Usage of variables (ex. [vVar]) within the comment block will be parsed and displayed when running the script.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is for visual purposes only")]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    public class CommentCommand : ScriptCommand
    {
        public CommentCommand()
        {
            //this.CommandName = "CommentCommand";
            //this.SelectionName = "Add Code Comment";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.DisplayForeColor = System.Drawing.Color.ForestGreen;
        }

        public override string GetDisplayValue()
        {
            return "// Comment: " + this.v_Comment;
        }
    }
}
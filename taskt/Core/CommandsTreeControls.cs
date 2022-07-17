using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.Core
{
    internal class CommandsTreeControls
    {
        public static TreeNode[] CreateAllCommandsArray(Core.ApplicationSettings settings)
        {
            List<taskt.UI.CustomControls.AutomationCommand> automationCommands = UI.CustomControls.CommandControls.GenerateCommandsandControls();

            List<TreeNode> commandsTreeList = new List<TreeNode>();

            if (settings.ClientSettings.GroupingBySubgroup)
            {
                CommandsSortBySubGroupAndGroup(commandsTreeList, automationCommands);
            }
            else
            {
                CommandsSortByGroup(commandsTreeList, automationCommands);
            }

            return commandsTreeList.ToArray();
        }

        private static void CommandsSortByGroup(List<TreeNode> treeCommands, List<taskt.UI.CustomControls.AutomationCommand> commands)
        {
            var groupedCommands = commands.GroupBy(f => f.DisplayGroup);

            foreach (var cmd in groupedCommands)
            {
                TreeNode newGroup = new TreeNode(cmd.Key, 1, 1);

                foreach (var subcmd in cmd)
                {
                    TreeNode subNode = new TreeNode(subcmd.ShortName);
                    subNode.ToolTipText = subcmd.ShortName;

                    if (!subcmd.Command.CustomRendering)
                    {
                        subNode.ForeColor = System.Drawing.Color.Red;
                    }
                    newGroup.Nodes.Add(subNode);
                }
                treeCommands.Add(newGroup);
            }
        }
        private static void CommandsSortBySubGroupAndGroup(List<TreeNode> treeCommands, List<taskt.UI.CustomControls.AutomationCommand> commands)
        {
            var groupedCommands = commands.GroupBy(f => new { f.DisplayGroup, f.DisplaySubGroup })
                                    .OrderBy(f => f.Key.DisplayGroup);
            string prevPrimayGroup = "----";
            TreeNode pGroup = null;
            foreach (var primaryGroup in groupedCommands)
            {
                if (primaryGroup.Key.DisplayGroup != prevPrimayGroup)
                {
                    if (prevPrimayGroup != "----")
                    {
                        treeCommands.Add(pGroup);
                    }
                    pGroup = new TreeNode(primaryGroup.Key.DisplayGroup, 1, 1);
                    prevPrimayGroup = primaryGroup.Key.DisplayGroup;
                }

                string prevSubGroup = "----";
                TreeNode sGroup = null;
                foreach (var cmd in primaryGroup)
                {
                    if (cmd.DisplaySubGroup != prevSubGroup)
                    {
                        if (prevSubGroup != "----")
                        {
                            if (prevSubGroup != "")
                            {
                                pGroup.Nodes.Add(sGroup);
                            }
                        }
                        prevSubGroup = cmd.DisplaySubGroup;

                        if (cmd.DisplaySubGroup != "")
                        {
                            sGroup = new TreeNode(cmd.DisplaySubGroup, 1, 1);
                        }
                    }

                    TreeNode subNode = new TreeNode(cmd.ShortName);
                    subNode.ToolTipText = cmd.ShortName;
                    if (!cmd.Command.CustomRendering)
                    {
                        subNode.ForeColor = System.Drawing.Color.Red;
                    }
                    if (cmd.DisplaySubGroup == "")
                    {
                        pGroup.Nodes.Add(subNode);
                    }
                    else
                    {
                        sGroup.Nodes.Add(subNode);
                    }
                }
                if (prevSubGroup != "")
                {
                    pGroup.Nodes.Add(sGroup);
                }
            }
            treeCommands.Add(pGroup);
        }

        public static ImageList CreateCommandImageList()
        {
            ImageList commandImages = new ImageList();
            commandImages.ImageSize = new System.Drawing.Size(16, 16);
            commandImages.Images.Add(Properties.Resources.file_icon);
            commandImages.Images.Add(Properties.Resources.command_group);

            return commandImages;
        }

        public static void ShowAllCommands(TreeView tv, TreeNode[] commandTree)
        {
            tv.BeginUpdate();
            tv.SuspendLayout();

            tv.Nodes.Clear();

            tv.Nodes.AddRange((TreeNode[])commandTree.Clone());
            tv.Sort();

            tv.ResumeLayout();
            tv.EndUpdate();
        }

    }
}

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
        public static TreeNode[] CreateAllCommandsArray(ClientSettings settings)
        {
            List<taskt.UI.CustomControls.AutomationCommand> automationCommands = UI.CustomControls.CommandControls.GenerateCommandsandControls();

            List<TreeNode> commandsTreeList = new List<TreeNode>();

            if (settings.GroupingBySubgroup)
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

        public static void ShowCommandsTree(TreeView tvCommands, TreeNode[] commandTree, bool expandAllNodes = false)
        {
            tvCommands.BeginUpdate();
            tvCommands.SuspendLayout();

            tvCommands.Nodes.Clear();

            //tvCommands.Nodes.AddRange((TreeNode[])commandTree.Clone());
            foreach(TreeNode node in commandTree)
            {
                tvCommands.Nodes.Add((TreeNode)node.Clone());
            }
            tvCommands.Sort();

            tvCommands.ResumeLayout();
            tvCommands.EndUpdate();

            tvCommands.BeginUpdate();
            if (expandAllNodes)
            {
                tvCommands.ExpandAll();
            }
            tvCommands.EndUpdate();
        }

        public static TreeNode[] FilterCommands(string keyword, TreeNode[] allCommands, ClientSettings settings)
        {
            List<TreeNode> matchedCommands = new List<TreeNode>();

            foreach (TreeNode parentGroup in allCommands)
            {
                TreeNode pGroup = new TreeNode(parentGroup.Text, 1, 1);

                bool parentMatched = false;

                // check match parent group
                if (settings.SearchTargetGroupName)
                {
                    if (parentGroup.Text.ToLower().Contains(keyword))
                    {
                        // greedly
                        if (settings.SearchGreedlyGroupName)
                        {
                            foreach (TreeNode item in parentGroup.Nodes)
                            {
                                //pGroup.Nodes.Add(item.Text);
                                if (item.Nodes.Count == 0)
                                {
                                    pGroup.Nodes.Add(item.Text);
                                }
                                else
                                {
                                    TreeNode sGroup = new TreeNode(item.Text, 1, 1);
                                    foreach (TreeNode i in item.Nodes)
                                    {
                                        sGroup.Nodes.Add(i.Text);
                                    }
                                    pGroup.Nodes.Add(sGroup);
                                }
                            }
                        }
                        else
                        {
                            parentMatched = true;
                        }
                    }
                }

                // not greedly
                if (pGroup.Nodes.Count == 0)
                {
                    foreach (TreeNode item in parentGroup.Nodes)
                    {
                        if (item.Nodes.Count > 0)
                        {
                            TreeNode sGroup = new TreeNode(item.Text, 1, 1);

                            bool subMatched = false;

                            if (settings.SearchTargetSubGroupName)
                            {
                                if (item.Text.ToLower().Contains(keyword))
                                {
                                    if (settings.SearchGreedlySubGroupName)
                                    {
                                        foreach (TreeNode itm in item.Nodes)
                                        {
                                            sGroup.Nodes.Add(itm.Text);
                                        }
                                    }
                                    else
                                    {
                                        subMatched = true;
                                    }
                                }
                            }

                            if (sGroup.Nodes.Count == 0)
                            {
                                foreach (TreeNode itm in item.Nodes)
                                {
                                    if (itm.Text.ToLower().Contains(keyword))
                                    {
                                        sGroup.Nodes.Add(itm.Text);
                                    }
                                }
                            }

                            if ((sGroup.Nodes.Count > 0) || subMatched)
                            {
                                pGroup.Nodes.Add(sGroup);
                            }
                        }
                        else
                        {
                            if (item.Text.ToLower().Contains(keyword))
                            {
                                pGroup.Nodes.Add(item.Text);
                            }
                        }
                    }
                }

                if ((pGroup.Nodes.Count > 0) || parentMatched)
                {
                    matchedCommands.Add(pGroup);
                }
            }

            if (matchedCommands.Count == 0)
            {
                matchedCommands.Add(new TreeNode("nothing :-("));
            }

            return (TreeNode[])matchedCommands.ToArray();
        }

        public static string GetSelectedFullCommandName(TreeView tvCommands)
        {
            if (tvCommands.SelectedNode == null)
            {
                return "";
            }

            switch (tvCommands.SelectedNode.Level)
            {
                case 0:
                    return "";

                case 1:
                    if (tvCommands.SelectedNode.Nodes.Count > 0)
                    {
                        return "";
                    }
                    else if (tvCommands.SelectedNode.ImageIndex == 1)
                    {
                        return "";
                    }
                    else
                    {
                        return tvCommands.SelectedNode.Parent.Text + " - " + tvCommands.SelectedNode.Text;
                    }

                case 2:
                    return tvCommands.SelectedNode.Parent.Parent.Text + " - " + tvCommands.SelectedNode.Text;

                default:
                    return "";
            }
        }

        public static void FocusCommand(string group, string command, TreeView tvCommands)
        {
            TreeNode parentNode = null;
            foreach (TreeNode node in tvCommands.Nodes)
            {
                if (node.Text == group)
                {
                    parentNode = node;
                    break;
                }
            }
            if (parentNode != null)
            {
                parentNode.Expand();
                foreach (TreeNode node in parentNode.Nodes)
                {
                    if (node.Nodes.Count > 0)
                    {
                        foreach (TreeNode no in node.Nodes)
                        {
                            if (no.Text == command)
                            {
                                node.Expand();
                                tvCommands.SelectedNode = no;
                                tvCommands.Focus();
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (node.Text == command)
                        {
                            tvCommands.SelectedNode = node;
                            tvCommands.Focus();
                            break;
                        }
                    }
                }
            }
        }

    }
}

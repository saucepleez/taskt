using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmEngineContextViewer : ThemedForm
    {
        public frmEngineContextViewer(string context, int closeAfter)
        {
            InitializeComponent();
            LoadEngineContext(context);

            if (closeAfter > 0)
            {
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = (closeAfter * 1000);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void FrmEngineContextViewer_Load(object sender, EventArgs e)
        {

        }

        private void LoadEngineContext(string context)
        {

            var root = Newtonsoft.Json.JsonConvert.DeserializeObject<JToken>(context);
            DisplayTreeView(root, "Engine Context");

        }
        private void DisplayTreeView(JToken root, string rootName)
        {
            tvContext.BeginUpdate();
            try
            {
                tvContext.Nodes.Clear();
                var tNode = tvContext.Nodes[tvContext.Nodes.Add(new TreeNode(rootName))];
                tNode.Tag = root;

                AddNode(root, tNode);
            }
            finally
            {
                tvContext.EndUpdate();
            }
        }
        private void AddNode(JToken token, TreeNode inTreeNode)
        {
            if (token == null)
                return;
            if (token is JValue)
            {
                var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToString()))];
                childNode.Tag = token;
            }
            else if (token is JObject)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
                    childNode.Tag = property;
                    AddNode(property.Value, childNode);
                }
            }
            else if (token is JArray)
            {
                var array = (JArray)token;
                for (int i = 0; i < array.Count; i++)
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToString()))];
                    childNode.Tag = array[i];
                    AddNode(array[i], childNode);
                }
            }
            else
            {
                Console.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
            }
        }

        private void UiBtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}

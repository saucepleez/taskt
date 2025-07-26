using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows.Forms;
using taskt.Core.IO;

namespace taskt.UI.Forms.ScriptBuilderModern
{
    public partial class frmScriptBuilderModern : Form
    {
        public frmScriptBuilderModern()
        {
            InitializeComponent();
        }

        private async void frmScriptBuilderModern_Load(object sender, EventArgs e)
        {
            var udfPath = Path.Combine(Folders.GetSettingsFolderPath(), "webview2");
            var webView2Environment = await CoreWebView2Environment.CreateAsync(userDataFolder: udfPath);
            await webView.EnsureCoreWebView2Async(webView2Environment);

            var htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ModernUI", "index.html");
            if (File.Exists(htmlPath))
            {
                webView.CoreWebView2.SetVirtualHostNameToFolderMapping("app", Path.GetDirectoryName(htmlPath), CoreWebView2HostResourceAccessKind.Allow);
                webView.Source = new Uri($"https://app/{Path.GetFileName(htmlPath)}");
            }
        }

        private async void frmScriptBuilderModern_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (webView.CoreWebView2 != null)
            {
                await webView.CoreWebView2.Profile.ClearBrowsingDataAsync();
            }
        }
    }
}

using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace taskt.Core.Update
{
    public static class ApplicationUpdate
    {
        private static bool SkipBeta = false;

        /// <summary>
        /// convert json text to UpdateManifest
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static UpdateManifest ConvertToUpdateManifest(string json)
        {
            //initialize config
            try
            {
                return JsonConvert.DeserializeObject<UpdateManifest>(json);
            }
            catch (Exception)
            {
                //bad json received
                throw;
            }
        }

        /// <summary>
        /// check Update available
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        private static UpdateManifest CheckUpdateAvailable(UpdateManifest manifest)
        {
            manifest.LocalVersionProper = new Version(Application.ProductVersion);

            var remote = new Version(manifest.RemoteVersion);
            var beta = new Version(manifest.RemoteBetaVersion);
            if (SkipBeta)
            {
                manifest.RemoteVersionProper = remote;
            }
            else
            {
                manifest.RemoteVersionProper = remote.CompareTo(beta) > 0 ? remote : beta;
            }

            manifest.IsRemoteVersionNewer = (manifest.RemoteVersionProper.CompareTo(manifest.LocalVersionProper) > 0);

            return manifest;
        }

        /// <summary>
        /// update process Sync
        /// </summary>
        /// <param name="skipBeta"></param>
        /// <param name="silent"></param>
        public static void ShowUpdateResultSync(bool skipBeta, bool silent = true)
        {
            ApplicationUpdate.SkipBeta = skipBeta;

            UpdateManifest manifest;
            try
            {
                WebClient webClient = new WebClient();
                var manifestString = webClient.DownloadString(MyURLs.LatestJSONURL);
                manifest = ConvertToUpdateManifest(manifestString);
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    using (var fm = new taskt.UI.Forms.Supplemental.frmDialog("Error getting manifest: " + ex.ToString(), "Error", taskt.UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }
                return;
            }

            CheckUpdateAvailable(manifest);

            if (manifest.IsRemoteVersionNewer)
            {
                ShowUpdateForm(manifest);
            }
            else
            {
                if (!silent)
                {
                    using (var fm = new taskt.UI.Forms.Supplemental.frmDialog("taskt is currently up-to-date!", "No Updates Available", taskt.UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }
            }
        }

        /// <summary>
        /// update process Async
        /// </summary>
        public static void ShowUpdateResultAsync(bool skipBeta)
        {
            ApplicationUpdate.SkipBeta = skipBeta;

            //get manifest
            try
            {
                WebClient wc = new WebClient
                {
                    Encoding = Encoding.UTF8,
                };
                wc.DownloadStringCompleted += CompleteDownloadUpdateManifest;
                wc.DownloadStringAsync(new Uri(MyURLs.LatestJSONURL));
            }
            catch (Exception)
            {
                // silent! ;-)
                //throw new Exception("Fail check update : " + ex.Message);
            }
        }

        /// <summary>
        /// event when updateManifest download finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CompleteDownloadUpdateManifest(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string manifestString = e.Result;

                var manifestConfig = ConvertToUpdateManifest(manifestString);

                CheckUpdateAvailable(manifestConfig);

                if (manifestConfig.IsRemoteVersionNewer)
                {
                    ShowUpdateForm(manifestConfig);
                }
            }
            catch (Exception)
            {
                // silent! ;-)
                //bad json received
                //throw new Exception("Fail parse update info : " + ex.Message);
            }
        }

        /// <summary>
        /// show update form
        /// </summary>
        /// <param name="manifestConfig"></param>
        private static void ShowUpdateForm(UpdateManifest manifestConfig)
        {
            using (var frmUpdate = new taskt.UI.Forms.Supplement_Forms.frmUpdate(manifestConfig))
            {
                if (frmUpdate.ShowDialog() == DialogResult.OK)
                {

                    //move update exe to root folder for execution
                    var updaterExecutionResources = System.IO.Path.Combine(Application.StartupPath, "Resources", "taskt-updater.exe");

                    if (!System.IO.File.Exists(updaterExecutionResources))
                    {
                        using (var fm = new taskt.UI.Forms.Supplemental.frmDialog("taskt-updater.exe not found in Resources folder!", "Error", taskt.UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                        {
                            fm.ShowDialog();
                        }
                        return;
                    }

                    var updateProcess = new System.Diagnostics.Process();
                    updateProcess.StartInfo.FileName = updaterExecutionResources;

                    var zipURL = manifestConfig.PackageURL2.Replace("%release_url%", MyURLs.GitReleaseURL).Replace("%version%", manifestConfig.RemoteVersionProper.ToString());
                    updateProcess.StartInfo.Arguments = "/d " + zipURL;

                    updateProcess.Start();
                    Application.Exit();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace taskt.Core
{
    public class ApplicationUpdate
    {
        public UpdateManifest GetManifest()
        {
            //create web client
            WebClient webClient = new WebClient();
            string manifestData = "";

            //get manifest
            try
            {
                manifestData = webClient.DownloadString(MyURLs.LatestJSONURL);           
            }
            catch (Exception)
            {
                //unable to get the manifest
                throw;
            }

            //initialize config
            UpdateManifest manifestConfig = new UpdateManifest();

            try
            {
                manifestConfig = JsonConvert.DeserializeObject<UpdateManifest>(manifestData);
            }
            catch (Exception)
            {
                //bad json received
                throw;
            }

            //create versions
            manifestConfig.RemoteVersionProper = new Version(manifestConfig.RemoteVersion);
            manifestConfig.LocalVersionProper = new Version(System.Windows.Forms.Application.ProductVersion);

            //determine comparison
            int versionCompare = manifestConfig.LocalVersionProper.CompareTo(manifestConfig.RemoteVersionProper);

            if (versionCompare < 0)
            {
                manifestConfig.RemoteVersionNewer = true;
            }
            else
            {
                manifestConfig.RemoteVersionNewer = false;
            }

            return manifestConfig;
        }

        public static void ShowUpdateResult(bool silent = true)
        {
            taskt.Core.ApplicationUpdate updater = new Core.ApplicationUpdate();
            Core.UpdateManifest manifest = new Core.UpdateManifest();
            try
            {
                manifest = updater.GetManifest();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error getting manifest: " + ex.ToString());
                if (!silent)
                {
                    using (var fm = new taskt.UI.Forms.Supplemental.frmDialog("Error getting manifest: " + ex.ToString(), "Error", taskt.UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }
                return;
            }

            if (manifest.RemoteVersionNewer)
            {
                //Supplement_Forms.frmUpdate frmUpdate = new Supplement_Forms.frmUpdate(manifest);
                //if (frmUpdate.ShowDialog() == DialogResult.OK)
                //{

                //    //move update exe to root folder for execution
                //    var updaterExecutionResources = Application.StartupPath + "\\resources\\taskt-updater.exe";
                //    var updaterExecutableDestination = Application.StartupPath + "\\taskt-updater.exe";

                //    if (!System.IO.File.Exists(updaterExecutionResources))
                //    {
                //        MessageBox.Show("taskt-updater.exe not found in resources directory!");
                //        return;
                //    }
                //    else
                //    {
                //        System.IO.File.Copy(updaterExecutionResources, updaterExecutableDestination);
                //    }

                //    var updateProcess = new System.Diagnostics.Process();
                //    updateProcess.StartInfo.FileName = updaterExecutableDestination;
                //    updateProcess.StartInfo.Arguments = manifest.PackageURL;

                //    updateProcess.Start();
                //    Application.Exit();
                //}
                using (var fm = new taskt.UI.Forms.Supplement_Forms.frmUpdate(manifest))
                {
                    fm.ShowDialog();
                }
            }
            else
            {
                //MessageBox.Show("The application is currently up-to-date!", "No Updates Available", MessageBoxButtons.OK);
                if (!silent)
                {
                    using (var fm = new taskt.UI.Forms.Supplemental.frmDialog("taskt is currently up-to-date!", "No Updates Available", taskt.UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }
            }
        }

        public static void ShowUpdateResultAsync()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            //get manifest
            try
            {
                wc.DownloadStringCompleted += CompleteDownloadUpdateInformation;
                wc.DownloadDataAsync(new Uri(MyURLs.LatestJSONURL));
            }
            catch (Exception ex)
            {
                // silent! ;-)
                //throw new Exception("Fail check update : " + ex.Message);
            }
        }

        private static void CompleteDownloadUpdateInformation(object sender, DownloadStringCompletedEventArgs e)
        {
            string manifestString = e.Result;
            try
            {
                //initialize config
                UpdateManifest manifestConfig = new UpdateManifest();

                manifestConfig = JsonConvert.DeserializeObject<UpdateManifest>(manifestString);
                //create versions
                manifestConfig.RemoteVersionProper = new Version(manifestConfig.RemoteVersion);
                manifestConfig.LocalVersionProper = new Version(System.Windows.Forms.Application.ProductVersion);

                //determine comparison
                int versionCompare = manifestConfig.LocalVersionProper.CompareTo(manifestConfig.RemoteVersionProper);

                if (versionCompare < 0)
                {
                    manifestConfig.RemoteVersionNewer = true;
                }
                else
                {
                    manifestConfig.RemoteVersionNewer = false;
                }

                if (manifestConfig.RemoteVersionNewer)
                {
                    // show new version message
                    using (var fm = new taskt.UI.Forms.Supplement_Forms.frmUpdate(manifestConfig))
                    {
                        fm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                // silent! ;-)
                //bad json received
                //throw new Exception("Fail parse update info : " + ex.Message);
            }
        }
    }

    public class UpdateManifest
    {
        //from manifest
        public string RemoteVersion { get; set; }
        public string PackageURL { get; set; }

        //helpers
        public bool RemoteVersionNewer { get; set; }
        public Version RemoteVersionProper { get; set; }
        public Version LocalVersionProper { get; set; }
    }
}

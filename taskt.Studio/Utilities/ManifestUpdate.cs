using System;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Forms;
namespace taskt.Utilities
{
    public class ManifestUpdate
    {
        //from manifest
        private string _remoteVersion { get; set; }
        public string PackageURL { get; private set; }

        //helpers
        public bool RemoteVersionNewer { get; private set; }
        public Version RemoteVersionProper { get; private set; }
        public Version LocalVersionProper { get; private set; }

        public ManifestUpdate()
        {

        }

        public static ManifestUpdate GetManifest()
        {
            //create web client
            WebClient webClient = new WebClient();
            string manifestData = "";

            //get manifest
            try
            {
             manifestData = webClient.DownloadString("http://www.taskt.net/updates/latest.json");           
            }
            catch (Exception)
            {
                //unable to get the manifest
                throw;
            }

            //initialize config
            ManifestUpdate manifestConfig = new ManifestUpdate();

            try
            {
                 manifestConfig = JsonConvert.DeserializeObject<ManifestUpdate>(manifestData);
            }
            catch (Exception)
            {
                //bad json received
                throw;
            }

            //create versions
            manifestConfig.RemoteVersionProper = new Version(manifestConfig._remoteVersion);
            manifestConfig.LocalVersionProper = new Version(Application.ProductVersion);

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
    }
}

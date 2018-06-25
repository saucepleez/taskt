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
             manifestData = webClient.DownloadString("http://www.taskt.net/updates/latest.json");           
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

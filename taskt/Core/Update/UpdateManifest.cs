using System;

namespace taskt.Core.Update
{
    public class UpdateManifest
    {
        //from manifest
        public string RemoteVersion { get; set; }
        public string RemoteBetaVersion { get; set; }
        public bool Beta { get; set; }
        public string PackageURL { get; set; }
        public string PackageURL2 { get; set; }

        //helpers
        public bool IsRemoteVersionNewer { get; set; }
        public Version RemoteVersionProper { get; set; }
        public Version LocalVersionProper { get; set; }
    }
}

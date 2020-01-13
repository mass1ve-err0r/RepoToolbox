using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoToolbox.Utilities;

namespace RepoToolbox.Structs
{
    [Serializable]
    public class Package
    {
        // internals
        public string DepictionMDRAW;
        public string ChangelogMDRAW;
        public string FilePathLOCAL;
        public string[] Screenshots;
        public string Icon;
        public string Banner;
        // Depictions
        public string DepictionHTML;
        public string DepictionMD;
        public string ChangelogHTML;
        public string ChangelogMD;
        // Package Stuff
        public string PackageID;
        public string Name;
        public string Maintainer;
        public string Author;
        public string Depends;
        public string Conflicts;
        public string Version;
        public string Section;
        public string DescriptionShort;
        public string SileoDepiction;
        public string Depiction;
        public string MinIOS;
        public string MaxIOS;
        // images
        public string IconURL;
        public string BannerURL;
        // Packages -specific
        public string FileSize;
        public string RepoFilePath;
        public string MD5;
        public string SHA1;
        public string SHA256;

        public Package() {  }
        // <-- FIN -->
    }
}

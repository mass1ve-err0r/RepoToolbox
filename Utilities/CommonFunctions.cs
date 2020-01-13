using RepoToolbox.Structs;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;

namespace RepoToolbox.Utilities
{

    class CommonFunctions
    {

        private static string REPO_FOLDER = Environment.CurrentDirectory + "\\RepoFolder";
        private static string ASSETS_FOLDER = REPO_FOLDER + "\\assets";
        private static string DEB_FOLDER = REPO_FOLDER + "\\debs";

        public static String calculateHash(string HashType, string PathToFile) {
            var HashAlgo = HashAlgorithm.Create(HashType);

            FileStream fs = File.OpenRead(PathToFile);
            byte[] FileHash = HashAlgo.ComputeHash(fs);
            fs.Close();

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < FileHash.Length; i++) {
                builder.Append(FileHash[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public static Int64 calculateFilesize(string PathToFile) {
            return new FileInfo(PathToFile).Length;
        }

        public static void ReleaseGenerator(string RepoName) {
            string ReleaseF = "" +
                "Origin: " + RepoName + "\n" +
                "Label: " + RepoName + "\n" +
                "Suite: stable\n" +
                "Version: 1.0\n" +
                "Codename: iOSRepository\n" +
                "Architectures: iphoneos-arm\n" +
                "Components: main\n" +
                "Description: " + RepoName;
            File.WriteAllText(REPO_FOLDER + "\\Release", ReleaseF);
        }

        public static void MoveRepoFiles(Package pkg) {
            // copy deb
            File.Copy(pkg.FilePathLOCAL, DEB_FOLDER + "\\" + pkg.PackageID + "_" + pkg.Version + ".deb", true);
            // copy Icon
            File.Copy(pkg.Icon, ASSETS_FOLDER + "\\" + pkg.PackageID + "_icon.png", true);
            // copy Banner
            File.Copy(pkg.Banner, ASSETS_FOLDER + "\\" + pkg.PackageID + "_banner.png", true);
            // copy over screenshots
            int i = 0;
            if (!GeneralExtensions.IsNullOrEmpty(pkg.Screenshots)) {
                foreach (var screenshot in pkg.Screenshots) {
                    File.Copy(screenshot, ASSETS_FOLDER + "\\" + pkg.PackageID + "_" + i + ".png", true);
                    i++;
                }
            }

        }

        public static void UpdateDebfile(Package pkg) {
            File.Copy(pkg.FilePathLOCAL, DEB_FOLDER + "\\" + pkg.PackageID + "_" + pkg.Version + ".deb", true);
        }

        public static void GeneratePackageEntry(Package pkg) {
            // construct entry
            string PackageEntry = "" +
                "Package: " + pkg.PackageID + "\n" +
                "Name: " + pkg.Name + "\n" +
                "Version: " + pkg.Version + "\n" +
                "Architecture: iphoneos-arm\n" +
                "Maintainer: " + pkg.Maintainer + "\n" +
                "Description: " + pkg.DescriptionShort + "\n" +
                "Author: " + pkg.Author + "\n" +
                "Section: " + pkg.Section + "\n" +
                "Depends: " + pkg.Depends + "\n" +
                "Conflicts: " + pkg.Conflicts + "\n" +
                "Homepage: https://" + Homescreen.prefs.Homepage + "\n" +
                "Depiction: https://" + Homescreen.prefs.Homepage + "/cydiad/" + pkg.PackageID + ".html\n" +
                "Icon: https://" + Homescreen.prefs.Homepage + "/assets/" + pkg.PackageID + "_icon.png\n" +
                "SileoDepiction: https://" + Homescreen.prefs.Homepage + "/sileod/" + pkg.PackageID + ".json\n" +
                "Filename: ./debs/" + pkg.PackageID + "_" + pkg.Version + ".deb\n" +
                "Size: " + pkg.FileSize + "\n" +
                "MD5sum: " + pkg.MD5 + "\n" +
                "SHA1: " + pkg.SHA1 + "\n" +
                "SHA256: " + pkg.SHA256 + "\n\n";
            // write entry + newline (Unix-LF)
            File.AppendAllText(REPO_FOLDER + "\\Packages", PackageEntry);
        }

        public static void GenerateBZ2() {
            string packagesBZ2 = "\\Packages.bz2";
            BZip2.Compress(File.OpenRead(REPO_FOLDER + "\\Packages"), File.Create(REPO_FOLDER + packagesBZ2), true, 9);
        }

        // <-- FIN -->
    }
}

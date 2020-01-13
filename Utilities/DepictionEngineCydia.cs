using System;
using System.IO;
using System.Text;
using RepoToolbox.Structs;

namespace RepoToolbox.Utilities
{
    public class DepictionEngineCydia
    {
        private static string HOME;
        private static string CYDIA_PATH = Environment.CurrentDirectory + "\\RepoFolder\\cydiad\\";

        public static void Create(Package pkg) {
            StringBuilder temp = new StringBuilder("https://");
            temp.Append(Homescreen.prefs.Homepage);
            HOME = temp.ToString();
            string CydiaDepiction = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
     <meta name=""viewport"" content=""width=device-width, initial-scale=1, minimum-scale=1.0, user-scalable=0"">    
        <title>$_PACKAGENAME</title>
        <link rel=""stylesheet"" type=""text/css"" href=""cyd.css"">
        <style>a, .tint, .table - btn:after { color: $_REPOCOLOR} .active { color: $_REPOCOLOR; border - bottom: 2px solid $_REPOCOLOR; }</style>
        <script src=""cyd.js""></script>
        <meta name=""description"" content=""A package by $_AUTHOR for iOS $_MINIOS to $_MAXIOS"">
        <meta property = ""og:title"" content=""$_PACKAGENAME"" >
        <meta property = ""og:description"" content=""A package by $_AUTHOR for iOS $_MINIOS to $_MAXIOS"" >
        <meta property = ""og:image"" content=""../assets/$_PACKAGEID_banner.png"" >
        <meta name=""twitter:card"" content=""summary_large_image"" >
        <meta name=""twitter:title"" content=""$_PACKAGENAME"" >
        <meta name=""twitter:description"" content=""A package by $_AUTHOR for iOS $_MINIOS to $_MAXIOS"" >
        <meta name=""twitter:image"" content=""$_REPOURL/assets/$_PACKAGEID_banner.png"" >
        <meta name=""theme-color"" content=""$_REPOCOLOR"" >
</head>
<body>
        <div style = ""background-image: url(../assets/$_PACKAGEID_banner.png)"" class=""banner_underlay""></div>
        <div class=""package package_head"">
            <img src = ""../assets/$_PACKAGEID_icon.png"" >
            <div class=""package_info"">
                <h1 class=""package_name"">$_PACKAGENAME</h1>
                <p class=""package_caption"">$_AUTHOR</p>
            </div>
        </div>
        <div class=""nav"">
            <div class=""nav_btn active tweak_info_btn"" onclick=""swap('.changelog','.tweak_info');"">Details</div>
            <div class=""nav_btn changelog_btn"" onclick=""swap('.tweak_info','.changelog');"">Changelog</div>
        </div>
        <div class=""tweak_info"">
            <p class=""compatibility""><b>Compatibility:</b> This package is compatible with iOS $_MINIOS to $_MAXIOS</p>
			$_SCREENSHOTDIV
            <div class=""md_view"">
			$_DEPICTIONMD
            </div>
            <h3>Information</h3>
            <div class=""table"">
                <div class=""cell"">
                    <div class=""title"">Developer</div>
                    <div class=""text"">$_AUTHOR</div>
                    <br><br>
                </div>
                <div class=""cell"">
                    <div class=""title"">Version</div>
                    <div class=""text"">$_VERSION</div>
                    <br><br>
                </div>
                <div class=""cell"">
                    <div class=""title"">Compatibility</div>
                    <div class=""text"">iOS $_MINIOS to $_MAXIOS</div>
                    <br><br>
                </div>
                <div class=""cell"">
                    <div class=""title"">Section</div>
                    <div class=""text"">$_SECTION</div>
                    <br><br>
                </div>
            </div>
        </div>
        <div class=""changelog"">
			<div class=""md_view"">
				$_CHANGELOGENTRY
            </div>
		</div>
        </div>
        <div class=""caption center footer"">© $_AUTHOR</div>
</body>
<script>compatible(""$_MINIOS"",""$_MAXIOS"",""iOS $_MINIOS to $_MAXIOS""); externalize()</script>
</html>";

            // core processing

            StringBuilder cd = new StringBuilder(CydiaDepiction);
            string AssetsRoot = HOME + "/assets/" + pkg.PackageID;

            cd.Replace("$_PACKAGENAME", pkg.Name);
            cd.Replace("$_REPOCOLOR", Homescreen.prefs.RepoColor);
            cd.Replace("$_AUTHOR", pkg.Author);
            cd.Replace("$_MINIOS", pkg.MinIOS);
            cd.Replace("$_MAXIOS", pkg.MaxIOS);
            cd.Replace("$_PACKAGEID", pkg.PackageID);
            cd.Replace("$_REPOURL", HOME);
            string ScreenshotItemsCY = (pkg.Screenshots != null) ? CydiaScreenshotFieldCreator(pkg.PackageID, pkg.Screenshots.Length) : "";
            cd.Replace("$_SCREENSHOTDIV", ScreenshotItemsCY);
            cd.Replace("$_DEPICTIONMD", pkg.DepictionHTML);
            cd.Replace("$_VERSION", pkg.Version);
            cd.Replace("$_SECTION", pkg.Section);
            cd.Replace("$_CHANGELOGENTRY", pkg.ChangelogHTML);

            File.WriteAllText(CYDIA_PATH + pkg.PackageID + ".html", cd.ToString());
        }

        private static string CydiaScreenshotFieldCreator(string PackageID, int NumScreenshots) {
            StringBuilder SSField = new StringBuilder("<div class=\"scroll_view\">");
            string entry;
            for (int i = 0; i < NumScreenshots; i++) {
                entry = "<img class=\"img_card\" src=\"../../assets/" + PackageID + "_" + i + ".png>";
                SSField.Append(entry);
            }
            return SSField.ToString();
        }

        // <-- FIN -->
    }
}

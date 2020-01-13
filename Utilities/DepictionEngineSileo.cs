using RepoToolbox.Structs;
using System;
using System.IO;
using System.Text;

namespace RepoToolbox.Utilities
{
    public class DepictionEngineSileo
    {
        private static string HOME;
        private static string SILEO_PATH = Environment.CurrentDirectory + "\\RepoFolder\\sileod\\";

        public static void Create(Package pkg) {
            StringBuilder temp = new StringBuilder("https://");
            temp.Append(Homescreen.prefs.Homepage);
            HOME = temp.ToString();
            string SileoDepiction = @"{ 
    ""minVersion"":""0.1"",
    ""headerImage"":""$_HeaderImage"",
    ""tintColor"":""$_TintColor"",
    ""tabs"":[
        { 
            ""tabname"":""Details"",
            ""views"":[
                $_ScreenshotField
                { 
                    ""markdown"":$_MarkdownText,
                    ""useSpacing"":""true"",
                    ""class"":""DepictionMarkdownView""
                },
                { 
                    ""class"":""DepictionSpacerView""
                },
                { 
                    ""class"":""DepictionHeaderView"",
                    ""title"":""Information""
                },
                { 
                    ""class"":""DepictionTableTextView"",
                    ""title"":""Developer"",
                    ""text"":""$_DeveloperName""
                },
                { 
                    ""class"":""DepictionTableTextView"",
                    ""title"":""Version"",
                    ""text"":""$_Version""
                },
                { 
                    ""class"":""DepictionTableTextView"",
                    ""title"":""Compatibility"",
                    ""text"":""iOS $_MiniOS to $_MaxiOS""
                },
                { 
                    ""class"":""DepictionTableTextView"",
                    ""title"":""Section"",
                    ""text"":""$_Section""
                },
                { 
                    ""class"":""DepictionSpacerView""
                },
                { 
                    ""class"":""DepictionLabelView"",
                    ""text"":""\u00a9 $_DeveloperName"",
                    ""textColor"":""#999999"",
                    ""fontSize"":""10.0"",
                    ""alignment"":1
                }
            ],
            ""class"":""DepictionStackView""
        },
        { 
            ""tabname"":""Changelog"",
            ""views"":[
                { 
                    ""class"":""DepictionMarkdownView"",
                    ""markdown"":$_MarkdownChangelog
                },
                { 
                    ""class"":""DepictionLabelView"",
                    ""text"":""\u00a9 $_DeveloperName"",
                    ""textColor"":""#999999"",
                    ""fontSize"":""10.0"",
                    ""alignment"":1
                }
            ],
            ""class"":""DepictionStackView""
        }
    ],
    ""class"":""DepictionTabView""
}";
            // the core processing
            StringBuilder sd = new StringBuilder(SileoDepiction);
            string AssetsRoot = HOME + "/assets/" + pkg.PackageID;

            sd.Replace("$_HeaderImage", AssetsRoot + "_banner.png");
            sd.Replace("$_TintColor", Homescreen.prefs.RepoColor);
            string ScreenshotItems = (pkg.Screenshots != null) ? SileoScreenshotFieldCreator(AssetsRoot, pkg.Screenshots.Length) : "";
            sd.Replace("$_ScreenshotField", ScreenshotItems);
            sd.Replace("$_MarkdownText", pkg.DepictionMD);
            sd.Replace("$_DeveloperName", pkg.Author);
            sd.Replace("$_Version", pkg.Version);
            sd.Replace("$_MiniOS", pkg.MinIOS);
            sd.Replace("$_MaxiOS", pkg.MaxIOS);
            sd.Replace("$_Section", pkg.Section);
            sd.Replace("$_MarkdownChangelog", pkg.ChangelogMD);

            File.WriteAllText(SILEO_PATH + pkg.PackageID + ".json", sd.ToString());
        }

        private static string SileoScreenshotFieldCreator(string HomeURL, int NumScreenshots) {
            StringBuilder SSField = new StringBuilder("{\"class\":\"DepictionScreenshotsView\",\"screenshots\":[");
            string entry;
            for (int i = 0; i < NumScreenshots; i++) {
                entry = "{\"url\":\"" + HomeURL + "_" + i + ".png" + "\",\"accessibilityText\":\"Screenshot\"},";
                SSField.Append(entry);
            }
            SSField.Append("\"itemCornerRadius\":8,\"itemSize\":\"{185, 400}\"},");
            return SSField.ToString();
        }

        // <-- FIN -->
    }
}

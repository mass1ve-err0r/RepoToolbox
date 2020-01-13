using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ExtraTools;
using RepoToolbox.Structs;
using RepoToolbox.Utilities;

namespace RepoToolbox.Windows
{
    /// <summary>
    /// Interaktionslogik für DeletePackage.xaml
    /// </summary>
    public partial class DeletePackage : Window
    {

        private static string PackagesFile = Environment.CurrentDirectory + "\\RepoFolder\\Packages";
        private static string REPO_FOLDER = Environment.CurrentDirectory + "\\RepoFolder";
        public DeletePackage() {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e) {
            Homescreen.isOpenDeletePackage = false;
            base.OnClosing(e);
        }


        private void DeleteNow(object sender, MouseButtonEventArgs e) {
            // show security warning
            DialogBox.Show("CAUTION", "THIS PROCESS IS NOT REVERSIBLE. ARE YOU REALLY SURE?", "Yes", "No");
            switch (DialogBox.Result) {
                case DialogBox.ResultEnum.LeftButtonClicked:
                    tbCard.Text = "Working...";
                    DeletePackagesEntries();
                    DeleteEntryData();
                    tbCard.Text = "DONE";
                    break;
                case DialogBox.ResultEnum.RightButtonClicked:
                    break;
            }
        }

        private void DeletePackagesEntries() {
            StringBuilder sb = new StringBuilder();
            string SearchSTR = "Package: " + tbDelPackageID.Text;

            StreamReader sr = new StreamReader(PackagesFile);
            string line;
            while((line = sr.ReadLine()) != null) {
                if (line.Equals(SearchSTR)) {
                    sr.SkipLines(18);
                } else if(line.Equals("\n")) {
                    sb.Append("\n");
                } else {
                    sb.Append(line + "\n");
                }
            }
            sr.Close();
            string pkgstr = sb.ToString();
            string outstr = Regex.Replace(pkgstr, @"\n\n\n", "\n\n");
            File.WriteAllText(REPO_FOLDER + "\\Packages", outstr);
            CommonFunctions.GenerateBZ2();
        }

        private void DeleteEntryData() {
            string pid = tbDelPackageID.Text;
            
            try {
                // deb
                String[] files = Directory.GetFiles(REPO_FOLDER + "\\debs");
                for (int i = 0; i < files.Length; i++) {
                    if (Path.GetFileName(files[i]).Split('_')[0].Equals(pid)) {
                        File.Delete(files[i]);
                    }
                }
                // Depictions
                File.Delete(REPO_FOLDER + "\\cydiad\\" + pid + ".html");
                File.Delete(REPO_FOLDER + "\\sileod\\" + pid + ".json");
                // assets
                String[] assets = Directory.GetFiles(REPO_FOLDER + "\\assets");
                for (int i = 0; i < assets.Length; i++) {
                    if (Path.GetFileName(assets[i]).Split('_')[0].Equals(pid)) {
                        File.Delete(assets[i]);
                    }
                }
                //
            } catch (IOException e) {
                DialogBox.Show("Error", "An error occured during deletion, please check the respective directories yourself", "Alright");
            }
        }
        // <-- FIN -->
    }
}

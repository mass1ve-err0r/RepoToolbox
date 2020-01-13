using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Forms;
using RepoToolbox.Structs;
using ExtraTools;
using RepoToolbox.Utilities;

namespace RepoToolbox.Windows
{
    /// <summary>
    /// Interaktionslogik für AddPackage.xaml
    /// </summary>
    public partial class AddPackage : Window
    {
        private Package pkg;
        public static bool isOpenDepictionEditor = false;
        public static bool isOpenChangelogEditor = false;
        public static bool isDebfileSelected = false;
        public static bool isIconSelected = false;
        public static bool isBannerSelected = false;

        public AddPackage() {
            InitializeComponent();
            pkg = new Package();
        }

        protected override void OnClosing(CancelEventArgs e) {
            // TODO save all package details!
            Homescreen.isOpenAddPackage = false;
            base.OnClosing(e);
        }

        private void DebfilePicker(object sender, MouseButtonEventArgs e) {
            OpenFileDialog FilePicker = new OpenFileDialog();
            FilePicker.Title = "Select Package";
            FilePicker.Filter = "Debian Archives (*.deb)|*.deb|All files (*.*)|*.*";
            if (FilePicker.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                mdc_tb_DebfileName.Foreground = Brushes.Green;
                mdc_tb_DebfileName.Text = "Selected!";
                pkg.FilePathLOCAL = FilePicker.FileName;
                isDebfileSelected = true;
            }
            //
        }

        private void ScreenshotPicker(object sender, MouseButtonEventArgs e) {
            OpenFileDialog FilePicker = new OpenFileDialog();
            FilePicker.Title = "Select Screenshots";
            FilePicker.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            FilePicker.Multiselect = true;
            if (FilePicker.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                mdc_tb_Screenshots.Foreground = Brushes.Green;
                mdc_tb_Screenshots.Text = "Selected!";
                pkg.Screenshots = FilePicker.FileNames;
            }
        }

        private void IconPicker(object sender, MouseButtonEventArgs e) {
            OpenFileDialog FilePicker = new OpenFileDialog();
            FilePicker.Title = "Select Icon";
            FilePicker.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            FilePicker.Multiselect = false;
            FilePicker.InitialDirectory = Environment.CurrentDirectory + "\\_internals\\baseIcons";
            if (FilePicker.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                mdc_tb_Icon.Foreground = Brushes.Green;
                mdc_tb_Icon.Text = "Selected!";
                pkg.Icon = FilePicker.FileName;
                isIconSelected = true;
            }
        }

        private void BannerPicker(object sender, MouseButtonEventArgs e) {
            OpenFileDialog FilePicker = new OpenFileDialog();
            FilePicker.Title = "Select Banner";
            FilePicker.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            FilePicker.Multiselect = false;
            FilePicker.InitialDirectory = Environment.CurrentDirectory + "\\_internals\\baseBanners";
            if (FilePicker.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                mdc_tb_Icon.Foreground = Brushes.Green;
                mdc_tb_Icon.Text = "Selected!";
                pkg.Banner = FilePicker.FileName;
                isBannerSelected = true;
            }
        }

        private void OpenDepictionEditor(object sender, MouseButtonEventArgs e) {
            if (!isOpenDepictionEditor) {
                MarkdownDepictionEditor mdeditor = new MarkdownDepictionEditor(ref pkg);
                mdeditor.Show();
                isOpenDepictionEditor = true;
            } else {
                DialogBox.Show("Error", "The Editor is already open!", "Alright");
            }
            // once done, we got depictions
        }

        private void OpenChangelogEditor(object sender, MouseButtonEventArgs e) {
            MarkdownChangelogEditor mdceditor = new MarkdownChangelogEditor(ref pkg);
            mdceditor.Show();
            // once done, we will have our changelog in both forms
        }

        private bool areFieldsSet() {
            if (String.IsNullOrWhiteSpace(tbAuthor.Text) ||
                String.IsNullOrWhiteSpace(tbDesc.Text) ||
                String.IsNullOrWhiteSpace(tbMaintainer.Text) ||
                String.IsNullOrWhiteSpace(tbMaxOS.Text) ||
                String.IsNullOrWhiteSpace(tbMinOS.Text) ||
                String.IsNullOrWhiteSpace(tbName.Text) ||
                String.IsNullOrWhiteSpace(tbPackageID.Text) ||
                String.IsNullOrWhiteSpace(tbVersion.Text)) {
                return false;
            } else {
                return true;
            }
        }

        private async void UpdatePackages(object sender, MouseButtonEventArgs e) {
            if (isDebfileSelected != true && areFieldsSet() == false && isIconSelected && isBannerSelected) {
                DialogBox.Show("Error", "Some fields are missing, please check again!", "Alright");
            } else {
                // set package props
                tbAPProgress.Foreground = Brushes.Black;
                tbAPProgress.Text = "Working...";
                await Task.Delay(2000);
                pkg.PackageID = tbPackageID.Text;
                pkg.Name = tbName.Text;
                pkg.Depends = tbDepends.Text;
                pkg.Conflicts = tbConflicts.Text;
                pkg.Maintainer = tbMaintainer.Text;
                pkg.Author = tbAuthor.Text;
                pkg.Version = tbVersion.Text;
                pkg.Section = cbSection.Text;
                pkg.MinIOS = tbMinOS.Text;
                pkg.MaxIOS = tbMaxOS.Text;
                pkg.DescriptionShort = tbDesc.Text;
                pkg.MD5 = CommonFunctions.calculateHash("MD5", pkg.FilePathLOCAL);
                pkg.SHA1 = CommonFunctions.calculateHash("SHA1", pkg.FilePathLOCAL);
                pkg.SHA256 = CommonFunctions.calculateHash("SHA256", pkg.FilePathLOCAL);
                pkg.FileSize = CommonFunctions.calculateFilesize(pkg.FilePathLOCAL).ToString();
                // be sure at this point everything is correctly named & moved into their directories
                CommonFunctions.MoveRepoFiles(pkg);
                pkg.RepoFilePath = Homescreen.prefs.Homepage + "/debs/" + pkg.PackageID + "_" + pkg.Version + ".deb";
                pkg.IconURL = Homescreen.prefs.Homepage + "/assets/" + pkg.PackageID + "_icon.png";
                pkg.BannerURL = Homescreen.prefs.Homepage + "/assets/" + pkg.PackageID + "_banner.png";
                // generate depictions
                DepictionEngineSileo.Create(pkg);
                DepictionEngineCydia.Create(pkg);
                // generate package entry
                CommonFunctions.GeneratePackageEntry(pkg);
                // create the corresponding bz2
                CommonFunctions.GenerateBZ2();
                // serialize and save object entry
                InternalPackageManager.SerializeEntry(pkg);
                // At this point we are done
                tbAPProgress.Text = "Done!";
                tbAPProgress.Foreground = Brushes.Green;
            }
        }

        // <-- FIN -->
    }
}

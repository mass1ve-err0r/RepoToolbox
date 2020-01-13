using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using RepoToolbox.Structs;
using RepoToolbox.Utilities;
using System.IO;
using System.ComponentModel;
using ExtraTools;
using System.Windows.Forms;

namespace RepoToolbox.Windows
{
    /// <summary>
    /// Interaktionslogik für UpdatePackage.xaml
    /// </summary>
    public partial class UpdatePackage : Window
    {
        
        private ObservableCollection<String> Entries = new ObservableCollection<String>();
        private static string ENTRY_DIR = Environment.CurrentDirectory + "\\_internals\\packages_record";
        public static bool isSelectedCBITEM = false;
        private Package Lpkg;

        public UpdatePackage() {
            InitializeComponent();
            FillComboBox();
        }

        protected override void OnClosing(CancelEventArgs e) {
            Homescreen.isOpenUpdatePackage = false;
            base.OnClosing(e);
        }

        private void FillComboBox() {
            var x1 = Directory.GetFiles(ENTRY_DIR);
            foreach (var item in x1) {
                Entries.Add(Path.GetFileNameWithoutExtension(item));
                cbPackages.Items.Add(Path.GetFileNameWithoutExtension(item));
            }
        }

        private void NewDebfilePicker(object sender, MouseButtonEventArgs e) {
            if (cbPackages.SelectedIndex == -1) {
                DialogBox.Show("Error", "No Package selected !", "Alright");
            } else {
                Lpkg = InternalPackageManager.DeserializeEntry(cbPackages.SelectedItem.ToString());
                OpenFileDialog FilePicker = new OpenFileDialog();
                FilePicker.Title = "Select Package";
                FilePicker.Filter = "Debian Archives (*.deb)|*.deb|All files (*.*)|*.*";
                if (FilePicker.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    mdc_tb_NewDebfileName.Foreground = Brushes.Green;
                    mdc_tb_NewDebfileName.Text = "Selected!";
                    Lpkg.FilePathLOCAL = FilePicker.FileName;
                }
                //
            }
        }

        private void NewChangelogEditor(object sender, MouseButtonEventArgs e) {
            if (cbPackages.SelectedIndex == -1) {
                DialogBox.Show("Error", "No Package selected to edit!", "Alright");
            } else {
                MarkdownChangelogEditor mdce = new MarkdownChangelogEditor(ref Lpkg);
                mdce.Show();
            }
        }

        private void UpdateSelectedPackage(object sender, MouseButtonEventArgs e) {
            if (String.IsNullOrWhiteSpace(tbNewVersion.Text)) {
                DialogBox.Show("Error", "New Version has NOT been set!", "Alright");
            } else if (Lpkg.Version.Equals(tbNewVersion.Text)) {
                DialogBox.Show("Error", "Entered Version is the same as previous!", "Alright");
            } else if (cbPackages.SelectedIndex == -1) {
                DialogBox.Show("Error", "No Package selected !", "Alright");
            } else {
                Lpkg.Version = tbNewVersion.Text;
                // move new debfile
                CommonFunctions.UpdateDebfile(Lpkg);
                // generate depictions
                DepictionEngineSileo.Create(Lpkg);
                DepictionEngineCydia.Create(Lpkg);
                // generate package entry
                CommonFunctions.GeneratePackageEntry(Lpkg);
                // serialize and save object entry
                InternalPackageManager.SerializeEntry(Lpkg);
            }
        }

        // <-- FIN -->
    }
}

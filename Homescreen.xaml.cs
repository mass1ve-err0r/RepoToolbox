using System;
using System.Windows;
using System.Windows.Input;
using RepoToolbox.Utilities;
using RepoToolbox.Windows;
using ExtraTools;
using SevenZip;
using RepoToolbox.Structs;

namespace RepoToolbox
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class Homescreen : Window
    {
        public static Preferences prefs;
        private SnackbarHandler SnackbarHandler;
        private PreferenceLoader PreferenceLoader;

        public static bool isOpenAddPackage = false;
        public static bool isOpenUpdatePackage = false;
        public static bool isOpenDeletePackage = false;
        public static bool isOpenSettings = false;
        private string SevenZipPath = Environment.CurrentDirectory + "\\_internals\\7z.dll";

        public Homescreen() {
            // inits & obj inits
            InitializeComponent();
            SnackbarHandler = new SnackbarHandler();
            PreferenceLoader = new PreferenceLoader();

            // load prefs
            prefs = PreferenceLoader.LoadPreferences();
            string welcome = "Hello, " + prefs.UserName + " !";
            SnackbarHandler.ShowMessage(welcome, HomeSnackbar.MessageQueue);

            // 7z init
            SevenZipBase.SetLibraryPath(SevenZipPath);
        }

        private void CardOpenAddPackage(object sender, MouseButtonEventArgs e) {
            if (!isOpenAddPackage) {
                AddPackage ap = new AddPackage();
                ap.Show();
                isOpenAddPackage = true;
            } else {
                DialogBox.Show("Error", "There is already an instance open!", "Alight");
            }
        }

        private void CardOpenUpdatePackage(object sender, MouseButtonEventArgs e) {
            if (!isOpenUpdatePackage) {
                UpdatePackage updpkg = new UpdatePackage();
                updpkg.Show();
                isOpenUpdatePackage = true;
            }
            else {
                DialogBox.Show("Error", "There is already an instance open!", "Alight");
            }
        }

        private void CardOpenDeletePackage(object sender, MouseButtonEventArgs e) {
            if (!isOpenDeletePackage) {
                DeletePackage dp = new DeletePackage();
                dp.Show();
                isOpenDeletePackage = true;
            } else {
                DialogBox.Show("Error", "There is already an instance open!", "Alight");
            }
        }

        private void CardOpenSettings(object sender, MouseButtonEventArgs e) {
            if (!isOpenSettings) {
                Settings sw = new Settings(prefs, PreferenceLoader);
                sw.Show();
                isOpenSettings = true;
            } else {
                DialogBox.Show("Error", "A Settings Window is already open!", "Alright");
            }
        }


        private void ShowMessage(string WindowTitle, string Message, string ButtonCaption) {
            DialogBox.Show(WindowTitle, Message, ButtonCaption);
        }
        //
    }
}

using System;
using System.Windows;
using RepoToolbox.Utilities;
using ExtraTools;
using System.ComponentModel;
using RepoToolbox.Structs;

namespace RepoToolbox.Windows
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private Preferences prefs;
        private PreferenceLoader ploader;

        public Settings(Preferences Prefs, PreferenceLoader PrefLoader) {
            InitializeComponent();
            prefs = Prefs;
            ploader = PrefLoader;
        }

        protected override void OnClosing(CancelEventArgs e) {
            Homescreen.isOpenSettings = false;
            base.OnClosing(e);
        }

        private void SaveSettings(object sender, RoutedEventArgs e) {
            string uname = (String.IsNullOrEmpty(tbUsername.Text)) ? prefs.UserName : tbUsername.Text;
            string hpage = (String.IsNullOrEmpty(tbHomepage.Text)) ? prefs.Homepage : tbHomepage.Text;
            string rcolor = (String.IsNullOrEmpty(tbColor.Text)) ? prefs.RepoColor : tbColor.Text;
            
            ploader.SavePreferences(uname, hpage, rcolor);
            CommonFunctions.ReleaseGenerator(uname + "'s Repository");

            DialogBox.Show("Saved", "Your Settings have been updated! Please relauncht the application", "Alright");
            prefs = null;
            ploader = null;
            this.Close();
        }
    }
}

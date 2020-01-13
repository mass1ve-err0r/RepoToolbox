using System;

namespace RepoToolbox.Structs
{
    public class Preferences
    {
        public string UserName;
        public string Homepage;
        public string RepoColor;

        public Preferences(string _UserName, string _Homepage, string _RepoColor) {
            this.UserName = _UserName;
            this.Homepage = _Homepage;
            this.RepoColor = _RepoColor;
        }
    }
}
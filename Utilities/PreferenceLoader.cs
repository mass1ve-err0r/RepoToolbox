using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using RepoToolbox.Structs;

namespace RepoToolbox.Utilities
{
    public class PreferenceLoader
    {
        string PREF_FILE = Environment.CurrentDirectory + "\\RepoToolbox.prefs";

        public void SavePreferences(string UserName, string Homepage, string RColor) {
            Preferences prefs = new Preferences(UserName, Homepage, RColor);
            string JSONprefs = JsonConvert.SerializeObject(prefs);

            File.WriteAllText(PREF_FILE, JSONprefs);
        }

        public Preferences LoadPreferences() {
            StreamReader pf = File.OpenText(PREF_FILE);
            JsonTextReader reader = new JsonTextReader(pf);
            JObject JSONprefs = (JObject)JToken.ReadFrom(reader);
            reader.Close();
            pf.Close();

            string un = JSONprefs.GetValue("UserName").ToString();
            string hpage = JSONprefs.GetValue("Homepage").ToString();
            string rc = JSONprefs.GetValue("RepoColor").ToString();

            return new Preferences(un, hpage, rc);
        }
    }
}

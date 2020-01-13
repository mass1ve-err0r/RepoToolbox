using System;
using System.IO;
using RepoToolbox.Structs;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace RepoToolbox.Utilities
{
    public class InternalPackageManager
    {

        private static string INTERNAL_DIR = Environment.CurrentDirectory + "\\_internals\\packages_record";

        public static void SerializeEntry(Package pkg) {
            IFormatter formatter = new BinaryFormatter();
            string fn = INTERNAL_DIR + "\\" + pkg.PackageID + ".rpentry";
            Stream stream = new FileStream(fn, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, pkg);
            stream.Close();
        }

        public static Package DeserializeEntry(string Entry) {
            IFormatter formatter = new BinaryFormatter();
            string fn = INTERNAL_DIR + "\\" + Entry + ".rpentry";
            Stream stream = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.Read);
            Package pkg = (Package)formatter.Deserialize(stream);
            stream.Close();
            return pkg;
        }


    }
}

using System.IO;

namespace RepoToolbox.Structs
{
    public static class GeneralExtensions
    {
        public static bool IsNullOrEmpty<T>(this T[] array) {
            return (array == null || array.Length == 0);
        }

        public static void SkipLines(this StreamReader sr, int LinesToSkip) {
            for (int i = 0; i < LinesToSkip; i++) {
                sr.ReadLine();
            }
        }

        //
    }
}

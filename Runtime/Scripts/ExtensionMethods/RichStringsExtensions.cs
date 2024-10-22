namespace MG_Utilities
{
    public static class RichStringsExtensions
    {
        public static string Bold(this string str) => "<b>" + str + "</b>";
        public static string Italics(this string str) => "<i>" + str + "</i>";
        public static string Underline(this string str) => "<u>" + str + "</u>";
        public static string Size(this string str, int size) => "<size=" + size + ">" + str + "</size>";
        public static string Color(this string str, string colorName) => "<color=" + colorName + ">" + str + "</color>";
    }
}

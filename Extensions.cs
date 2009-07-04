using System.Web.UI.WebControls;

namespace Naspinski.Controls.FormFields
{
    public static class Extensions
    {
        public static void Set(this ListControl lc, string findByVal)
        { // attempts to set a ListControl (ddl, listbox, etc.) to the 'findByVal'
            try { lc.SelectedIndex = lc.Items.IndexOf(lc.Items.FindByValue(findByVal)); }
            catch { };
        }

        public static string GetPluralized(this string s)
        {
            if (s.EndsWith("y")) return s.Substring(0, s.Length - 1) + "ies";
            else if (s.EndsWith("s")) return s;
            else return s + "s";
        }
    }
}

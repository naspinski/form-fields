using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Naspinski.Controls.FormFields
{
    public static class Extensions
    {
        public static void Set(this DropDownList ddl, string findByVal)
        { // attempts to set a DDL to the 'findByVal'
            try { ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(findByVal)); }
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

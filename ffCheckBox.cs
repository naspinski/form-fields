using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Naspinski.Controls.FormFields
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), DefaultProperty("Text"),
    ToolboxData("<{0}:textBox runat=\"server\"> </{0}:textBox>")]
    public class checkBox : FormField
    {
        public CheckBox CheckBox;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckBox = new CheckBox() { ID = "chk" };
            this.FieldInit(CheckBox);
        }
    }
}

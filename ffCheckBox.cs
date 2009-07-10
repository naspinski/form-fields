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
    ToolboxData("<{0}:checkBox runat=\"server\"> </{0}:checkBox>")]
    public class checkBox : FormField
    {
        public CheckBox CheckBox;
        [Category("Behavior"), Description("Changing the value will cause postback"), DefaultValue("False")]
        public bool AutoPostBack { get; set; }
        [Category("Appearance"), DefaultValue("False")]
        public bool Checked { internal get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckBox = new CheckBox() { ID = "chk", AutoPostBack = AutoPostBack, Checked = this.Checked };
            this.FieldInit(CheckBox);
            CheckBox.CssClass = FormElementCssClass;
        }
    }
}

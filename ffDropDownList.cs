using System;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;
using System.ComponentModel;

namespace Naspinski.Controls.FormFields
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), DefaultProperty("Text"),
    ToolboxData("<{0}:dropDownList runat=\"server\"> </{0}:dropDownList>")]
    public class dropDownList : DataFormField
    {
        public DropDownList DropDownList;
        [Category("Appearance"), Description("Value to be selected"), DefaultValue("")]
        public string SelectedValue { internal get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DropDownList = new DropDownList()
            {
                ID = "ddl",
                AppendDataBoundItems = true
            };
            if (string.IsNullOrEmpty(L2STableName) && string.IsNullOrEmpty(DataSourceID))
                FieldInit(this.DropDownList);
            else
                BindToDataSource(this.DropDownList, true);

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(SelectedValue))
                    this.DropDownList.Set(SelectedValue);
            }
            else SelectedValue = this.DropDownList.SelectedValue;
            DropDownList.CssClass = FormElementCssClass;
        }
    }
}
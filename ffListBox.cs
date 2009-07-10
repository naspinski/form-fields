using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;
using System.ComponentModel;
using System.Configuration;

namespace Naspinski.Controls.FormFields
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), DefaultProperty("Text"),
    ToolboxData("<{0}:listBox runat=\"server\"> </{0}:listBox>")]
    public class listBox : DataFormField
    {
        public ListBox ListBox;
        [Category("Appearance"), Description("Values to be selected if the SelectionMode is Multiple values are to be seperated by semicolons [;]"), DefaultValue("")]
        public string SelectedValue { internal get; set; }
        [Category("Appearance"), Description("Number of rows high the ListBox will be"), DefaultValue("4")]
        public int Rows { get; set; }
        [Category("Behavior"), Description("SelectionMode of the ListBox"), DefaultValue("Single")]
        public ListSelectionMode SelectionMode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Rows = Rows < 1 ? 4 : Rows;
            this.ListBox = new ListBox()
            {
                ID = "lst",
                AppendDataBoundItems = true,
                SelectionMode = this.SelectionMode,
                Rows = Rows
            };

            if (string.IsNullOrEmpty(L2STableName) && string.IsNullOrEmpty(DataSourceID))
                FieldInit(this.ListBox);
            else
                BindToDataSource(this.ListBox, false);

            if (!IsPostBack)
                setSelectedValues();
            ListBox.CssClass = FormElementCssClass;
        }

        private void setSelectedValues()
        {
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                if (SelectionMode == ListSelectionMode.Multiple)
                {
                    string[] values = SelectedValue.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (ListItem li in this.ListBox.Items) li.Selected = values.Contains(li.Value);
                }
                else
                {
                    foreach (ListItem li in this.ListBox.Items)
                    {
                        if (li.Value.Equals(SelectedValue))
                        {
                            li.Selected = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}

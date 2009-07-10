using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Naspinski.Controls.FormFields
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), DefaultProperty("Text"),
    ToolboxData("<{0}:checkBoxList runat=\"server\"> </{0}:checkBoxDownList>")]
    public class checkBoxList : DataFormField
    {
        public CheckBoxList CheckBoxList;
        [Category("Behavior"), Description("Changing the value will cause postback"), DefaultValue("False")]
        public bool AutoPostBack { get; set; }
        [Category("Appearance"), Description("Values to be selected; multiple values are to be seperated by semicolons [;]"), DefaultValue("")]
        public string SelectedValue { internal get; set; }
        [Category("Appearance"), Description("Sets repeated direction to be horizontal or vertical"), DefaultValue("Horizontal")]
        public RepeatDirection RepeatDirection { get; set; }
        [Category("Appearance"), Description("Flow or Table layout"), DefaultValue("Table")]
        public RepeatLayout RepeatLayout { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckBoxList = new CheckBoxList()
            {
                ID = "cbl",
                AutoPostBack = AutoPostBack,
                RepeatDirection = this.RepeatDirection,
                RepeatLayout = this.RepeatLayout
            };
            BindToDataSource(CheckBoxList, false);
            CheckBoxList.DataBind();
            setSelectedValues();
            CheckBoxList.CssClass = FormElementCssClass;
        }

        private void setSelectedValues()
        {
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                string[] values = SelectedValue.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (ListItem li in this.CheckBoxList.Items)
                    li.Selected = values.Contains(li.Value);
            }
        }
    }
}
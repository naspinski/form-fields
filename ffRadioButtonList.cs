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
    ToolboxData("<{0}:radioButtonList runat=\"server\"> </{0}:radioButtonList>")]
    public class radioButtonList : DataFormField
    {
        public RadioButtonList RadioButtonList;
        [Category("Behavior"), Description("Changing the value will cause postback"), DefaultValue("False")]
        public bool AutoPostBack { get; set; }
        [Category("Appearance"), Description("Sets repeated direction to be horizontal or vertical"), DefaultValue("Horizontal")]
        public RepeatDirection RepeatDirection { get; set; }
        [Category("Appearance"), Description("Flow or Table layout"), DefaultValue("Table")]
        public RepeatLayout RepeatLayout { get; set; }
        [Category("Appearance"), Description("Value to be selected"), DefaultValue("")]
        public string SelectedValue { internal get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            RadioButtonList = new RadioButtonList()
            {
                ID = "rbl",
                AutoPostBack = AutoPostBack,
                RepeatDirection = this.RepeatDirection,
                RepeatLayout = this.RepeatLayout
            };
            BindToDataSource(RadioButtonList, false);
            RadioButtonList.DataBind();

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(SelectedValue))
                    this.RadioButtonList.Set(SelectedValue);
            }
            else SelectedValue = this.RadioButtonList.SelectedValue;
            RadioButtonList.CssClass = FormElementCssClass;
        }
    }
}

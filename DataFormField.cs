using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for DataConnection
/// </summary>
namespace Naspinski.Controls.FormFields
{
    public class DataFormField : FormField
    {
        [Category("Data"), Description("Linq-to-SQL Table Name"), DefaultValue("")]
        public string L2STableName { get; set; }
        [Category("Data"), Description("Column to order by when using Linq-to-SQL"), DefaultValue("")]
        public string L2SOrderByColumn { get; set; }
        [Category("Data"), Description("Name of your Linq-to-SQL DataContext"), DefaultValue("")]
        public string L2SDataContextName { get; set; }
        [Category("Data"), Description("Column displayed in the DropDownList"), DefaultValue("")]
        public string DataTextField { get; set; }
        [Category("Data"), Description("Column used for Values in the DropDownList"), DefaultValue("")]
        public string DataValueField { get; set; }
        [Category("Data"), Description("DataSourceID"), DefaultValue("")]
        public string DataSourceID { get; set; }

        public void BindToDataSource(ListControl C, bool add_initial_item)
        {
            ConnectToData(C);
            LinqDataSource ds = new LinqDataSource() { ID = "ds", OrderBy = L2SOrderByColumn };
            FieldInit(C);

            if (!IsPostBack)
            {
                if (add_initial_item) C.Items.Add(InitialValue);
                if (!string.IsNullOrEmpty(L2STableName))
                {
                    ds.ContextTypeName = L2SDataContextName;
                    ds.TableName = L2STableName.GetPluralized();
                    ds.Select = "new (" + DataTextField + ", " + DataValueField + ")";
                    C.DataSource = ds;
                }
                else if (!string.IsNullOrEmpty(DataSourceID))
                {
                    C.DataSource = null;
                    C.DataSourceID = DataSourceID;
                }
                C.DataBind();
            }
        }

        private void ConnectToData(ListControl C)
        {
            if (!string.IsNullOrEmpty(L2SDataContextName) && string.IsNullOrEmpty(L2STableName))
                throw new Exception("No L2STableName declared");
            else if (string.IsNullOrEmpty(L2SDataContextName) && !string.IsNullOrEmpty(L2STableName) && string.IsNullOrEmpty(Settings.DataContext))
                throw new Exception("No L2SDataContext declared");



            if (string.IsNullOrEmpty(DataTextField)) DataTextField = Settings.DataTextField;
            if (!string.IsNullOrEmpty(Settings.TextSuffix) && string.IsNullOrEmpty(DataTextField) && !string.IsNullOrEmpty(L2STableName))
                DataTextField = L2STableName + Settings.TextSuffix;
            C.DataTextField = DataTextField;

            if (string.IsNullOrEmpty(L2SDataContextName)) L2SDataContextName = Settings.DataContext;
            if (!string.IsNullOrEmpty(L2STableName))
            {
                if (string.IsNullOrEmpty(L2SOrderByColumn)) L2SOrderByColumn = Settings.OrderBy;
                if (!string.IsNullOrEmpty(Settings.OrderSuffix) && string.IsNullOrEmpty(L2SOrderByColumn) && !string.IsNullOrEmpty(L2STableName))
                    L2SOrderByColumn = L2STableName + Settings.OrderSuffix;

                if (!string.IsNullOrEmpty(DataTextField) && string.IsNullOrEmpty(L2SOrderByColumn)) L2SOrderByColumn = DataTextField;
                if (string.IsNullOrEmpty(L2SOrderByColumn)) L2SOrderByColumn = Settings.OrderBy;
            }

            if (!string.IsNullOrEmpty(DataSourceID)) C.DataSourceID = DataSourceID;
            if (string.IsNullOrEmpty(DataValueField)) DataValueField = Settings.DataValueField;
            if (!string.IsNullOrEmpty(Settings.ValueSuffix) && string.IsNullOrEmpty(DataValueField) && !string.IsNullOrEmpty(L2STableName))
                DataValueField = L2STableName + Settings.ValueSuffix;
            C.DataValueField = DataValueField;
        }
    }
}
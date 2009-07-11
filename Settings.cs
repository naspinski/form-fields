using System.Configuration;

/// <summary>
/// Summary description for Settings
/// </summary>
public class Settings
{
    public static string InitialValue { get { return ConfigurationSettings.AppSettings["InitialValue"]; } }
    public static string RequiredErrorMessage { get { return ConfigurationSettings.AppSettings["RequiredErrorMessage"]; } }
    public static string FieldCssClass { get { return ConfigurationSettings.AppSettings["FieldCssClass"]; } }
    public static string ValidatorCssClass { get { return ConfigurationSettings.AppSettings["ValidatorCssClass"]; } }
    public static string FormElementCssClass { get { return ConfigurationSettings.AppSettings["FormElementCssClass"]; } }
    public static string WatermarkCssClass { get { return ConfigurationSettings.AppSettings["WatermarkCssClass"]; } }
    public static string ValueSuffix { get { return ConfigurationSettings.AppSettings["ValueSuffix"]; } }
    public static string TextSuffix { get { return ConfigurationSettings.AppSettings["TextSuffix"]; } }
    public static string OrderSuffix { get { return ConfigurationSettings.AppSettings["OrderSuffix"]; } }
    public static string DataValueField { get { return ConfigurationSettings.AppSettings["DataValueField"]; } }
    public static string DataTextField { get { return ConfigurationSettings.AppSettings["DataTextField"]; } }
    public static string OrderBy { get { return ConfigurationSettings.AppSettings["OrderBy"]; } }
    public static string DataContext { get { return ConfigurationSettings.AppSettings["DataContext"]; } }
}

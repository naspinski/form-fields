using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Naspinski.Controls.FormFields
{
    public partial class FormField : UserControl
    {
        public Panel Field;
        public Label Header;

        [Category("Appearance"), Description("Form Field Title (displayed in an <h3> tag)"), DefaultValue("")]
        public string Title { get; set; }
        [Category("Appearance"), Description("CSS Class used for the validator"), DefaultValue("validate")]
        public string ValidatorCssClass { get; set; }
        [Category("Appearance"), Description("Css Class for the <div> wrapping the control"), DefaultValue("formField")]
        public string FieldCssClass { get; set; }
        [Category("Appearance"), Description("Css Class used for the input control"), DefaultValue("")]
        public string FormElementCssClass { get; set; }
        [Category("Appearance"), Description("Defines whether or not to use the RequiredFieldvalidator"), DefaultValue("false")]
        public bool Required { get; set; }
        [Category("Appearance"), Description("ErrorMessage for the RequiredFieldValidator"), DefaultValue("required")]
        public string RequiredErrorMessage { get; set; }
        public string InitialValue { get; set; }

        public RequiredFieldValidator RequiredValidator;
        public PlaceHolder ValidationPlaceHolder;

        private Type[] TYPES_THAT_DONT_VALIDATE = new Type[] { typeof(CheckBox), typeof(CheckBoxList), typeof(RadioButton), typeof(RadioButtonList) };

        public void FieldInit(Control c)
        {
            SetDefaults();
            Type test = c.GetType();
            bool validate = !TYPES_THAT_DONT_VALIDATE.Contains(test);
            Field = new Panel() { CssClass = FieldCssClass };
            Header = new Label()
            {
                AssociatedControlID = c.ID,
                Text = Title
            };
            ValidationPlaceHolder = new PlaceHolder();
            if (validate && Required)
            {
                RequiredValidator = new RequiredFieldValidator()
                {
                    CssClass = ValidatorCssClass,
                    ControlToValidate = c.ID,
                    Display = ValidatorDisplay.Dynamic,
                    ErrorMessage = RequiredErrorMessage,
                    InitialValue = InitialValue
                };
            }
            else Header.Text += (validate ? (Header.Text.Length > 0 ? "*" : string.Empty) : string.Empty);

            Field.Controls.Add(new LiteralControl("<h3>"));
            if (validate && Required) Field.Controls.Add(RequiredValidator);
            Field.Controls.Add(ValidationPlaceHolder);
            if (!string.IsNullOrEmpty(Title)) Field.Controls.Add(Header);
            Field.Controls.Add(new LiteralControl("</label></h3>"));
            Field.Controls.Add(c);
            this.Controls.Add(Field);
        }

        private void SetDefaults()
        {
            if (string.IsNullOrEmpty(InitialValue)) InitialValue = InitialValue = Settings.InitialValue;
            if (string.IsNullOrEmpty(RequiredErrorMessage)) RequiredErrorMessage = Settings.RequiredErrorMessage;
            if (string.IsNullOrEmpty(FieldCssClass)) FieldCssClass = Settings.FieldCssClass;
            if (string.IsNullOrEmpty(ValidatorCssClass)) ValidatorCssClass = Settings.ValidatorCssClass;
        }
    }
}
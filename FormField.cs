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
        public Literal _Title;

        [Bindable(false), Category("Appearance"), Description("Form Field Title (displayed in an <h3> tag)"), DefaultValue(""), Localizable(true)]
        public string Title;
        [Bindable(false), Category("Appearance"), Description("CSS Class used for the validator"), DefaultValue("validate"), Localizable(true)]
        public string ValidatorCssClass = "validate";
        [Bindable(false), Category("Appearance"), Description("Css Class for the <div> wrapping the control"), DefaultValue("formField"), Localizable(true)]
        public string FieldCssClass = "formField";
        [Bindable(false), Category("Appearance"), Description("Css Class used for the input control"), DefaultValue(""), Localizable(true)]
        public string FormElementCssClass;
        [Bindable(false), Category("Appearance"), Description("Defines whether or not to use the RequiredFieldvalidator"), DefaultValue("false"), Localizable(true)]
        public bool Required = false;
        [Bindable(false), Category("Appearance"), Description("ErrorMessage for the RequiredFieldValidator"), DefaultValue("required"), Localizable(true)]
        public string RequiredErrorMessage = "required";

        public RequiredFieldValidator RequiredValidator;
        public PlaceHolder ValidationPlaceHolder;

        private Type[] TYPES_THAT_DONT_VALIDATE = new Type[] { typeof(CheckBox), typeof(CheckBoxList), typeof(RadioButton), typeof(RadioButtonList) };

        public void FieldInit(Control c)
        {
            Type test = c.GetType();
            bool validate = !TYPES_THAT_DONT_VALIDATE.Contains(test);
            Field = new Panel() { CssClass = FieldCssClass };
            _Title = new Literal();
            _Title.Text = Title;
            ValidationPlaceHolder = new PlaceHolder();
            if (validate && Required)
            {
                RequiredValidator = new RequiredFieldValidator()
                {
                    CssClass = ValidatorCssClass,
                    ControlToValidate = c.ID,
                    Display = ValidatorDisplay.Dynamic,
                    ErrorMessage = RequiredErrorMessage
                };
            }
            else _Title.Text += (validate ? "*" : string.Empty);

            Field.Controls.Add(new LiteralControl("<h3><label for='" + c.ClientID + "'>"));
            if (validate && Required) Field.Controls.Add(RequiredValidator);
            Field.Controls.Add(ValidationPlaceHolder);
            if (!string.IsNullOrEmpty(Title)) Field.Controls.Add(_Title);
            Field.Controls.Add(new LiteralControl("</label></h3>"));
            Field.Controls.Add(c);
            this.Controls.Add(Field);
        }
    }
}
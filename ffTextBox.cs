using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

namespace Naspinski.Controls.FormFields
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    ToolboxData("<{0}:textBox runat=\"server\"> </{0}:textBox>")]
    public class textBox : FormField
    {
        public enum Types { Text, Integer, Money, Email, PhoneNumber, ZipCode, Date };
        public TextBox TextBox;
        [Category("Appearance"), Description("Type of specialized input"), DefaultValue("Text")]
        public Types Type { get; set; }
        [Category("Appearance"), Description("TextMode of the input Field"), DefaultValue("SingleLine")]
        public TextBoxMode TextMode { get; set; }
        [Category("Appearance"), Description("Regular Expression to validate against"), DefaultValue(""), Localizable(true)]
        public string RegEx { get; set; }
        [Category("Appearance"), Description("Error Message associated with the additional validator (not the RequiredFieldValidator)"), DefaultValue("error")]
        public string ErrorMessage { get; set; }
        private int regCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox = new TextBox() { ID = "txt", CssClass = FormElementCssClass };
            if (string.IsNullOrEmpty(ErrorMessage)) ErrorMessage = "error";
            this.FieldInit(TextBox);

            TextBox.TextMode = TextMode;
            switch (TextMode)
            {
                case TextBoxMode.MultiLine:
                    TextBox.Rows = 4;
                    break;
            }

            if (!string.IsNullOrEmpty(RegEx)) { AddRegExValidator(); }

            if (!string.IsNullOrEmpty(Type.ToString()))
            {
                switch (Type)
                {
                    case Types.Money:
                        MaskedEditExtender meeMoney = new MaskedEditExtender()
                        {
                            Mask = "999,999,999,999.99",
                            TargetControlID = TextBox.ID,
                            InputDirection = MaskedEditInputDirection.RightToLeft,
                            DisplayMoney = MaskedEditShowSymbol.Left,
                            ID = "mee"
                        };
                        meeMoney.MaskType = MaskedEditType.Number;
                        ValidationPlaceHolder.Controls.Add(meeMoney);
                        break;

                    case Types.Date:
                        MaskedEditExtender meeDate = new MaskedEditExtender()
                        {
                            Mask = "99/99/9999",
                            MaskType = MaskedEditType.Date,
                            TargetControlID = TextBox.ID,
                            ID = "mee"
                        };
                        ValidationPlaceHolder.Controls.Add(meeDate);
                        CalendarExtender ceDate = new CalendarExtender()
                        {
                            TargetControlID = TextBox.ID,
                            Format = "MM/dd/yyyy",
                            PopupPosition = CalendarPosition.BottomLeft,
                            ID = "cal"
                        };
                        ValidationPlaceHolder.Controls.Add(ceDate);
                        TextBoxWatermarkExtender tweDate = new TextBoxWatermarkExtender()
                        {
                            TargetControlID = TextBox.ID,
                            WatermarkText = "mm/dd/yyyy",
                            ID = "twe"
                        };
                        ValidationPlaceHolder.Controls.Add(tweDate);
                        if (ErrorMessage.Equals("error")) ErrorMessage = "invalid date";
                        CompareValidator cvDate = new CompareValidator()
                        {
                            ControlToValidate = TextBox.ID,
                            ErrorMessage = ErrorMessage,
                            CssClass = ValidatorCssClass,
                            Display = ValidatorDisplay.Dynamic,
                            ID = "cv",
                            Operator = ValidationCompareOperator.DataTypeCheck,
                            Type = ValidationDataType.Date
                        };
                        ValidationPlaceHolder.Controls.Add(cvDate);
                        break;

                    case Types.Integer:
                        if (ErrorMessage.Equals("error")) ErrorMessage = "integers only";
                        RegEx = @"^[0-9]+$";
                        AddRegExValidator();
                        break;

                    case Types.Email:
                        if (ErrorMessage.Equals("error")) ErrorMessage = "invalid email address";
                        RegEx = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                        AddRegExValidator();
                        break;

                    case Types.ZipCode:
                        if (ErrorMessage.Equals("error")) ErrorMessage = "invalid zip code";
                        RegEx = @"^\d{5}((-|\s)?\d{4})?$";
                        AddRegExValidator();
                        break;

                    case Types.PhoneNumber:
                        if (ErrorMessage.Equals("error")) ErrorMessage = "invalid phone number";
                        RegEx = @"^([\(]{1}[0-9]{3}[\)]{1}[\.| |\-]{0,1}|^[0-9]{3}[\.|\-| ]?)?[0-9]{3}(\.|\-| )?[0-9]{4}$";
                        AddRegExValidator();
                        break;
                }
            }
        }

        private void AddRegExValidator()
        {
            RegularExpressionValidator reg = new RegularExpressionValidator()
            {
                ControlToValidate = TextBox.ID,
                ErrorMessage = ErrorMessage,
                Display = ValidatorDisplay.Dynamic,
                ID = "reg" + regCount++.ToString(),
                CssClass = ValidatorCssClass,
                ValidationExpression = RegEx,
            };
            ValidationPlaceHolder.Controls.Add(reg);
        }

        private void SetDefaults()
        {
            if (string.IsNullOrEmpty(ErrorMessage)) ErrorMessage = "error";
        }
    }
}
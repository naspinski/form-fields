using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Security.Permissions;
using System.ComponentModel;
using AjaxControlToolkit;

namespace Naspinski.Controls.FormFields
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), DefaultProperty("Text"),
    ToolboxData("<{0}:textBox runat=\"server\"> </{0}:textBox>")]
    public class textBox : FormField
    {
        public enum Types { Text, Integer, Money, Email, PhoneNumber, ZipCode, Date, SocialSecurityNumber };
        public TextBox TextBox;
        [Category("Appearance"), DefaultValue("")]
        public string Text { set; internal get; }
        [Category("Behavior"), Description("Type of specialized input"), DefaultValue("Text")]
        public Types Type { get; set; }
        [Category("Behavior"), Description("TextMode of the input Field"), DefaultValue("SingleLine")]
        public TextBoxMode TextMode { get; set; }
        [Category("Behavior"), Description("Regular Expression to validate against"), DefaultValue(""), Localizable(true)]
        public string RegEx { get; set; }
        [Category("Behavior"), Description("Error Message associated with the additional validator (not the RequiredFieldValidator)"), DefaultValue("error")]
        public string ErrorMessage { get; set; }
        [Category("Behavior"), Description("Specifies the mask to put on the textbox"), DefaultValue("")]
        public string Mask { get; set; }
        [Category("Behavior"), Description("Specifies the mask type to use in conjunciton with the Mask")]
        public MaskedEditType MaskType { get; set; }
        [Category("Behavior"), Description("Specifies the mask type to use in conjunciton with the Mask")]
        public MaskedEditInputDirection InputDirection { get; set; }
        [Category("Behavior"), Description("Specifies the mask type to use in conjunciton with the Mask")]
        public MaskedEditShowSymbol DisplayMoney { get; set; }
        private int regCount = 0;

        public MaskedEditExtender MaskedEditExtender;
        public CalendarExtender CalendarExtender;
        public CompareValidator CompareValidator;
        public RegularExpressionValidator RegularExpressionValidator;
        public TextBoxWatermarkExtender TextBoxWatermarkExtender;

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox = new TextBox() { ID = "txt", CssClass = FormElementCssClass, Text = Text };
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
            bool mask_made = false;

            if (!string.IsNullOrEmpty(Type.ToString()))
            {
                switch (Type)
                {
                    case Types.Money:
                        InputDirection = MaskedEditInputDirection.RightToLeft;
                        DisplayMoney = MaskedEditShowSymbol.Left;
                        MaskType = MaskedEditType.Number;
                        if (string.IsNullOrEmpty(Mask)) Mask = "999,999,999,999.99";
                        MakeMask();
                        ValidationPlaceHolder.Controls.Add(MaskedEditExtender);
                        mask_made = true;
                        break;

                    case Types.Date:
                        if (string.IsNullOrEmpty(Mask)) Mask = "99/99/9999";
                        MaskType = MaskedEditType.Date;
                        MakeMask();
                        ValidationPlaceHolder.Controls.Add(MaskedEditExtender);
                        mask_made = true;
                        CalendarExtender = new CalendarExtender()
                        {
                            TargetControlID = TextBox.ID,
                            Format = "MM/dd/yyyy",
                            PopupPosition = CalendarPosition.BottomLeft,
                            ID = "cal"
                        };
                        ValidationPlaceHolder.Controls.Add(CalendarExtender);
                        TextBoxWatermarkExtender = new TextBoxWatermarkExtender()
                        {
                            TargetControlID = TextBox.ID,
                            WatermarkText = "mm/dd/yyyy",
                            ID = "twe"
                        };
                        ValidationPlaceHolder.Controls.Add(TextBoxWatermarkExtender);
                        if (ErrorMessage.Equals("error")) ErrorMessage = "invalid date";
                        CompareValidator = new CompareValidator()
                        {
                            ControlToValidate = TextBox.ID,
                            ErrorMessage = ErrorMessage,
                            CssClass = ValidatorCssClass,
                            Display = ValidatorDisplay.Dynamic,
                            ID = "cv",
                            Operator = ValidationCompareOperator.DataTypeCheck,
                            Type = ValidationDataType.Date
                        };
                        ValidationPlaceHolder.Controls.Add(CompareValidator);
                        break;

                    case Types.SocialSecurityNumber:
                        if (ErrorMessage.Equals("error")) ErrorMessage = "invalid ssn";
                        RegEx = @"^(?!000)([0-6]\d{2}|7([0-6]\d|7[012]))([ -]?)(?!00)\d\d\3(?!0000)\d{4}$";
                        AddRegExValidator();
                        Mask = "999-99-9999";
                        MakeMask();
                        ValidationPlaceHolder.Controls.Add(MaskedEditExtender);
                        mask_made = true;
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
            if (!mask_made && !string.IsNullOrEmpty(Mask))
            {
                MakeMask();
                ValidationPlaceHolder.Controls.Add(MaskedEditExtender);
            }
        }

        private void AddRegExValidator()
        {
            RegularExpressionValidator = new RegularExpressionValidator()
            {
                ControlToValidate = TextBox.ID,
                ErrorMessage = ErrorMessage,
                Display = ValidatorDisplay.Dynamic,
                ID = "reg" + regCount++.ToString(),
                CssClass = ValidatorCssClass,
                ValidationExpression = RegEx,
            };
            ValidationPlaceHolder.Controls.Add(RegularExpressionValidator);
        }

        private void SetDefaults()
        {
            if (string.IsNullOrEmpty(ErrorMessage)) ErrorMessage = "error";
        }

        private void MakeMask()
        {
            MaskedEditExtender = new MaskedEditExtender()
            {
                Mask = this.Mask,
                MaskType = this.MaskType,
                TargetControlID = TextBox.ID,
                ID = "mee"
            };
        }
    }
}
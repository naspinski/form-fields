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
        [Category("Behavior"), Description("Specifies the mask")]
        public MaskedEditType MaskType { get; set; }
        [Category("Behavior"), Description("Specifies the mask type to use in conjunction with the Mask")]
        public MaskedEditInputDirection InputDirection { get; set; }
        [Category("Behavior"), Description("Specifies masked input direction")]
        public MaskedEditShowSymbol DisplayMoney { get; set; }
        [Category("Appearance"), Description("Css Class for the Mask"), DefaultValue("")]
        public string WatermarkCssClass { get; set; }
        [Category("Appearance"), Description("Text for the watermark"), DefaultValue("")]
        public string WatermarkText { get; set; } 
        
        private int regCount = 0;
        private bool mask_made, watermark_made;

        public MaskedEditExtender MaskedEditExtender;
        public CalendarExtender CalendarExtender;
        public CompareValidator CompareValidator;
        public RegularExpressionValidator RegularExpressionValidator;
        public TextBoxWatermarkExtender TextBoxWatermarkExtender;

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox = new TextBox() { ID = "txt", Text = Text };
            SetTextBoxDefaults();
            this.FieldInit(TextBox);
            TextBox.CssClass = FormElementCssClass;

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
                        InputDirection = MaskedEditInputDirection.RightToLeft;
                        DisplayMoney = MaskedEditShowSymbol.Left;
                        MaskType = MaskedEditType.Number;
                        if (string.IsNullOrEmpty(Mask)) Mask = "999,999,999,999.99";
                        MakeMask();
                        ValidationPlaceHolder.Controls.Add(MaskedEditExtender);
                        break;

                    case Types.Date:
                        if (string.IsNullOrEmpty(Mask)) Mask = "99/99/9999";
                        MaskType = MaskedEditType.Date;
                        MakeMask();
                        ValidationPlaceHolder.Controls.Add(MaskedEditExtender);
                        CalendarExtender = new CalendarExtender()
                        {
                            TargetControlID = TextBox.ID,
                            Format = "MM/dd/yyyy",
                            PopupPosition = CalendarPosition.BottomLeft,
                            ID = "cal"
                        };
                        ValidationPlaceHolder.Controls.Add(CalendarExtender);

                        WatermarkText = "MM/dd/yyyy";
                        MakeWatermark();
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
                        break;

                    case Types.Integer:
                        if (ErrorMessage.Equals("error")) ErrorMessage = "integers only";
                        RegEx = @"^-?[0-9]+$";
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
            if (!watermark_made && !string.IsNullOrEmpty(WatermarkText))
            {
                MakeWatermark();
                ValidationPlaceHolder.Controls.Add(TextBoxWatermarkExtender);
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

        private void SetTextBoxDefaults()
        {
            if (string.IsNullOrEmpty(ErrorMessage)) ErrorMessage = "error";
            if (string.IsNullOrEmpty(WatermarkCssClass)) WatermarkCssClass = Settings.WatermarkCssClass;
            mask_made = false;
            watermark_made = false;
        }

        private void MakeMask()
        {
            MaskedEditExtender = new MaskedEditExtender()
            {
                Mask = this.Mask,
                MaskType = this.MaskType,
                TargetControlID = TextBox.ID,
                ID = "mee" + regCount++.ToString(),
            };
            watermark_made = true;
        }

        private void MakeWatermark()
        {
            TextBoxWatermarkExtender = new TextBoxWatermarkExtender()
            {
                TargetControlID = TextBox.ID,
                WatermarkCssClass = WatermarkCssClass,
                WatermarkText = WatermarkText,
                ID = "twe"
            };
            mask_made = true;
        }
    }
}
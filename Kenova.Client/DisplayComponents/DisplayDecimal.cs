using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{

    public class DisplayDecimal : InputBase<decimal>
    {
        private string _mask = "99999999.99";

        [Parameter]
        public decimal MinValue { get; set; } = decimal.MinValue;

        [Parameter]
        public decimal MaxValue { get; set; } = decimal.MaxValue;

        [Parameter]
        public string Mask
        {
            get { return _mask; }
            set
            {
                if (_mask == value)
                    return;

                InputDecimal.ValidateMask(value);

                _mask = value;
            }
        }

        public DisplayDecimal()
        {
            _act_as_displaycomponent = true;
            ShowClipboardButton = true;
        }


        protected override void ConvertValue2Text()
        {
            this.Text = FieldLink.Value.ToString();
        }

        protected override void ConvertText2Value()
        {
        }

        internal static void ValidateMask(string mask)
        {
            const string EXAMPLE = " Example of a valid mask '999999.99' or '99.99999' or '999'";

            if (mask == null)
                throw new Exception("The Mask property cannot be set to null." + EXAMPLE);

            if (mask.Contains("9") == false)
                throw new Exception("The Mask must contain at least one '9' character." + EXAMPLE);

            mask = mask.Replace(',', '.');

            int nine_before_decimalseparator = 0;
            int nine_after_decimalseparator = 0;
            int decimal_separator_count = 0;
            int illegal_character_count = 0;

            for (int i = 0; i < mask.Length; i++)
            {
                char current = mask[i];

                if (current == '9')
                {
                    if (decimal_separator_count == 0)
                        nine_before_decimalseparator++;
                    else
                        nine_after_decimalseparator++;
                }
                else if (current == '.')
                {
                    decimal_separator_count++;
                }
                else
                {
                    illegal_character_count++;
                }
            }

            if (illegal_character_count > 0)
                throw new Exception("Illegal character detected in Mask." + EXAMPLE);

            if (decimal_separator_count > 1)
                throw new Exception("Mask contains multiple decimal separators." + EXAMPLE);

            // The validation made sure that:
            // 1.

        }

        /// <summary>
        /// Only pass a validated mask to this method.
        /// </summary>
        internal static (int precision, int scale) GetPrecisionAndScale(string mask)
        {
            // 9999.99 = precision 6, scale 2

            int precision = 0;
            int scale = 0;
            bool found_separator = false;

            // The mask has already been validated
            mask = mask.Replace(',', '.');

            for (int i = 0; i < mask.Length; i++)
            {
                char current = mask[i];

                if (current == '9')
                {
                    precision++;

                    if (found_separator == true)
                        scale++;
                }
                else if (current == '.')
                {
                    found_separator = true;
                }
                else
                    throw new Exception("should not happen");

            }

            return (precision, scale);
        }

    }
}




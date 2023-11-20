using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{

    public class InputInt16Nullable : InputBase<Int16?>
    {
        [Parameter]
        public Int16 MinValue { get; set; } = Int16.MinValue;

        [Parameter]
        public Int16 MaxValue { get; set; } = Int16.MaxValue;

        protected override void ConvertText2Value()
        {
            string completedtext = this.Text.Trim();

            if (completedtext.Length == 0)
            {
                FieldLink.Value = null;
                return;
            }

            completedtext = AutoCompletionTools.AutoCompleteIntegral(completedtext);

            bool valid = Int16.TryParse(completedtext, out Int16 value_candidate);

            if (valid == false)
                return;

            if (value_candidate < this.MinValue)
            {
                FieldLink.Value = this.MinValue;
                return;
            }

            if (value_candidate > this.MaxValue)
            {
                FieldLink.Value = this.MaxValue;
                return;
            }

            FieldLink.Value = value_candidate;
        }

        protected override void ConvertValue2Text()
        {
            this.Text = FieldLink.Value.ToString();
        }

    }
}


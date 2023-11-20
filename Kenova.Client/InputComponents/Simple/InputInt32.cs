using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{

    public class InputInt32 : InputBase<Int32>
    {
        [Parameter]
        public Int32 MinValue { get; set; } = Int32.MinValue;

        [Parameter]
        public Int32 MaxValue { get; set; } = Int32.MaxValue;

        protected override void ConvertText2Value()
        {
            string completedtext = this.Text.Trim();

            completedtext = AutoCompletionTools.AutoCompleteIntegral(completedtext);

            bool valid = Int32.TryParse(completedtext, out Int32 value_candidate);

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


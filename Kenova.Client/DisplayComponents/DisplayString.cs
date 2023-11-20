using Microsoft.AspNetCore.Components;

namespace Kenova.Client.Components
{
    public class DisplayString : InputBase<string>
    {
        private InputStringFormatting _formatting = InputStringFormatting.None;

        [Parameter]
        public InputStringFormatting Formatting
        {
            get { return _formatting; }
            set
            {
                if (_formatting == value)
                    return;

                _formatting = value;

                // Note: this.Value already contains the updated value
                ConvertValue2Text();
            }
        }

        public DisplayString()
        {
            _act_as_displaycomponent = true;
            ShowClipboardButton = true;
        }

        protected override void ConvertValue2Text()
        {

            string text_candidate = FieldLink.Value;

            if (text_candidate == null)
                this.Text = "";
            else
                this.Text = text_candidate;
        }

        protected override void ConvertText2Value()
        {
        }


    }
}

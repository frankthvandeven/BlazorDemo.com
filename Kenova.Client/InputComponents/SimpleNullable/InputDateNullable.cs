using Kenova.Client.Components.Panels;
using System;

namespace Kenova.Client.Components
{

    public class InputDateNullable : InputBase<DateTime?>
    {

        protected override void ConvertText2Value()
        {
            string completedtext = this.Text.Trim();

            if (completedtext.Length == 0)
            {
                FieldLink.Value = null;
                return;
            }

            completedtext = AutoCompletionTools.AutoCompleteDate(completedtext);

            completedtext = completedtext.Trim();

            bool valid = DateTime.TryParse(completedtext, out DateTime value_candidate);

            if (valid == false)
                return;

            FieldLink.Value = value_candidate;
        }

        protected override void ConvertValue2Text()
        {
            DateTime? dt = FieldLink.Value;

            if (dt == null)
                this.Text = "";
            else
                this.Text = dt.Value.ToShortDateString();
        }

        protected override void OnInitialized()
        {
            this.ShowDropdownButton = true;
            base.OnInitialized();
        }

        private LayerDefinition<PanelDatePicker> _ld = null;

        public override async ValueTask OnDropdownButtonClickedAsync()
        {
            if (this.DropdownOpen == true)
            {
                await _ld.CloseCancelAsync(); // Close the layer
                return;
            }

            this.DropdownOpen = true;

            _ld = new LayerDefinition<PanelDatePicker>
            {
                Kind = LayerKind.Dropdown,
                OwnerID = InputboxContainerId,
                AfterClosed = LayerDefinition_DropdownWasClosed,
                [i => i.SelectedDate] = FieldLink.Value
            };

            _ld.Parameter(p => p.LayerDefinition, _ld); // not possible to refer to self in object initializer syntax

            await _ld.OpenNonBlockingAsync();

            await base.OnDropdownButtonClickedAsync();
        }

        /// <summary>
        /// This method is called by LayerManager after closing the dropdown.
        /// A dropdown is closed by calling LayerManager.Close()
        /// When clicking outside the dropdown element and outside dropdown panel, the 
        /// LayerManager.Close() will be called automatically.
        /// </summary>
        private void LayerDefinition_DropdownWasClosed(LayerResult lr)
        {
            object returnvalue = lr.Data;

            if (returnvalue is DateTime)
            {
                FieldLink.Value = (DateTime)returnvalue;
                ConvertValue2Text();
            }

            this.DropdownOpen = false;
            _ld = null;

            this.StateHasChanged();
        }

    }
}


//private void OnValuePropertyChanged(DependencyPropertyChangedEventArgs args)
//{
// Note: this.Value already contains the updated value

//OLD DateTime value = (DateTime)args.NewValue;
//OLD DateTime timeremoved = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day);
//OLD this.TextEditor.Value = timeremoved.ToShortDateString();

//ConvertValue2Text();
//}

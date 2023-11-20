using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components.Panels
{
    public partial class PanelDatePicker : KenovaDialogBase
    {

        [Parameter]
        public DateTime SelectedDate { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                //hyper_grid.SetFocus();

            }
        }

        private void DateClicked(DateTimeOffset? dt)
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("Date was selected");

            if (dt == null)
                return;

            this.SelectedDate = dt.Value.Date;

            _ = this.CloseOkAsync(this.SelectedDate);
        }


    }
}

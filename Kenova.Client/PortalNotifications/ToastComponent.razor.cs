using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Kenova.Client.SystemComponents
{
    public partial class ToastComponent : KenovaComponentBase, IDisposable
    {
        [CascadingParameter]
        private PortalToaster Parent { get; set; }

        [Parameter]
        public NotificationItem NotificationItem { get; set; }

        private System.Timers.Timer aTimer;

        // A  toast message remains visible for 4 seconds.
        private const double INTERVAL_MS = 4000;

        protected override void OnInitialized()
        {
            aTimer = new System.Timers.Timer(INTERVAL_MS);
            aTimer.Elapsed += Timer_Elapsed;
            aTimer.AutoReset = false;

            aTimer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            NotificationItem.ToastVisible = false;

            _ = this.InvokeAsync(() =>
            {
                Parent.Rerender();
            });

        }

        public void Dispose()
        {
            if (aTimer != null)
            {
                aTimer.Elapsed -= Timer_Elapsed;
                aTimer.Stop();
                aTimer.Dispose();
            }
        }

        private Task Close_Div_Clicked(MouseEventArgs e)
        {
            aTimer.Stop();

            NotificationItem.ToastVisible = false;
            Parent.Rerender();

            return Task.CompletedTask;
        }

    }
}
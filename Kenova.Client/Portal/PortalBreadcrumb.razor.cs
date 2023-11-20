using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Kenova.Client.Components
{
    public partial class PortalBreadcrumb : KenovaComponentBase, IDisposable
    {
        [Parameter]
        public long ForceRender { get; set; }

        //private ObservableCollection<string> Items = new ObservableCollection<string>();

        private List<LayerDefinition> ActiveBreadCrumbs = new List<LayerDefinition>();

        protected override void OnInitialized()
        {
            //Items.Add("Home");
            //Items.Add("Bike Stores");
            //Items.Add("Bicycles");

            Portal.BreadcrumbRefreshWasCalled += Portal_BreadcrumbRefreshWasCalled;
        }

        public void Dispose()
        {
            Portal.BreadcrumbRefreshWasCalled -= Portal_BreadcrumbRefreshWasCalled;
        }

        private void Portal_BreadcrumbRefreshWasCalled()
        {
            this.StateHasChanged();
        }

        private async Task HomeBreadcrumbClickedAsync()
        {
            await NavigationManager.PortalNavigateToAsync("/");

            //LayerManager.CloseAll();
            //Portal.Refresh();
        }


        private async void BaseBreadcrumbClickedAsync()
        {
            await LayerManager.CloseAllAsync();
            
            Portal.Refresh();
        }

        private async void BreadcrumbClickedAsync(LayerDefinition clicked_ld)
        {
            var stack = LayerManager.LayerStack;

            while (stack.Count > 0)
            {
                var peek = stack.Peek();

                if (peek.Equals(clicked_ld))
                    break;

                await peek.CloseCancelAsync();
            }

            Portal.Refresh();
        }

    }
}

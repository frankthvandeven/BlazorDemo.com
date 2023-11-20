using Kenova.Client.SystemComponents;
using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{
    public partial class PortalTopbar : KenovaComponentBase, IDisposable
    {
        private string avatarmenu_id = KenovaClientConfig.GetUniqueElementID();

        [Parameter]
        public long ForceRender { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private async Task HomeClickAsync()
        {
            await NavigationManager.PortalNavigateToAsync("/");
        }

        private LayerDefinition<PortalNotificationsPanel> _notifications_layer;
        private LayerDefinition<PortalSettingsPanel> _settings_layer;
        private LayerDefinition<PortalUserPanel> _userpanel_layer;

        protected override void OnInitialized()
        {
            _notifications_layer = new LayerDefinition<PortalNotificationsPanel>
            {
                Kind = LayerKind.TopmostModelessRight,
            };

            _settings_layer = new LayerDefinition<PortalSettingsPanel>
            {
                Kind = LayerKind.TopmostModelessRight,
            };

            Portal.TopbarRefreshWasCalled += Portal_TopbarRefreshWasCalled;
            Portal.ActivatePortalMenuWasCalled += Portal_ActivatePortalMenuWasCalled;
        }

        public void Dispose()
        {
            Portal.TopbarRefreshWasCalled -= Portal_TopbarRefreshWasCalled;
            Portal.ActivatePortalMenuWasCalled -= Portal_ActivatePortalMenuWasCalled;
        }

        private void Portal_TopbarRefreshWasCalled()
        {
            this.StateHasChanged();
        }

        private async ValueTask NotificationsClickAsync()
        {
            //if (_notifications_layer != null)
            //{
            //    LayerManager.CloseCancel(_notifications_layer);
            //    return;
            //}

            await _notifications_layer.ToggleOpenCloseAsync();
        }

        private async ValueTask PortalSettingsClickAsync()
        {
            await _settings_layer.ToggleOpenCloseAsync();
        }


        private async ValueTask PortalAvatarMenuClickAsync()
        {
            if (_userpanel_layer != null)
            {
                await _userpanel_layer.CloseCancelAsync();
                return;
            }

            _userpanel_layer = new LayerDefinition<PortalUserPanel>
            {
                Kind = LayerKind.Dropdown,
                OwnerID = avatarmenu_id,
                AfterClosed = PortalUserPanelClosed,

            };

            //_settings_layer.Parameter("customer_id", record.customer_id);

            await _userpanel_layer.OpenNonBlockingAsync();
        }

        private void PortalUserPanelClosed(LayerResult lr)
        {
            _userpanel_layer = null;
        }

        private void Portal_ActivatePortalMenuWasCalled()
        {
            if (KenovaClientConfig.Settings.PortalMenuMode == PortalMenuMode.Flyout)
            {
                PortalFlyoutClick();
            }

        }

        private void PortalFlyoutClick()
        {
            var ld = new LayerDefinition<PortalFlyoutPanel>
            {
                Kind = LayerKind.FlyoutLeft
            };

            _ = ld.OpenNonBlockingAsync();
        }

    }
}

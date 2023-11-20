using Kenova.Client.Components;
using Kenova.Client.Core;
using Microsoft.AspNetCore.Components;

namespace Kenova.Client.SystemComponents
{
    public partial class PortalSettingsPanel : KenovaDialogBase
    {
        string portalMenuIdentifier;

        private string SelectedLanguage = "nl";
        private string SelectedCulture = null;

        private Settings _settings = KenovaClientConfig.Settings;

        private TabItemCollection tabItems = new TabItemCollection();

        protected override void OnDialogInitialized()
        {
            this.SelectedLanguage = _settings.CurrentLanguage;
            this.SelectedCulture = _settings.CurrentCulture;

            portalMenuIdentifier = _settings.PortalMenuMode == PortalMenuMode.Flyout ? "flyout" : "docked";

            tabItems.Add("general", Loc["portal_tab_general"]);
            tabItems.Add("region", Loc["portal_tab_region"]);

        }

        private async void MenuModeChangedAsync()
        {
            if (portalMenuIdentifier == "docked")
                _settings.PortalMenuMode = PortalMenuMode.Docked;
            else
                _settings.PortalMenuMode = PortalMenuMode.Flyout;

            await _settings.SaveSettingsAsync();

            //this.StateHasChanged();
            Portal.Refresh();
        }

        private void LanguageFieldChanged()
        {
            // do nothing, the automatic StateHasChanged() will do.
        }

        private void CultureNameFieldChanged()
        {
            // do nothing, the automatic StateHasChanged() will do.
        }

        private async Task btnApplyAsync()
        {
            _settings.CurrentLanguage = this.SelectedLanguage;
            _settings.CurrentCulture = this.SelectedCulture;
            
            await _settings.SaveSettingsAsync();

            await NavigationManager.PortalNavigateToAsync(NavigationManager.Uri, forceLoad: true);
        }

        private void btnDiscard()
        {
            this.SelectedLanguage = _settings.CurrentLanguage;
            this.SelectedCulture = _settings.CurrentCulture;
        }

        private bool DidLanguageOrCultureChange()
        {
            if (this.SelectedLanguage != _settings.CurrentLanguage)
                return true;

            if (this.SelectedCulture != _settings.CurrentCulture)
                return true;

            return false;

        }


    }
}

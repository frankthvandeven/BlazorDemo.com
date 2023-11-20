using Microsoft.AspNetCore.Components;

namespace Kenova.Client.SystemComponents
{
    public partial class PortalNotificationsPanel : KenovaDialogBase
    {
        //string portalMenuIdentifier;
        //private string SelectedLanguage = "nl";
        //private string SelectedCulture = null;



        protected override void OnDialogInitialized()
        {
            //this.SelectedLanguage = _settings.CurrentLanguage;
            //this.SelectedCulture = _settings.CurrentCulture;

            //portalMenuIdentifier = _settings.PortalMenuMode == PortalMenuMode.Flyout ? "flyout" : "docked";

            //tabItems.Add("general", Loc["portal_tab_general"]);
            //tabItems.Add("region", Loc["portal_tab_region"]);

        }

        private void MenuModeChanged()
        {
            //if (portalMenuIdentifier == "docked")
            //    _settings.PortalMenuMode = PortalMenuMode.Docked;
            //else
            //    _settings.PortalMenuMode = PortalMenuMode.Flyout;

            //_settings.SaveSettings();

            ////this.StateHasChanged();
            //Portal.Refresh();
        }


    }
}

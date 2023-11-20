using Kenova.Client.Components;
using System;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Kenova.Client.Core
{
    public class Settings
    {
        private readonly string[] KENOVA_RESX_LANGUAGES = { "en", "nl" };

        private string _current_language;
        private string _current_culture;

        private ListItemCollection<string> _supportedLanguagesList = new();
        private ListItemCollection<string> _supportedCulturesList = new();
        private PortalMenuItemCollection _portalMenuItems = new();

        internal Settings()
        {
            //_current_language = "nl";
            //_current_culture = "nl-NL";
        }

        /// <summary>
        // Common language names "en" and "nl"
        /// </summary>
        public string CurrentLanguage
        {
            get { return _current_language; }
            set
            {
                _current_language = value;

                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(value);

                string sys_lang = value;

                if (KENOVA_RESX_LANGUAGES.Any(p => p == this._current_language) == false)
                {
                    // fall back to English
                    sys_lang = "en";
                }

                //var ci = new CultureInfo(value);
                // This is for the UI language
                // Blazor WebAssembly does not allow different DefaultThreadCurrentUICulture and DefaultThreadCurrentCulture
                // That is why DefaultThreadCurrentUICulture is commented out for now. See below.
                // CultureInfo.DefaultThreadCurrentUICulture = ci;

                ResourceManager rm = new ResourceManager($"Kenova.Client.Resources.KenovaResources_{sys_lang}", this.GetType().Assembly);

                KenovaClientConfig.Localizer.SetResourceManager(rm);

            }

        }

        /// <summary>
        // List of cultures, date formats etc: https://www.basicdatepicker.com/samples/cultureinfo.aspx
        // Common culture names "en-US", "nl-NL" (date separator -), "nl-BE" (date separator /) and "ms-MY"
        /// </summary>
        public string CurrentCulture
        {
            get { return _current_culture; }
            set
            {
                _current_culture = value;

                // This is for the number and date format etc..
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(value);

                // Bankers rounding:
                //decimal.Round(yourValue, 2, MidpointRounding.AwayFromZero);
                //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"Test date pattern {DateTime.Now.ToShortDateString()}");
                //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"Test decimal separator {(1234567890.34M).ToString("F")}");
            }

        }

        public ListItemCollection<string> SupportedLanguagesList
        {
            get { return _supportedLanguagesList; }
        }

        public ListItemCollection<string> SupportedCulturesList
        {
            get { return _supportedCulturesList; }
        }

        public PortalMenuMode PortalMenuMode = PortalMenuMode.Flyout;
        public bool PortalMenuCollapsed = false;

        public PortalMenuItemCollection PortalMenuItems
        {
            get { return _portalMenuItems; }
        }

        /// <summary>
        /// Load settings from the browser's local storage.
        /// Settings loaded: Settings.CurrentLanguage, Settings.CurrentCulture, Settings.PortalMenuMode and
        /// Settings.PortalMenuCollapsed.
        /// Only call LoadSettings after filling the SupportedLanguages and SupportedCulturesList list.
        /// </summary>
        public async ValueTask LoadSettingsAsync()
        {
            if (KenovaClientConfig.Settings.SupportedLanguagesList.Count == 0 || KenovaClientConfig.Settings.SupportedCulturesList.Count == 0)
                throw new InvalidOperationException("Only call LoadSettings after filling the SupportedLanguages and SupportedCulturesList list");

            // Load the CurrentLanguage
            string lang = await LocalStorage.GetStringAsync("portal_language", null);

            bool found = this._supportedLanguagesList.Any(p => p.Value == lang);

            if (lang == null || !found)
            {
                this.CurrentLanguage = this._supportedLanguagesList[0].Value;
            }
            else
            {
                this.CurrentLanguage = lang;
            }

            // Load the CurrentCulture
            string curcult = await LocalStorage.GetStringAsync("portal_culture", null);

            found = this._supportedCulturesList.Any(p => p.Value == curcult);

            if (curcult == null || !found)
            {
                this.CurrentCulture = this._supportedCulturesList[0].Value;
            }
            else
            {
                this.CurrentCulture = curcult;
            }

            var storage_value = await LocalStorage.GetStringAsync("portal_menu_mode", "docked");

            if (storage_value == "docked")
                PortalMenuMode = PortalMenuMode.Docked;
            else
                PortalMenuMode = PortalMenuMode.Flyout;

            storage_value = await LocalStorage.GetStringAsync("portal_menu_collapsed", "false");

            PortalMenuCollapsed = ( storage_value == "true") ? true : false;
        }

        /// <summary>
        /// Save settings to the browser's local storage.
        /// Settings saved: Settings.CurrentLanguage, Settings.CurrentCulture, Settings.PortalMenuMode and
        /// Settings.PortalMenuCollapsed.
        /// </summary>
        public async ValueTask SaveSettingsAsync()
        {
            await LocalStorage.SetStringAsync("portal_language", CurrentLanguage);
            await LocalStorage.SetStringAsync("portal_culture", CurrentCulture);
            await LocalStorage.SetStringAsync("portal_menu_mode", PortalMenuMode == PortalMenuMode.Flyout ? "flyout" : "docked");
            await LocalStorage.SetStringAsync("portal_menu_collapsed", PortalMenuCollapsed ? "true" : "false");
        }

    }
}

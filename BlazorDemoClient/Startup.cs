using BlazorDemo.Client.Pages;
using BlazorDemo.Shared;
using Kenova.Client;
using Kenova.Client.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorDemo.Client
{
    public class Startup : KenovaStartup
    {

        public override async Task SetupPortalAsync()
        {
            KenovaClientConfig.Labels.HomeButton = "Kenova 17";
            KenovaClientConfig.Labels.TopbarCenterTitle = "Presentation of cross-platform Kenova (net6.0)";
            KenovaClientConfig.Labels.AppName = "Kenova Demo";

            var settings = KenovaClientConfig.Settings;

            settings.SupportedLanguagesList.Add("en", "English");
            settings.SupportedLanguagesList.Add("nl", "Nederlands");

            // List of date formats etc: https://www.basicdatepicker.com/samples/cultureinfo.aspx
            // Common culture names: en-US, nl-NL (date separator -), nl-BE (date separator /), ms-MY
            // Invalid: en-NL

            #region A long list of cultures

            settings.SupportedCulturesList.Add("en-US", "English (United States)");
            settings.SupportedCulturesList.Add("en-GB", "English (United Kingdom)");

            settings.SupportedCulturesList.Add("en-001", "English (World)");
            settings.SupportedCulturesList.Add("en-029", "English (Caribbean)");
            settings.SupportedCulturesList.Add("en-150", "English (Europe)");
            settings.SupportedCulturesList.Add("en-AG", "English (Antigua and Barbuda)");
            settings.SupportedCulturesList.Add("en-AI", "English (Anguilla)");
            settings.SupportedCulturesList.Add("en-AS", "English (American Samoa)");
            settings.SupportedCulturesList.Add("en-AT", "English (Austria)");
            settings.SupportedCulturesList.Add("en-AU", "English (Australia)");
            settings.SupportedCulturesList.Add("en-BB", "English (Barbados)");
            settings.SupportedCulturesList.Add("en-BE", "English (Belgium)");
            settings.SupportedCulturesList.Add("en-BI", "English (Burundi)");
            settings.SupportedCulturesList.Add("en-BM", "English (Bermuda)");
            settings.SupportedCulturesList.Add("en-BS", "English (Bahamas)");
            settings.SupportedCulturesList.Add("en-BW", "English (Botswana)");
            settings.SupportedCulturesList.Add("en-BZ", "English (Belize)");
            settings.SupportedCulturesList.Add("en-CA", "English (Canada)");
            settings.SupportedCulturesList.Add("en-CC", "English (Cocos (Keeling) Islands)");
            settings.SupportedCulturesList.Add("en-CH", "English (Switzerland)");
            settings.SupportedCulturesList.Add("en-CK", "English (Cook Islands)");
            settings.SupportedCulturesList.Add("en-CM", "English (Cameroon)");
            settings.SupportedCulturesList.Add("en-CX", "English (Christmas Island)");
            settings.SupportedCulturesList.Add("en-CY", "English (Cyprus)");
            settings.SupportedCulturesList.Add("en-DE", "English (Germany)");
            settings.SupportedCulturesList.Add("en-DK", "English (Denmark)");
            settings.SupportedCulturesList.Add("en-DM", "English (Dominica)");
            settings.SupportedCulturesList.Add("en-ER", "English (Eritrea)");
            settings.SupportedCulturesList.Add("en-FI", "English (Finland)");
            settings.SupportedCulturesList.Add("en-FJ", "English (Fiji)");
            settings.SupportedCulturesList.Add("en-FK", "English (Falkland Islands)");
            settings.SupportedCulturesList.Add("en-FM", "English (Micronesia)");
            settings.SupportedCulturesList.Add("en-GD", "English (Grenada)");
            settings.SupportedCulturesList.Add("en-GG", "English (Guernsey)");
            settings.SupportedCulturesList.Add("en-GH", "English (Ghana)");
            settings.SupportedCulturesList.Add("en-GI", "English (Gibraltar)");
            settings.SupportedCulturesList.Add("en-GM", "English (Gambia)");
            settings.SupportedCulturesList.Add("en-GU", "English (Guam)");
            settings.SupportedCulturesList.Add("en-GY", "English (Guyana)");
            settings.SupportedCulturesList.Add("en-HK", "English (Hong Kong SAR)");
            settings.SupportedCulturesList.Add("en-ID", "English (Indonesia)");
            settings.SupportedCulturesList.Add("en-IE", "English (Ireland)");
            settings.SupportedCulturesList.Add("en-IL", "English (Israel)");
            settings.SupportedCulturesList.Add("en-IM", "English (Isle of Man)");
            settings.SupportedCulturesList.Add("en-IN", "English (India)");
            settings.SupportedCulturesList.Add("en-IO", "English (British Indian Ocean Territory)");
            settings.SupportedCulturesList.Add("en-JE", "English (Jersey)");
            settings.SupportedCulturesList.Add("en-JM", "English (Jamaica)");
            settings.SupportedCulturesList.Add("en-KE", "English (Kenya)");
            settings.SupportedCulturesList.Add("en-KI", "English (Kiribati)");
            settings.SupportedCulturesList.Add("en-KN", "English (Saint Kitts and Nevis)");
            settings.SupportedCulturesList.Add("en-KY", "English (Cayman Islands)");
            settings.SupportedCulturesList.Add("en-LC", "English (Saint Lucia)");
            settings.SupportedCulturesList.Add("en-LR", "English (Liberia)");
            settings.SupportedCulturesList.Add("en-LS", "English (Lesotho)");
            settings.SupportedCulturesList.Add("en-MG", "English (Madagascar)");
            settings.SupportedCulturesList.Add("en-MH", "English (Marshall Islands)");
            settings.SupportedCulturesList.Add("en-MO", "English (Macao SAR)");
            settings.SupportedCulturesList.Add("en-MP", "English (Northern Mariana Islands)");
            settings.SupportedCulturesList.Add("en-MS", "English (Montserrat)");
            settings.SupportedCulturesList.Add("en-MT", "English (Malta)");
            settings.SupportedCulturesList.Add("en-MU", "English (Mauritius)");
            settings.SupportedCulturesList.Add("en-MW", "English (Malawi)");
            settings.SupportedCulturesList.Add("en-MY", "English (Malaysia)");
            settings.SupportedCulturesList.Add("en-NA", "English (Namibia)");
            settings.SupportedCulturesList.Add("en-NF", "English (Norfolk Island)");
            settings.SupportedCulturesList.Add("en-NG", "English (Nigeria)");
            settings.SupportedCulturesList.Add("en-NL", "English (Netherlands)");
            settings.SupportedCulturesList.Add("en-NR", "English (Nauru)");
            settings.SupportedCulturesList.Add("en-NU", "English (Niue)");
            settings.SupportedCulturesList.Add("en-NZ", "English (New Zealand)");
            settings.SupportedCulturesList.Add("en-PG", "English (Papua New Guinea)");
            settings.SupportedCulturesList.Add("en-PH", "English (Philippines)");
            settings.SupportedCulturesList.Add("en-PK", "English (Pakistan)");
            settings.SupportedCulturesList.Add("en-PN", "English (Pitcairn Islands)");
            settings.SupportedCulturesList.Add("en-PR", "English (Puerto Rico)");
            settings.SupportedCulturesList.Add("en-PW", "English (Palau)");
            settings.SupportedCulturesList.Add("en-RW", "English (Rwanda)");
            settings.SupportedCulturesList.Add("en-SB", "English (Solomon Islands)");
            settings.SupportedCulturesList.Add("en-SC", "English (Seychelles)");
            settings.SupportedCulturesList.Add("en-SD", "English (Sudan)");
            settings.SupportedCulturesList.Add("en-SE", "English (Sweden)");
            settings.SupportedCulturesList.Add("en-SG", "English (Singapore)");
            settings.SupportedCulturesList.Add("en-SH", "English (St Helena, Ascension, Tristan da Cunha)");
            settings.SupportedCulturesList.Add("en-SI", "English (Slovenia)");
            settings.SupportedCulturesList.Add("en-SL", "English (Sierra Leone)");
            settings.SupportedCulturesList.Add("en-SS", "English (South Sudan)");
            settings.SupportedCulturesList.Add("en-SX", "English (Sint Maarten)");
            settings.SupportedCulturesList.Add("en-SZ", "English (Swaziland)");
            settings.SupportedCulturesList.Add("en-TC", "English (Turks and Caicos Islands)");
            settings.SupportedCulturesList.Add("en-TK", "English (Tokelau)");
            settings.SupportedCulturesList.Add("en-TO", "English (Tonga)");
            settings.SupportedCulturesList.Add("en-TT", "English (Trinidad and Tobago)");
            settings.SupportedCulturesList.Add("en-TV", "English (Tuvalu)");
            settings.SupportedCulturesList.Add("en-TZ", "English (Tanzania)");
            settings.SupportedCulturesList.Add("en-UG", "English (Uganda)");
            settings.SupportedCulturesList.Add("en-UM", "English (U.S. Outlying Islands)");
            settings.SupportedCulturesList.Add("en-VC", "English (Saint Vincent and the Grenadines)");
            settings.SupportedCulturesList.Add("en-VG", "English (British Virgin Islands)");
            settings.SupportedCulturesList.Add("en-VI", "English (U.S. Virgin Islands)");
            settings.SupportedCulturesList.Add("en-VU", "English (Vanuatu)");
            settings.SupportedCulturesList.Add("en-WS", "English (Samoa)");
            settings.SupportedCulturesList.Add("en-ZA", "English (South Africa)");
            settings.SupportedCulturesList.Add("en-ZM", "English (Zambia)");
            settings.SupportedCulturesList.Add("en-ZW", "English (Zimbabwe)");

            settings.SupportedCulturesList.Add("nl-NL", "Nederlands (Nederland)");
            settings.SupportedCulturesList.Add("nl-BE", "Nederlands (België)");
            settings.SupportedCulturesList.Add("nl-AW", "Nederlands (Aruba)");
            settings.SupportedCulturesList.Add("nl-BQ", "Nederlands (Bonaire, Sint Eustatius en Saba)");
            settings.SupportedCulturesList.Add("nl-CW", "Nederlands (Curaçao)");
            settings.SupportedCulturesList.Add("nl-SX", "Nederlands (Sint Maarten)");
            settings.SupportedCulturesList.Add("nl-SR", "Nederlands (Suriname)");

            #endregion

            await settings.LoadSettingsAsync();

            #region A list of portal menu items

            //settings.PortalMenuItems.Add("Add", null, IconKind.Vector);
            //settings.PortalMenuItems.IconData = "<svg viewBox=\"0 0 448 512\"><path d=\"M400 64c8.8 0 16 7.2 16 16v352c0 8.8-7.2 16-16 16H48c-8.8 0-16-7.2-16-16V80c0-8.8 7.2-16 16-16h352m0-32H48C21.5 32 0 53.5 0 80v352c0 26.5 21.5 48 48 48h352c26.5 0 48-21.5 48-48V80c0-26.5-21.5-48-48-48zm-60 206h-98v-98c0-6.6-5.4-12-12-12h-12c-6.6 0-12 5.4-12 12v98h-98c-6.6 0-12 5.4-12 12v12c0 6.6 5.4 12 12 12h98v98c0 6.6 5.4 12 12 12h12c6.6 0 12-5.4 12-12v-98h98c6.6 0 12-5.4 12-12v-12c0-6.6-5.4-12-12-12z\"/></svg>";
            //settings.PortalMenuItems.Add("Add", false, IconKind.Vector);
            //settings.PortalMenuItems.IconData = "<svg viewBox=\"0 0 448 512\"><path d=\"M400 64c8.8 0 16 7.2 16 16v352c0 8.8-7.2 16-16 16H48c-8.8 0-16-7.2-16-16V80c0-8.8 7.2-16 16-16h352m0-32H48C21.5 32 0 53.5 0 80v352c0 26.5 21.5 48 48 48h352c26.5 0 48-21.5 48-48V80c0-26.5-21.5-48-48-48zm-60 206h-98v-98c0-6.6-5.4-12-12-12h-12c-6.6 0-12 5.4-12 12v98h-98c-6.6 0-12 5.4-12 12v12c0 6.6 5.4 12 12 12h98v98c0 6.6 5.4 12 12 12h12c6.6 0 12-5.4 12-12v-98h98c6.6 0 12-5.4 12-12v-12c0-6.6-5.4-12-12-12z\"/></svg>";

            settings.PortalMenuItems.Add(KenovaClientConfig.Localizer["portal_home_caption"], "/", null, IconKind.FontAwesome, "fal fa-home");

            settings.PortalMenuItems.Add("All Kenova Controls", "/allcontrols", null, IconKind.FontAwesome, "fal fa-list-alt");
            settings.PortalMenuItems.Icon.HtmlColor = "darkorange";

            settings.PortalMenuItems.Add("HyperGrid Samples", "/hypergridsamples", null, IconKind.FontAwesome, "fal fa-th");
            settings.PortalMenuItems.Icon.HtmlColor = "red";

            settings.PortalMenuItems.Add("Treeview", "/treeview", null, IconKind.FontAwesome, "fal fa-folder-tree");
            settings.PortalMenuItems.Icon.HtmlColor = "forestgreen";

            settings.PortalMenuItems.Add("Bike Stores", "/bikestores", null, IconKind.FontAwesome, "fal fa-bicycle");
            settings.PortalMenuItems.Icon.HtmlColor = "rebeccapurple";

            // caption, url, enabled
            //

            settings.PortalMenuItems.Add("Customers", "/customers", null, IconKind.FontAwesome, "fal fa-user-friends");
            settings.PortalMenuItems.Add("Orders", "/orders", null, IconKind.FontAwesome, "fal fa-money-check-edit-alt");
            settings.PortalMenuItems.Add("Products", "/products", null, IconKind.FontAwesome, "fal fa-box");

            settings.PortalMenuItems.Add("WorkflowTest", "/workflowtest", null, IconKind.FontAwesome, "fal fa-bezier-curve");
            settings.PortalMenuItems.Icon.HtmlColor = "#15aabf";

            settings.PortalMenuItems.Add("Push Messages", "/pushtest", null, IconKind.FontAwesome, "fal fa-comments");
            settings.PortalMenuItems.Icon.HtmlColor = "darkblue";

            settings.PortalMenuItems.Add("WindowedTest", typeof(BlazorDemo.Client.Components.SearchCustomers), null, IconKind.FontAwesome, "fal fa-window");
            settings.PortalMenuItems.Icon.HtmlColor = "DarkGray";

            //settings.PortalMenuItems.Add("Pharmacy Demo", "/pharmacy", null, IconKind.FontAwesome, "fas fa-file-prescription");
            //settings.PortalMenuItems.Icon.HtmlColor = "forestgreen";

            #endregion

        }

        public override async Task RefreshTokenAsync()
        {
            var token = await KenovaClientConfig.AuthenticationStateProvider.GetSavedTokenAsync();

            if (string.IsNullOrEmpty(token))
                return;

            var refresh = new RefreshTokenRequest();

            refresh.Token = token;

            var loginFailed = true;

            try
            {
                LoginResult result = await KenovaHttp.PostJsonAsync<LoginResult>("api/user/refreshtoken", refresh);

                loginFailed = result.Token == null;

                if (loginFailed)
                    return;

                // Success! Store token in underlying auth state service
                await KenovaClientConfig.AuthenticationStateProvider.AuthenticateWithNewTokenAsync(result.Token);

                Console.WriteLine("SETTING DISPLAYNAME TO " + result.DisplayName);

                KenovaClientConfig.Labels.UserName = result.DisplayName;
                KenovaClientConfig.Labels.DirectoryInfo = "Database: BikeStores";

            }
            catch (HttpRequestException)
            { }

        }

    }
}

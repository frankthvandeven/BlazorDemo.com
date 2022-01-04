using Kenova.Client;
using Kenova.Client.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorDemo.Client
{
    public static class ViewSourceCode
    {
        private static string _client_base = "https://github.com/frankthvandeven/BlazorDemo.com/tree/master/BlazorDemoClient/";

        public static void SourceCodeButton(this ToolbarItemCollection toolbar, string url_part)
        {
            toolbar.Add("Source Code", async () => await ViewSourceCode.OpenAsync(url_part), null, IconKind.FontAwesome, "fab fa-github");
        }


        /// <summary>
        /// For example Open("BikeStores/SearchCustomers");
        /// </summary>
        public static ValueTask OpenAsync(string url_part)
        {
            string url = _client_base + url_part;

            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("open", url, "_blank");

        }

    }
}

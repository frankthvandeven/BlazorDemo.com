using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.JSInterop;

namespace BlazorDemo.Client
{
    public static class ViewSourceCode
    {
        private static string _client_base = "https://github.com/frankthvandeven/BlazorDemo.com/tree/master/BlazorDemoClient/";

        public static void SourceCodeButton(this ToolbarItemCollection toolbar, string url_part)
        {
            toolbar.Add("Source Code", () => ViewSourceCode.Open(url_part), null, IconKind.FontAwesome, "fab fa-github");
        }


        /// <summary>
        /// For example Open("BikeStores/SearchCustomers");
        /// </summary>
        public static void Open(string url_part)
        {
            string url = _client_base + url_part;

            KenovaClientConfig.JSInProcessRuntime.InvokeVoid("open", url, "_blank");

        }

    }
}




//NavigationManager.NavigateTo(url, false);//opens the new page on same browser tab

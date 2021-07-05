using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            toolbar.Add("Source Code", () => ViewSourceCode.OpenClient(url_part), null, IconKind.FontAwesome, "fab fa-github");
        }

        //BikeStores/SearchCustomers
        public static void OpenClient(string url_part)
        {
            string url = _client_base + url_part;

            //NavigationManager.NavigateTo(url, false);//opens the new page on same browser tab

            string[] values = { url, "_blank" };
            CancellationToken token = new CancellationToken(false);
            
            //_ = KenovaClientConfig.JSRuntime.InvokeAsync<object>("open", token, values);

            KenovaClientConfig.JSInProcessRuntime.InvokeVoid("open", values);


        }

    }
}

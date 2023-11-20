using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

// https://blazorwheelzoom.m4f.eu/

namespace Kenova.Client.Components
{
    // https://ourcodeworld.com/articles/read/491/how-to-retrieve-images-from-the-clipboard-with-javascript-in-the-browser

    public partial class ImageCanvas : KenovaComponentBase
    {
        private string canvas_id = KenovaClientConfig.GetUniqueElementID();


        private void FocusCallback()
        {
            var runtime = (IJSInProcessRuntime)KenovaClientConfig.JSRuntime;
            runtime.InvokeVoid("HookupImageCanvas", new object[] { canvas_id });
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("Hooking up canvas");
        }

        private void UnfocusCallback()
        {

            var runtime = (IJSInProcessRuntime)KenovaClientConfig.JSRuntime;
            //runtime.InvokeVoid("UnhookImageCanvas", new object[] { canvas_id });

            if (KenovaClientConfig.Diagnostics) Console.WriteLine("Unhooked canvas");

        }


    }
}

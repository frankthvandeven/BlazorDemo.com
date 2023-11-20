using Microsoft.JSInterop;
using System;

namespace Kenova.Client.Components
{
    public static class Clipboard
    {

        public static ValueTask CopyTextToClipboardAsync(string text)
        {
            if (text is null)
                throw new ArgumentNullException("text");

            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNCopyTextToClipboard", text);

        }

        public static ValueTask CopyElementTextToClipboardAsync(string elementName)
        {
            if (elementName is null)
                throw new ArgumentNullException("elementName");
            
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNCopyElementTextToClipboard", elementName);

        }

    }
}

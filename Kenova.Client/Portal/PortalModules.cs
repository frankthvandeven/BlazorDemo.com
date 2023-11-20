#if gshghgfgfd

using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Kenova.Client.Core;

public class PortalModules
{
    //public IJSObjectReference PortalRoot = await KenovaClientConfig.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Kenova.Client/portalroot.interop.js");

    /// <summary>
    /// Modules must be imported asynchronously, or else it doesn't work.
    /// </summary>
    internal async ValueTask LoadModules()
    {
        PortalRoot = await KenovaClientConfig.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Kenova.Client/portalroot.interop.js");
    }

    internal async ValueTask UnloadModules()
    {
        await PortalRoot.DisposeAsync();
    }

}

#endif
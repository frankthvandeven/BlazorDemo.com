using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Kenova.Client.Util
{
    /// <summary>
    /// Cast IJSRuntime to IJSInProcessRuntime for non-async Invoke.
    /// </summary>
    public static class JavaScriptCaller
    {
        //internal static void KNTaskrunnerDisplayFailure(string caption, string message)
        //{
        //    var runtime = (IJSInProcessRuntime)KenovaClientConfig.JSRuntime;
        //    runtime.InvokeVoid("KNTaskrunnerDisplayFailure", caption, message);
        //}

        internal static ValueTask KNShowTaskrunnerAsync(string caption)
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNShowTaskrunner", caption);
        }

        internal static ValueTask KNHideTaskrunnerAsync()
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNHideTaskrunner");
        }

        internal static ValueTask KNSaveCurrentFocusAsync()
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNSaveCurrentFocus");
        }

        internal static ValueTask KNAddLayerAsync(bool is_dropdown, string dropdown_id = null, string overlay_id = null)
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNAddLayer", is_dropdown, dropdown_id, overlay_id);
        }

        internal static ValueTask KNRemoveLayerAsync()
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNRemoveLayer");
        }

        internal static ValueTask KNTriggerIgnoreScrollResizeEventsAsync()
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNTriggerIgnoreScrollResizeEvents");
        }

        public static ValueTask<BoundingClientRect> KNGetBoundingClientRectByPosAsync(double ClientX, double ClientY)
        {
            return KenovaClientConfig.JSRuntime.InvokeAsync<BoundingClientRect>("KNGetBoundingClientRectByPos", ClientX, ClientY);
        }

        public static ValueTask<BoundingClientRect> KNGetBoundingClientRectByIdAsync(string element_id)
        {
            return KenovaClientConfig.JSRuntime.InvokeAsync<BoundingClientRect>("KNGetBoundingClientRectById", element_id);
        }

        public static ValueTask<double> KNGetWindowInnerHeightAsync()
        {
            return KenovaClientConfig.JSRuntime.InvokeAsync<double>("KNGetWindowInnerHeight");
        }

        public static ValueTask<double> KNGetWindowInnerWidthAsync()
        {
            return KenovaClientConfig.JSRuntime.InvokeAsync<double>("KNGetWindowInnerWidth");
        }

        public static ValueTask<bool> KNElementHiddenAsync(string element_id)
        {
            return KenovaClientConfig.JSRuntime.InvokeAsync<bool>("KNElementHidden", element_id);
        }

        public static ValueTask<bool> KNIsChildOfAsync(string parent_id, string child_id)
        {
            return KenovaClientConfig.JSRuntime.InvokeAsync<bool>("KNIsChildOf", parent_id, child_id);
        }

        public static ValueTask KNSelectAllAsync(string element_id)
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNSelectAll", element_id);
        }

        public static async ValueTask KNFocusAsync(string element_id, bool select_all = false)
        {
            // https://github.com/dotnet/aspnetcore/issues/30070#issuecomment-823938686

            await Task.Yield();

            await KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNFocus", element_id, select_all);
        }

        public static ValueTask KNUnfocusAsync()
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNUnfocus");
        }

        /// <summary>
        /// If a child of the element_id has focus, then unfocus it.
        /// </summary>
        public static ValueTask KNChildUnfocusAsync(string element_id)
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNChildUnfocus", element_id);
        }

        public static ValueTask<string> KNGetCssVariableAsync(string var_name)
        {
            return KenovaClientConfig.JSRuntime.InvokeAsync<string>("KNGetCssVariable", var_name);
        }

        public static ValueTask KNSetCssVariableAsync(string var_name, string value)
        {
            return KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNSetCssVariable", var_name, value);
        }

    }
}




# if xxvsdf
        public static async Task<BoundingClientRect> KNGetBoundingClientRectByPosAsync(double ClientX, double ClientY)
        {
            return KenovaClientConfig.JSRuntime.InvokeAsync<BoundingClientRect>("KNGetBoundingClientRectByPos", ClientX, ClientY);
        }
#endif

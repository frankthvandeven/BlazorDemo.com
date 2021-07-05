using Microsoft.AspNetCore.Components;
using Kenova.WebAssembly.Client.Components;
using System.Threading.Tasks;
using Kenova.WebAssembly.Client;

namespace BlazorDemo.Client.Pages
{
    public partial class TreeViewDemo : LayerComponentBase
    {
        public TreeViewDemoModel Model = new();

        protected override async Task OnLayerInitializedAsync()
        {
            Breadcrumb = "TreeView";
            ProgressCaption = "Loading customers from Azure Amsterdam";

            await Model.LoadTreeviewTask();

        }



    }
}


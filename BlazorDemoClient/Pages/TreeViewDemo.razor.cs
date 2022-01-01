using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Pages
{
    public partial class TreeViewDemo : KenovaDialogBase
    {
        public TreeViewDemoModel Model = new();

        protected override async Task OnDialogInitializedAsync()
        {
            Breadcrumb = "TreeView";
            ProgressCaption = "Loading customers from Azure Amsterdam";

            await Model.LoadTreeviewTask();

        }



    }
}


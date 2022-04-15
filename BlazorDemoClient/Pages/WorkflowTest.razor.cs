using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Pages
{
    public partial class WorkflowTest : KenovaDialogBase
    {
        private Workflow WorkflowComponent;

        protected override void OnDialogInitialized()
        {
            Breadcrumb = "WorkflowTest";

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if( firstRender)
            {
                WorkflowData data = new WorkflowData();

                data.items.Add(new WorkflowItem { x = 10, y = 10 });
                data.items.Add(new WorkflowItem { x = 100, y = 10 });
                data.items.Add(new WorkflowItem { x = 200, y = 10 });

                await WorkflowComponent.SetWorkflowData(data);

            }


        }


    }

}



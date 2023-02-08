using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflows.Client.Components;

namespace BlazorDemo.Client.Pages
{
    public partial class WorkflowTest : KenovaDialogBase
    {
        private WorkflowComponent WorkflowComponent;

        protected override void OnDialogInitialized()
        {
            Breadcrumb = "WorkflowTest";

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if( firstRender)
            {
                WorkflowData data = new WorkflowData();
                WorkflowItem item = null;

                item = new WorkflowItem();
                item.id = "aap";
                item.x = 10;
                item.y = 10;
                item.inConnectors.Add(new WorkflowConnector());
                item.inConnectors.Add(new WorkflowConnector());
                item.inConnectors.Add(new WorkflowConnector());
                item.outConnectors.Add(new WorkflowConnector { connected = true, connectedTo = "mies", connectedIndex = 0 });
                item.outConnectors.Add(new WorkflowConnector());
                item.outConnectors.Add(new WorkflowConnector());
                data.items.Add(item);

                item = new WorkflowItem();
                item.id = "noot";
                item.x = 100;
                item.y = 10;
                item.inConnectors.Add(new WorkflowConnector());
                item.inConnectors.Add(new WorkflowConnector());
                item.outConnectors.Add(new WorkflowConnector());
                item.outConnectors.Add(new WorkflowConnector());
                data.items.Add(item);

                item = new WorkflowItem();
                item.id = "mies";
                item.x = 200;
                item.y = 10;
                item.inConnectors.Add(new WorkflowConnector());
                item.outConnectors.Add(new WorkflowConnector());
                data.items.Add(item);

                await WorkflowComponent.SetWorkflowData(data);

            }


        }


    }

}



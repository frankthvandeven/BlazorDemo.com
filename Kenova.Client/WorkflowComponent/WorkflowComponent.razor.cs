using Microsoft.AspNetCore.Components;
using Kenova.Client.Util;
using Kenova.Client;

namespace Workflows.Client.Components;

public partial class WorkflowComponent : KenovaComponentBase, IAsyncDisposable
{
    [CascadingParameter]
    public KenovaDialogBase LayerComponent { get; set; }

    private readonly string _container_id = KenovaClientConfig.GetUniqueElementID();
    private readonly string _canvas_id = KenovaClientConfig.GetUniqueElementID();
    
    private ComponentWingman<WorkflowComponent> _wingman = new();

    protected override void OnInitialized()
    {
        //LayerComponent?.RegisterComponent(this);

    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

        try
        {
            await this.Mutex.WaitAsync();

            if (firstRender)
            {
                await _wingman.InstantiateAsync(this, "WorkflowComponent");

                await _wingman.InvokeVoidAsync("Start", _container_id, _canvas_id);
            }

            //await _wingman.InvokeVoidAsync("OnAfterRender");

            if (firstRender)
            {
                //LayerComponent?.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
            }

        }
        finally
        {
            this.Mutex.Release();
        }
    }
    public async ValueTask DisposeAsync() //async ValueTask IAsyncDisposable.DisposeAsync()
    {
        try
        {
            await this.Mutex.WaitAsync();

            await _wingman.InvokeVoidAsync("Stop");

            await _wingman.DisposeAsync();

            //LayerComponent?.UnregisterComponent(this);
        }
        finally
        {
            this.Mutex.Release();
        }

    }

    // https://swimburger.net/blog/dotnet/communicating-between-dotnet-and-javascript-in-blazor-with-in-browser-samples

    public async Task SetWorkflowData(WorkflowData data)
    {
        if( data == null)
            throw new ArgumentNullException("data");

        //KeyValuePair<string,string> kvp = new KeyValuePair<string, string>("data", "fggffgdfdg");

        await _wingman.InvokeVoidAsync("SetWorkflowData", data);


    }


}

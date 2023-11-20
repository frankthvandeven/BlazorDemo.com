using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kenova.Client.Components;

/*
 * Parameters and ComponentBase events.
 */

public partial class HyperGrid<ItemType> : KenovaComponentBase, IRerender, IAsyncDisposable
{

    #region Parameters

    [Parameter, EditorRequired]
    public string DebugName { get; set; }

    [Parameter, EditorRequired]
    public HyperData<ItemType> Data { get; set; } = null;

    [Parameter]
    public string AdditionalStyle { get; set; } = null;

    /// <summary>
    /// The default value for this parameter is HeightMode.Container
    /// </summary>
    [Parameter]
    public HeightMode HeightMode { get; set; } = HeightMode.Container;

    [Parameter]
    public double Height { get; set; } = -1;

    [Parameter]
    public double MaxHeight { get; set; } = -1;

    [Parameter]
    public double Width { get; set; } = -1;

    [Parameter]
    public bool DisplayFilterAtBottom { get; set; } = false;

    /// <summary>
    /// Change the padding 8px/8px/8px to 20px/10px/20px (left/middle/right)
    /// </summary>
    [Parameter]
    public bool WidePadding { get; set; } = false;

    [Parameter]
    public EventCallback LoadmoreButtonCallback { get; set; }

    [Parameter]
    public string InputElementId { get; set; } = KenovaClientConfig.GetUniqueElementID();

    [Parameter]
    public EventCallback<HyperlinkEventArgs<ItemType>> HyperlinkClicked { get; set; }

    [Parameter]
    public EventCallback<ItemType> RowDoubleClicked { get; set; }

    #endregion

    //private const string JS_URL = "./_content/Kenova.Client/hypergrid.interop.js";

    private ComponentWingman<HyperGrid<ItemType>> _wingman = new();

    private readonly string _id_base;

    private readonly string _container_id = KenovaClientConfig.GetUniqueElementID();
    private readonly string _header_div_id = KenovaClientConfig.GetUniqueElementID();
    private readonly string _table_div_id = KenovaClientConfig.GetUniqueElementID();

    public HyperGrid()
    {
        _id_base = KenovaClientConfig.GetUniqueElementID() + "-";
    }


    protected override void OnInitialized()
    {
        if (KenovaClientConfig.Diagnostics) Console.WriteLine($"📈 HyperGrid ({this.DebugName}) - OnInitialized");

        LayerComponent?.RegisterComponent(this);

        if (this.Data == null)
        {
            throw new InvalidOperationException("Parameter Data is not set.");
        }

        if (this.Data.Items == null)
        {
            throw new InvalidOperationException("The Items property of the HyperData object in the Data parameter is not set.");
        }

        this.Data.DataChanged += HyperData_DataChanged;

        this.Data.RecalculateCheckedItemsCount();
    }

    private async void HyperData_DataChanged(HyperDataEventArgs<ItemType> e)
    {
        if (e.Action == HyperDataEventAction.Reset)
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("📈 HyperGrid - Received event from HyperData: Reset");

            await this.ScrollToTopAsync();
            await this.ScrollToLeftAsync();
            StateHasChanged();
        }
        if (e.Action == HyperDataEventAction.Add)
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("📈 HyperGrid - Received event from HyperData: Add");

            //this.ScrollToTop();
            await this.ScrollToLeftAsync();
            //if (KenovaClientConfig.Diagnostics) Console.WriteLine("📈 HyperGrid - StateHasChanged");
            StateHasChanged();

            //ItempType last = this.this.Data.DisplayItems[this.this.Data.DisplayItems.Count];

            int last = this.Data.DisplayItems.Count - 1;

            await this.MakeLastItemInViewAsync(last);

            //if (KenovaClientConfig.Diagnostics) Console.WriteLine("📈 HyperGrid - StateHasChanged");
            StateHasChanged();

        }

    }

    //private bool _disposed;

    public async ValueTask DisposeAsync() //async ValueTask IAsyncDisposable.DisposeAsync()
    {
        try
        {
            await this.Mutex.WaitAsync();

            //_disposed = true;

            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"💀💀💀 📈 HyperGrid ({this.DebugName}) - DisposeAsync");

            this.Data.DataChanged -= HyperData_DataChanged;

            if (_scroll_sync_active)
            {
                await _wingman.InvokeVoidAsync("KNStopScrollSync");
                _scroll_sync_active = false;
            }

            await _wingman.InvokeVoidAsync("Stop");

            await _wingman.DisposeAsync();

            LayerComponent?.UnregisterComponent(this);
        }
        finally
        {
            this.Mutex.Release();
        }

    }

    //protected override Task OnParametersSetAsync()
    //{
    //    //if (KenovaClientConfig.Diagnostics) Console.WriteLine("📈 HyperGrid - OnParametersSetAsync");

    //    return Task.CompletedTask;
    //}

    public void Rerender()
    {
        if (KenovaClientConfig.Diagnostics) Console.WriteLine($"📈 HyperGrid ({this.DebugName}) - Rerender() method");

        this.StateHasChanged();
    }

    private async Task FilterTextChanged()
    {
        //if (KenovaClientConfig.Diagnostics) Console.WriteLine("📈 HyperGrid - FilterTextChanged() method");

        await this.InvokeAsync(StateHasChanged);
    }

    // Event handling in Blazor: https://docs.microsoft.com/en-us/aspnet/core/blazor/event-handling?view=aspnetcore-3.1

    [JSInvokable]
    public void OnSpacePressed(int row_index)
    {
        if (KenovaClientConfig.Diagnostics) Console.WriteLine("Space pressed");

        ItemType item = this.Data.DisplayItems[row_index];

        if (!isRowEnabled(item))
            return;

        if (this.Data.UseMultiCheck == MultiCheck.Off)
        {
            if (this.Data.SelectedItemExpression != null)
            {
                this.Data.SelectedItem = item;
            }
            else
            {
                _ = this.RowDoubleClicked.InvokeAsync(item);
            }
        }
        else
        {
            if (this.Data.SelectedItemExpression != null)
            {
                this.Data.ToggleCheck(item);
            }
            else
            {
                this.Data.ToggleCheck(item);
            }

        }

        this.StateHasChanged();
    }

    [JSInvokable]
    public void OnEnterPressed(int row_index)
    {
        if (KenovaClientConfig.Diagnostics) Console.WriteLine("Enter pressed");

        ItemType item = this.Data.DisplayItems[row_index];

        if (!isRowEnabled(item))
            return;

        if (this.Data.UseMultiCheck == MultiCheck.Off)
        {
            if (this.Data.SelectedItemExpression != null)
            {
                this.Data.SelectedItem = item;
                _ = this.RowDoubleClicked.InvokeAsync(item);
            }
            else
            {
                _ = this.RowDoubleClicked.InvokeAsync(item);
            }
        }
        else
        {
            if (this.Data.SelectedItemExpression != null)
            {
                _ = this.RowDoubleClicked.InvokeAsync(item);
            }
            else
            {
                _ = this.RowDoubleClicked.InvokeAsync(item);
            }

        }

        this.StateHasChanged();
    }

    [JSInvokable]
    public void OnSelectPressed(int row_index)
    {
        if (KenovaClientConfig.Diagnostics) Console.WriteLine("Select pressed");

        ItemType item = this.Data.DisplayItems[row_index];

        if (!isRowEnabled(item))
            return;

        if (this.Data.SelectedItemExpression == null)
            return;

        this.Data.SelectedItem = item;

        this.StateHasChanged();
    }

    [JSInvokable]
    public void SelectAllPressed()
    {
        if (this.Data.CheckedItemsCount == this.Data.DisplayItems.Count)
        {
            this.Data.UncheckAll();
        }
        else
        {
            this.Data.CheckAll();
        }

        this.StateHasChanged();
    }

    private bool isRowEnabled(ItemType item)
    {
        if (this.Data.RowEnabledExpression == null)
            return true;

        return this.Data.RowEnabledExpression(item);
    }

    /// <summary>
    /// Captures the DOM's row-onclick event.
    /// </summary>
    private void OnRowClick(MouseEventArgs e, ItemType item)
    {
        if (KenovaClientConfig.Diagnostics) Console.WriteLine("Row clicked");

        if (!isRowEnabled(item))
            return;

        if (this.Data.UseMultiCheck == MultiCheck.Off)
        {
            if (this.Data.SelectedItemExpression != null)
            {
                this.Data.SelectedItem = item;
            }
            else
            {
                _ = this.RowDoubleClicked.InvokeAsync(item);
            }
        }
        else
        {
            if (this.Data.SelectedItemExpression != null)
            {
                this.Data.SelectedItem = item;
            }
            else
            {
                this.OnOffBoxClicked(item);
            }

        }

    }

    /// <summary>
    /// Captures the DOM's row-ondblclick event.
    /// </summary>
    private void OnRowDoubleClick(MouseEventArgs e, ItemType item)
    {
        if (KenovaClientConfig.Diagnostics) Console.WriteLine("Row double clicked");

        if (!isRowEnabled(item))
            return;

        if (this.Data.UseMultiCheck == MultiCheck.Off)
        {
            if (this.Data.SelectedItemExpression != null)
            {
                _ = this.RowDoubleClicked.InvokeAsync(item);
            }
            else
            {
                // do nothing
            }
        }
        else
        {
            if (this.Data.SelectedItemExpression != null)
            {
                _ = this.RowDoubleClicked.InvokeAsync(item);
            }
            else
            {
                // do nothing
            }
        }

    }


    private async void BtnLoadmoreClick()
    {
        await LoadmoreButtonCallback.InvokeAsync();

        this.StateHasChanged();
    }

    private Task BtnExportClick()
    {
        return LongRunningTask.SimpleRun(Loc["exporting"], this.Data.ExportToExcelAsync);
    }

    private async Task OnAnchorClick(MouseEventArgs e, ItemType item, string fieldName, string elementId)
    {
        await HyperlinkClicked.InvokeAsync(new HyperlinkEventArgs<ItemType>(item, fieldName, elementId));
    }

    private bool _scroll_sync_active = false;

    private bool _initial_actions_done = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

        try
        {
            await this.Mutex.WaitAsync();

            if (firstRender)
            {
                await _wingman.InstantiateAsync(this,"HyperGridComponent");

                bool virtualize = this.Data.Mode == DisplayMode.Virtualization;

                await _wingman.InvokeVoidAsync("Start", virtualize, _container_id, _table_div_id, this.InputElementId);
            }

            // The scrollsync support must be moved to JS completely.

            if (_didrender_header && _didrender_table)
            {
                if (_scroll_sync_active == false)
                {
                    await _wingman.InvokeVoidAsync("KNStartScrollSync", _header_div_id);
                    _scroll_sync_active = true;
                }
            }
            else if (_scroll_sync_active == true)
            {
                await _wingman.InvokeVoidAsync("KNStopScrollSync");
                _scroll_sync_active = false;
            }

            await _wingman.InvokeVoidAsync("OnAfterRender");

            if (_initial_actions_done == false)
            {
                _initial_actions_done = true;

                if (this.Data.InitialScrollTo == InitialScrollTo.SelectedItem)
                {
                    await this.ScrollToSelectedItemAsync();
                }
                else if (this.Data.InitialScrollTo == InitialScrollTo.Bottom)
                {
                    await this.ScrollToBottomAsync();
                }
            }


            if (firstRender)
            {
                LayerComponent?.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
            }

        }
        finally
        {
            this.Mutex.Release();
        }
    }

}


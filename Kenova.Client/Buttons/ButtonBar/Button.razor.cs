using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Kenova.Client.Components;

public sealed partial class Button : KenovaComponentBase, IDisposable, IRerender, IKenovaComponent
{
    public readonly string ContainerID = KenovaClientConfig.GetUniqueElementID();

    [CascadingParameter]
    public KenovaDialogBase LayerComponent { get; set; }

    [CascadingParameter]
    internal ButtonBar Parent { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public bool Enabled { get; set; } = true;

    [Parameter]
    public string FocusID { get; set; }

    [Parameter]
    public bool LightBackground { get; set; } = false;

    [Parameter]
    public EventCallback ButtonClicked { get; set; }

    [Parameter]
    public bool AutoFocus { get; set; } = false;

    [Parameter]
    public int AutoFocusPriority { get; set; } = 100;

    [Parameter]
    public bool Default { get; set; } = false;

    [Parameter]
    public bool Cancel { get; set; } = false;

    /// <summary>
    /// Display the button as text only, without border or color.
    /// The default value is false.
    /// </summary>
    [Parameter]
    public bool TextOnly { get; set; } = false;

    /// <summary>
    /// The minimum width in pixels.
    /// The default value is -1
    /// </summary>
    [Parameter]
    public double MinWidth { get; set; } = -1;

    private async Task Item_Div_Clicked(MouseEventArgs e)
    {
        if (Enabled == false)
            return;

        await ButtonClicked.InvokeAsync(null);
    }

    protected override void OnInitialized()
    {
        if (LayerComponent == null)
            throw new InvalidOperationException($"The {this.GetType().Name} component must be placed inside a LayerBaseComponent");

        LayerComponent.RegisterComponent(this);

        if (Parent == null)
            throw new InvalidOperationException("ButtonBase must exist within a ButtonGroupBase");

        Parent.Register(this);
    }

    public void Dispose()
    {
        Parent.Unregister(this);
        LayerComponent.UnregisterComponent(this);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            LayerComponent.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
        }
    }


    public void Rerender()
    {
        this.StateHasChanged();
    }

    async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string focusID)
    {
        if (this.FocusID != null && focusID == this.FocusID)
        {
            await JavaScriptCaller.KNFocusAsync(ContainerID);
            return true;
        }

        return false;
    }

    async ValueTask IKenovaComponent.PerformAutoFocusAsync()
    {
        await JavaScriptCaller.KNFocusAsync(ContainerID);
    }

    ValueTask<int> IKenovaComponent.MeasureAutoFocusPriorityAsync()
    {

        if (this.AutoFocus && this.Enabled)
        {
            return ValueTask.FromResult<int>(this.AutoFocusPriority);
        }

        return ValueTask.FromResult<int>(-1);
    }

    async ValueTask<bool> IKenovaComponent.PerformEnterPressedAsync()
    {
        if (this.Default && this.Enabled)
        {
            await processDefaultAsync();
            return true;
        }

        return false;
    }

    private async Task processDefaultAsync()
    {
        await JavaScriptCaller.KNFocusAsync(ContainerID);

        _ = ButtonClicked.InvokeAsync();
    }

    async ValueTask<bool> IKenovaComponent.PerformEscapePressedAsync()
    {
        if (this.Cancel && this.Enabled)
        {
            await ButtonClicked.InvokeAsync();
            return true;
        }

        return false;
    }

    ValueTask<bool> IKenovaComponent.ComponentHiddenAsync()
    {
        return JavaScriptCaller.KNElementHiddenAsync(ContainerID);
    }

    ValueTask<bool> IKenovaComponent.IsChildOfAsync(string parent_id)
    {
        return JavaScriptCaller.KNIsChildOfAsync(parent_id, ContainerID);
    }

}

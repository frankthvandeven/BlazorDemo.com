using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;


namespace Kenova.Client.Components
{

    public partial class HyperGrid<ItemType> : KenovaComponentBase, IKenovaComponent
    {
        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public int AutoFocusPriority { get; set; } = 100;

        [Parameter]
        public string FocusID { get; set; }

        public async ValueTask SetFocusAsync()
        {
            if (this.Data.UseFilter)
            {
                await JavaScriptCaller.KNFocusAsync(this.InputElementId);
            }
            else
            {
                await _wingman.InvokeVoidAsync("SetFocus");

            }
        }

        async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string focusID)
        {
            if (this.FocusID != null && focusID == this.FocusID)
            {
                await SetFocusAsync();
                return true;
            }

            return false;
        }

        ValueTask IKenovaComponent.PerformAutoFocusAsync()
        {
            return this.SetFocusAsync();
        }

        ValueTask<int> IKenovaComponent.MeasureAutoFocusPriorityAsync()
        {
            if (this.AutoFocus)
            {
                return ValueTask.FromResult(this.AutoFocusPriority);
            }

            return ValueTask.FromResult(-1);
        }

        ValueTask<bool> IKenovaComponent.PerformEnterPressedAsync()
        {
            return ValueTask.FromResult(false);
        }

        ValueTask<bool> IKenovaComponent.PerformEscapePressedAsync()
        {
            return ValueTask.FromResult(false);
        }

        ValueTask<bool> IKenovaComponent.ComponentHiddenAsync()
        {
            return JavaScriptCaller.KNElementHiddenAsync(_container_id);
        }

        ValueTask<bool> IKenovaComponent.IsChildOfAsync(string parent_id)
        {
            return JavaScriptCaller.KNIsChildOfAsync(parent_id, _container_id);
        }


    }

}



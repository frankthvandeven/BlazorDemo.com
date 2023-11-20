using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Kenova.Client.Components
{
    public partial class CheckBox : KenovaComponentBase, IKenovaComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = true;

        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        [Parameter]
        public EventCallback IndeterminateClicked { get; set; }

        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        [Parameter]
        public string FocusID { get; set; } = null;

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public int AutoFocusPriority { get; set; } = 100;

        private void Item_Div_Clicked(MouseEventArgs e)
        {
            this.processClick();
        }

        [JSInvokable]
        public void OnEnterPressed()
        {
            this.processClick();
        }

        [JSInvokable]
        public void OnSpacePressed()
        {
            this.processClick();
        }

        private void processClick()
        {
            if (Enabled == false)
                return;

            if (Indeterminate)
            {
                _ = IndeterminateClicked.InvokeAsync(null);
                return;
            }

            CheckedFieldLink.Value = !CheckedFieldLink.Value;

        }

        public ValueTask SetFocusAsync()
        {
            return JavaScriptCaller.KNFocusAsync(container_id);
        }

        async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string focusID)
        {
            if (this.FocusID != null && focusID == this.FocusID)
            {
                if (this.Enabled == true)
                {
                    await JavaScriptCaller.KNFocusAsync(container_id);
                }
                return true;
            }

            return false;
        }

        ValueTask IKenovaComponent.PerformAutoFocusAsync()
        {
            return JavaScriptCaller.KNFocusAsync(container_id);
        }

        ValueTask<int> IKenovaComponent.MeasureAutoFocusPriorityAsync()
        {
            if (this.AutoFocus == true && this.Enabled == true)
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
            return JavaScriptCaller.KNElementHiddenAsync(container_id);
        }

        ValueTask<bool> IKenovaComponent.IsChildOfAsync(string parent_id)
        {
            return JavaScriptCaller.KNIsChildOfAsync(parent_id, container_id);
        }
    }
}


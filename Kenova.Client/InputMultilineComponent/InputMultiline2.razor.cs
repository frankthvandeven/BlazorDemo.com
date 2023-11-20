using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Timers;

namespace Kenova.Client.Components
{
    public partial class InputMultiline : KenovaComponentBase, IKenovaComponent
    {

        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        [Parameter]
        public string FocusID { get; set; } = null;

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public int AutoFocusPriority { get; set; } = 100;

        public ValueTask SetFocusAsync()
        {
            return JavaScriptCaller.KNFocusAsync(InputElementId);
        }

        async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string focusID)
        {
            if (this.FocusID != null && focusID == this.FocusID)
            {
                if (this.Enabled == true)
                {
                    await JavaScriptCaller.KNFocusAsync(InputElementId);
                }
                return true;
            }

            return false;
        }

        ValueTask IKenovaComponent.PerformAutoFocusAsync()
        {
            return JavaScriptCaller.KNFocusAsync(InputElementId);
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
            return JavaScriptCaller.KNElementHiddenAsync(InputboxContainerId);
        }

        ValueTask<bool> IKenovaComponent.IsChildOfAsync(string parent_id)
        {
            return JavaScriptCaller.KNIsChildOfAsync(parent_id, InputboxContainerId);
        }

    }
}

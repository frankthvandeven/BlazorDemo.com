using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Kenova.Client.Components
{
    public partial class Toolbar : KenovaComponentBase, IKenovaComponent
    {

        async ValueTask<bool> IKenovaComponent.PerformFocusAsync(string focusID)
        {
            foreach (var item in this.Buttons)
            {
                if (item.FocusID != null && item.FocusID == focusID)
                {
                    await JavaScriptCaller.KNFocusAsync(item.ContainerID);
                    return true;
                }
            }

            return false;
        }

        async ValueTask IKenovaComponent.PerformAutoFocusAsync()
        {
            if (_autofocus_item == null)
                return;

            await JavaScriptCaller.KNFocusAsync(_autofocus_item.ContainerID);
        }

        private ToolbarItem _autofocus_item = null;

        ValueTask<int> IKenovaComponent.MeasureAutoFocusPriorityAsync()
        {
            foreach (var item in this.Buttons)
            {
                if (item.AutoFocus && item.EnabledExpression())
                {
                    _autofocus_item = item;
                    return ValueTask.FromResult(this.Buttons.AutoFocusPriority);
                }
            }

            return ValueTask.FromResult(-1);
        }

        async ValueTask<bool> IKenovaComponent.PerformEnterPressedAsync()
        {
            foreach (var item in this.Buttons)
            {
                if (item.Kind == ButtonKind.Default && item.EnabledExpression())
                {
                    await processDefaultAsync(item);
                    return true;
                }
            }

            return false;
        }

        private async Task processDefaultAsync(ToolbarItem item)
        {
            await JavaScriptCaller.KNFocusAsync(item.ContainerID);

            item.Clicked?.Invoke();
        }

        ValueTask<bool> IKenovaComponent.PerformEscapePressedAsync()
        {
            foreach (var item in this.Buttons)
            {
                if (item.Kind == ButtonKind.Cancel && item.EnabledExpression())
                {
                    item.Clicked?.Invoke();
                    return ValueTask.FromResult(true);
                }
            }

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

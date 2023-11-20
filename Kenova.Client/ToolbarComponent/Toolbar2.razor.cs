using Microsoft.JSInterop;

namespace Kenova.Client.Components
{
    public partial class Toolbar : IKenovaComponent
    {

        [JSInvokable]
        public void OnButtonClicked(int index)
        {
            processToolbarButtonClick(index);
        }

        [JSInvokable]
        public void OnSpacePressed(int index)
        {
            processToolbarButtonClick(index);
        }

        private void processToolbarButtonClick(int index)
        {
            var item = this.Buttons[index];

            if (item.EnabledExpression() == false)
                return;

            if (item.Clicked == null)
                return;

            item.Clicked();
        }

        public void Rerender()
        {
            this.StateHasChanged();
        }

    }
}
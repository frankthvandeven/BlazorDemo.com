using Microsoft.AspNetCore.Components;

namespace Kenova.Client.Components
{

    /*
     * 
     * Public methods and properties.
     * 
     */

    public partial class HyperGrid<ItemType> : KenovaComponentBase
    {

        public async ValueTask MakeFirstItemInViewAsync(ItemType item)
        {
            int index = this.Data.DisplayItems.IndexOf(item);

            if (index == -1)
                return;

            await MakeFirstItemInViewAsync(index);
        }

        public ValueTask MakeFirstItemInViewAsync(int index)
        {
            return _wingman.InvokeVoidAsync("MakeFirstItemInView", index);
        }

        public async ValueTask MakeCenterItemInViewAsync(ItemType item)
        {
            int index = this.Data.DisplayItems.IndexOf(item);

            if (index == -1)
                return;

            await MakeCenterItemInViewAsync(index);
        }

        public ValueTask MakeCenterItemInViewAsync(int index)
        {
            return _wingman.InvokeVoidAsync("MakeCenterItemInView", index);
        }

        public async ValueTask MakeLastItemInView(ItemType item)
        {
            int index = this.Data.DisplayItems.IndexOf(item);

            if (index == -1)
                return;

            await MakeLastItemInViewAsync(index);
        }

        public ValueTask MakeLastItemInViewAsync(int index)
        {
            return _wingman.InvokeVoidAsync("MakeLastItemInView", index);
        }


        public async ValueTask ScrollToSelectedItemAsync()
        {
            if (this.Data.SelectedItem != null)
            {
                int index = this.Data.DisplayItems.IndexOf(this.Data.SelectedItem);

                if (index != -1)
                {
                    await this.MakeCenterItemInViewAsync(index);
                }
            }
        }

        public ValueTask ScrollToTopAsync()
        {
            return _wingman.InvokeVoidAsync("ScrollToTop");
        }

        public ValueTask ScrollToLeftAsync()
        {
            return _wingman.InvokeVoidAsync("ScrollToLeft");
        }

        public ValueTask ScrollToBottomAsync()
        {
            return _wingman.InvokeVoidAsync("ScrollToBottom");
        }

    }
}

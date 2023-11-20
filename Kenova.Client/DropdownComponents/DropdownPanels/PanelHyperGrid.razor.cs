using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components.Panels
{
    public partial class PanelHyperGrid<ItemType> : KenovaDialogBase
    {

        HyperGrid<ItemType> hyper_grid = null;

        [Parameter]
        public HyperData<ItemType> Data { get; set; }

        //[Parameter]
        //public Expression<Func<ItemType>> SelectedItem { get; set; }

        //[Parameter]
        //public EventCallback CheckedItemsChanged { get; set; }

        protected override void OnDialogInitialized()
        {
            this.Data.SelectedItemChanged = SelectedItemChanged;
            this.Data.CheckedItemsChanged = LocalCheckedItemsChanged;

        }

        protected override void OnAfterRender(bool firstRender)
        {

            if (firstRender)
            {
                //if (this.Data.SelectedItem != null)
                //{
                //    int index = this.Data.DisplayItems.IndexOf(this.Data.SelectedItem);

                //    if( index != -1)
                //    {
                //        hyper_grid.MakeCenterItemInView(index);
                //    }

                //}

                _ = hyper_grid.SetFocusAsync();
            }

        }

        private void RowDoubleClicked(ItemType item)
        {
            if (this.Data.UseMultiCheck == MultiCheck.Off)
                return;

            _ = this.CloseCancelAsync();
        }

        private void SelectedItemChanged(ItemType item)
        {
            if (item == null)
                return;

            if (this.Data.UseMultiCheck != MultiCheck.Off)
                return;

            _ = this.CloseOkAsync(item);
        }

        public void LocalCheckedItemsChanged()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("PanelHyperGrid - LocalCheckedItemsChanged - SendValue");
            this.SendValue(null);
        }


    }
}

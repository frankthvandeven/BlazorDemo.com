using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class StoreEditOrder : LayerComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public StoreEditOrderModel Model { get; set; }

        ToolbarItemCollection toolbarButtons = new ToolbarItemCollection();


        protected override void OnLayerInitialized()
        {
            if (Model == null)
                throw new ArgumentNullException("model");

            this.Breadcrumb = $"Edit order {Model.order_id}";
            this.Title = $"Edit order {Model.order_id}";

            toolbarButtons.Add("Save", SaveClicked, () => Model.IsModelModified, IconKind.FontAwesome, "far fa-save");
            toolbarButtons.Add("Close", CloseClicked, () => true, IconKind.FontAwesome, "far fa-times");

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Model.ValidateAllAsync();
            }
        }

        private async void SaveClicked()
        {
            bool result = await LongRunningTask.SimpleRun("Saving", Model.SaveExecTask);

            if (result == true)
                this.CloseOk();
        }

        private void CloseClicked()
        {
            this.CloseCancel();
        }

        private async Task CustomerZoomClicked()
        {
            var SearchModel = new SearchCustomersModel
            {
                Mode = SearchCustomersModel.ModelMode.Lookup,
                SearchCustomerId = this.Model.customer_id
            };

            var ld = new LayerDefinition<SearchCustomers>
            {
                Kind = LayerKind.Modal,
                [i => i.Model] = SearchModel
            };

            var result = await ld.OpenAsync();

            if (result.Cancelled)
                return;

            this.Model.customer_id = SearchModel.Recordset.CurrentRecord.customer_id;

        }

    }
}

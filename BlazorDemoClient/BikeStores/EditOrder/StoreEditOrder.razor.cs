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
        public StoreEditOrderModel Model { get; set; }

        private string Title;
        private ToolbarItemCollection toolbar = new ToolbarItemCollection();

        protected override void OnLayerInitialized()
        {
            if (Model == null)
                throw new ArgumentNullException("model");

            this.Breadcrumb = $"Edit order {Model.order_id}";
            this.Title = $"Edit order {Model.order_id}";

            toolbar.Add("Save", SaveClicked, () => Model.IsModelModified, IconKind.FontAwesome, "far fa-save");
            toolbar.Add("Close", CloseClicked, () => true, IconKind.FontAwesome, "far fa-times");
            toolbar.SourceCodeButton("BikeStores/EditOrder");

        }

        private async void SaveClicked()
        {
            bool result = await LongRunningTask.SimpleRun("Saving", Model.SaveTask);

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
                LookupMode = true
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

using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class SearchProducts : LayerComponentBase
    {
        private string Title;

        private SearchProductsModel Model = new();

        ToolbarItemCollection toolbar = new();

        private HyperData<SearchProductsRecord> data = new();

        protected override async Task OnLayerInitializedAsync()
        {
            this.Breadcrumb = "Products";
            this.Title = "Products";

            toolbar.Add("Edit", EditClicked, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-pencil-alt");
            toolbar.Add("New", NewClicked, null, IconKind.FontAwesome, "fas fa-plus");
            toolbar.Add("Delete", null, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-trash");
            toolbar.SourceCodeButton("BikeStores/SearchProducts");

            data.Items = this.Model.Recordset;
            data.SelectedItemExpression = () => Model.Recordset.CurrentRecord;
            data.Mode = DisplayMode.Virtualization;

            data.Columns.Add(c => c.product_id, "Product#", 80, false);
            data.Columns.Add(c => c.product_name, "Product name", 300, false);

            await Model.SearchExec();

        }

        private async void EditClicked()
        {
            var EditModel = new StoreEditProductModel
            {
                CreateNew = false,
                product_id = Model.Recordset.CurrentRecord.product_id
            };

            var ld = new LayerDefinition<StoreEditProduct>
            {
                Kind = LayerKind.Modal,
                [i => i.Model] = EditModel
            };

            LayerResult layerResult = await ld.OpenThenWaitForCloseAsync();

            if (layerResult.Cancelled)
                return;

            // Update the DataGrid with modified data.
            PropertyCopier.Copy(EditModel, Model.Recordset.CurrentRecord);

            this.StateHasChanged(); // Re-render the component
        }

        private async void NewClicked()
        {
            var EditModel = new StoreEditProductModel
            {
                CreateNew = true
            };

            var ld = new LayerDefinition<StoreEditProduct>
            {
                Kind = LayerKind.Modal,
                [i => i.Model] = EditModel
            };

            LayerResult layerResult = await ld.OpenThenWaitForCloseAsync();

            if (layerResult.Cancelled)
                return;

            // Update the DataGrid with modified data.
            PropertyCopier.Copy(EditModel, Model.Recordset.CurrentRecord);

            this.StateHasChanged(); // Re-render the component
        }


        private async Task SearchClicked()
        {
            await LongRunningTask.SimpleRun("Searching", Model.SearchExec);

            if (Model.Recordset.RecordCount > 0)
                await this.SetFocusAsync("datagrid");
            else
                await this.SetFocusAsync("name");
        }

        private void ClearClicked()
        {
            Model.SearchProductID = null;
            Model.SearchText = null;
            Model.Recordset.Clear();

            _ = this.SetFocusAsync("name");
        }


    }
}

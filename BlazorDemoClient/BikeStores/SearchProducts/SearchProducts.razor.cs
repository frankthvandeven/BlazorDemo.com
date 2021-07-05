using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class SearchProducts : LayerComponentBase
    {
        private string Title = "Products";

        private SearchProductsModel Model = new SearchProductsModel();

        ToolbarItemCollection toolbar = new ToolbarItemCollection();

        private HyperData<SearchProductsRecord> data = new();

        //private Incr_sales_customers_Record SelectedRecord;

        protected override void OnLayerInitialized()
        {
            this.Breadcrumb = "Products";

            toolbar.Add("Edit", EditClicked, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-pencil-alt");
            toolbar.Add("New", NewClicked, null, IconKind.FontAwesome, "fas fa-plus");
            toolbar.Add("Delete", null, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-trash");
            toolbar.SourceCodeButton("BikeStores/SearchProducts");

            data.Items = this.Model.Recordset;
            data.SelectedItemExpression = () => Model.Recordset.CurrentRecord;
            data.Mode = DisplayMode.Virtualization;
            //data.PageSize = 15;
            data.UseHeader = true;
            data.CheckedItemsChanged = CheckedItemsChanged;

            data.Columns.Add(c => c.product_id, "Product#", 80, false);
            data.Columns.Add(c => c.product_name, "Product name", 300, false);

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if( firstRender)
            {
                await LongRunningTask.SimpleRun("Loading", Model.SearchExecTask);
            }
        }

        private async void EditClicked()
        {
            var EditModel = new StoreEditOrderModel();
            
            EditModel.Mode = StoreEditOrderModel.ModelMode.Edit;
            EditModel.order_id = Model.Recordset.CurrentRecord.product_id;

            bool runResult = await LongRunningTask.SimpleRun("Loading", EditModel.LoadExecTask);

            if (runResult == false)
                return;

            var ld = new LayerDefinition<StoreEditOrder>
            {
                Kind = LayerKind.Modal,
                [i => i.Model] = EditModel
            };

            LayerResult layerResult = await ld.OpenAsync();

            if (layerResult.Cancelled)
                return;

            // Update the DataGrid with modified data.

            //Model.rs.CurrentRecord.first_name = EditModel.first_name;
            //Model.rs.CurrentRecord.last_name = EditModel.last_name;
            //Model.rs.CurrentRecord.street = EditModel.street;
            //Model.rs.CurrentRecord.city = EditModel.city;
            //Model.rs.CurrentRecord.state = EditModel.state;
            //Model.rs.CurrentRecord.zip_code = EditModel.zip_code;

            this.StateHasChanged(); // Re-render the component
        }

        private void NewClicked()
        {
            //var caption = $"New customer";

            //var EditModel = new StoreEditCustomerModel();

            //EditModel.Mode = StoreEditCustomerModel.ModelMode.New;

            //var ld = new LayerDefinition(LayerKind.ModalFullsize);

            //ld.Breadcrumb = caption;
            //ld.Parameter("Title", caption);
            //ld.Parameter("Model", EditModel);

            //LayerResult result = await LayerManager.OpenAsync<StoreEditCustomer>(ld);

            //if (result.Cancelled)
            //    return;

            //// Update the DataGrid with modified data.

            //Model.rs.Append(); // Automatically sets the CurrentRecord

            //Model.rs.CurrentRecord.customer_id = EditModel.LastRecord.customer_id;
            //Model.rs.CurrentRecord.first_name = EditModel.LastRecord.first_name;
            //Model.rs.CurrentRecord.last_name = EditModel.LastRecord.last_name;
            //Model.rs.CurrentRecord.street = EditModel.LastRecord.street;
            //Model.rs.CurrentRecord.city = EditModel.LastRecord.city;
            //Model.rs.CurrentRecord.state = EditModel.LastRecord.state;
            //Model.rs.CurrentRecord.zip_code = EditModel.LastRecord.zip_code;

            //this.StateHasChanged(); // Re-render the component
        }



        private async Task SearchClicked()
        {
            await LongRunningTask.SimpleRun("Searching", Model.SearchExecTask);
        }

        private void ClearClicked()
        {
            Model.SearchProductID = null;
            Model.SearchText = null;
            Model.Recordset.Clear();
        }


        private void CheckedItemsChanged()
        {
        }


    }
}

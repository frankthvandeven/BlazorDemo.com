using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class SearchOrders : LayerComponentBase
    {
        private string Title;

        private SearchOrdersModel Model = new SearchOrdersModel();

        ToolbarItemCollection toolbar = new ToolbarItemCollection();

        private HyperData<SearchOrdersRecord> Data = new();

        protected override async Task OnLayerInitializedAsync()
        {
            this.Breadcrumb = "Orders";
            this.Title = "Orders";

            if (Model == null)
                throw new InvalidOperationException("Parameter Model can not be null");

            toolbar.Add("Edit", EditClicked, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-pencil-alt");
            toolbar.Add("New", NewClicked, null, IconKind.FontAwesome, "fas fa-plus");
            toolbar.Add("Delete", null, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-trash");
            toolbar.SourceCodeButton("BikeStores/SearchOrders");

            Data.Items = this.Model.Recordset;
            Data.SelectedItemExpression = () => Model.Recordset.CurrentRecord;
            Data.Mode = DisplayMode.Virtualization;

            Data.Columns.Add(c => c.order_date, "Date", 120, false);
            Data.Columns.Add(c => c.order_id, "Order number", 100, false);
            Data.Columns.Add(c => c.customer_id, "Customer", 100, false);
            Data.Columns.Add(c => c.first_name, "First name", 200);
            Data.Columns.Add(c => c.last_name, "Last name", 200);

            await Model.SearchExecTask();
        }

        private async void EditClicked()
        {
            var EditModel = new StoreEditOrderModel();

            EditModel.order_id = Model.Recordset.CurrentRecord.order_id;

            bool runResult = await LongRunningTask.SimpleRun("Loading", EditModel.LoadTask);

            if (runResult == false)
                return;

            var ld = new LayerDefinition<StoreEditOrder>
            {
                Kind = LayerKind.Modal,
                [i => i.Model] = EditModel
            };

            LayerResult layerResult = await ld.OpenThenWaitForCloseAsync();

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

            if (Model.Recordset.RecordCount > 0)
                await this.SetFocusAsync("datagrid");
            else
                await this.SetFocusAsync("searchtext");
        }

        private void ClearClicked()
        {
            Model.SearchOrderID = null;
            Model.SearchText = null;
            Model.Recordset.Clear();

            _ = this.SetFocusAsync("searchtext");
        }


    }
}

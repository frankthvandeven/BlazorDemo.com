using BlazorDemo.Client.VenturaRecordsets;
using Kenova.Client;
using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class SearchOrdersEF : KenovaDialogBase
    {
        private string Title;

        private SearchOrdersModelEF Model = new SearchOrdersModelEF();

        ToolbarItemCollection toolbar = new ToolbarItemCollection();

        private HyperData<SearchOrdersRecordEF> Data = new();

        protected override async Task OnDialogInitializedAsync()
        {
            this.Breadcrumb = "Orders EF";
            this.Title = "Orders EF";

            if (Model == null)
                throw new InvalidOperationException("Parameter Model can not be null");

            toolbar.Add("Edit", EditClicked, () => Model.CurrentRecord != null, IconKind.FontAwesome, "fas fa-pencil-alt");
            toolbar.Add("New", NewClicked, null, IconKind.FontAwesome, "fas fa-plus");
            toolbar.Add("Delete", null, () => Model.CurrentRecord != null, IconKind.FontAwesome, "fas fa-trash");
            toolbar.SourceCodeButton("BikeStores/SearchOrdersEF");

            Data.Items = this.Model.Recordset;
            Data.SelectedItemExpression = () => Model.CurrentRecord;
            //Data.Mode = DisplayMode.Virtualization;

            Data.Columns.Add(c => c.orderDate, "Date", 120, false);
            Data.Columns.Add(c => c.orderId, "Order number", 100, false);
            Data.Columns.Add(c => c.customerId, "Customer", 100, false);
            Data.Columns.Add(c => c.firstName, "First name", 200);
            Data.Columns.Add(c => c.lastName, "Last name", 200);

            await Model.SearchExecTask();
            Data.Items = this.Model.Recordset; // TEST
        }

        private async void EditClicked()
        {
            var EditModel = new StoreEditOrderModel();

            EditModel.order_id = Model.CurrentRecord.orderId;

            bool runResult = await LongRunningTask.SimpleRun("Loading", EditModel.LoadTask);

            if (runResult == false)
                return;

            var ld = new LayerDefinition<StoreEditOrder>
            {
                Kind = LayerKind.Modal,
                [i => i.Model] = EditModel
            };

            LayerResult layerResult = await ld.OpenThenWaitForCloseAsync();

            if (layerResult.Cancelled || layerResult.Aborted)
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

            Data.Items = this.Model.Recordset; // TEST

            if (Model.Recordset.Count > 0)
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

    public class SearchOrdersRecordEF
    {
        public int orderId { get; set; }
        public int customerId { get; set; }
        public int orderStatus { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime requiredDate { get; set; }
        public DateTime? shippedDate { get; set; }
        public int storeId { get; set; }
        public int staffId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string city { get; set; }
    }
}

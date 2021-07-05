﻿using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class SearchOrders : LayerComponentBase
    {
        private string Title = "Orders";

        private SearchOrdersModel Model = new SearchOrdersModel();

        ToolbarItemCollection toolbarButtons = new ToolbarItemCollection();

        private HyperData<SearchOrdersRecord> Data = new();

        //private Incr_sales_customers_Record SelectedRecord;

        protected override void OnLayerInitialized()
        {
            this.Breadcrumb = "Orders";

            if (Model == null)
                throw new InvalidOperationException("Parameter Model can not be null");

            toolbarButtons.Add("Edit", EditClicked, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-pencil-alt");
            toolbarButtons.Add("New", NewClicked, null, IconKind.FontAwesome, "fas fa-plus");
            toolbarButtons.Add("Delete", null, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-trash");

            Data.Items = this.Model.Recordset;
            Data.SelectedItemExpression = () => Model.Recordset.CurrentRecord;
            Data.Mode = DisplayMode.Virtualization;
            //data.PageSize = 15;
            Data.UseHeader = true;
            Data.CheckedItemsChanged = CheckedItemsChanged;

            Data.Columns.Add(c => c.order_date, "Date", 120, false);
            //data.Columns.

            Data.Columns.Add(c => c.order_id, "Order number", 100, false);
            Data.Columns.Add(c => c.customer_id, "Customer", 100, false);
            Data.Columns.Add(c => c.first_name, "First name", 200);
            Data.Columns.Add(c => c.last_name, "Last name", 200);

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LongRunningTask.SimpleRun("Loading", Model.SearchExecTask);
            }


        }



        private async void EditClicked()
        {
            var EditModel = new StoreEditOrderModel();

            EditModel.Mode = StoreEditOrderModel.ModelMode.Edit;
            EditModel.order_id = Model.Recordset.CurrentRecord.order_id;

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
            Model.SearchOrderID = null;
            Model.SearchText = null;
            Model.Recordset.Clear();
        }


        private void CheckedItemsChanged()
        {
        }


    }
}

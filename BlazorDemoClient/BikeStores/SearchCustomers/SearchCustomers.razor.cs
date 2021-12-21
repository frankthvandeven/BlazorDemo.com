using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class SearchCustomers : LayerComponentBase
    {
        private string Title;

        [Parameter]
        public SearchCustomersModel Model { get; set; } = new SearchCustomersModel();

        ToolbarItemCollection toolbar = new();

        private HyperData<SearchCustomersRecord> Data = new HyperData<SearchCustomersRecord>();

        private HyperGrid<SearchCustomersRecord> _hypergrid;

        protected override void OnLayerInitialized()
        {
            this.Breadcrumb = "Customers";
            this.Title = "Customers";

            if (Model == null)
                throw new InvalidOperationException("Parameter Model can not be null");

            if (Model.LookupMode)
            {
                this.Title = "Select customer";

                toolbar.Add("Select", SelectClicked, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "far fa-bullseye-pointer");
                //toolbar.ButtonKind = ButtonKind.Default;
            }

            toolbar.Add("Edit", EditClicked, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-pencil-alt");
            toolbar.Add("New", NewClicked, null, IconKind.FontAwesome, "fas fa-plus");
            toolbar.Add("Delete", null, () => Model.Recordset.CurrentRecord != null, IconKind.FontAwesome, "fas fa-trash");

            if (Model.LookupMode)
            {
                toolbar.Add("Close", () => this.CloseCancel(), null, IconKind.FontAwesome, "far fa-times");
            }

            toolbar.SourceCodeButton("BikeStores/SearchCustomers");

            Data.Items = Model.Recordset;
            Data.Mode = DisplayMode.Virtualization;
            Data.SelectedItemExpression = () => Model.Recordset.CurrentRecord;

            Data.Columns.Add(c => c.customer_id, "Customer#", 100, false);
            Data.Columns.Add(c => c.first_name, "First name", 150);
            Data.Columns.Add(c => c.last_name, "Last name", 250);
            Data.Columns.Add(c => c.street, "Street", 200);
            Data.Columns.Add(c => c.city, "City", 200);
            Data.Columns.Add(c => c.state, "State", 150);
            Data.Columns.Add(c => c.zip_code, "Zip code", 100);

        }

        protected override async Task OnLayerInitializedAsync()
        {
            await Model.SearchExec();
        }

        private void RowDoubleClicked()
        {
            if (Model.LookupMode)
            {
                SelectClicked();
                return;
            }

            EditClicked();
        }

        private void SelectClicked()
        {
            this.CloseOk();
        }

        private async void EditClicked()
        {

            var EditModel = new StoreEditCustomerModel
            {
                CreateNew = false,
                customer_id = Model.Recordset.CurrentRecord.customer_id
            };

            var ld = new LayerDefinition<StoreEditCustomer>
            {
                Kind = LayerKind.Modal,
                [i => i.Model] = EditModel
            };

            LayerResult result = await ld.OpenWaitForCloseAsync();

            if (result.Cancelled)
                return;

            // Update the DataGrid with modified data.

            PropertyCopier.Copy(EditModel, Model.Recordset.CurrentRecord);

            //Model.Recordset.CurrentRecord.first_name = EditModel.first_name;
            //Model.Recordset.CurrentRecord.last_name = EditModel.last_name;
            //Model.Recordset.CurrentRecord.street = EditModel.street;
            //Model.Recordset.CurrentRecord.city = EditModel.city;
            //Model.Recordset.CurrentRecord.state = EditModel.state;
            //Model.Recordset.CurrentRecord.zip_code = EditModel.zip_code;

            this.StateHasChanged(); // Re-render the component
        }

        private async void NewClicked()
        {
            var EditModel = new StoreEditCustomerModel
            {
                CreateNew = true
            };

            var ld = new LayerDefinition<StoreEditCustomer>
            {
                Kind = LayerKind.Modal,
                [i => i.Model] = EditModel
            };

            LayerResult result = await ld.OpenWaitForCloseAsync();

            if (result.Cancelled)
                return;

            // Update the DataGrid with modified data.

            var record = Model.Recordset.NewRecord(); // Automatically sets the CurrentRecord

            PropertyCopier.Copy(EditModel.Recordset, record);

            //record.customer_id = EditModel.Rs.customer_id;
            //record.first_name = EditModel.Rs.first_name;
            //record.last_name = EditModel.Rs.last_name;
            //record.street = EditModel.Rs.street;
            //record.city = EditModel.Rs.city;
            //record.state = EditModel.Rs.state;
            //record.zip_code = EditModel.Rs.zip_code;

            Model.Recordset.Append(record);

            this.StateHasChanged(); // Re-render the component
        }

        private async Task SearchClicked()
        {
            await LongRunningTask.SimpleRun("Searching", () => Model.SearchExec());

            if (Model.Recordset.RecordCount > 0)
                this.SetFocus("datagrid");
            else
                this.SetFocus("name");

        }

        private void ClearClicked()
        {
            this.Model.SearchName = null;
            this.Model.SearchCustomerId = null;
            this.Model.Recordset.Clear();
            this.Model.ResetModelModified();

            this.SetFocus("name");
        }

    }
}

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

        private LayerDefinition<SelectStaff> _ld_staff = new();
        private LayerDefinition<SelectStore> _ld_store = new();

        private string Title;
        private ToolbarItemCollection toolbar = new ToolbarItemCollection();

        protected override void OnLayerInitialized()
        {
            if (Model == null)
                throw new ArgumentNullException("model");

            this.Breadcrumb = $"Edit order {Model.order_id}";
            this.Title = $"Edit order {Model.order_id}";

            toolbar.Add("Save", SaveClicked, () => Model.IsModelModified, IconKind.FontAwesome, "far fa-save");
            toolbar.ButtonKind = ButtonKind.Default;

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


        private async Task StaffZoomClicked()
        {
            if( _ld_staff.IsOpen )
            {
                _ld_staff.CloseCancel();
                return;
            }

            var SelectStaffModel = new SelectStaffModel
            {
                staff_id = this.Model.staff_id
            };

            _ld_staff.Kind = LayerKind.ModelessRight;
            _ld_staff.Width = 400;
            _ld_staff[i => i.Model] = SelectStaffModel;

            var result = await _ld_staff.OpenAsync();

            if (result.Cancelled)
                return;

            this.Model.staff_id = SelectStaffModel.Recordset.CurrentRecord.staff_id;

        }

        private async Task StoreZoomClicked()
        {
            if (_ld_store.IsOpen)
            {
                _ld_store.CloseCancel();
                return;
            }

            var SelectStoreModel = new SelectStoreModel
            {
                store_id = this.Model.store_id
            };

            _ld_store.Kind = LayerKind.ModelessRight;
            _ld_store.Width = 400;
            _ld_store[i => i.Model] = SelectStoreModel;

            var result = await _ld_store.OpenAsync();

            if (result.Cancelled)
                return;

            this.Model.store_id = SelectStoreModel.Recordset.CurrentRecord.store_id;

        }

    }
}

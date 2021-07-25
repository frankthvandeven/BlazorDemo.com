using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class StoreEditCustomer : LayerComponentBase
    {
        [Parameter]
        public StoreEditCustomerModel Model { get; set; }

        private string Title;
        private ToolbarItemCollection toolbar = new();

        protected override async Task OnLayerInitializedAsync()
        {
            if (Model == null)
                throw new ArgumentNullException("model");


            if (Model.CreateNew)
            {
                this.Breadcrumb = $"New customer";
                this.Title = $"New customer";
            }
            else
            {
                this.Breadcrumb = $"Edit customer {Model.customer_id}";
                this.Title = $"Edit customer {Model.customer_id}";
            }

            toolbar.Add("Save", SaveClicked, () => true, IconKind.FontAwesome, "far fa-save");
            toolbar.ButtonKind = ButtonKind.Default;
            

            toolbar.Add("Close", CloseClicked, () => true, IconKind.FontAwesome, "far fa-times");
            toolbar.ButtonKind = ButtonKind.Cancel;
            toolbar.AutoFocus = true;

            toolbar.SourceCodeButton("BikeStores/EditCustomer");

            await Model.LoadTask();

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

    }
}

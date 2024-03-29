﻿using Kenova.Client;
using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class StoreEditCustomer : KenovaDialogBase
    {
        [Parameter]
        public StoreEditCustomerModel Model { get; set; }

        private string Title;
        private ToolbarItemCollection toolbar = new();

        protected override async Task OnDialogInitializedAsync()
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

            toolbar.SourceCodeButton("BikeStores/EditCustomer");

            await Model.LoadTask();

        }

        private async void SaveClicked()
        {
            bool result = await LongRunningTask.SimpleRun("Saving", Model.SaveTask);

            if (result == true)
                _ = this.CloseOkAsync();
        }

        private void CloseClicked()
        {
            _ = this.CloseCancelAsync();
        }

    }
}

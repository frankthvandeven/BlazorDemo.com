﻿using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    public partial class StoreEditProduct : LayerComponentBase
    {
        [Parameter]
        public StoreEditProductModel Model { get; set; }

        private string Title;
        private ToolbarItemCollection toolbar = new ToolbarItemCollection();

        protected override async Task OnLayerInitializedAsync()
        {
            if (Model == null)
                throw new ArgumentNullException("model");

            this.Breadcrumb = $"Edit product {Model.product_id}";
            this.Title = $"Edit product {Model.product_id}";

            toolbar.Add("Save", SaveClicked, () => Model.IsModelModified, IconKind.FontAwesome, "far fa-save");
            toolbar.ButtonKind = ButtonKind.Default;

            toolbar.Add("Close", CloseClicked, () => true, IconKind.FontAwesome, "far fa-times");

            toolbar.SourceCodeButton("BikeStores/EditProduct");

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
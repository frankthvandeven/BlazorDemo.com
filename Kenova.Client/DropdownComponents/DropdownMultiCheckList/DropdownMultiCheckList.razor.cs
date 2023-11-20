using Kenova.Client.Components.Panels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{
    public partial class DropdownMultiCheckList<ItemType> : KenovaComponentBase, IRerender, IDisposable
    {
        private string FieldText = "";
        private InputString InputComponent;
        private bool DropdownOpen = false;

        [Parameter]
        public IList<ItemType> Items { get; set; }

        /// <summary>
        /// For example: "i => i.Name"
        /// </summary>
        [Parameter]
        public Func<ItemType, string> DisplayExpression { get; set; }

        [Parameter]
        public Expression<Func<ItemType, object>> ColumnExpression { get; set; }

        [Parameter]
        public List<ItemType> CheckedItems { get; set; } = null;

        [Parameter]
        public Func<ItemType, bool> IsChecked { get; set; }

        [Parameter]
        public Action<ItemType> SetChecked { get; set; }

        [Parameter]
        public Action<ItemType> SetUnchecked { get; set; }

        [Parameter]
        public EventCallback CheckedItemsChanged { get; set; }

        [Parameter]
        public ColumnCollection<ItemType> Columns { get; set; } = null;

        [Parameter]
        public bool UseFilter { get; set; } = false;

        [Parameter]
        public bool UseHeader { get; set; } = false;

        private LayerDefinition<PanelHyperGrid<ItemType>> _ld = null;

        protected override void OnInitialized()
        {
            if (ColumnExpression == null && Columns == null)
                throw new InvalidOperationException("No list columns defined. Set either the ColumnExpression or Columns parameter.");

            if (DisplayExpression == null)
                throw new ArgumentNullException("DisplayExpression not set.");

        }

        public void Dispose()
        {
        }

        public void Rerender()
        {
            this.StateHasChanged();
        }


        public async void DropdownButtonClickedAsync()
        {
            if (this.DropdownOpen == true)
            {
                await _ld.CloseCancelAsync(); // Close the layer
                return;
            }

            HyperData<ItemType> data = new();

            data.Items = this.Items;
            data.DropdownMode = true;
            data.UseFilter = this.UseFilter;
            data.UseHeader = this.UseHeader;
            data.SelectedItemExpression = null;

            data.CheckedItems = this.CheckedItems;
            data.IsChecked = this.IsChecked;
            data.SetChecked = this.SetChecked;
            data.SetUnchecked = this.SetUnchecked;

            if (this.CheckedItems != null)
                data.UseMultiCheck = MultiCheck.List;
            else if (this.IsChecked != null)
                data.UseMultiCheck = MultiCheck.ItemField;

            if (this.ColumnExpression != null)
            {
                data.Columns.Add(ColumnExpression, "none", 10);
            }

            this.DropdownOpen = true;

            _ld = new LayerDefinition<PanelHyperGrid<ItemType>>
            {
                Kind = LayerKind.Dropdown,
                OwnerID = InputComponent.InputboxContainerId,
                ValueReceived = LayerDefinition_ValueReceived,
                AfterClosed = LayerDefinition_DropdownWasClosed,
                [i => i.Data] = data
            };

            _ld.Parameter(p => p.LayerDefinition, _ld); // not possible to refer to self in object initializer syntax

            await _ld.OpenNonBlockingAsync();

            // Call to have the control repainted with the dropdown arrow reversed.
            this.StateHasChanged();
        }



        private void LayerDefinition_ValueReceived(object receivedvalue)
        {
            this.StateHasChanged();
        }

        /// <summary>
        /// This method is called by LayerManager after closing the dropdown.
        /// A dropdown is closed by calling LayerManager.Close()
        /// When clicking outside the dropdown element and outside dropdown panel, the 
        /// LayerManager.Close() will be called automatically.
        /// </summary>
        private void LayerDefinition_DropdownWasClosed(LayerResult lr)
        {
            object returnvalue = lr.Data;

            this.DropdownOpen = false;
            _ld = null;

            if (returnvalue == null)
            {
                this.StateHasChanged();
                return;
            }

            if (DisplayExpression != null)
            {
                if (returnvalue is ItemType)
                {
                    FieldText = DisplayExpression.Invoke((ItemType)returnvalue).ToString();
                }
            }

            this.StateHasChanged();
        }

        public void LocalCheckedItemsChanged()
        {
            //if (KenovaClientConfig.Diagnostics) Console.WriteLine("LOCAL CHECKEDCHANGED");
            _ = CheckedItemsChanged.InvokeAsync();
        }


    }

}

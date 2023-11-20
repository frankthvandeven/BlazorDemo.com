using Kenova.Client.Components.Panels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{
    public partial class DropdownList<ItemType> : KenovaComponentBase, IRerender, IDisposable
    {
        private FieldLink<ItemType> SelectedItemFieldLink;

        /// <summary>
        /// For example "() => this.Model.CurrentCustomer"
        /// This parameter can only be set once. Any subsequent changes to the parameter value will be ignored.
        /// </summary>
        [Parameter]
        public Expression<Func<ItemType>> SelectedItemExpression { get; set; }

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
        public EventCallback<ItemType> SelectedItemChanged { get; set; }

        [Parameter]
        public ColumnCollection<ItemType> Columns { get; set; } = null;

        [Parameter]
        public bool UseFilter { get; set; } = true;

        private LayerDefinition<PanelHyperGrid<ItemType>> _ld = null;

        protected override void OnInitialized()
        {
            if (SelectedItemExpression == null)
                throw new ArgumentNullException("Parameter SelectedItem not set.");

            if (ColumnExpression == null && Columns == null)
                throw new ArgumentNullException("No list columns defined. Set either the ColumnExpression or Columns parameter.");

            if (DisplayExpression == null)
                throw new ArgumentNullException("Parameter DisplayExpression not set.");

            SelectedItemFieldLink = new FieldLink<ItemType>(this, SelectedItemExpression, true, null, SelectedItemChanged);
        }

        public void Dispose()
        {
            SelectedItemFieldLink.Dispose();
        }

        public void Rerender()
        {
            this.StateHasChanged();
        }

        public async Task DropdownButtonClickedAsync()
        {
            if (this.DropdownOpen == true)
            {
                await _ld.CloseCancelAsync(); // Close the layer
                return;
            }

            HyperData<ItemType> data = new();

            data.Items = Items;
            data.DropdownMode = true;
            data.UseFilter = this.UseFilter;
            data.UseHeader = false;
            data.UseMultiCheck = MultiCheck.Off;
            data.SelectedItemExpression = SelectedItemFieldLink.Expression;

            if (this.Columns != null)
                data.Columns = this.Columns;

            if (this.ColumnExpression != null)
            {
                data.Columns.Add(ColumnExpression, "none", 10);
            }

            this.DropdownOpen = true;

            //var tst = SelectedItemFieldLink.Value;

            _ld = new LayerDefinition<PanelHyperGrid<ItemType>>
            {
                Kind = LayerKind.Dropdown,
                OwnerID = InputComponent.InputboxContainerId,
                AfterClosed = LayerDefinition_DropdownWasClosed,
                [i => i.Data] = data
            };

            _ld.Parameter(p => p.LayerDefinition, _ld); // not possible to refer to self in object initializer syntax

            await _ld.OpenNonBlockingAsync();

            // Call to have the control repainted with the dropdown arrow reversed.
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

            if (!(returnvalue is ItemType))
                throw new Exception("returnvalue not of ItemType. Should not happen.");

            SelectedItemFieldLink.Value = (ItemType)returnvalue;

            this.StateHasChanged();

            _ = SelectedItemChanged.InvokeAsync();
        }

    }


}



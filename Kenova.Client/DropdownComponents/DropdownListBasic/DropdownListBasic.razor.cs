using Kenova.Client.Components.Panels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kenova.Client.Components
{
    public partial class DropdownListBasic<ValueType> : KenovaComponentBase, IRerender, IDisposable
    {
        protected FieldLink<ValueType> FieldLink { get; private set; }

        /// <summary>
        /// For example "() => this.Model.Name"
        /// </summary>
        [Parameter]
        public Expression<Func<ValueType>> FieldExpression { get; set; }

        private string FieldText = "";
        private InputString InputComponent;
        private bool DropdownOpen = false;

        [Parameter]
        public ListItemCollection<ValueType> Items { get; set; }

        [Parameter]
        public EventCallback<ValueType> FieldChanged { get; set; }

        [Parameter]
        public bool UseFilter { get; set; } = true;

        private LayerDefinition<PanelHyperGrid<ListItem<ValueType>>> _ld = null;

        protected override void OnInitialized()
        {
            if (FieldExpression == null)
                throw new InvalidOperationException("The 'FieldExpression' parameter was not set for this component.");

            FieldLink = new FieldLink<ValueType>(this, FieldExpression, true, null, FieldChanged);
        }

        public void Dispose()
        {
            FieldLink.Dispose();
        }

        public void Rerender()
        {
            this.StateHasChanged();
        }

        public async ValueTask DropdownButtonClicked()
        {
            if (this.DropdownOpen == true)
            {
                if (KenovaClientConfig.Diagnostics) Console.WriteLine("DropdownListBasic - DropdownOpen == true, CloseCancel()");
                await _ld.CloseCancelAsync(); // Close the layer
                return;
            }

            // Look up the selected item in the list.
            object selectedValue = FieldLink.Value;
            ListItem<ValueType> selectedItem = Items.FirstOrDefault(i => EqualityComparer<object>.Default.Equals(i.Value, selectedValue));
            Expression<Func<ListItem<ValueType>>> selectedItemExpression = () => selectedItem;

            HyperData<ListItem<ValueType>> data = new HyperData<ListItem<ValueType>>();

            data.Items = Items;
            data.DropdownMode = true;
            data.Columns.Add(i => i.Text, "none", 10);
            data.UseFilter = this.UseFilter;
            data.UseHeader = false;
            data.UseMultiCheck = MultiCheck.Off;
            data.SelectedItemExpression = selectedItemExpression;

            this.DropdownOpen = true;

            _ld = new LayerDefinition<PanelHyperGrid<ListItem<ValueType>>>
            {
                Kind = LayerKind.Dropdown,
                OwnerID = InputComponent.InputboxContainerId,
                [p => p.Data] = data,
                AfterClosed = LayerDefinition_DropdownWasClosed
            };

            _ld.Parameter(p => p.LayerDefinition, _ld); // not possible to refer to self in object initializer syntax

            //_ld.Parameter("CheckedItemsChanged", null);

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

            if (!(returnvalue is ListItem<ValueType>))
                throw new Exception("returnvalue not of type ListItem. Should not happen.");

            ListItem<ValueType> selectedItem = (ListItem<ValueType>)returnvalue;

            FieldLink.Value = (ValueType)selectedItem.Value;

            this.StateHasChanged();
        }


    }
}



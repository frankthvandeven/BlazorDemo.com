using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;

namespace Kenova.Client.SystemComponents
{
    public partial class MainMenu : KenovaComponentBase
    {
        private readonly string input_element_id = KenovaClientConfig.GetUniqueElementID();

        HyperGrid<MenuItem> hyper_grid = null;

        private string SearchText = "";

        private HyperData<MenuItem> Data = new();

        private bool Collapsed { get; set; } = false;

        [Parameter]
        public MenuItemCollection MenuItems { get; set; }

        [Parameter]
        public bool DisplayIcons { get; set; } = true;

        [Parameter]
        public Expression<Func<MenuItem>> SelectedMenuItemExpression { get; set; }

        [Parameter]
        public EventCallback<MenuItem> SelectedMenuItemChanged { get; set; }

        protected override void OnInitialized()
        {
            if (MenuItems == null)
                throw new ArgumentNullException("MenuItems");

            Data.Items = MenuItems;
            Data.DropdownMode = true;
            Data.RowEnabledExpression = (c) => c.EnabledExpression();
            Data.UseMultiCheck = MultiCheck.Off;
            Data.UseFilter = false;
            Data.UseHeader = false;

            Data.SelectedItemExpression = this.SelectedMenuItemExpression;
            Data.SelectedItemChanged = (i) => SelectedMenuItemChanged.InvokeAsync(i);


            if (DisplayIcons)
                Data.Columns.AddIcon(c => c.Icon);

            Data.Columns.Add(c => c.Caption, "none", 100);

        }

        private void FilterTextChanged(string filterString)
        {
            Data.FilterText = filterString;
        }

        private void CollapseClicked()
        {
            Collapsed = true;
        }

        private void ExpandClicked()
        {
            Collapsed = false;
        }

    }
}
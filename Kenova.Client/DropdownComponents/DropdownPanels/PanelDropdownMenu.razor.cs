using Microsoft.AspNetCore.Components;

namespace Kenova.Client.Components.Panels
{
    public partial class PanelDropdownMenu : KenovaDialogBase
    {

        HyperGrid<MenuItem> hyper_grid = null;

        private HyperData<MenuItem> Data = new();

        [Parameter]
        public MenuItemCollection MenuItems { get; set; }

        protected override void OnDialogInitialized()
        {
            bool use_icon_column = false;

            if (MenuItems != null && MenuItems.Count != 0 && MenuItems[0].Icon.IconKind != IconKind.None)
                use_icon_column = true;

            Data.Items = this.MenuItems;
            Data.DropdownMode = true;
            Data.UseMultiCheck = MultiCheck.Off;
            Data.UseFilter = false;
            Data.UseHeader = false;
            Data.RowEnabledExpression = (c) => c.EnabledExpression();
            //Data.SelectedItemChanged = SelectedItemChanged;

            if (use_icon_column)
                Data.Columns.AddIcon(c => c.Icon);

            Data.Columns.Add(c => c.Caption, "none", 100);

        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                //onsole.WriteLine("SETTING FOCUS FROM PANEL");
                _ = hyper_grid.SetFocusAsync();

            }


        }

        private void RowDoubleClicked(MenuItem item)
        {
            _ = this.CloseOkAsync(item);
        }

        //private void SelectedItemChanged(MenuItem item)
        //{
        //    int index = MenuItems.IndexOf(item);
        //    this.CloseOk(index);
        //}

    }
}

using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;

namespace Kenova.Client.SystemComponents
{
    public partial class PortalFlyoutPanel : KenovaDialogBase
    {

        HyperGrid<PortalMenuItem> hyper_grid = null;

        private HyperData<PortalMenuItem> Grid = new();

        //[Parameter]
        //public LayerDefinition LayerDefinition { get; set; }

        protected override void OnDialogInitialized()
        {
            bool use_icon_column = false;

            if (KenovaClientConfig.Settings.PortalMenuItems != null && KenovaClientConfig.Settings.PortalMenuItems.Count != 0 && KenovaClientConfig.Settings.PortalMenuItems[0].Icon.IconKind != IconKind.None)
                use_icon_column = true;

            Grid.Items = KenovaClientConfig.Settings.PortalMenuItems;
            Grid.DropdownMode = true;
            Grid.UseMultiCheck = MultiCheck.Off;
            Grid.UseFilter = false;
            Grid.UseHeader = false;

            Grid.TooltipExpression = c => c.Caption;
            Grid.RowEnabledExpression = (c) => c.EnabledExpression();

            if (use_icon_column)
                Grid.Columns.AddIcon(c => c.Icon);

            Grid.Columns.Add(c => c.Caption, "none", 100);

        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                //onsole.WriteLine("SETTING FOCUS FROM PANEL");
                //hyper_grid.SetFocus();

            }


        }

        private async ValueTask SelectItemAsync(PortalMenuItem item)
        {

            //int index = Portal.MenuItems.IndexOf(item);
            //LayerManager.Close(this, index);

            await this.CloseCancelAsync();

            await Portal.ProcessPortalMenuItemClickedAsync(NavigationManager, item);
        }

        private void HamburgerButtonClicked()
        {
            _ = this.CloseCancelAsync();
        }


    }


}

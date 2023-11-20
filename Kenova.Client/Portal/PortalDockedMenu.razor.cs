using Microsoft.AspNetCore.Components;

namespace Kenova.Client.Components
{
    public partial class PortalDockedMenu : KenovaComponentBase, IDisposable
    {
        [Parameter]
        public long ForceRender { get; set; }

        HyperGrid<PortalMenuItem> hyper_grid = null;

        private HyperData<PortalMenuItem> Data = new();

        protected override void OnInitialized()
        {
            Data.Items = KenovaClientConfig.Settings.PortalMenuItems;
            Data.DropdownMode = true;
            Data.UseMultiCheck = MultiCheck.Off;
            Data.UseFilter = false;
            Data.UseHeader = false;

            Data.TooltipExpression = c => c.Caption;
            Data.RowEnabledExpression = (c) => c.EnabledExpression();

            ResetColumns();

            Portal.ActivatePortalMenuWasCalled += Portal_ActivatePortalMenuWasCalled;
        }

        public void Dispose()
        {
            Portal.ActivatePortalMenuWasCalled -= Portal_ActivatePortalMenuWasCalled;
        }

        private void Portal_ActivatePortalMenuWasCalled()
        {
            _ = hyper_grid.SetFocusAsync();
        }

        private void ResetColumns()
        {
            Data.Columns.Clear();

            bool use_icon_column = false;

            if (KenovaClientConfig.Settings.PortalMenuItems != null && KenovaClientConfig.Settings.PortalMenuItems.Count != 0 && KenovaClientConfig.Settings.PortalMenuItems[0].Icon.IconKind != IconKind.None)
                use_icon_column = true;

            if (use_icon_column)
                Data.Columns.AddIcon(c => c.Icon);

            //if (!Portal.PortalMenuCollapsed)

            Data.Columns.Add(c => c.Caption, "", 100);

        }

        private void SelectItem(PortalMenuItem item)
        {
            //int index = Portal.MenuItems.IndexOf(item);

            //LayerManager.Close(this, index);

            _ = Portal.ProcessPortalMenuItemClickedAsync(NavigationManager, item);

            //return Task.CompletedTask;

        }

        private void CollapseClick()
        {
            KenovaClientConfig.Settings.PortalMenuCollapsed = !KenovaClientConfig.Settings.PortalMenuCollapsed;

            ResetColumns();

            _ = KenovaClientConfig.Settings.SaveSettingsAsync();

        }

    }
}

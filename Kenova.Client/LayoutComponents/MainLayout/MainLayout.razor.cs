using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kenova.Client.Components
{

    public partial class MainLayout : KenovaComponentBase, IRerender, IDisposable, INotifyPropertyChanged
    {
        private FieldLink<string> SelectedIdentifierFieldLink;

        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        [Parameter]
        public double Width { get; set; } = -1;

        [Parameter]
        public INotifyPropertyChanged RerenderTrigger { get; set; } = null;

        [Parameter]
        public bool DisplayHeader { get; set; } = true;

        [Parameter]
        public string Title { get; set; } = null;

        [Parameter]
        public string SubTitle { get; set; } = null;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// When set, the menu items list will be displayed.
        /// </summary>
        [Parameter]
        public MenuItemCollection MenuItems { get; set; }

        [Parameter]
        public Expression<Func<string>> SelectedIdentifierExpression { get; set; }

        [Parameter]
        public EventCallback<string> SelectedIdentifierChanged { get; set; }

        internal MenuItem ActiveMenuItem = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnInitialized()
        {
            if (LayerComponent == null)
                throw new InvalidOperationException($"The {this.GetType().Name} component must be placed inside a LayerBaseComponent");

            if (RerenderTrigger != null)
                RerenderTrigger.PropertyChanged += Model_PropertyChanged;

            SelectedIdentifierFieldLink = new FieldLink<string>(this, SelectedIdentifierExpression, false, FieldLinkChanged, SelectedIdentifierChanged);

            if (this.MenuItems != null)
                FieldLink2ActiveMenuItem();
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (KenovaClientConfig.Diagnostics)  Console.WriteLine("XXX RerenderModel_PropertyChanged");
            this.StateHasChanged();
        }

        public void Dispose()
        {
            SelectedIdentifierFieldLink.Dispose();

            if (RerenderTrigger != null)
                RerenderTrigger.PropertyChanged -= Model_PropertyChanged;
        }

        // This method converts the SelectedIdentifierFieldLink.Value identifier string to an ActiveMenuItem
        private void FieldLink2ActiveMenuItem()
        {
            var identifier = SelectedIdentifierFieldLink.Value;

            foreach (var item in this.MenuItems)
            {
                if (item.Identifier == identifier)
                {
                    if (ActiveMenuItem != item)
                    {
                        if (KenovaClientConfig.Diagnostics) Console.WriteLine("XXX ActiveMenuItem notify event");
                        ActiveMenuItem = item;
                        // The MainMenu components listens for this event.
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveMenuItem)));
                    }
                    return;
                }
            }

            if (this.MenuItems.Count > 0)
            {
                if (ActiveMenuItem != this.MenuItems[0])
                {
                    if (KenovaClientConfig.Diagnostics) Console.WriteLine("XXX ActiveMenuItem notify event");
                    ActiveMenuItem = this.MenuItems[0];
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveMenuItem)));
                }
                return;
            }

            if (ActiveMenuItem != null)
            {
                if (KenovaClientConfig.Diagnostics) Console.WriteLine("XXX ActiveMenuItem notify event");
                ActiveMenuItem = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveMenuItem)));
            }
        }

        // This method is called when the SelectedIdentifierFieldLink.Value is changed internally or externally
        private async void FieldLinkChanged()
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine($"XXX FieldLinkChanged to {this.SelectedIdentifierFieldLink.Value}, RENDERING!");

            if (this.MenuItems != null)
                FieldLink2ActiveMenuItem();

            await LayerComponent.RefreshAsync();

            if( ActiveMenuItem != null)
                await LayerComponent.PerformAutoFocusAsync(ActiveMenuItem.StagingAreaID);

        }

        // This method is called by the MainMenu component, when user interaction selected a different menu item.
        private void MainMenu_SelectedMenuItemChanged(MenuItem item)
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("XXX MainMenu_SelectedMenuItemChanged");

            //this.ActiveMenuItem = item;
            this.SelectedIdentifierFieldLink.Value = item.Identifier;

            //await LayerComponent.RefreshAsync();

            //LayerComponent.PerformAutoFocus(item.StagingAreaID);

        }

        public void Rerender()
        {
            Console.WriteLine("XXX Rerender");
            this.StateHasChanged();
        }

        internal MenuItem GetMenuItemForIdentifier(string identifier)
        {
            return this.MenuItems.FirstOrDefault(p => p.Identifier == identifier);
        }
    }
}

using Kenova.Client.Components.Panels;
using Kenova.Client.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq.Expressions;
using System.Text;

namespace Kenova.Client.Components
{
    public partial class TabComponent : KenovaComponentBase, IAsyncDisposable, IRerender
    {
        private string container_id = KenovaClientConfig.GetUniqueElementID();
        private string overflow_id = KenovaClientConfig.GetUniqueElementID();
        private StringBuilder _container_style = new StringBuilder(100);

        private ComponentWingman<TabComponent> _wingman = new();

        private FieldLink<string> SelectedTabFieldLink;

        /// <summary>
        /// For example "() => this.Model.ActiveTabIdentifier"
        /// </summary>
        [Parameter]
        public Expression<Func<string>> SelectedTabExpression { get; set; }

        [Parameter]
        public EventCallback<string> SelectedTabChanged { get; set; }

        [CascadingParameter]
        public KenovaDialogBase LayerComponent { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public TabItemCollection TabItems { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public string FocusID { get; set; }

        [Parameter]
        public int AutoFocusPriority { get; set; } = 100;


        [Parameter]
        public string AdditionalStyle { get; set; } = null;

        /// <summary>
        /// The default value for this parameter is HeightMode.Container
        /// </summary>
        [Parameter]
        public HeightMode HeightMode { get; set; } = HeightMode.Content;

        [Parameter]
        public double Height { get; set; } = -1;

        [Parameter]
        public double MaxHeight { get; set; } = -1;

        [Parameter]
        public double Width { get; set; } = -1;

        protected override void OnInitialized()
        {
            if (TabItems == null)
                throw new Exception("TabItems parameter must be set");

            if (LayerComponent == null)
                throw new InvalidOperationException($"The {this.GetType().Name} component must be placed inside a LayerBaseComponent");

            LayerComponent.RegisterComponent(this);

            SelectedTabFieldLink = new FieldLink<string>(this, SelectedTabExpression, false, FieldLinkValueChanged, SelectedTabChanged);

        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _wingman.InvokeVoidAsync("Stop");
            await _wingman.DisposeAsync();

            SelectedTabFieldLink.Dispose();

            LayerComponent.UnregisterComponent(this);
        }

        protected override void OnParametersSet()
        {
            if (TabItems.Count == 0)
                throw new InvalidOperationException("There must be at least one tab item");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await this.Mutex.WaitAsync();

                if (firstRender)
                {
                    await _wingman.InstantiateAsync(this,"TabComponent");

                    int index = this.getTabIndex(this.SelectedTabFieldLink.Value);

                    if (index == -1)
                        index = 0;

                    await _wingman.InvokeVoidAsync("Start", container_id, index);

                    LayerComponent.RegisterFirstRenderComplete(this); // must be at the end of OnAfterRender
                }

            }
            finally
            {
                this.Mutex.Release();
            }

        }

        private bool DropdownOpen = false;
        private LayerDefinition<PanelDropdownMenu> _ld = null;

        public void FieldLinkValueChanged()
        {
            if (_wingman != null)
            {
                //this.StateHasChanged();

                int index = this.getTabIndex(this.SelectedTabFieldLink.Value);

                if (index == -1)
                    index = 0;

                _ = _wingman.InvokeVoidAsync("SetSelectedIndex", index);
            }
        }

        [JSInvokable]
        public async ValueTask OnOverflowClicked()
        {
            if (this.DropdownOpen == true)
            {
                await _ld.CloseCancelAsync(); // Close the layer
                return;
            }

            var ddItems = new MenuItemCollection();

            for (int index = 0; index < this.TabItems.Count; index++)
            {
                var item = this.TabItems[index];
                ddItems.Add(item.Caption, null, item.EnabledExpression);
                ddItems.Tag = item;
            }

            this.DropdownOpen = true;

            _ld = new LayerDefinition<PanelDropdownMenu>
            {
                Kind = LayerKind.Dropdown,
                OwnerID = overflow_id,
                AfterClosed = LayerDefinition_DropdownWasClosed,
                [i => i.MenuItems] = ddItems
            };

            _ld.Parameter(p => p.LayerDefinition, _ld); // not possible to refer to self in object initializer syntax

            await _ld.OpenNonBlockingAsync();

        }

        /// <summary>
        /// This method is called by LayerManager after closing the dropdown.
        /// A dropdown is closed by calling LayerManager.Close()
        /// When clicking outside the dropdown element and outside dropdown panel, the 
        /// LayerManager.Close() will be called automatically.
        /// </summary>
        private void LayerDefinition_DropdownWasClosed(LayerResult result)
        {
            if (KenovaClientConfig.Diagnostics) Console.WriteLine("XXXXXX LayerDefinition_DropdownWasClosed ");

            this.DropdownOpen = false;

            _ld = null;

            if (result.Data == null)
            {
                return;
            }

            var menuItem = (MenuItem)result.Data;

            var tabItem = (TabItem)menuItem.Tag;

            this.SelectedTabFieldLink.Value = tabItem.Identifier;

            this.StateHasChanged();
        }

    }
}
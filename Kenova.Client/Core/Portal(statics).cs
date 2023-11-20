using Kenova.Client.Core;
using Microsoft.AspNetCore.Components;
using System;

namespace Kenova.Client.Components
{

    // Contains the static methods and static variables for the Portal class.

    public delegate void RefreshWasCalledEventHandler();
    public delegate void ActivatePortalMenuEventHandler();
    //public delegate void ActiveLayerComponentChangedEventHandler(LayerComponentBase component);

    public static class Portal
    {
        /// <summary>
        /// This event is fired when Portal.Refresh() is called. 
        /// The PortalRoot components hooks into the event and executes a StateHasChanged when called.
        /// </summary>
        public static event RefreshWasCalledEventHandler RefreshWasCalled;
        public static event RefreshWasCalledEventHandler TopbarRefreshWasCalled;
        public static event RefreshWasCalledEventHandler BreadcrumbRefreshWasCalled;
        public static event ActivatePortalMenuEventHandler ActivatePortalMenuWasCalled;

        private const string DEFAULT_BREADCRUMB_TEXT = "(breadcrumb not set)";
        private static string _routed_page_breadcrumb = DEFAULT_BREADCRUMB_TEXT;
        private static string _routed_page_routepath;
        private static object _routed_page_component_reference;

        //private static PortalModules _modules = new();

        internal static async ValueTask ProcessPortalMenuItemClickedAsync(NavigationManager navigation, PortalMenuItem item)
        {

            if (item.URL != null)
            {
                if (KenovaClientConfig.Diagnostics) Console.WriteLine($"ProcessPortalMenu - performing a NavigateTo {item.URL}");

                // werkt niet! var nm2 = KenovaClientConfig.ServiceProvider.GetService(typeof(NavigationManager)) as NavigationManager;
                await navigation.PortalNavigateToAsync(item.URL);
            }
            else if (item.ComponentType != null)
            {
                // Nav by ComponentType means that the Component will be opened as a layer of the already
                // open component.

                if (KenovaClientConfig.Diagnostics) Console.WriteLine($"ProcessPortalMenu - performing a nav by component type ({item.ComponentType.Name})");

                if (LayerManager.LayerStack.Count == 1 && LayerManager.LayerStack.Peek().ComponentType.Equals(item.ComponentType))
                {
                    // already open, do nothing
                    return;

                    // Missing functionality: When the layerstack is 2 or higher, then remove all layers
                    // until only 1 layer left.
                    // In other words: Close all extra overlays.
                }

                await LayerManager.CloseAllAsync();

                var ld = new LayerDefinition
                {
                    Kind = LayerKind.ModalWindow,
                    ComponentType = item.ComponentType
                };

                await ld.OpenNonBlockingAsync();

            }

        }

        public static void ActivatePortalMenu()
        {
            ActivatePortalMenuWasCalled?.Invoke();
        }

        public static void Refresh()
        {
            RefreshWasCalled?.Invoke();
        }

        public static void RefreshTopbar()
        {
            TopbarRefreshWasCalled?.Invoke();
        }

        public static void RefreshBreadcrumb()
        {
            BreadcrumbRefreshWasCalled?.Invoke();
        }

        //internal static void SetRootComponentReference(object component)
        //{
        //    _root_component_reference = component;

        //    //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"KenovaRouteView registered type {component?.GetType().Name} with Portal.SetRootComponentReference()");

        //}

        //public static LayerComponentBase RootLayerComponent
        //{
        //    get { return _root_component_reference as LayerComponentBase; }
        //}

        //internal static bool IsRoutedPage(LayerComponentBase component)
        //{
        //    if (component == null)
        //        throw new ArgumentNullException("component");

        //    if (_root_component_reference != null && _root_component_reference.Equals(component))
        //        return true;

        //    return false;
        //}

        /// <summary>
        /// Do not call this method before the component has rendered for the first time.
        /// This method depends on the component reference being set correctly.
        /// The component reference is either set by: KenovaRouteView.cs or PortalRoot.cs
        /// </summary>
        /// <param name="component"></param>
        internal static void UpdateBreadCrumbFor(KenovaDialogBase component)
        {
            if (component == null)
                return;

            if (component.IsOpenedAsRoutedPage)
            {
                _routed_page_component_reference = component;
                _routed_page_routepath = component.GetRoutePath();
            }

            if (_routed_page_component_reference != null && _routed_page_component_reference.Equals(component))
            {
                if (!string.IsNullOrEmpty(component.Breadcrumb))
                {
                    _routed_page_breadcrumb = component.Breadcrumb;
                }
                else
                {
                    _routed_page_breadcrumb = DEFAULT_BREADCRUMB_TEXT;
                }

                //if (KenovaClientConfig.Diagnostics) Console.WriteLine($"Recognized UpdateBreadCrumb parameter as the root component. Updating BreadCrumb for {component.GetType().Name} text {component.Breadcrumb}");

            }

            BreadcrumbRefreshWasCalled?.Invoke();
        }

        //internal static void RegisterLayerComponent(LayerComponentBase component)
        //{
        //    if (KenovaClientConfig.Diagnostics) Console.WriteLine($"Register LayerComponentBase {component.GetType().Name}");

        //}

        //internal static void UnregisterLayerComponent(LayerComponentBase component)
        //{
        //    if (KenovaClientConfig.Diagnostics) Console.WriteLine($"Unregister LayerComponentBase {component.GetType().Name}");
        //}

        /// <summary>
        /// The starting breadcrumb.
        /// </summary>
        internal static string RoutedPage_Breadcrumb
        {
            get { return _routed_page_breadcrumb; }
        }

        internal static string RoutedPage_RoutePath
        {
            get { return _routed_page_routepath; }
        }

        //internal static PortalModules Modules
        //{
        //    get { return _modules; }
        //}

    }

    public enum PortalMenuMode
    {
        Flyout,
        Docked
    }

}

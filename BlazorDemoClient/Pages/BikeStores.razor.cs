using BlazorDemo.Client.Components;
using Kenova.WebAssembly.Client;
using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.Client.Pages
{

    public partial class BikeStores : LayerComponentBase
    {
        private readonly BikeStoresModel Model = new BikeStoresModel();

        private MenuItemCollection MenuItems = new MenuItemCollection();

        private string Title = "Bike Stores Demo";

        protected override void OnLayerInitialized()
        {
            Breadcrumb = Title;


            //MenuItems.Add("overview", "Overview", null, IconKind.FontAwesome, "far fa-globe");
            MenuItems.Add("Customers", "/customers", null, IconKind.FontAwesome, "fas fa-user-friends");
            MenuItems.Add("Orders", "/orders", null, IconKind.FontAwesome, "fas fa-money-check-edit-alt");
            MenuItems.Add("Products", "/products", null, IconKind.FontAwesome, "fas fa-box");

        }

        private void TileClicked(MenuItem item)
        {
            //var ld = new LayerDefinition(LayerKind.ModalFullsize);

            //if (item.FocusID == "customers")
            //{
            //    ld.Breadcrumb = "Customers";
            //    LayerManager.Open<SearchCustomers>(ld);
            //}
            //else if (item.FocusID == "orders")
            //{
            //    ld.Breadcrumb = "Orders";
            //    LayerManager.Open<SearchOrders>(ld);
            //}

        }
    }

    public class BikeStoresModel : ModelTypedBase<BikeStoresModel>
    {



    }

}

using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.Client.Pages;

public partial class BikeStores : KenovaDialogBase
{
    private MenuItemCollection MenuItems = new MenuItemCollection();

    private string Title = "Bike Stores Demo";

    protected override void OnDialogInitialized()
    {
        Breadcrumb = Title;

        MenuItems.Add("Customers", "/customers", null, IconKind.FontAwesome, "fal fa-user-friends");
        MenuItems.AutoFocus = true;

        MenuItems.Add("Orders", "/orders", null, IconKind.FontAwesome, "fal fa-money-check-edit-alt");
        MenuItems.Add("Products", "/products", null, IconKind.FontAwesome, "fal fa-box");

    }

}

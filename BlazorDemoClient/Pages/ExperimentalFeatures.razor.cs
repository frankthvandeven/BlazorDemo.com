using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.Client.Pages;

public partial class ExperimentalFeatures : KenovaDialogBase
{
    private MenuItemCollection MenuItems = new MenuItemCollection();

    private string Title = "Experimental Features";

    protected override void OnDialogInitialized()
    {
        Breadcrumb = Title;

        MenuItems.Add("Treeview", "/treeview", null, IconKind.FontAwesome, "fal fa-folder-tree");
        MenuItems.Icon.HtmlColor = "forestgreen";
        MenuItems.AutoFocus = true;

        //MenuItems.Add("WorkflowTest", "/workflowtest", null, IconKind.FontAwesome, "fal fa-bezier-curve");
        //MenuItems.Icon.HtmlColor = "#15aabf";

        //MenuItems.Add("Push Messages", "/pushtest", null, IconKind.FontAwesome, "fal fa-comments");
        //MenuItems.Icon.HtmlColor = "darkblue";

        //PortalMenuItems.Add("WindowedTest", typeof(BlazorDemo.Client.Components.SearchCustomers), null, IconKind.FontAwesome, "fal fa-window");
        //PortalMenuItems.Icon.HtmlColor = "DarkGray";

    }

}

using Kenova.WebAssembly.Client.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.Client.Pages
{
    public partial class Index : LayerComponentBase
    {
        private MenuItemCollection MenuItems = new MenuItemCollection();

        protected override void OnLayerInitialized()
        {
            Breadcrumb = "Home";

            MenuItems.Add("All Kenova Controls", "/allcontrols", null, IconKind.FontAwesome, "fal fa-list-alt");
            MenuItems.AutoFocus = true;
            MenuItems.Icon.HtmlColor = "darkorange";

            MenuItems.Add("HyperGrid Samples", "/hypergridsamples", null, IconKind.FontAwesome, "fal fa-th");
            MenuItems.Icon.HtmlColor = "red";

            MenuItems.Add("Treeview Demo", "/treeview", null, IconKind.FontAwesome, "fal fa-folder-tree");
            MenuItems.Icon.HtmlColor = "forestgreen";

            MenuItems.Add("Bike Stores Demo", "/bikestores", null, IconKind.FontAwesome, "fal fa-bicycle");
            MenuItems.Icon.HtmlColor = "rebeccapurple";

            //MenuItems.Add("/pharmacy", "Pharmacy Demo", null, IconKind.FontAwesome, "fal fa-file-prescription");
            //MenuItems.Icon.HtmlColor = "forestgreen";

            //MenuItems.Add("vector","Vector", true, IconKind.Vector);
            //MenuItems.Icon.IconData = "<svg viewBox=\"0 0 448 512\"><path d=\"M400 64c8.8 0 16 7.2 16 16v352c0 8.8-7.2 16-16 16H48c-8.8 0-16-7.2-16-16V80c0-8.8 7.2-16 16-16h352m0-32H48C21.5 32 0 53.5 0 80v352c0 26.5 21.5 48 48 48h352c26.5 0 48-21.5 48-48V80c0-26.5-21.5-48-48-48zm-60 206h-98v-98c0-6.6-5.4-12-12-12h-12c-6.6 0-12 5.4-12 12v98h-98c-6.6 0-12 5.4-12 12v12c0 6.6 5.4 12 12 12h98v98c0 6.6 5.4 12 12 12h12c6.6 0 12-5.4 12-12v-98h98c6.6 0 12-5.4 12-12v-12c0-6.6-5.4-12-12-12z\"/></svg>";

        }

        private void TileClicked(MenuItem item)
        {
            //NavigationManager.NavigateTo(item.FocusID);

        }
    }
}





/*
          Dictionary<string, DateRange> DateRanges => new Dictionary<string, DateRange> {
            { "This month", new DateRange
                {
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                    End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddTicks(-1)
                }
            } ,
            { "Previous month" , new DateRange
                {
                    Start = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1),
                    End = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1).AddMonths(1).AddTicks(-1)
                }
            }
     };
*/

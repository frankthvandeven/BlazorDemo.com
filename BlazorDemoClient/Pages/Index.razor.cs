using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorDemo.Client.Pages
{
    public partial class Index : KenovaDialogBase
    {
        private MenuItemCollection MenuItems = new MenuItemCollection();

        private List<Pet> pets = new()
        {
            new Pet { PetId = 0, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 1, Name = "Salem Saberhagen" },
            new Pet { PetId = 2, Name = "K-9" },
            new Pet { PetId = 3, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 4, Name = "Salem Saberhagen" },
            new Pet { PetId = 5, Name = "K-9" },
            new Pet { PetId = 6, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 7, Name = "Salem Saberhagen" },
            new Pet { PetId = 8, Name = "K-9" },
            new Pet { PetId = 9, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 10, Name = "Salem Saberhagen" },
            new Pet { PetId = 11, Name = "K-9" },
            new Pet { PetId = 12, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 13, Name = "Salem Saberhagen" },
            new Pet { PetId = 14, Name = "K-9" },
            new Pet { PetId = 15, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 16, Name = "Salem Saberhagen" },
            new Pet { PetId = 17, Name = "K-9" },
            new Pet { PetId = 18, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 19, Name = "Salem Saberhagen" },
            new Pet { PetId = 20, Name = "K-9" },
            new Pet { PetId = 21, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 22, Name = "Salem Saberhagen" },
            new Pet { PetId = 23, Name = "K-9" },
            new Pet { PetId = 24, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 25, Name = "Salem Saberhagen" },
            new Pet { PetId = 26, Name = "K-9" },
            new Pet { PetId = 27, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 28, Name = "Salem Saberhagen" },
            new Pet { PetId = 29, Name = "K-9" },
            new Pet { PetId = 30, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 31, Name = "Salem Saberhagen" },
            new Pet { PetId = 32, Name = "K-9" },
            new Pet { PetId = 33, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 34, Name = "Salem Saberhagen" },
            new Pet { PetId = 35, Name = "K-9" },
            new Pet { PetId = 36, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 37, Name = "Salem Saberhagen" },
            new Pet { PetId = 38, Name = "K-9" },
            new Pet { PetId = 39, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 40, Name = "Salem Saberhagen" },
            new Pet { PetId = 41, Name = "K-9" },
            new Pet { PetId = 42, Name = "Mr. Bigglesworth" },
            new Pet { PetId = 43, Name = "Salem Saberhagen" },
            new Pet { PetId = 44, Name = "K-9" }
        };

        protected override void OnDialogInitialized()
        {
            Breadcrumb = "Home";

            MenuItems.Add("All Kenova Controls", "/allcontrols", null, IconKind.FontAwesome, "fal fa-list-alt");
            MenuItems.AutoFocus = true;
            MenuItems.Icon.HtmlColor = "darkorange";

            MenuItems.Add("HyperGrid Samples", "/hypergridsamples", null, IconKind.FontAwesome, "fal fa-th");
            MenuItems.Icon.HtmlColor = "red";

            MenuItems.Add("Bike Stores", "/bikestores", null, IconKind.FontAwesome, "fal fa-bicycle");
            MenuItems.Icon.HtmlColor = "rebeccapurple";

            MenuItems.Add("Experimental", "/experimental", null, IconKind.FontAwesome, "fad fa-flask");
            MenuItems.Icon.HtmlColor = "#15aabf";

            //MenuItems.Add("/pharmacy", "Pharmacy Demo", null, IconKind.FontAwesome, "fal fa-file-prescription");
            //MenuItems.Icon.HtmlColor = "forestgreen";

            //MenuItems.Add("vector","Vector", true, IconKind.Vector);
            //MenuItems.Icon.IconData = "<svg viewBox=\"0 0 448 512\"><path d=\"M400 64c8.8 0 16 7.2 16 16v352c0 8.8-7.2 16-16 16H48c-8.8 0-16-7.2-16-16V80c0-8.8 7.2-16 16-16h352m0-32H48C21.5 32 0 53.5 0 80v352c0 26.5 21.5 48 48 48h352c26.5 0 48-21.5 48-48V80c0-26.5-21.5-48-48-48zm-60 206h-98v-98c0-6.6-5.4-12-12-12h-12c-6.6 0-12 5.4-12 12v98h-98c-6.6 0-12 5.4-12 12v12c0 6.6 5.4 12 12 12h98v98c0 6.6 5.4 12 12 12h12c6.6 0 12-5.4 12-12v-98h98c6.6 0 12-5.4 12-12v-12c0-6.6-5.4-12-12-12z\"/></svg>";

        }

        //private void XRowClicked(Pet pet)
        //{
        //    NavigationManager.PortalNavigateToAsync("/zzzzz");
        //}

        private void TileClicked(MenuItem item)
        {
            //NavigationManager.PortalNavigateTo(item.FocusID);

        }
    }

    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; }
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

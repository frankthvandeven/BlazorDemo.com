using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorDemo.Client.Pages
{
    public partial class HyperGridSamples : KenovaDialogBase
    {
        private readonly DemoModel Model = new DemoModel();
        private MenuItemCollection MenuItems = new MenuItemCollection();

        protected override void OnDialogInitialized()
        {
            Breadcrumb = "HyperGrid Samples";

            MenuItems.Add("Introduction", "intro", null, IconKind.FontAwesome, "fal fa-medal");

            MenuItems.Add("MultiCheck", "multicheck", null, IconKind.FontAwesome, "fas fa-check-square");


            foreach (var item in MenuItems)
                item.Icon.HtmlColor = "red";

        }

        private void CheckedItemsChanged()
        {

        }


        IList<Item> ExpandedNodes = new List<Item>();
        //Item selectedNode;

        private IEnumerable<Item> GetItems()
        {
            IEnumerable<Item> Items = new[]
            {
                new Item { Text = "Item 1" },
                new Item {
                    Text = "Item 2",
                    Children = new []
                    {
                        new Item { Text = "Item 2.1" },
                        new Item { Text = "Item 2.2", Children = new []
                        {
                            new Item { Text = "Item 2.2.1" },
                            new Item { Text = "Item 2.2.2" },
                            new Item { Text = "Item 2.2.3" },
                            new Item { Text = "Item 2.2.4" }
                        }
                    },
                    new Item { Text = "Item 2.3" },
                    new Item { Text = "Item 2.4" }
                    }
                },
                new Item { Text = "Item 3" },
            };

            return Items;
        }

    }

    public class Item
    {
        public string Text { get; set; }
        public IEnumerable<Item> Children { get; set; }
    }

}


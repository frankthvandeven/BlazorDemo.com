using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Pages
{

    [ViewModel]
    public partial class TreeViewDemoModel
    {
        public SearchCustomersRecordset Recordset = new();

        private string __Name;
        private string __City;
        private decimal __Amount;
        private string __Description;

        public TreeViewData Data = new();

        public TreeViewDemoModel()
        {
            Register(m => m.Name);
            Register(m => m.City);
            Register(m => m.Amount);
            Register(m => m.Description);

            Name = "Frank Th. van de Ven";
            City = "Maastricht";
            Amount = 1234.56m;
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
        }

        public async Task LoadTreeviewTask()
        {

#if NEVERCOMPILE
            Data.FetchOrCreateFolder("Level0-d\\Level1-a");
            Data.FetchOrCreateFolder("Level0-d");
            Data.FetchOrCreateFolder("Level0-c\\Level1-a");
            Data.FetchOrCreateFolder("Level0-c\\Level1-a\\Level2-a");

            Data.AddDataItem("", "Item In The Root", new TestData { });
            Data.AddDataItem("", "Another Item In The Root", new TestData { });
            Data.AddDataItem("Level0-a\\Software", "Level0-Software", new TestData { });
            Data.AddDataItem("Level0-a\\Software", "Level0-Software", new TestData { });
            Data.AddDataItem("Level0-a\\Software", "Level0-Software", new TestData { });
            Data.AddDataItem("Level0-a\\Software", "Level0-Software", new TestData { });

            Data.AddDataItem("Level0-b\\Software", "Level1-Software", new TestData { });
            Data.AddDataItem("Level0-b\\Software", "Level1-Software", new TestData { });
            Data.AddDataItem("Level0-b\\Software", "Level1-Software", new TestData { });
            Data.AddDataItem("Level0-b\\Software", "Level1-Software", new TestData { });

#endif

            Recordset.RowLimit = 3000; // 300; // 100000;

            await Recordset.ExecSqlAsync(null, null);

            foreach (var cust in Recordset)
            {
                Data.AddDataItem($"Customers\\{cust.state}\\{cust.city}", cust.last_name + " " + cust.first_name, cust);
            }

        }


    }

}

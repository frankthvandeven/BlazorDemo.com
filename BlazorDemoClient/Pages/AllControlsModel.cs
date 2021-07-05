using Kenova.WebAssembly.Client.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Pages
{

    [ViewModel]
    public partial class AllControlsModel
    {

        private string __Name;
        private string __City;
        private decimal __Amount;
        private string __Description;

        public AllControlsModel()
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


    }
}

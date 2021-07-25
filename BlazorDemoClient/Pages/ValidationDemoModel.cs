using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Pages
{

    [ViewModel]
    public partial class ValidationDemoModel
    {
        public SearchCustomersRecordset Recordset = new();

        private string __Name;
        private string __Name2;
        private string __Name3;
        private string __Name4;
        private string __City;
        private decimal __Amount;
        private string __Description;

        public ValidationDemoModel()
        {
            Register(m => m.Name);
            Register(m => m.Name2);
            Register(m => m.Name3);
            Register(m => m.Name4);
            Register(m => m.City);
            Register(m => m.Amount);
            Register(m => m.Description);

            Name = "Frank Th. van de Ven";
            Name2 = "Frank Th. van de Ven";
            Name3 = "Frank Th. van de Ven";
            Name4 = "Frank Th. van de Ven";
            City = "Maastricht";
            Amount = 1234.56m;
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
        }

        protected override async Task ValidateEventAsync(ValidateEventArgs<ValidationDemoModel> e)
        {
            if (e.IsMember(m => m.Name))
            {
                await Task.Delay(2000);
                e.RemarkText = $"Timer ticks {DateTime.Now.Ticks}";
                return;
            }

            if (e.IsMember(m => m.Name2))
            {
                await Task.Delay(2000);
                e.RemarkText = $"Timer ticks {DateTime.Now.Ticks}";
                return;
            }

            if (e.IsMember(m => m.Name3))
            {
                await Task.Delay(2000);
                e.RemarkText = $"Timer ticks {DateTime.Now.Ticks}";
                return;
            }

            if (e.IsMember(m => m.Name4))
            {
                await Task.Delay(2000);
                e.RemarkText = $"Timer ticks {DateTime.Now.Ticks}";
                return;
            }

        }

        public async Task LoadTreeviewTask()
        {
            await Task.CompletedTask;
        }

    }
}

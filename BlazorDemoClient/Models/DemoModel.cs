using Kenova.WebAssembly.Client.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorDemo.Client
{
    [ViewModel]
    public partial class DemoModel
    {
        private string __CurrentOption = "one";
        private bool __PumpActive = true;

        public string SelectedString = "Fred Pietersen";
        public DateTime SelectedDate = DateTime.Now;
        public Decimal SelectedDecimal = 7998.76m;


        private string __name;


        public bool SomeBooleanProperty { get; set; }
        public DateTime? SomeDateTimeProperty { get; set; }
        public int SomeIntegerProperty { get; set; }
        public decimal SomeDecimalProperty { get; set; }
        public string SomeStringProperty { get; set; }
        public string SomeMultiLineStringProperty { get; set; }
        public SomeStateEnum SomeSelectProperty { get; set; } = SomeStateEnum.Active;

        public DemoModel()
        {
            Register(m => m.CurrentOption);
            Register(m => m.SelectedString);
            Register(m => m.SelectedDate);
            Register(m => m.SelectedDecimal);
            Register(m => m.PumpActive);
        }

        protected override async Task ValidateEventAsync()
        {
            if (e.IsMember(m => m.CurrentOption))
            {
                await Task.CompletedTask;
                return;
            }

            if (e.IsMember(m => m.PumpActive))
            {
                await Task.CompletedTask;
                return;
            }
        }

    }

    public enum SomeStateEnum
    {
        Pending,
        Active,
        Suspended
    }
}

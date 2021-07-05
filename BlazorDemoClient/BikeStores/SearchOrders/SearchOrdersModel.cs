using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class SearchOrdersModel : ModelTypedBase<SearchOrdersModel>
    {
        public SearchOrdersRecordset Recordset = new SearchOrdersRecordset();

        public int? __SearchOrderID = null;
        public string __SearchText = null;

        public SearchOrdersModel()
        {
            Register(m => m.SearchOrderID);
            Register(m => m.SearchText);
        }

        protected override async Task ValidateEventAsync()
        {
            //if (e.IsMember(m => m.first_name))
            //{
            //    Console.WriteLine($"The value of first_name was changed to {this.first_name}");
            //}
            //else if (e.IsMember(m => m.last_name))
            //{
            //    Console.WriteLine($"The value of last_name was changed to {this.last_name}");
            //}

            await Task.CompletedTask;
            
        }

        public async Task SearchExecTask()
        {
            Recordset.RowLimit = 99999;

            await Recordset.ExecSqlAsync(this.SearchText, this.SearchOrderID);
        }



    }
}

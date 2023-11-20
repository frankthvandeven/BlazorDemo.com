using BlazorDemo.Client.VenturaRecordsets;
using Kenova.Client;
using Kenova.Client.Components;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class SearchOrdersModelEF : ModelTypedBase<SearchOrdersModelEF>
    {
        public ObservableCollection<SearchOrdersRecordEF> Recordset = new ObservableCollection<SearchOrdersRecordEF>();
        public bool LookupMode = false;

        public int? __SearchOrderID = null;
        public string __SearchText = null;

        public SearchOrdersRecordEF CurrentRecord = null;

        public async Task SearchExecTask()
        {
            //Recordset.RowLimit = 2000;

            //await Recordset.ExecSqlAsync(this.SearchText, this.SearchOrderID);

            Recordset = await KenovaHttp.PostJsonAsync<ObservableCollection<SearchOrdersRecordEF>>("api/orders/search", new { this.SearchOrderID, this.SearchText  });


        }



    }
}

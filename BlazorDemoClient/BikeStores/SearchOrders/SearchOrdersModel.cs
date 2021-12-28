using BlazorDemo.Client.VenturaRecordsets;
using Kenova.Client.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class SearchOrdersModel : ModelTypedBase<SearchOrdersModel>
    {
        public SearchOrdersRecordset Recordset = new SearchOrdersRecordset();
        public bool LookupMode = false;

        public int? __SearchOrderID = null;
        public string __SearchText = null;

        public async Task SearchExecTask()
        {
            Recordset.RowLimit = 2000;

            await Recordset.ExecSqlAsync(this.SearchText, this.SearchOrderID);
        }



    }
}

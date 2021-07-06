using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class SearchProductsModel : ModelTypedBase<SearchProductsModel>
    {
        public SearchProductsRecordset Recordset = new SearchProductsRecordset();
        public bool LookupMode = false;

        public int? __SearchProductID = null;
        public string __SearchText = null;

        public async Task SearchExec()
        {
            Recordset.RowLimit = 2000;

            await Recordset.ExecSqlAsync(this.SearchText, this.SearchProductID);
        }

    }
}

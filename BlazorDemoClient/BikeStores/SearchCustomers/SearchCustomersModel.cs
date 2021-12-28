using BlazorDemo.Client.VenturaRecordsets;
using Kenova.Client.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{

    [ViewModel]
    public partial class SearchCustomersModel : ModelTypedBase<SearchCustomersModel>
    {
        public SearchCustomersRecordset Recordset = new SearchCustomersRecordset();
        public bool LookupMode = false;

        private string __SearchName = null;
        private int? __SearchCustomerId = null;

        public async Task SearchExec()
        {
            Recordset.RowLimit = 2000;

            await Recordset.ExecSqlAsync(this.SearchName, this.SearchCustomerId);

        }

    }
}

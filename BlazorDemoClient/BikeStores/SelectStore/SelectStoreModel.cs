using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.WebAssembly.Client.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class SelectStoreModel : ModelTypedBase<SelectStoreModel>
    {
        public GetAll_sales_stores_Recordset Recordset = new();

        private int __store_id;

        public async Task SearchExec()
        {
            Recordset.RowLimit = 2000;

            await Recordset.ExecSqlAsync();
        }

    }
}

using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.Client.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class SelectStaffModel : ModelTypedBase<SelectStaffModel>
    {
        public GetAll_sales_staffs_Recordset Recordset = new();

        private int __staff_id;

        public async Task SearchExec()
        {
            Recordset.RowLimit = 2000;

            await Recordset.ExecSqlAsync();
        }

    }
}

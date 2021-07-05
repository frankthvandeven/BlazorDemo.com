using System.Threading.Tasks;
using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.WebAssembly.Client.Components;

namespace BlazorDemo.Client
{
    public static class Validators
    {

        public static async Task ValidateCustomerID(ValidateEventArgs e, int? customer_id)
        {
            if (customer_id == null || customer_id == 0)
            {
                e.RemarkText = "Customer ID is missing.";
                return;
            }

            var rs = new PriKey_sales_customers_Recordset();

            await rs.ExecSqlAsync(customer_id);

            if (rs.RecordCount == 0)
            {
                e.RemarkText = $"Customer id {customer_id} not found.";
                return;
            }

            e.RemarkText = rs.first_name + " " + rs.last_name;
            e.IsValid = true;
        }

    }
}

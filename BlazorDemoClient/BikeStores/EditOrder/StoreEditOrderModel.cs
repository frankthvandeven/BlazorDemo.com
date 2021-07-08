using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.WebAssembly.Client.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class StoreEditOrderModel : ModelTypedBase<StoreEditOrderModel>
    {
        public PriKey_sales_orders_Recordset Recordset = new();

        public bool CreateNew = false;

        private int __order_id;
        private int? __customer_id;
        private byte __order_status;
        private DateTime __order_date;
        private DateTime __required_date;
        private DateTime? __shipped_date;
        private int __store_id;
        private int __staff_id;



        public ListItemCollection<byte> Statusses = new();

        public StoreEditOrderModel()
        {

            Statusses.Add(1, "Pending");
            Statusses.Add(2, "Processing");
            Statusses.Add(3, "Rejected");
            Statusses.Add(4, "Completed");

            Register(m => m.customer_id);
            //Register(m => m.order_status);
            //Register(m => m.order_date);
            Register(m => m.required_date);
            Register(m => m.shipped_date);
            Register(m => m.store_id);
            Register(m => m.staff_id);
        }

        protected override async Task ValidateEventAsync(ValidateEventArgs<StoreEditOrderModel> e)
        {

            if (e.IsMember(m => m.customer_id))
            {
                await Validators.ValidateCustomerID(e, this.customer_id);
                return;
            }

            if (e.IsMember(m => m.order_status))
            {
                await Task.CompletedTask;
                e.RemarkText = $"The value of order_status was changed to {this.order_status}";
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.order_date))
            {
                await Task.CompletedTask;
                e.RemarkText = $"The value of order_date was changed to {this.order_date}";
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.required_date))
            {
                await Task.CompletedTask;
                e.RemarkText = $"The value of required_date was changed to {this.required_date}";
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.shipped_date))
            {
                await Task.CompletedTask;
                e.RemarkText = $"The value of shipped_date was changed to {this.shipped_date}";
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.store_id))
            {
                if (store_id is not > 0)
                {
                    e.RemarkText = "Store is required.";
                    return;
                }

                PriKey_sales_stores_Recordset rs = new();

                await rs.ExecSqlAsync(store_id);

                if (rs.RecordCount == 0)
                {
                    e.RemarkText = $"Store code {store_id} is unknown.";
                    return;
                }

                e.RemarkText = rs.store_name;
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.staff_id))
            {
                if (staff_id is not > 0)
                {
                    e.RemarkText = "Staff id is required.";
                    return;
                }

                PriKey_sales_staffs_Recordset rs = new();

                await rs.ExecSqlAsync(staff_id);

                if (rs.RecordCount == 0)
                {
                    e.RemarkText = $"Staff code {staff_id} is unknown.";
                    return;
                }

                e.RemarkText = rs.first_name + " " + rs.last_name;
                e.IsValid = true;
                return;
            }

        }

        public async Task LoadTask()
        {
            if (this.CreateNew)
            {
                this.order_id = 0;
                this.customer_id = null;
                this.order_status = 0;
                this.order_date = DateTime.Today;
                this.required_date = DateTime.Today;
                this.shipped_date = null;
                this.store_id = 0;
                this.staff_id = 0;
            }
            else
            {
                // edit
                await Recordset.ExecSqlAsync(this.order_id);

                if (Recordset.RecordCount == 0)
                    throw new Exception($"Order {this.order_id} not found in database.");

                this.order_id = Recordset.order_id;
                this.customer_id = Recordset.customer_id;
                this.order_status = Recordset.order_status;
                this.order_date = Recordset.order_date;
                this.required_date = Recordset.required_date;
                this.shipped_date = Recordset.shipped_date;
                this.store_id = Recordset.store_id;
                this.staff_id = Recordset.staff_id;

            }
        }

        public async Task SaveTask()
        {
            bool valid = await this.ValidateAllAsync();

            if (!valid)
                throw new Exception("Correct input.");

            if (this.CreateNew)
            {
                Recordset.Append();
                // skipped primary key column Recordset.order_id as it is AutoIncrement
            }
            else
            {
                await Recordset.ExecSqlAsync(this.order_id);

                if (Recordset.RecordCount == 0)
                    throw new Exception($"Order {this.order_id} not found in database.");
            }

            Recordset.customer_id = this.customer_id;
            Recordset.order_status = this.order_status;
            Recordset.order_date = this.order_date;
            Recordset.required_date = this.required_date;
            Recordset.shipped_date = this.shipped_date;
            Recordset.store_id = this.store_id;
            Recordset.staff_id = this.staff_id;

            await Recordset.SaveChangesAsync();
        }

    }
}

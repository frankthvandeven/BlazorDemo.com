using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.WebAssembly.Client.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class StoreEditCustomerModel : ModelTypedBase<StoreEditCustomerModel>
    {
        public PriKey_sales_customers_Recordset Rs = new PriKey_sales_customers_Recordset();
        public bool CreateNew = false;

        private int __customer_id;
        private string __first_name;
        private string __last_name;
        private string __phone;
        private string __email;
        private string __street;
        private string __city;
        private State __SelectedState = null;
        private string __zip_code;

        public string state
        {
            get { return this.SelectedState != null ? this.SelectedState.Abbreviation : ""; }
        }

        public StoreEditCustomerModel()
        {
            Register(m => m.customer_id);
            Register(m => m.first_name);
            Register(m => m.last_name);
            Register(m => m.phone);
            Register(m => m.email);
            Register(m => m.street);
            Register(m => m.city);
            //Register(m => m.state);
            Register(m => m.zip_code);
        }

        protected override async Task ValidateEventAsync()
        {
            if (e.IsMember(m => m.first_name))
            {
                await Task.CompletedTask;
                //Console.WriteLine($"The value of first_name was changed to {this.first_name}");
            }
            else if (e.IsMember(m => m.last_name))
            {
                //Console.WriteLine($"The value of last_name was changed to {this.last_name}");
            }

        }

        public async Task LoadCustomerTask()
        {
            if (this.CreateNew)
            {
                //this.customer_id = 0;
                this.first_name = "";
                this.last_name = "";
                this.phone = "";
                this.email = "";
                this.street = "";
                this.city = "";
                //this.state = null;
                this.zip_code = "";
            }
            else
            {
                await Rs.ExecSqlAsync(this.customer_id);

                if (Rs.RecordCount == 0)
                    throw new Exception($"Customer ID {this.customer_id} not found in database");

                this.first_name = Rs.first_name;
                this.last_name = Rs.last_name;
                this.phone = Rs.phone;
                this.email = Rs.email;
                this.street = Rs.street;
                this.city = Rs.city;
                //this.state = rs.state;
                this.zip_code = Rs.zip_code;

                this.SelectedState = UnitedStates.GenericList.Find(i => i.Abbreviation == Rs.state);
            }

        }

        public async Task SaveCustomerTask()
        {
            if (this.CreateNew)
            {
                Rs.Append();
            }
            else
            {
                await Rs.ExecSqlAsync(this.customer_id);

                if (Rs.RecordCount == 0)
                    throw new Exception($"Customer ID {this.customer_id} not found");
            }

            //rs.customer_id = this.customer_id;
            Rs.first_name = this.first_name;
            Rs.last_name = this.last_name;
            Rs.phone = this.phone;
            Rs.email = this.email;
            Rs.street = this.street;
            Rs.city = this.city;
            Rs.state = this.state;
            Rs.zip_code = this.zip_code;

            await Rs.SaveChangesAsync();

        }


    }
}

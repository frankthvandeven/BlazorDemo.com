using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.WebAssembly.Client.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class StoreEditCustomerModel : ModelTypedBase<StoreEditCustomerModel>
    {
        public PriKey_sales_customers_Recordset Recordset = new PriKey_sales_customers_Recordset();
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

        public StoreEditCustomerModel()
        {
            Register(m => m.first_name);
            Register(m => m.last_name);
            Register(m => m.SelectedState);
        }

        protected override async Task ValidateEventAsync()
        {
            await Task.CompletedTask; // A bit strange but acceptable

            if (e.IsMember(m => m.first_name))
            {
                if (string.IsNullOrEmpty(this.first_name) == false)
                {
                    e.IsValid = true;
                }

                e.RemarkText = $"Enter a first name.";
                return;
            }

            if (e.IsMember(m => m.last_name))
            {
                if (string.IsNullOrEmpty(this.first_name) == false)
                {
                    e.IsValid = true;
                }

                e.RemarkText = $"Enter a last name.";
                return;
            }

            if (e.IsMember(m => m.SelectedState))
            {
                if (this.SelectedState != null)
                {
                    e.IsValid = true;
                    return;
                }

                e.RemarkText = $"No state selected.";
                return;
            }

        }

        public async Task LoadTask()
        {
            if (this.CreateNew)
            {
                this.first_name = "";
                this.last_name = "";
                this.phone = "";
                this.email = "";
                this.street = "";
                this.city = "";
                this.SelectedState = UnitedStates.GenericList[0];
                this.zip_code = "";
            }
            else
            {
                await Recordset.ExecSqlAsync(this.customer_id);

                if (Recordset.RecordCount == 0)
                    throw new Exception($"Customer ID {this.customer_id} not found in database");

                this.first_name = Recordset.first_name;
                this.last_name = Recordset.last_name;
                this.phone = Recordset.phone;
                this.email = Recordset.email;
                this.street = Recordset.street;
                this.city = Recordset.city;
                this.SelectedState = UnitedStates.GenericList.Find(i => i.Abbreviation == Recordset.state);
                this.zip_code = Recordset.zip_code;

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
            }
            else
            {
                await Recordset.ExecSqlAsync(this.customer_id);

                if (Recordset.RecordCount == 0)
                    throw new Exception($"Customer ID {this.customer_id} not found");
            }

            Recordset.first_name = this.first_name;
            Recordset.last_name = this.last_name;
            Recordset.phone = this.phone;
            Recordset.email = this.email;
            Recordset.street = this.street;
            Recordset.city = this.city;
            Recordset.state = this.SelectedState.Abbreviation;
            Recordset.zip_code = this.zip_code;

            await Recordset.SaveChangesAsync();

        }


    }
}

using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.WebAssembly.Client.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
    [ViewModel]
    public partial class StoreEditProductModel : ModelTypedBase<StoreEditProductModel>
    {
        public PriKey_production_products_Recordset Recordset = new();

        public bool CreateNew = false;

        private int __product_id;
        private string __product_name;
        private int __brand_id;
        private int __category_id;
        private short __model_year;
        private decimal __list_price;

        public StoreEditProductModel()
        {
            Register(m => m.product_name);
            Register(m => m.brand_id);
            Register(m => m.category_id);
            Register(m => m.model_year);
            Register(m => m.list_price);
        }

        protected override async Task ValidateEventAsync()
        {

            if (e.IsMember(m => m.product_name))
            {
                await Task.CompletedTask;
                e.RemarkText = $"The value of product_name was changed to {this.product_name}";
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.brand_id))
            {
                await Task.CompletedTask;
                e.RemarkText = $"The value of brand_id was changed to {this.brand_id}";
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.category_id))
            {
                await Task.CompletedTask;
                e.RemarkText = $"The value of category_id was changed to {this.category_id}";
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.model_year))
            {
                await Task.CompletedTask;
                e.RemarkText = $"The value of model_year was changed to {this.model_year}";
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.list_price))
            {
                await Task.CompletedTask;
                e.RemarkText = $"The value of list_price was changed to {this.list_price}";
                e.IsValid = true;
                return;
            }

        }

        public async Task LoadTask()
        {
            if (this.CreateNew)
            {
                this.product_id = 0;
                this.product_name = "";
                this.brand_id = 0;
                this.category_id = 0;
                this.model_year = 0;
                this.list_price = 0.0m;
            }
            else
            {
                // edit
                await Recordset.ExecSqlAsync(this.product_id);

                if (Recordset.RecordCount == 0)
                    throw new Exception($"Product {this.product_id} not found in database.");

                this.product_id = Recordset.product_id;
                this.product_name = Recordset.product_name;
                this.brand_id = Recordset.brand_id;
                this.category_id = Recordset.category_id;
                this.model_year = Recordset.model_year;
                this.list_price = Recordset.list_price;

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
                // skipped primary key column Recordset.product_id as it is AutoIncrement
            }
            else
            {   
                await Recordset.ExecSqlAsync(this.product_id);

                if (Recordset.RecordCount == 0)
                    throw new Exception($"Product {this.product_id} not found in database.");
            }

            Recordset.product_name = this.product_name;
            Recordset.brand_id = this.brand_id;
            Recordset.category_id = this.category_id;
            Recordset.model_year = this.model_year;
            Recordset.list_price = this.list_price;

            await Recordset.SaveChangesAsync();
        }

    }
}

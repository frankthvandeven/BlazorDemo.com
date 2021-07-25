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
            Register(m => m.product_name, ValidateProductNameAsync);
            Register(m => m.brand_id);
            Register(m => m.category_id);
        }

        private async Task ValidateProductNameAsync(ValidateEventArgs e)
        {
            await Task.CompletedTask; // This is an acceptable solution

            if (string.IsNullOrEmpty(this.product_name))
            {
                e.RemarkText = "Enter a product name.";
                return;
            }

            e.IsValid = true;
        }

        protected override async Task ValidateEventAsync(ValidateEventArgs<StoreEditProductModel> e)
        {
            if (e.IsMember(m => m.brand_id))
            {
                if (brand_id is not > 0)
                {
                    e.RemarkText = "Brand is required.";
                    return;
                }

                PriKey_production_brands_Recordset rs = new();

                await rs.ExecSqlAsync(brand_id);

                if (rs.RecordCount == 0)
                {
                    e.RemarkText = $"Brand code {brand_id} is unknown.";
                    return;
                }

                e.RemarkText = rs.brand_name;
                e.IsValid = true;
                return;
            }

            if (e.IsMember(m => m.category_id))
            {
                if (category_id is not > 0)
                {
                    e.RemarkText = "Category is required.";
                    return;
                }

                PriKey_production_categories_Recordset rs = new();

                await rs.ExecSqlAsync(category_id);

                if (rs.RecordCount == 0)
                {
                    e.RemarkText = $"Category code {category_id} is unknown.";
                    return;
                }

                e.RemarkText = rs.category_name;
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
                this.brand_id = 1;
                this.category_id = 1;
                this.model_year = Convert.ToInt16(DateTime.Now.Year);
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

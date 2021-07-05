using BlazorDemo.Client.VenturaRecordsets;
using Kenova.WebAssembly.Client.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{

    [ViewModel]
    public partial class SearchCustomersModel
    {
        public SearchCustomersRecordset Recordset = new SearchCustomersRecordset();

        public ModelMode Mode = ModelMode.Regular;

        private string __SearchName = null;
        private int? __SearchCustomerId = null;

        //private SearchCustomersRecord __SelectedCustomer;

        public enum ModelMode
        {
            Regular,
            Lookup
        }

        public async Task SearchExec()
        {
            Recordset.RowLimit = 50; // 300; // 100000;
            await Recordset.ExecSqlAsync(this.SearchName, this.SearchCustomerId);
            
            //Console.WriteLine($"Search customer yielded {Recordset.RecordCount} records.");

            //this.first_name = rs.first_name;
            //this.last_name = rs.last_name;
            //this.phone = rs.phone;
            //this.email = rs.email;
            //this.street = rs.street;
            //this.city = rs.city;
            //this.state = rs.state;
            //this.zip_code = rs.zip_code;

        }

        public void Test()
        {
            ValidationContext context = new ValidationContext(this, null, null);

            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(this, context, validationResults, true);

            if (!valid)
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    Console.WriteLine("{0}", validationResult.ErrorMessage);
                }
            }


        }


    }
}

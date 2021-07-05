using BlazorDemo.Client.VenturaAutoCreate;
using Kenova.WebAssembly.Client.Components;
using System;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Components
{
	[ViewModel]
	public partial class StoreEditProductModel
	{
		public enum ModelMode { Edit, New }

		public ModelMode Mode;

		private int __order_id;
		private int? __customer_id;
		private byte __order_status;
		private DateTime __order_date;
		private DateTime __required_date;
		private DateTime? __shipped_date;
		private int __store_id;
		private int __staff_id;

		public StoreEditProductModel()
		{
			Register(m => m.order_id);
			Register(m => m.customer_id);
			Register(m => m.order_status);
			Register(m => m.order_date);
			Register(m => m.required_date);
			Register(m => m.shipped_date);
			Register(m => m.store_id);
			Register(m => m.staff_id);
		}


        protected override async Task ValidateEventAsync()
        {
			if (e.IsMember(m => m.customer_id))
			{
				await Validators.ValidateCustomerID(e, this.customer_id);
			}
			else if (e.IsMember(m => m.order_id))
			{
				e.RemarkText = "Validator of order id";
				e.IsValid = false;
			}
			else if (e.IsMember(m => m.customer_id))
			{
				Console.WriteLine($"The value of customer_id was changed to {this.customer_id}");
			}
			else if (e.IsMember(m => m.order_status))
			{
				Console.WriteLine($"The value of order_status was changed to {this.order_status}");
			}
			else if (e.IsMember(m => m.order_date))
			{
				Console.WriteLine($"The value of order_date was changed to {this.order_date}");
			}
			else if (e.IsMember(m => m.required_date))
			{
				Console.WriteLine($"The value of required_date was changed to {this.required_date}");
			}
			else if (e.IsMember(m => m.shipped_date))
			{
				Console.WriteLine($"The value of shipped_date was changed to {this.shipped_date}");
			}
			else if (e.IsMember(m => m.store_id))
			{
				Console.WriteLine($"The value of store_id was changed to {this.store_id}");
			}
			else if (e.IsMember(m => m.staff_id))
			{
				Console.WriteLine($"The value of staff_id was changed to {this.staff_id}");
			}

		}

		public PriKey_sales_orders_Record LastRecord;

		public async Task LoadExecTask()
		{
			LastRecord = null;

			var rs = new PriKey_sales_orders_Recordset();

			await rs.ExecSqlAsync(this.order_id);

			if (rs.RecordCount == 0)
				throw new Exception($"Order {this.order_id} not found in database.");

			//this.order_id = rs.order_id;
			this.customer_id = rs.customer_id;
			this.order_status = rs.order_status;
			this.order_date = rs.order_date;
			this.required_date = rs.required_date;
			this.shipped_date = rs.shipped_date;
			this.store_id = rs.store_id;
			this.staff_id = rs.staff_id;

			LastRecord = rs.CurrentRecord;
		}

		public async Task SaveExecTask()
		{
			LastRecord = null;

			var rs = new PriKey_sales_orders_Recordset();

			if (this.Mode == ModelMode.Edit)
			{
				await rs.ExecSqlAsync(this.order_id);

				if (rs.RecordCount == 0)
					throw new Exception($"Order {this.order_id} not found in database.");
			}
			else
			{
				rs.Append();
			}

			//rs.order_id = this.order_id; // IsKey, IsIdentity, IsAutoIncrement, IsReadOnly

			rs.customer_id = this.customer_id;
			rs.order_status = this.order_status;
			rs.order_date = this.order_date;
			rs.required_date = this.required_date;
			rs.shipped_date = this.shipped_date;
			rs.store_id = this.store_id;
			rs.staff_id = this.staff_id;

			await rs.SaveChangesAsync();

			LastRecord = rs.CurrentRecord;
		}

	}
}

using System;
using System.Collections.Generic;

namespace BlazorDemo.Server
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public byte OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int StoreId { get; set; }
        public int StaffId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}

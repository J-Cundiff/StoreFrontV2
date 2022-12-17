using System;
using System.Collections.Generic;

namespace StoreFrontV2.DATA.EF.Models
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }
        public string CustomerId { get; set; } = null!;
        public int ProductId { get; set; }
        public string ShipperName { get; set; } = null!;
        public string ShippedCity { get; set; } = null!;
        public string? ShippedState { get; set; }
        public string ShippedCountry { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal? ShippingCost { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace StoreFrontV2.DATA.EF.Models
{
    public partial class Shipper
    {
        public Shipper()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ShipperId { get; set; }
        public string ShipperName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

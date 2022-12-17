using System;
using System.Collections.Generic;

namespace StoreFrontV2.DATA.EF.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; } = null!;
        public int ShipperId { get; set; }
        public string ShipperName { get; set; } = null!;
        public string ShippedCity { get; set; } = null!;
        public string? ShippedState { get; set; }
        public string ShippedCountry { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal? ShippingCost { get; set; }

        //Our form does not supply an entire Category object/record.
        //The property below created by Entity Framework does not allow
        //null values for Category. In  Order to ensure we are able to edit
        //products (pass Model Vaildation) we will need to update the property 
        //to allow nulls.
        //public virtual Category Category { get; set; } = null!;
        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Shipper? Shipper { get; set; }
    }
}

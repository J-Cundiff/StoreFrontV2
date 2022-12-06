using System;
using System.Collections.Generic;

namespace StoreFrontV2.DATA.EF.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public string? Phone { get; set; }
        public string? SupplierDescription { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace StoreFrontV2.DATA.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public int QtyInStock { get; set; }
        public int QtyOnOrder { get; set; }
        public int StatusId { get; set; }
        public string? ProductImage { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ProductStatus? Status { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace StoreFrontV2.DATA.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
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


        //Our form does not supply an entire Category object/record.
        //The property below created by Entity Framework does not allow
        //null values for Category. In  Order to ensure we are able to edit
        //products (pass Model Vaildation) we will need to update the property 
        //to allow nulls.
        //public virtual Category Category { get; set; } = null!;
        public virtual Category? Category { get; set; }
        public virtual ProductStatus? Status { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

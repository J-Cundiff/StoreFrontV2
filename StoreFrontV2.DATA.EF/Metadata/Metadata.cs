using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontV2.DATA.EF.Models/*Metadata*/
{
    public class CategoryMetadata
    {
        //public int CategoryId { get; set; }-PK

        [Required(ErrorMessage = "*Category is required")]
        [StringLength(20, ErrorMessage = "*Cannot exceed 20 characters")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;

        [StringLength(200, ErrorMessage = "*Cannnot exceed 200 characters")]
        [Display(Name = "Description")]
        public string? CategoryDescription { get; set; }
    }

    public class CustomerMetadata
    {
        //public string CustomerId { get; set; } = null!;-PK

        [Required(ErrorMessage = "*First Name is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "*Last Name is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "*Address is required")]
        [StringLength(150, ErrorMessage = "*Cannot exceed 150 characters")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "*City is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        public string City { get; set; } = null!;

        [StringLength(2, ErrorMessage = "*Must be 2 characters")]
        public string? State { get; set; }

        [Required(ErrorMessage = "*Zip Code is required")]
        [StringLength(5, ErrorMessage = "*Must be 5 characters")]
        [DataType(DataType.PostalCode)]
        public string Zip { get; set; } = null!;

        [Required(ErrorMessage = "*Country is required")]
        [StringLength(50, ErrorMessage = "*Cannot not exceed 50 characters")]
        public string Country { get; set; } = null!;

        [StringLength(24, ErrorMessage = "*Cannot exceed 24 characters")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
    }

    public class OrderDetailMetadata
    {
        //public int OrderId { get; set; }-PK

        //public string CustomerId { get; set; } = null!;
        //public int ShipperId { get; set; }
        //public int ProductId { get; set; }

        [Required(ErrorMessage = "*Shipper Name is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        [Display(Name = "Shipper")]
        public string ShipperName { get; set; } = null!;

        [Required(ErrorMessage = "*City shipped to required")]
        [StringLength(100, ErrorMessage = "*Cannot exceed 100 characters")]
        [Display(Name = "City")]
        public string ShippedCity { get; set; } = null!;

        [StringLength(2, ErrorMessage = "*Must be 2 characters")]
        [Display(Name = "State")]
        public string? ShippedState { get; set; }

        [Required(ErrorMessage = "*Country shipped to required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        [Display(Name = "Country")]
        public string ShippedCountry { get; set; } = null!;

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Display(Name = "Shipping Cost")]
        public decimal? ShippingCost { get; set; }
    }

    public class ProductMetadata
    {
        //public int ProductId { get; set; }-PK

        [Required(ErrorMessage = "*Product Name is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = null!;

        [Required]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }

        [StringLength(200, ErrorMessage = "*Cannot exceed 200 characters")]
        [Display(Name = "Description")]
        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "*Quantity in stock is required")]
        [Display(Name = "Quantity In Stock")]
        public int QtyInStock { get; set; }

        [Required(ErrorMessage = "*Quantity on order is required")]
        [Display(Name = "Quantity On Order")]
        public int QtyOnOrder { get; set; }

        public int StatusId { get; set; }

        [StringLength(75, ErrorMessage = "*Cannot exceed 75 characters")]
        public string? ProductImage { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
    }

    public class ProductStatusMetadata
    {
        //public int StatusId { get; set; }

        [Required(ErrorMessage = "*Status Name is required")]
        [StringLength(50, ErrorMessage = "*Cannot  exceed 50 characters")]
        [Display(Name = "Status Name")]
        public string StatusName { get; set; } = null!;
    }

    public class ShipperMetadata
    {
        //public int ShipperId { get; set; }-PK

        [Required(ErrorMessage = "*Shipper Name is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        [Display(Name = "Shipper")]
        public string ShipperName { get; set; } = null!;

        [Required(ErrorMessage = "*City is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "*State is required")]
        [StringLength(2, ErrorMessage = "*Must be 2 characters")]
        public string State { get; set; } = null!;

        [Required(ErrorMessage = "*Country is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        public string Country { get; set; } = null!;
    }

    public class SupplierMetadata
    {
        //public int SupplierId { get; set; }-PK

        [Required(ErrorMessage = "*Supplier Name is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; } = null!;

        [Required(ErrorMessage = "*City is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        public string City { get; set; } = null!;

        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        public string? State { get; set; }

        [Required(ErrorMessage = "*Country is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters")]
        public string Country { get; set; } = null!;

        [StringLength(24, ErrorMessage = "*Cannot exceed 24 characters")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [StringLength(200, ErrorMessage = "*Cannot exceed 200 characters")]
        [Display(Name = "Description")]
        public string? SupplierDescription { get; set; }
    }

}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontV2.DATA.EF.Models/*Metadata*/
{
    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {

    }

    [ModelMetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {

    }

    [ModelMetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail
    {

    }

    [ModelMetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        [NotMapped]
        public IFormFile? Image { get; set; }
    }

    [ModelMetadataType(typeof(ProductStatusMetadata))]
    public partial class ProductStatus
    {

    }

    [ModelMetadataType(typeof(ShipperMetadata))]
    public partial class Shipper
    {

    }

    [ModelMetadataType(typeof(SupplierMetadata))]
    public partial class Supplier
    {

    }
}

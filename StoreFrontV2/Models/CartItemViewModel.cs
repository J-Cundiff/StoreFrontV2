using Microsoft.Build.ObjectModelRemoting;
using StoreFrontV2.DATA.EF.Models;

namespace StoreFrontV2.Models
{
    public class CartItemViewModel
    {

        public int Qty { get; set; }

        public Product Product { get; set; }

        public CartItemViewModel(int qty, Product product)
        {
            Qty = qty;
            Product = product;
        }
    }
}

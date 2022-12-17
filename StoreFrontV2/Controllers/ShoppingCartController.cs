using Microsoft.AspNetCore.Mvc;
using StoreFrontV2.DATA.EF.Models;//Added for access to the context
using Microsoft.AspNetCore.Identity;//Added for access to UserManager
using StoreFrontV2.Models;//Added for access to the CartItemViewModel
using Newtonsoft.Json;//This provides easier managment of the shopping cart


namespace StoreFrontV2.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly StoreFrontContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(StoreFrontContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            //Retrieve the cart contents
            var sessionCart = HttpContext.Session.GetString("cart");

            //Create a shell for the local (C# version) of the cart
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //Check to see if the sessionCart was null
            if (sessionCart == null || sessionCart.Count() == 0)
            {
                //User either hasnt put anything in the cart, or the have removed
                //all items from the cart. So, we'll se shoppingCart to an empty Dictionary
                shoppingCart = new Dictionary<int, CartItemViewModel>();

                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                ViewBag.Message = null;

                //Deserialize the cart contents from JSON into C#
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }


            return View(shoppingCart);

        }

        public IActionResult AddToCart(int id)
        {
            //Empty shell for the local shopping cart variable
            //int (key) => ProductId
            //CartItemViewModel (Value) => Product & Qty
            Dictionary<int, CartItemViewModel> shoppingCart = null;


            var sessionCart = HttpContext.Session.GetString("cart");

            //Check to see if the Session object exist
            //If not, instantiate the new local connection
            if (string.IsNullOrEmpty(sessionCart))
            {
                //If the session didn't exist yet, istantiate the collection as a new object
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                //Cart already exists - transfer the exiting cart items from the Session into our 
                //local shoppingCart.
                //DeserializeObject() is a method that converts JSON to C# - we MUST tell this method
                //which C# class to convert the JSON into (here, that's our Disctionary<int, CartItemViewModel>)
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            }

            //Add any newly selected products to the cart
            //Retrieve the product from the database
            Product product = _context.Products.Find(id);

            //Instantiate the CartItemViewModel object so we can add it to the cart
            CartItemViewModel civm = new CartItemViewModel(1, product);//Adding 1 of the selected products to the cart

            //If the product was already in the cart lets increase the QTY by 1.
            //Otherwise we will add the new CIVM item to the cart
            if (shoppingCart.ContainsKey(product.ProductId))
            {
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                shoppingCart.Add(product.ProductId, civm);
            }

            //Update the Session version of the cart
            //Take local copy and Serialize it as JSON
            //Then assign that value to our session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");

        }

        public IActionResult RemoveFromCart(int id)
        {
            //Retrieve the cart from the Session
            var sessionCart = HttpContext.Session.GetString("cart");

            //Convert the JSON to C#
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Remove the item
            shoppingCart.Remove(id);

            //If there are no remaining items in the cart, lets 
            //remove it from the session
            if (shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                //Ptherwise, update the session with the new cart contents
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int productId, int qty)
        {
            //Retrieve the cart
            var sessionCart = HttpContext.Session.GetString("cart");

            //Convert from JSON to C#
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int,
                CartItemViewModel>>(sessionCart);

            //Update the qty for our specific cart item

            if (qty == 0)
            {
                //shoppingCart[productId].Qty = 1;
                shoppingCart.Remove(productId);
            }
            else
            {
                shoppingCart[productId].Qty = qty;
            }


             

            //Update the Session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");

        }

        //TODO Submit Order
        public async Task<IActionResult> SubmitOrder()
        {
            //Retrieve the current user's ID
            string? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            //Retreive the Customer Record
            Customer cust = _context.Customers.Find(userId);

            //Create an Order object assign the value
            OrderDetail od = new OrderDetail()
            {
                OrderDate = DateTime.Now,
                CustomerId = userId,
                ShipperName = cust.FirstName + " " + cust.LastName,
                ShippedCity = cust.City,
                ShippedState = cust.State,
                ShippedCountry = cust.Country,
            };

            //Add the Order object to the _context
            _context.OrderDetails.Add(od);

            //Retrieve sessionCart
            var sessionCart = HttpContext.Session.GetString("cart");

            //Convert cart to C#
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int,
                CartItemViewModel>>(sessionCart);

            //Create OrderProduct object for each item in the cart
            foreach (var item in shoppingCart)
            {
                OrderProduct op = new OrderProduct()
                {
                    OrderId = od.OrderId,
                    ProductId = item.Key,
                    ProductPrice = item.Value.Product.ProductPrice,
                    Quantity = (short)item.Value.Qty

                };

                od.OrderProducts.Add(op);

            }

            //Commit the changes and save to the database
            _context.SaveChanges();

            return RedirectToAction("Index", "OrderDetails");


        }




    }
}

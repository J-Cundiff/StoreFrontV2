using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFrontV2.DATA.EF.Models;
using StoreFrontV2.Utilities;
using X.PagedList;

namespace StoreFrontV2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly StoreFrontContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;//gives access to infomation about the host(Server)
        public ProductsController(StoreFrontContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var storeFrontContext = _context.Products.Include(p => p.Category)
                .Include(p => p.Status)
                .Include(p => p.OrderProducts)
                .Include(p => p.Supplier);
            return View(await storeFrontContext.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> TiledProducts(string searchTerm, int categoryId = 0, int page = 1)
        {
            
            //Items oer page
            int pageSize = 12;

            //var storeFrontContext = _context.Products.Include(p => p.Category)
            //    .Include(p => p.Status)
            //    .Include(p => p.OrderProducts)
            //    .Include(p => p.Supplier);

            //Category Filter
            var products = _context.Products.Where(p => p.StatusId != 4)
                .Include(p => p.Status)
                .Include(p => p.OrderProducts)
                .Include(p => p.Supplier).ToList();

            //Create a ViewData object to send a list of categories to the View
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewBag.Category = 0;//Adding this variable to persist (save) the selected Category

            if (categoryId != 0)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();

                //Repopulate the dropdown with the category selected
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);
                ViewBag.Category = categoryId;
            }

            //Search Term Option
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                products = _context.Products.Where(p =>
                
                p.ProductName.ToLower().Contains(searchTerm) ||
                p.Supplier.SupplierName.ToLower().Contains(searchTerm) ||
                p.Category.CategoryName.ToLower().Contains(searchTerm) ||
                p.ProductDescription.ToLower().Contains(searchTerm)).ToList();

                //Store the number of results
                ViewBag.NbrResults = products.Count;

                //Store thesearch
                ViewBag.SearchTerm = searchTerm;
            }
            else
            {
                ViewBag.NbrResults = null;
                ViewBag.SearchTerm = null;
            }



            //return View(await products.ToListAsync());
            //return View(products);  
            return View(products.ToPagedList(page, pageSize));
        }

        [AllowAnonymous]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Status)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusName");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProductDescription,QtyInStock,QtyOnOrder,StatusId,ProductImage,CategoryId,SupplierId,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                #region fileUpload
                //Check if a file was uploaded
                if (product.Image != null)
                {
                    //Check the file type
                    // - Store the extention of the image in a string
                    string ext = Path.GetExtension(product.Image.FileName);

                    //Create a list of vaild extentions to check the ext against
                    string[] vaildExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //Verify the uploaded file has an extention matching one of the extetions in the list above
                    //We will also want to verify the file size will work with our .NET app
                    if (vaildExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //Generate a unique file name
                        product.ProductImage = Guid.NewGuid() + ext;

                        //Save the file to the web sever (here, save to the wwwroot/images
                        //To access the wwwroot, we added the _webHostEnvironment feild to the controller (See the top)
                        //Retrieve the path to the wwwroot
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        //Variable for the full imagepath (this is where we will save the image)
                        string fullImagePath = webRootPath + "/images/";

                        //Creat a MemoryStream to read the image into the server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            //Tranfer the file from the request to the server memory
                            await product.Image.CopyToAsync(memoryStream);

                            //Genrate the image using the info captured from the MemroyStream
                            //Add using staement for System.Drawing to get access to the Image class
                            using (var img = Image.FromStream(memoryStream))
                            {
                                //Send the image to the ImageUtility for resizing and thumbnail creation
                                //Items needed for the ImageUtility.ResizeImage()
                                //- a string with the path where the file should be saved
                                //- a string with the name of the file
                                //- The actual Image itself
                                //- An Int with the maximum size (in pixels)
                                //- An int with the maximum thumbnail size

                                int maxImageSize = 500; //In pixels
                                int maxThumbSize = 100; //In pixels

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);

                                //myFile.Save("path/to/folder", "filename"); => How to save something that is not an image.
                            }
                        }
                    }
                }
                else
                {
                    //No image was uplaoded, assign a default fileName
                    //And include a default image (With that fileName) in the images folder
                    product.ProductImage = "noimage.png";
                }


                #endregion




                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusName", product.StatusId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusName", product.StatusId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProductDescription,QtyInStock,QtyOnOrder,StatusId,ProductImage,CategoryId,SupplierId,Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                #region FILE UPLOAD - EDIT

                //Retain the old Image file name so we can delete it if a new file was uploaded
                string oldImageName = product.ProductImage;

                //check if the user uploaded the file
                if (product.Image != null)
                {
                    //Get the files extention
                    string ext = Path.GetExtension(product.Image.FileName);

                    //Create the list of vaild extentions
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //check the files extention & file size
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //Generate a unqiue file name
                        product.ProductImage = Guid.NewGuid() + ext;

                        //Build the file path to save the image
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullImagePath = webRootPath + "/images/";

                        //Delete the old image
                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(fullImagePath, oldImageName);
                        }

                        //Save the new image to the webroot
                        using (var memoryStream = new MemoryStream())
                        {
                            //Get the image from the MemoryStream
                            await product.Image.CopyToAsync(memoryStream);


                            //Copy the image from the stream into an Image on the server
                            using (var img = Image.FromStream(memoryStream))
                            {
                                //SEtup the max image and maxThumbNail
                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                //resize and save the image
                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }



                #endregion


                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusName", product.StatusId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Status)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'StoreFrontContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}

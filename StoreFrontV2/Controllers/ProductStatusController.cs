﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFrontV2.DATA.EF.Models;

namespace StoreFrontV2.Controllers
{
    public class ProductStatusController : Controller
    {
        private readonly StoreFrontContext _context;

        public ProductStatusController(StoreFrontContext context)
        {
            _context = context;
        }

        // GET: ProductStatus
        public async Task<IActionResult> Index()
        {
              return View(await _context.ProductStatuses.ToListAsync());
        }

        // GET: ProductStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductStatuses == null)
            {
                return NotFound();
            }

            var productStatus = await _context.ProductStatuses
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (productStatus == null)
            {
                return NotFound();
            }

            return View(productStatus);
        }

        // GET: ProductStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,StatusName")] ProductStatus productStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productStatus);
        }

        // GET: ProductStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductStatuses == null)
            {
                return NotFound();
            }

            var productStatus = await _context.ProductStatuses.FindAsync(id);
            if (productStatus == null)
            {
                return NotFound();
            }
            return View(productStatus);
        }

        // POST: ProductStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,StatusName")] ProductStatus productStatus)
        {
            if (id != productStatus.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductStatusExists(productStatus.StatusId))
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
            return View(productStatus);
        }

        // GET: ProductStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductStatuses == null)
            {
                return NotFound();
            }

            var productStatus = await _context.ProductStatuses
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (productStatus == null)
            {
                return NotFound();
            }

            return View(productStatus);
        }

        // POST: ProductStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductStatuses == null)
            {
                return Problem("Entity set 'StoreFrontContext.ProductStatuses'  is null.");
            }
            var productStatus = await _context.ProductStatuses.FindAsync(id);
            if (productStatus != null)
            {
                _context.ProductStatuses.Remove(productStatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductStatusExists(int id)
        {
          return _context.ProductStatuses.Any(e => e.StatusId == id);
        }
    }
}

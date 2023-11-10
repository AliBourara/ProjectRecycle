using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectRecycle.Models;

namespace ProjectRecycle.Controllers
{
    public class OffersController : Controller
    {
        private readonly MyContext _context;

        public OffersController(MyContext context)
        {
            _context = context;
        }

        // GET: Offers
        public async Task<IActionResult> Index()
        {
            var myContext = _context.Offers.Include(o => o.Waste)
                ;
            return View(await myContext.ToListAsync());
        }

        // GET: Offers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Offers == null)
            {
                return NotFound();
            }

            var offer = _context.Offers
    .Where(o => o.OfferId == id)
    .Include(o => o.Waste)
    .Include(o => o.Bids) // Include the related Bids for the Offer
        .ThenInclude(b => b.Bidder) // Include the related Bidder (Company)
    .FirstOrDefault();
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }
        [HttpPost]
        public async Task<IActionResult> Bid(Bid bid)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bid);
                await _context.SaveChangesAsync();
                return Redirect($"Details/{bid.OfferId}");
            }
            return Redirect($"Details/{bid.OfferId}");
        }

        // GET: Offers/Create
        public IActionResult Create()
        {
            ViewData["WasteId"] = new SelectList(_context.Wastes, "WasteId", "WasteId");
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WasteId"] = new SelectList(_context.Wastes, "WasteId", "WasteId", offer.WasteId);
            return View(offer);
        }

        // GET: Offers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Offers == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            ViewData["WasteId"] = new SelectList(_context.Wastes, "WasteId", "WasteId", offer.WasteId);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Offer offer)
        {
            if (id != offer.OfferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.OfferId))
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
            ViewData["WasteId"] = new SelectList(_context.Wastes, "WasteId", "WasteId", offer.WasteId);
            return View(offer);
        }

        // GET: Offers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Offers == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Waste)
                .FirstOrDefaultAsync(m => m.OfferId == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Offers == null)
            {
                return Problem("Entity set 'MyContext.Offers'  is null.");
            }
            var offer = await _context.Offers.FindAsync(id);
            if (offer != null)
            {
                _context.Offers.Remove(offer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(int id)
        {
          return (_context.Offers?.Any(e => e.OfferId == id)).GetValueOrDefault();
        }
    }
}
